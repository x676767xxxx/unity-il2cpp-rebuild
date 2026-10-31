using System;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Dispatches coalesced preview callbacks using the Android 1.0 behavior:
    /// report an individual callback exception and continue with later callbacks.
    /// The Unity-facing adapter supplies the actual error reporter.
    /// </summary>
    public static class BackgroundPreviewCallbackDispatcher
    {
        public static void Dispatch<T>(IEnumerable<Action<T>> callbacks, T value,
            Action<Exception> reportError)
        {
            if (callbacks == null) throw new ArgumentNullException(nameof(callbacks));
            if (reportError == null) throw new ArgumentNullException(nameof(reportError));

            foreach (Action<T> callback in callbacks)
            {
                try
                {
                    callback(value);
                }
                catch (Exception error)
                {
                    reportError(error);
                }
            }
        }
    }
}
