using System;
using System.IO;
using AzureArchive.BackgroundCompat;

internal static class Program
{
    private static int Main()
    {
        string root = Path.Combine(Path.GetTempPath(), "azurearchive-background-tests", "resources");
        string expected = Path.GetFullPath(Path.Combine(root, "images", "scene.png"));

        AssertEqual(expected, SafeResourcePath.ResolveUnderRoot(root, "images/scene.png"), "nested path");
        AssertEqual(expected, SafeResourcePath.ResolveUnderRoot(root, @"images\scene.png"), "Windows separators");

        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot("", "a.png"), "empty root");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, " "), "empty relative");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "../outside.png"), "parent traversal");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, @"images\..\..\outside.png"), "mixed traversal");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, Path.GetFullPath("outside.png")), "absolute path");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, @"C:\outside.png"), "drive-qualified path");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/./scene.png"), "dot segment");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/file:stream.png"), "alternate data stream");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/trailing./scene.png"), "trailing dot segment");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/\0scene.png"), "null character");
        AssertThrows<InvalidDataException>(() => SafeResourcePath.ResolveUnderRoot(root, "images/\nscene.png"), "control character");

        var local = new[] { "local-other", "local-match", "local-second-match" };
        var global = new[] { "global-match" };
        AssertTrue(BackgroundOverrideResolver.TryResolve(local, global, x => x.EndsWith("match", StringComparison.Ordinal), out string selected), "local override found");
        AssertEqual("local-match", selected, "local override precedes global");
        AssertTrue(BackgroundOverrideResolver.TryResolve(Array.Empty<string>(), global, x => x == "global-match", out selected), "global fallback");
        AssertEqual("global-match", selected, "global override selected");
        AssertFalse(BackgroundOverrideResolver.TryResolve<string>(null, null, x => true, out selected), "null lists have no result");
        AssertFalse(BackgroundOverrideResolver.TryResolve(local, global, x => x == "missing", out selected), "no matching override");
        AssertThrows<ArgumentNullException>(() => BackgroundOverrideResolver.TryResolve(local, global, null!, out selected), "null predicate");

        Console.WriteLine("PASS: 13 SafeResourcePath checks; 5 BackgroundOverrideResolver checks");
        return 0;
    }

    private static void AssertTrue(bool value, string name)
    {
        if (!value) throw new Exception(name);
    }

    private static void AssertFalse(bool value, string name)
    {
        if (value) throw new Exception(name);
    }

    private static void AssertEqual(string expected, string actual, string name)
    {
        if (!string.Equals(expected, actual, StringComparison.Ordinal))
            throw new Exception(name + ": expected '" + expected + "', got '" + actual + "'");
    }

    private static void AssertThrows<T>(Action action, string name) where T : Exception
    {
        try
        {
            action();
        }
        catch (T)
        {
            return;
        }

        throw new Exception(name + ": expected " + typeof(T).Name);
    }
}
