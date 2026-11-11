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
                AddressSet one = new AddressSet(at, at);
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
            instructionCount++;

            if (existing.getFlowType().isCall()) {
                if (existing.hasFallthrough()) queue.addLast(existing.getFallThrough());
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
                if (next != null && !visited.contains(next.getOffset())) queue.addLast(next);
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
        }
        if (function == null || !function.getEntryPoint().equals(entry)) {
            throw new IllegalStateException("Function entry mismatch for " + name);
        }

        DecompInterface decompiler = new DecompInterface();
        String pseudocode;
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
            if (pseudocode == null || pseudocode.trim().isEmpty()) {
                throw new IllegalStateException("Empty pseudocode for " + name);
            }
        } finally {
            decompiler.dispose();
        }

        int blockCount = 0;
        InstructionIterator it = currentProgram.getListing().getInstructions(decoded, true);
        while (it.hasNext()) { it.next(); blockCount++; }

        Files.createDirectories(codePath.getParent());
        Files.createDirectories(metaPath.getParent());
        Files.writeString(codePath, pseudocode, StandardCharsets.UTF_8);
        StringBuilder skipped = new StringBuilder();
        for (Long value : skippedMethodEntries) skipped.append(String.format("0x%x%n", value));
        Path skippedPath = metaPath.resolveSibling(metaPath.getFileName().toString() + ".skipped-method-entries.txt");
        Files.writeString(skippedPath, skipped.toString(), StandardCharsets.UTF_8);

        String digest = sha256(pseudocode.getBytes(StandardCharsets.UTF_8));
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
            "  \"pseudocode_sha256\": " + quote(digest) + ",\n" +
            "  \"pseudocode_file\": " + quote(codePath.toString()) + ",\n" +
            "  \"method_entry_list\": " + quote(entriesPath.toString()) + ",\n" +
            "  \"analysis_mode\": \"bounded reachable-flow disassembly; calls not followed; other metadata method entries excluded\",\n" +
            "  \"limits\": [\"indirect jump tables may remain unresolved\", \"exception LSDA edges may be incomplete\", \"reachable blocks are not proof of full source-level CFG\"]\n" +
            "}\n";
        Files.writeString(metaPath, json, StandardCharsets.UTF_8);
        println("REA_FLOW_FUNCTION_OK " + json.replace("\n", " "));
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
