using System;
namespace UnityEngine
{
    public enum RuntimeInitializeLoadType { BeforeSceneLoad }
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RuntimeInitializeOnLoadMethodAttribute : Attribute
    {
        public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType) { }
    }
    public static class Debug
    {
        public static void LogError(object message) { }
    }
    public class Object
    {
        public bool Destroyed { get; private set; }
        public static void Destroy(Object value) { value.Destroyed = true; }
    }
    public enum TextureFormat { RGBA32 = 4 }
    public enum TextureWrapMode { Clamp = 1 }
    public sealed class Texture2D : Object
    {
        public Texture2D(int width, int height, TextureFormat format, bool mipChain)
        { Width=width; Height=height; Format=format; MipChain=mipChain; }
        public int Width { get; }
        public int Height { get; }
        public TextureFormat Format { get; }
        public bool MipChain { get; }
        public string name { get; set; } = string.Empty;
        public TextureWrapMode wrapMode { get; set; }
    }
    public static class ImageConversion
    {
        public static bool ReturnValue { get; set; } = true;
        public static bool ThrowOnLoad { get; set; }
        public static Texture2D? LastTexture { get; private set; }
        public static void Reset() { ReturnValue=true; ThrowOnLoad=false; LastTexture=null; }
        public static bool LoadImage(Texture2D texture, byte[] bytes, bool markNonReadable)
        {
            LastTexture=texture;
            if (ThrowOnLoad) throw new InvalidOperationException("synthetic decoder failure");
            return ReturnValue;
        }
    }
}


namespace UnityEngine.ResourceManagement.AsyncOperations
{
    public enum AsyncOperationStatus { None, Succeeded, Failed }

    public sealed class AsyncOperationHandle<T>
    {
        public event Action<AsyncOperationHandle<T>>? Completed;
        public AsyncOperationStatus Status { get; private set; }
        public T Result { get; private set; } = default!;
        public Exception? OperationException { get; private set; }
        public bool Released { get; set; }

        public void Complete(T result)
        {
            Result = result;
            Status = AsyncOperationStatus.Succeeded;
            Completed?.Invoke(this);
        }

        public void Fail(Exception exception)
        {
            OperationException = exception;
            Status = AsyncOperationStatus.Failed;
            Completed?.Invoke(this);
        }
    }
}

namespace UnityEngine.AddressableAssets
{
    using UnityEngine.ResourceManagement.AsyncOperations;

    public static class Addressables
    {
        public static Func<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation, string>? InternalIdTransformFunc { get; set; }
        public static object? LastOperation { get; private set; }
        public static int ReleaseCount { get; private set; }
        public static bool ThrowOnLoad { get; set; }

        public static AsyncOperationHandle<T> LoadAssetAsync<T>(object key)
        {
            if (ThrowOnLoad) throw new InvalidOperationException("synthetic Addressables start failure");
            var operation = new AsyncOperationHandle<T>();
            LastOperation = operation;
            return operation;
        }

        public static void Release<T>(AsyncOperationHandle<T> operation)
        {
            ReleaseCount++;
            operation.Released = true;
        }

        public static void Reset()
        {
            LastOperation = null;
            ReleaseCount = 0;
            ThrowOnLoad = false;
            InternalIdTransformFunc = null;
        }

        public static AsyncOperationHandle<T> Last<T>() =>
            (AsyncOperationHandle<T>)LastOperation!;
    }
}

namespace UnityEngine.ResourceManagement.ResourceLocations
{
    public interface IResourceLocation
    {
        string InternalId { get; }
    }

    public sealed class TestResourceLocation : IResourceLocation
    {
        public TestResourceLocation(string internalId) { InternalId = internalId; }
        public string InternalId { get; }
    }
}
