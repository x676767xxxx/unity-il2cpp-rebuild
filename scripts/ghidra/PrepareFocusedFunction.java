// Focused, bounded Ghidra preparation for one IL2CPP method.
// The caller imports the ELF with -noanalysis; this script decodes only the
// metadata-bounded method interval, creates one function from those decoded
// instructions, and obtains pseudocode. It avoids whole-program auto-analysis.

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
        if (args.length != 5) {
            throw new IllegalArgumentException(
                "Expected: <entry> <end-exclusive> <function-name> <pseudocode-path> <metadata-json-path>");
        }

        Address entry = toAddr(Long.decode(args[0]));
        Address endExclusive = toAddr(Long.decode(args[1]));
        String functionName = args[2];
        Path pseudocodePath = Paths.get(args[3]).toAbsolutePath();
        Path metadataPath = Paths.get(args[4]).toAbsolutePath();

        if (entry.compareTo(endExclusive) >= 0) {
            throw new IllegalArgumentException("Invalid empty/reversed target range");
        }
        Address last = endExclusive.subtract(1);
        MemoryBlock block = currentProgram.getMemory().getBlock(entry);
        if (block == null || !block.isInitialized() || !block.isExecute()) {
            throw new IllegalStateException("Target entry is not in initialized executable memory: " + entry);
        }
        if (!block.contains(last)) {
            throw new IllegalStateException("Metadata-bounded target range crosses a memory block: " + entry + ".." + last);
        }

        AddressSet restricted = new AddressSet(entry, last);
        DisassembleCommand disassemble = new DisassembleCommand(entry, restricted, true);
        if (!disassemble.applyTo(currentProgram, monitor)) {
            throw new IllegalStateException("Bounded disassembly failed: " + disassemble.getStatusMsg());
        }
        AddressSet decoded = disassemble.getDisassembledAddressSet();
        if (decoded == null || decoded.isEmpty() || !decoded.contains(entry)) {
            throw new IllegalStateException("No instruction was decoded at target entry: " + entry);
        }

        int instructionCount = 0;
        InstructionIterator iterator = currentProgram.getListing().getInstructions(decoded, true);
        while (iterator.hasNext()) {
            Instruction instruction = iterator.next();
            if (instruction.getAddress().compareTo(last) > 0) {
                throw new IllegalStateException("Disassembler escaped the metadata-bounded range");
            }
            instructionCount++;
        }
        if (instructionCount == 0) {
            throw new IllegalStateException("Target range produced no instructions");
        }

        Function function = currentProgram.getFunctionManager().getFunctionAt(entry);
        if (function == null) {
            CreateFunctionCmd create = new CreateFunctionCmd(
                functionName, entry, decoded, SourceType.ANALYSIS);
            if (!create.applyTo(currentProgram, monitor)) {
                throw new IllegalStateException("Function creation failed: " + create.getStatusMsg());
            }
            function = create.getFunction();
        }
        if (function == null || !function.getEntryPoint().equals(entry)) {
            throw new IllegalStateException("Ghidra did not create the requested function entry");
        }
        if (function.getBody().getMinAddress().compareTo(entry) < 0 ||
            function.getBody().getMaxAddress().compareTo(last) > 0) {
            throw new IllegalStateException(
                "Explicit function body escaped metadata interval: " +
                function.getBody().getMinAddress() + ".." + function.getBody().getMaxAddress());
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
            "  \"metadata_end_exclusive\": " + quote(endExclusive.toString()) + ",\n" +
            "  \"function_body_min\": " + quote(function.getBody().getMinAddress().toString()) + ",\n" +
            "  \"function_body_max\": " + quote(function.getBody().getMaxAddress().toString()) + ",\n" +
            "  \"instruction_count\": " + instructionCount + ",\n" +
            "  \"pseudocode_chars\": " + pseudocode.length() + ",\n" +
            "  \"pseudocode_sha256\": " + quote(digest) + ",\n" +
            "  \"pseudocode_file\": " + quote(pseudocodePath.toString()) + ",\n" +
            "  \"analysis_mode\": \"noanalysis plus explicit metadata-bounded function body\",\n" +
            "  \"limitation\": \"Decompiler output may reference out-of-line branch blocks beyond the explicitly seeded interval; validate those blocks and method boundaries separately.\"\n" +
            "}\n";
        Files.writeString(metadataPath, json, StandardCharsets.UTF_8);

        println("REA_FOCUSED_FUNCTION_OK " + json.replace("\n", " "));
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
