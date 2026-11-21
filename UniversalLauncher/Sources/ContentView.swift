import SwiftUI
import UniformTypeIdentifiers

struct ContentView: View {
    @State private var isImporting = false
    @State private var importedFiles: [ImportedFile] = []
    @State private var errorMessage: String?
    @State private var infoMessage: String?

    private let libraryKey = "UniversalLauncher.importedFiles"

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
                ToolbarItem(placement: .navigationBarTrailing) {
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
            .onAppear(perform: loadLibrary)
            .alert("Notice", isPresented: Binding(
                get: { errorMessage != nil || infoMessage != nil },
                set: { if !$0 { errorMessage = nil; infoMessage = nil } }
            )) {
                Button("OK", role: .cancel) {
                    errorMessage = nil
                    infoMessage = nil
                }
            } message: {
                Text(errorMessage ?? infoMessage ?? "")
            }
        }
    }

    private var emptyState: some View {
        VStack(spacing: 16) {
            Image(systemName: "square.stack.3d.up")
                .font(.system(size: 48))
                .foregroundStyle(.secondary)
            Text("No games or files yet")
                .font(.title3.weight(.semibold))
            Text("Import an APK or an archive. The launcher will identify the file type automatically.")
                .multilineTextAlignment(.center)
                .foregroundStyle(.secondary)
                .padding(.horizontal)
            Button {
                isImporting = true
            } label: {
                Label("Choose files", systemImage: "folder")
            }
            .buttonStyle(.borderedProminent)
        }
        .padding()
    }

    private func fileRow(_ file: ImportedFile) -> some View {
        HStack(alignment: .top, spacing: 12) {
            Image(systemName: file.kind.systemImage)
                .font(.title2)
                .frame(width: 34)
                .foregroundColor(file.kind == .apk ? .blue : .green)

            VStack(alignment: .leading, spacing: 6) {
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

                if file.kind == .zip {
                    Button {
                        extract(file)
                    } label: {
                        Label("Extract ZIP", systemImage: "archivebox")
                    }
                    .font(.caption)
                    .buttonStyle(.bordered)
                }

                if file.kind == .apk {
                    Button {
                        inspectAPK(file)
                    } label: {
                        Label("Inspect APK", systemImage: "doc.text.magnifyingglass")
                    }
                    .font(.caption)
                    .buttonStyle(.bordered)
                }
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
                    saveLibrary()
                } catch {
                    errorMessage = "\(sourceURL.lastPathComponent): \(error.localizedDescription)"
                    break
                }
            }
        }
    }

    private func loadLibrary() {
        guard let data = UserDefaults.standard.data(forKey: libraryKey),
              let saved = try? JSONDecoder().decode([ImportedFile].self, from: data) else {
            return
        }
        importedFiles = saved.filter {
            FileManager.default.fileExists(atPath: $0.storedURL.path)
        }
    }

    private func saveLibrary() {
        guard let data = try? JSONEncoder().encode(importedFiles) else { return }
        UserDefaults.standard.set(data, forKey: libraryKey)
    }

    private func extract(_ file: ImportedFile) {
        do {
            let destination = try ArchiveExtractor.extractZIP(at: file.storedURL)
            infoMessage = "ZIP extracted successfully. Folder: \(destination.lastPathComponent)"
        } catch {
            errorMessage = error.localizedDescription
        }
    }

    private func inspectAPK(_ file: ImportedFile) {
        do {
            let report = try APKInspector.inspect(file.storedURL)
            infoMessage = report.summary
        } catch {
            errorMessage = error.localizedDescription
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
