// Focused Ghidra preparation for one IL2CPP method.
// Import is performed with -noanalysis. Only flow reachable from the selected
// entry is disassembled inside a bounded executable window; calls are not
// turned into whole-program auto-analysis. Ghidra then computes the function
// body from the decoded control flow, including out-of-line basic blocks.

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
import java.security.MessageDigest;

public class PrepareFocusedFunction extends GhidraScript {
    @Override
    protected void run() throws Exception {
        String[] args = getScriptArgs();
        if (args.length != 6) {
            throw new IllegalArgumentException(
                "Expected: <entry> <next-method-entry> <max-range-bytes> <function-name> <pseudocode-path> <metadata-json-path>");
        }

        Address entry = toAddr(Long.decode(args[0]));
        Address nextMethodEntry = toAddr(Long.decode(args[1]));
        long maxRangeBytes = Long.decode(args[2]);
        String functionName = args[3];
        Path pseudocodePath = Paths.get(args[4]).toAbsolutePath();
        Path metadataPath = Paths.get(args[5]).toAbsolutePath();

        if (maxRangeBytes < 4 || maxRangeBytes > 0x1000000L) {
            throw new IllegalArgumentException("max-range-bytes must be between 4 and 16 MiB");
        }
        if (entry.equals(nextMethodEntry)) {
            throw new IllegalArgumentException("The next method entry must differ from the target entry");
        }

        MemoryBlock block = currentProgram.getMemory().getBlock(entry);
        if (block == null || !block.isInitialized() || !block.isExecute()) {
            throw new IllegalStateException("Target entry is not in initialized executable memory: " + entry);
        }
        if (currentProgram.getMemory().getBlock(nextMethodEntry) != block) {
            throw new IllegalStateException("Next method entry is not in the same executable memory block");
        }

        Address rangeEnd = entry.add(maxRangeBytes - 1);
        if (rangeEnd.compareTo(block.getEnd()) > 0) {
            rangeEnd = block.getEnd();
        }
        if (rangeEnd.compareTo(nextMethodEntry) < 0) {
            throw new IllegalStateException("Disassembly range does not reach the next-method guard");
        }

        AddressSet restricted = new AddressSet(entry, rangeEnd);
        DisassembleCommand disassemble = new DisassembleCommand(entry, restricted, true);
        if (!disassemble.applyTo(currentProgram, monitor)) {
            throw new IllegalStateException("Bounded flow disassembly failed: " + disassemble.getStatusMsg());
        }
        AddressSet decoded = disassemble.getDisassembledAddressSet();
        if (decoded == null || decoded.isEmpty() || !decoded.contains(entry)) {
            throw new IllegalStateException("No instruction was decoded at target entry: " + entry);
        }

        int decodedInstructionCount = countInstructions(decoded);
        if (decodedInstructionCount == 0) {
            throw new IllegalStateException("Bounded flow disassembly produced no instructions");
        }

        Function function = currentProgram.getFunctionManager().getFunctionAt(entry);
        if (function == null) {
            CreateFunctionCmd create = new CreateFunctionCmd(entry);
            if (!create.applyTo(currentProgram, monitor)) {
                throw new IllegalStateException("Function creation failed: " + create.getStatusMsg());
            }
            function = create.getFunction();
        }
        if (function == null || !function.getEntryPoint().equals(entry)) {
            throw new IllegalStateException("Ghidra did not create the requested function entry");
        }
        function.setName(functionName, SourceType.ANALYSIS);

        if (function.getBody().getMinAddress().compareTo(entry) < 0 ||
            function.getBody().getMaxAddress().compareTo(rangeEnd) > 0) {
            throw new IllegalStateException(
                "Computed function body escaped the bounded disassembly window: " +
                function.getBody().getMinAddress() + ".." + function.getBody().getMaxAddress());
        }
        if (function.getBody().contains(nextMethodEntry)) {
            throw new IllegalStateException(
                "Computed function body overlaps the next metadata method entry: " + nextMethodEntry);
        }

        int bodyInstructionCount = countInstructions(function.getBody());
        if (bodyInstructionCount == 0) {
            throw new IllegalStateException("Computed function body contains no instructions");
        }

        DecompInterface decompiler = new DecompInterface();
        String pseudocode;
        try {
            if (!decompiler.openProgram(currentProgram)) {
                throw new IllegalStateException("Ghidra DecompInterface could not open the imported program");
            }
            DecompileResults result = decompiler.decompileFunction(function, 120, monitor);
            if (result == null || !result.decompileCompleted() || result.getDecompiledFunction() == null) {
                String reason = result == null ? "no result" : result.getErrorMessage();
                throw new IllegalStateException("Focused Ghidra decompilation failed: " + reason);
            }
            pseudocode = result.getDecompiledFunction().getC();
            if (pseudocode == null || pseudocode.trim().isEmpty()) {
                throw new IllegalStateException("Ghidra returned empty pseudocode");
            }
        }
        finally {
            decompiler.dispose();
        }

        Files.createDirectories(pseudocodePath.getParent());
        Files.createDirectories(metadataPath.getParent());
        Files.writeString(pseudocodePath, pseudocode, StandardCharsets.UTF_8);
        String digest = sha256(pseudocode.getBytes(StandardCharsets.UTF_8));
        String json =
            "{\n" +
            "  \"format\": \"azurearchive-focused-ghidra-v1\",\n" +
            "  \"program_name\": " + quote(currentProgram.getName()) + ",\n" +
            "  \"function_name\": " + quote(functionName) + ",\n" +
            "  \"entry\": " + quote(entry.toString()) + ",\n" +
            "  \"next_method_entry_guard\": " + quote(nextMethodEntry.toString()) + ",\n" +
            "  \"disassembly_range_end\": " + quote(rangeEnd.toString()) + ",\n" +
            "  \"function_body_min\": " + quote(function.getBody().getMinAddress().toString()) + ",\n" +
            "  \"function_body_max\": " + quote(function.getBody().getMaxAddress().toString()) + ",\n" +
            "  \"decoded_instruction_count\": " + decodedInstructionCount + ",\n" +
            "  \"body_instruction_count\": " + bodyInstructionCount + ",\n" +
            "  \"pseudocode_chars\": " + pseudocode.length() + ",\n" +
            "  \"pseudocode_sha256\": " + quote(digest) + ",\n" +
            "  \"pseudocode_file\": " + quote(pseudocodePath.toString()) + ",\n" +
            "  \"analysis_mode\": \"noanalysis plus bounded flow disassembly and Ghidra-computed function body\"\n" +
            "}\n";
        Files.writeString(metadataPath, json, StandardCharsets.UTF_8);

        println("REA_FOCUSED_FUNCTION_OK " + json.replace("\n", " "));
    }

    private int countInstructions(ghidra.program.model.address.AddressSetView addresses) {
        int count = 0;
        InstructionIterator iterator = currentProgram.getListing().getInstructions(addresses, true);
        while (iterator.hasNext()) {
            Instruction instruction = iterator.next();
            count++;
        }
        return count;
    }

    private String quote(String value) {
        StringBuilder out = new StringBuilder();
        out.append('"');
        for (int i = 0; i < value.length(); i++) {
            char c = value.charAt(i);
            switch (c) {
                case '"': out.append("\\\""); break;
                case '\\': out.append("\\\\"); break;
                case '\n': out.append("\\n"); break;
                case '\r': out.append("\\r"); break;
                case '\t': out.append("\\t"); break;
                default:
                    if (c < 0x20) out.append(String.format("\\u%04x", (int)c));
                    else out.append(c);
            }
        }
        out.append('"');
        return out.toString();
    }

    private String sha256(byte[] bytes) throws Exception {
        byte[] digest = MessageDigest.getInstance("SHA-256").digest(bytes);
        StringBuilder out = new StringBuilder();
        for (byte value : digest) out.append(String.format("%02x", value & 0xff));
        return out.toString();
    }
}
