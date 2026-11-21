import SwiftUI
import UniformTypeIdentifiers

struct ContentView: View {
    @State private var isImporting = false
    @State private var importedFiles: [ImportedFile] = []
    @State private var errorMessage: String?

    var body: some View {
        NavigationStack {
            Group {
                if importedFiles.isEmpty {
                    emptyState
                } else {
                    List {
                        Section("Imported files") {
                            ForEach(importedFiles) { file in
                                fileRow(file)
                            }
                        }
                    }
                    .listStyle(.insetGrouped)
                }
            }
            .navigationTitle("Universal Launcher")
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button {
                        isImporting = true
                    } label: {
                        Label("Import", systemImage: "plus")
                    }
                }
            }
            .fileImporter(
                isPresented: $isImporting,
                allowedContentTypes: [.item],
                allowsMultipleSelection: true,
                onCompletion: handleImport
            )
            .alert("Import error", isPresented: Binding(
                get: { errorMessage != nil },
                set: { if !$0 { errorMessage = nil } }
            )) {
                Button("OK", role: .cancel) { errorMessage = nil }
            } message: {
                Text(errorMessage ?? "Unknown error")
            }
        }
    }

    private var emptyState: some View {
        ContentUnavailableView {
            Label("No games or files yet", systemImage: "square.stack.3d.up")
        } description: {
            Text("Import an APK or an archive. The launcher will identify the file type automatically.")
        } actions: {
            Button {
                isImporting = true
            } label: {
                Label("Choose files", systemImage: "folder")
            }
            .buttonStyle(.borderedProminent)
        }
    }

    private func fileRow(_ file: ImportedFile) -> some View {
        HStack(alignment: .top, spacing: 12) {
            Image(systemName: file.kind.systemImage)
                .font(.title2)
                .frame(width: 34)
                .foregroundStyle(file.kind == .apk ? .blue : .green)

            VStack(alignment: .leading, spacing: 4) {
                Text(file.name)
                    .font(.headline)
                    .lineLimit(2)
                Text("\(file.kind.title) · \(file.sizeDescription)")
                    .font(.caption)
                    .foregroundStyle(.secondary)
                Text(file.kind.detail)
                    .font(.caption2)
                    .foregroundStyle(.secondary)
                    .fixedSize(horizontal: false, vertical: true)
            }
        }
        .padding(.vertical, 4)
    }

    private func handleImport(_ result: Result<[URL], Error>) {
        switch result {
        case .failure(let error):
            errorMessage = error.localizedDescription
        case .success(let urls):
            for sourceURL in urls {
                do {
                    let imported = try copyIntoLibrary(sourceURL)
                    importedFiles.append(imported)
                } catch {
                    errorMessage = "\(sourceURL.lastPathComponent): \(error.localizedDescription)"
                    break
                }
            }
        }
    }

    private func copyIntoLibrary(_ sourceURL: URL) throws -> ImportedFile {
        let hasAccess = sourceURL.startAccessingSecurityScopedResource()
        defer {
            if hasAccess {
                sourceURL.stopAccessingSecurityScopedResource()
            }
        }

        let manager = FileManager.default
        let documents = try manager.url(
            for: .documentDirectory,
            in: .userDomainMask,
            appropriateFor: nil,
            create: true
        )
        let library = documents.appendingPathComponent("Imported", isDirectory: true)
        try manager.createDirectory(at: library, withIntermediateDirectories: true)

        let destination = library.appendingPathComponent("\(UUID().uuidString)-\(sourceURL.lastPathComponent)")
        try manager.copyItem(at: sourceURL, to: destination)

        let attributes = try manager.attributesOfItem(atPath: destination.path)
        let size = (attributes[.size] as? NSNumber)?.int64Value ?? 0

        return ImportedFile(
            id: UUID(),
            name: sourceURL.lastPathComponent,
            storedURL: destination,
            kind: FileClassifier.classify(sourceURL),
            byteCount: size
        )
    }
}
