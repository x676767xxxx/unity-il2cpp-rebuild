using System;
using System.Collections.Generic;

namespace AzureArchive.BackgroundCompat
{
    /// <summary>
    /// Tracks Unity objects created while constructing a runtime Spine asset.
    /// The caller supplies the equality comparer appropriate to its Unity runtime.
    /// This class never destroys objects; the Unity adapter must clean up the
    /// returned snapshot on the main thread if construction aborts.
    /// </summary>
    public sealed class SpineAssetConstructionLedger<TObject> where TObject : class
    {
        private enum LedgerState
        {
            Open,
            Committed,
            Aborted
        }

        private readonly HashSet<TObject> owned;
        private LedgerState state;

        public SpineAssetConstructionLedger(IEqualityComparer<TObject>? comparer = null)
        {
            owned = new HashSet<TObject>(comparer);
            state = LedgerState.Open;
        }

        public int Count => owned.Count;
        public bool IsCompleted => state != LedgerState.Open;

        /// <summary>
        /// Registers a non-null object once. Returns false for null or an object
        /// already registered according to the supplied comparer.
        /// </summary>
        public bool Register(TObject? value)
        {
            EnsureOpen();
            return value != null && owned.Add(value);
        }

        /// <summary>
        /// Transfers the unique owned-object snapshot to the completed asset.
        /// No object is destroyed by this operation.
        /// </summary>
        public TObject[] Commit()
        {
            EnsureOpen();
            state = LedgerState.Committed;
            TObject[] snapshot = new TObject[owned.Count];
            owned.CopyTo(snapshot);
            owned.Clear();
            return snapshot;
        }

        /// <summary>
        /// Returns the partial construction snapshot for caller-managed rollback.
        /// A second abort is harmless and returns an empty snapshot.
        /// </summary>
        public TObject[] Abort()
        {
            if (state == LedgerState.Aborted)
                return Array.Empty<TObject>();
            EnsureOpen();
            state = LedgerState.Aborted;
            TObject[] snapshot = new TObject[owned.Count];
            owned.CopyTo(snapshot);
            owned.Clear();
            return snapshot;
        }

        private void EnsureOpen()
        {
            if (state != LedgerState.Open)
                throw new InvalidOperationException("The Spine asset construction ledger is already complete.");
        }
    }
}
