// Follow reachable ARM64 control flow beyond the initial metadata method interval.
// Calls are not followed. Branch targets that equal another IL2CPP method-pointer
// entry are treated as separate methods and are not absorbed into this function.
import ghidra.app.script.GhidraScript;
import ghidra.app.cmd.disassemble.DisassembleCommand;
import ghidra.app.cmd.function.CreateFunctionCmd;
import ghidra.app.decompiler.DecompInterface;
import ghidra.app.decompiler.DecompileResults;
import ghidra.program.model.address.Address;
import ghidra.program.model.address.AddressSet;
import ghidra.program.model.listing.Function;
import ghidra.program.model.listing.Instruction;
import ghidra.program.model.listing.InstructionIterator;
import ghidra.program.model.pcode.HighFunction;
import ghidra.program.model.pcode.PcodeOpAST;
import java.util.Iterator;
import java.util.TreeSet;
import java.util.Map;
import java.util.TreeMap;
import ghidra.program.model.mem.MemoryBlock;
import ghidra.program.model.symbol.SourceType;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayDeque;
import java.util.HashSet;
import java.util.Set;

public class PrepareFlowFunction extends GhidraScript {
    @Override
    protected void run() throws Exception {
        String[] args = getScriptArgs();
        if (args.length != 7) {
            throw new IllegalArgumentException(
                "Expected: <entry> <max-instructions> <max-span-bytes> <name> <pseudocode> <metadata> <method-entry-list>");
        }
        Address entry = toAddr(Long.decode(args[0]));
        int maxInstructions = Integer.parseInt(args[1]);
        long maxSpan = Long.decode(args[2]);
        String name = args[3];
        Path codePath = Paths.get(args[4]).toAbsolutePath();
        Path metaPath = Paths.get(args[5]).toAbsolutePath();
        Path entriesPath = Paths.get(args[6]).toAbsolutePath();

        Set<Long> methodEntries = new HashSet<>();
        for (String line : Files.readAllLines(entriesPath, StandardCharsets.UTF_8)) {
            String value = line.trim();
            if (!value.isEmpty()) methodEntries.add(Long.decode(value));
        }

        ArrayDeque<Address> queue = new ArrayDeque<>();
        Set<Long> visited = new HashSet<>();
        Set<Long> skippedMethodEntries = new HashSet<>();
        AddressSet decoded = new AddressSet();
        Set<Long> decodedInstructionStarts = new HashSet<>();
        queue.add(entry);
        int instructionCount = 0;
        int decodeFailures = 0;

        while (!queue.isEmpty() && instructionCount < maxInstructions) {
            monitor.checkCancelled();
            Address at = queue.removeFirst();
            long offset = at.getOffset();
            if (visited.contains(offset)) continue;
            if (Math.abs(offset - entry.getOffset()) > maxSpan) continue;
            MemoryBlock block = currentProgram.getMemory().getBlock(at);
            if (block == null || !block.isInitialized() || !block.isExecute()) continue;

            Instruction existing = currentProgram.getListing().getInstructionAt(at);
            if (existing == null) {
                AddressSet one = new AddressSet(at, at.add(3));
                DisassembleCommand cmd = new DisassembleCommand(at, one, false);
                if (!cmd.applyTo(currentProgram, monitor)) {
                    decodeFailures++;
                    visited.add(offset);
                    continue;
                }
                existing = currentProgram.getListing().getInstructionAt(at);
            }
            visited.add(offset);
            if (existing == null) {
                decodeFailures++;
                continue;
            }

            Address last = existing.getMaxAddress();
            if (!block.contains(last)) {
                decodeFailures++;
                continue;
            }
            decoded.add(at, last);
            decodedInstructionStarts.add(offset);
            instructionCount++;

            if (existing.getFlowType().isCall()) {
                if (existing.hasFallthrough()) {
                    Address next = existing.getFallThrough();
                    if (next != null && next.getOffset() != entry.getOffset() &&
                        methodEntries.contains(next.getOffset())) {
                        skippedMethodEntries.add(next.getOffset());
                    } else if (next != null && !visited.contains(next.getOffset())) {
                        queue.addLast(next);
                    }
                }
                continue;
            }

            Address[] flows = existing.getFlows();
            for (Address target : flows) {
                if (target == null) continue;
                long targetOffset = target.getOffset();
                if (targetOffset != entry.getOffset() && methodEntries.contains(targetOffset)) {
                    skippedMethodEntries.add(targetOffset);
                    continue;
                }
                if (!visited.contains(targetOffset)) queue.addLast(target);
            }
            if (existing.hasFallthrough()) {
                Address next = existing.getFallThrough();
                if (next != null && next.getOffset() != entry.getOffset() &&
                    methodEntries.contains(next.getOffset())) {
                    skippedMethodEntries.add(next.getOffset());
                } else if (next != null && !visited.contains(next.getOffset())) {
                    queue.addLast(next);
                }
            }
        }

        if (instructionCount == 0 || !decoded.contains(entry)) {
            throw new IllegalStateException("No reachable instructions decoded at method entry " + entry);
        }

        Function function = currentProgram.getFunctionManager().getFunctionAt(entry);
        if (function == null) {
            CreateFunctionCmd create = new CreateFunctionCmd(name, entry, decoded, SourceType.ANALYSIS);
            if (!create.applyTo(currentProgram, monitor)) {
                throw new IllegalStateException("CreateFunctionCmd failed: " + create.getStatusMsg());
            }
            function = create.getFunction();
        } else {
            // Replace any import-created/default body with the exact reachable address set.
            // If another function overlaps this set, fail instead of silently decompiling a stale body.
            function.setBody(decoded);
        }
        if (function == null || !function.getEntryPoint().equals(entry)) {
            throw new IllegalStateException("Function entry mismatch for " + name);
        }
        if (function.getBody().getNumAddresses() != decoded.getNumAddresses()) {
            throw new IllegalStateException("Function body byte count does not match reachable address set for " + name);
        }
        Set<Long> functionBodyInstructionStarts = new HashSet<>();
        InstructionIterator bodyInstructions = currentProgram.getListing().getInstructions(function.getBody(), true);
        while (bodyInstructions.hasNext()) {
            functionBodyInstructionStarts.add(bodyInstructions.next().getAddress().getOffset());
        }
        Set<Long> bodyStartsMissingFromDecoded = new HashSet<>(decodedInstructionStarts);
        bodyStartsMissingFromDecoded.removeAll(functionBodyInstructionStarts);
        Set<Long> bodyStartsOutsideDecoded = new HashSet<>(functionBodyInstructionStarts);
        bodyStartsOutsideDecoded.removeAll(decodedInstructionStarts);
        if (!bodyStartsMissingFromDecoded.isEmpty() || !bodyStartsOutsideDecoded.isEmpty()) {
            throw new IllegalStateException("Function body instruction-start set differs from reachable set for " + name +
                "; missing=" + bodyStartsMissingFromDecoded.size() + "; outside=" + bodyStartsOutsideDecoded.size());
        }

        DecompInterface decompiler = new DecompInterface();
        String pseudocode;
        Set<Long> decompilerInstructionAddresses = new HashSet<>();
        StringBuilder pcodeProvenance = new StringBuilder("pcode_address\tcontaining_function_entry\tcontaining_function_name\tinside_target_recovered_body\tcontaining_function_body_contains_address\topcode\n");
        try {
            if (!decompiler.openProgram(currentProgram)) {
                throw new IllegalStateException("Could not open Ghidra program in decompiler");
            }
            DecompileResults result = decompiler.decompileFunction(function, 240, monitor);
            if (result == null || !result.decompileCompleted() || result.getDecompiledFunction() == null) {
                throw new IllegalStateException("Decompilation failed: " +
                    (result == null ? "no result" : result.getErrorMessage()));
            }
            pseudocode = result.getDecompiledFunction().getC();
            HighFunction highFunction = result.getHighFunction();
            if (highFunction != null) {
                Iterator<PcodeOpAST> pcodeOps = highFunction.getPcodeOps();
                while (pcodeOps.hasNext()) {
                    PcodeOpAST op = pcodeOps.next();
                    Address pcodeAddress = op.getSeqnum().getTarget();
                    if (pcodeAddress != null) {
                        decompilerInstructionAddresses.add(pcodeAddress.getOffset());
                        Function owner = currentProgram.getFunctionManager().getFunctionContaining(pcodeAddress);
                        String ownerEntry = owner == null ? "" : owner.getEntryPoint().toString();
                        String ownerName = owner == null ? "" : owner.getName();
                        boolean insideTargetBody = function.getBody().contains(pcodeAddress);
                        boolean ownerBodyContainsAddress = owner != null && owner.getBody().contains(pcodeAddress);
                        pcodeProvenance.append(String.format("0x%x\t%s\t%s\t%s\t%s\t%s%n",
                            pcodeAddress.getOffset(), ownerEntry, ownerName, insideTargetBody, ownerBodyContainsAddress, op.getMnemonic()));
                    }
                }
            }
            if (pseudocode == null || pseudocode.trim().isEmpty()) {
                throw new IllegalStateException("Empty pseudocode for " + name);
            }
        } finally {
            decompiler.dispose();
        }

        int blockCount = 0;
        InstructionIterator it = currentProgram.getListing().getInstructions(decoded, true);
        while (it.hasNext()) { it.next(); blockCount++; }

        // For selected provenance-heavy methods, inspect every distinct P-code source address.
        // This distinguishes missing function ownership from missing/non-executable code bytes.
        if (name.startsWith("RuntimeSpineLoader_") || name.startsWith("RuntimeSpineAsset_") ||
            name.startsWith("PageTextureLoader_")) {
            StringBuilder forensic = new StringBuilder("pcode_address\tmemory_block\tinitialized\texecutable\tinstruction_present\tmnemonic\tinstruction_text\tbytes_hex\tflow_type\tflow_targets\tflow_target_owners\towner_entry\towner_name\ttarget_body\n");
            TreeSet<Long> sortedPcodeAddresses = new TreeSet<>(decompilerInstructionAddresses);
            for (Long rawAddress : sortedPcodeAddresses) {
                Address address = toAddr(rawAddress);
                MemoryBlock block = currentProgram.getMemory().getBlock(address);
                Instruction instruction = currentProgram.getListing().getInstructionAt(address);
                if (instruction == null && block != null && block.isInitialized() && block.isExecute()) {
                    AddressSet one = new AddressSet(address, address.add(3));
                    DisassembleCommand command = new DisassembleCommand(address, one, false);
                    command.applyTo(currentProgram, monitor);
                    instruction = currentProgram.getListing().getInstructionAt(address);
                }
                Function owner = currentProgram.getFunctionManager().getFunctionContaining(address);
                String mnemonic = instruction == null ? "" : instruction.getMnemonicString();
                String instructionText = instruction == null ? "" : instruction.toString().replace("\t", " ").replace("\n", " ");
                String bytesHex = "";
                if (instruction != null) {
                    byte[] rawBytes = instruction.getBytes();
                    StringBuilder hex = new StringBuilder();
                    for (byte value : rawBytes) hex.append(String.format("%02x", value & 0xff));
                    bytesHex = hex.toString();
                }
                String flowType = instruction == null ? "" : instruction.getFlowType().toString();
                StringBuilder flowTargets = new StringBuilder();
                StringBuilder flowTargetOwners = new StringBuilder();
                if (instruction != null) {
                    for (Address target : instruction.getFlows()) {
                        if (flowTargets.length() > 0) flowTargets.append(",");
                        flowTargets.append(target.toString());
                        Function targetOwner = currentProgram.getFunctionManager().getFunctionContaining(target);
                        if (flowTargetOwners.length() > 0) flowTargetOwners.append(",");
                        flowTargetOwners.append(target.toString()).append("=");
                        if (targetOwner != null) flowTargetOwners.append(targetOwner.getEntryPoint()).append(":").append(targetOwner.getName());
                    }
                }
                forensic.append(String.format("0x%x\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\n",
                    rawAddress, block == null ? "" : block.getName(), block != null && block.isInitialized(),
                    block != null && block.isExecute(), instruction != null, mnemonic, instructionText, bytesHex,
                    flowType, flowTargets.toString(), flowTargetOwners.toString(), owner == null ? "" : owner.getEntryPoint().toString(),
                    owner == null ? "" : owner.getName(), function.getBody().contains(address)));
            }
            Path forensicPath = metaPath.resolveSibling(metaPath.getFileName().toString() + ".pcode-address-forensics.tsv");
            Files.writeString(forensicPath, forensic.toString(), StandardCharsets.UTF_8);

            // Export unique statically resolved call destinations separately from P-code
            // source addresses. These are candidates for review, not automatic function entries.
            Map<Long, TreeSet<Long>> callSitesByTarget = new TreeMap<>();
            for (Long rawAddress : sortedPcodeAddresses) {
                Address source = toAddr(rawAddress);
                Instruction sourceInstruction = currentProgram.getListing().getInstructionAt(source);
                if (sourceInstruction == null || !sourceInstruction.getFlowType().isCall()) continue;
                for (Address target : sourceInstruction.getFlows()) {
                    if (target == null) continue;
                    callSitesByTarget.computeIfAbsent(target.getOffset(), k -> new TreeSet<>()).add(rawAddress);
                }
            }
            StringBuilder candidates = new StringBuilder(
                "target\tmetadata_method_entry\tmemory_block\tinitialized\texecutable\tinstruction_present\ttarget_instruction\ttarget_bytes_hex\tcontaining_function_entry\tcontaining_function_name\txref_count\txref_sources\tcall_sites\tpreceding_instructions\n");
            for (Map.Entry<Long, TreeSet<Long>> candidate : callSitesByTarget.entrySet()) {
                Address target = toAddr(candidate.getKey());
                MemoryBlock targetBlock = currentProgram.getMemory().getBlock(target);
                Instruction targetInstruction = currentProgram.getListing().getInstructionAt(target);
                if (targetInstruction == null && targetBlock != null && targetBlock.isInitialized() && targetBlock.isExecute()) {
                    DisassembleCommand targetCommand = new DisassembleCommand(target, new AddressSet(target, target.add(3)), false);
                    targetCommand.applyTo(currentProgram, monitor);
                    targetInstruction = currentProgram.getListing().getInstructionAt(target);
                }
                Function targetOwner = currentProgram.getFunctionManager().getFunctionContaining(target);
                String targetText = targetInstruction == null ? "" :
                    targetInstruction.toString().replace("\t", " ").replace("\n", " ");
                String targetBytes = "";
                if (targetInstruction != null) {
                    StringBuilder hex = new StringBuilder();
                    for (byte value : targetInstruction.getBytes()) hex.append(String.format("%02x", value & 0xff));
                    targetBytes = hex.toString();
                }
                ghidra.program.model.symbol.ReferenceIterator refs =
                    currentProgram.getReferenceManager().getReferencesTo(target);
                TreeSet<String> refSources = new TreeSet<>();
                int refCount = 0;
                while (refs.hasNext()) {
                    ghidra.program.model.symbol.Reference ref = refs.next();
                    refSources.add(ref.getFromAddress().toString());
                    refCount++;
                }
                StringBuilder callSites = new StringBuilder();
                for (Long site : candidate.getValue()) {
                    if (callSites.length() > 0) callSites.append(",");
                    callSites.append(String.format("0x%x", site));
                }
                StringBuilder preceding = new StringBuilder();
                Instruction prev = currentProgram.getListing().getInstructionBefore(target);
                int prevCount = 0;
                while (prev != null && prevCount < 4) {
                    if (preceding.length() > 0) preceding.insert(0, " || ");
                    preceding.insert(0, prev.getAddress() + ": " +
                        prev.toString().replace("\t", " ").replace("\n", " "));
                    prev = currentProgram.getListing().getInstructionBefore(prev.getAddress());
                    prevCount++;
                }
                StringBuilder refList = new StringBuilder();
                for (String refSource : refSources) {
                    if (refList.length() > 0) refList.append(",");
                    refList.append(refSource);
                }
                candidates.append(String.format("0x%x\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%s\t%d\t%s\t%s\t%s\n",
                    candidate.getKey(), methodEntries.contains(candidate.getKey()),
                    targetBlock == null ? "" : targetBlock.getName(),
                    targetBlock != null && targetBlock.isInitialized(),
                    targetBlock != null && targetBlock.isExecute(),
                    targetInstruction != null, targetText, targetBytes,
                    targetOwner == null ? "" : targetOwner.getEntryPoint().toString(),
                    targetOwner == null ? "" : targetOwner.getName(), refCount,
                    refList.toString(), callSites.toString(), preceding.toString()));
            }
            Path candidatePath = metaPath.resolveSibling(metaPath.getFileName().toString() + ".call-target-candidates.tsv");
            Files.writeString(candidatePath, candidates.toString(), StandardCharsets.UTF_8);
        }

        Files.createDirectories(codePath.getParent());
        Files.createDirectories(metaPath.getParent());
        Files.writeString(codePath, pseudocode, StandardCharsets.UTF_8);
        Path pcodePath = metaPath.resolveSibling(metaPath.getFileName().toString() + ".pcode-provenance.tsv");
        Files.writeString(pcodePath, pcodeProvenance.toString(), StandardCharsets.UTF_8);
        StringBuilder skipped = new StringBuilder();
        for (Long value : skippedMethodEntries) skipped.append(String.format("0x%x%n", value));
        Path skippedPath = metaPath.resolveSibling(metaPath.getFileName().toString() + ".skipped-method-entries.txt");
        Files.writeString(skippedPath, skipped.toString(), StandardCharsets.UTF_8);

        Set<Long> missingFromDecompiler = new HashSet<>(decodedInstructionStarts);
        missingFromDecompiler.removeAll(decompilerInstructionAddresses);
        Set<Long> outsideDecodedSet = new HashSet<>(decompilerInstructionAddresses);
        outsideDecodedSet.removeAll(decodedInstructionStarts);
        Set<Long> pcodeOutsideFunctionBody = new HashSet<>(decompilerInstructionAddresses);
        pcodeOutsideFunctionBody.removeAll(functionBodyInstructionStarts);
        String digest = sha256(pseudocode.getBytes(StandardCharsets.UTF_8));
        String missingSample = addressSample(missingFromDecompiler);
        String outsideSample = addressSample(outsideDecodedSet);
        String bodyMissingSample = addressSample(bodyStartsMissingFromDecoded);
        String bodyOutsideSample = addressSample(bodyStartsOutsideDecoded);
        String json = "{\n" +
            "  \"format\": \"azurearchive-flow-ghidra-v1\",\n" +
            "  \"program_name\": " + quote(currentProgram.getName()) + ",\n" +
            "  \"function_name\": " + quote(name) + ",\n" +
            "  \"entry\": " + quote(entry.toString()) + ",\n" +
            "  \"reachable_instruction_count\": " + instructionCount + ",\n" +
            "  \"decoded_instruction_count\": " + blockCount + ",\n" +
            "  \"decode_failures\": " + decodeFailures + ",\n" +
            "  \"skipped_other_method_entries\": " + skippedMethodEntries.size() + ",\n" +
            "  \"pseudocode_chars\": " + pseudocode.length() + ",\n" +
            "  \"decoded_instruction_start_count\": " + decodedInstructionStarts.size() + ",\n" +
            "  \"decompiler_pcode_instruction_address_count\": " + decompilerInstructionAddresses.size() + ",\n" +
            "  \"decoded_starts_missing_from_decompiler\": " + missingFromDecompiler.size() + ",\n" +
            "  \"decompiler_addresses_outside_decoded_set\": " + outsideDecodedSet.size() + ",\n" +
            "  \"pcode_addresses_outside_function_body\": " + pcodeOutsideFunctionBody.size() + ",\n" +
            "  \"function_body_instruction_start_count\": " + functionBodyInstructionStarts.size() + ",\n" +
            "  \"function_body_starts_missing_from_decoded\": " + bodyStartsMissingFromDecoded.size() + ",\n" +
            "  \"function_body_starts_outside_decoded\": " + bodyStartsOutsideDecoded.size() + ",\n" +
            "  \"pcode_missing_address_sample\": " + quote(missingSample) + ",\n" +
            "  \"pcode_outside_address_sample\": " + quote(outsideSample) + ",\n" +
            "  \"body_missing_address_sample\": " + quote(bodyMissingSample) + ",\n" +
            "  \"body_outside_address_sample\": " + quote(bodyOutsideSample) + ",\n" +
            "  \"pseudocode_sha256\": " + quote(digest) + ",\n" +
            "  \"pseudocode_file\": " + quote(codePath.toString()) + ",\n" +
            "  \"pcode_provenance_file\": " + quote(pcodePath.toString()) + ",\n" +
            "  \"method_entry_list\": " + quote(entriesPath.toString()) + ",\n" +
            "  \"analysis_mode\": \"bounded reachable-flow disassembly; calls not followed; other metadata method entries excluded\",\n" +
            "  \"limits\": [\"indirect jump tables may remain unresolved\", \"exception LSDA edges may be incomplete\", \"reachable blocks are not proof of full source-level CFG\"]\n" +
            "}\n";
        Files.writeString(metaPath, json, StandardCharsets.UTF_8);
        println("REA_FLOW_FUNCTION_OK " + json.replace("\n", " "));
    }

    private String addressSample(Set<Long> values) {
        TreeSet<Long> sorted = new TreeSet<>(values);
        StringBuilder out = new StringBuilder();
        int count = 0;
        for (Long value : sorted) {
            if (count++ >= 12) break;
            if (out.length() > 0) out.append(",");
            out.append(String.format("0x%x", value));
        }
        return out.toString();
    }

    private String quote(String value) {
        StringBuilder out = new StringBuilder("\"");
        for (int i = 0; i < value.length(); i++) {
            char c = value.charAt(i);
            switch (c) {
                case '"': out.append("\\\""); break;
                case '\\': out.append("\\\\"); break;
                case '\n': out.append("\\n"); break;
                case '\r': out.append("\\r"); break;
                case '\t': out.append("\\t"); break;
                default: if (c < 0x20) out.append(String.format("\\u%04x", (int)c)); else out.append(c);
            }
        }
        return out.append('"').toString();
    }

    private String sha256(byte[] bytes) throws Exception {
        byte[] digest = java.security.MessageDigest.getInstance("SHA-256").digest(bytes);
        StringBuilder out = new StringBuilder();
        for (byte value : digest) out.append(String.format("%02x", value & 0xff));
        return out.toString();
    }
}
