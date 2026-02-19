#if CPP
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
#if INTEROP
using Il2CppInterop.Runtime;
#else
using UnhollowerBaseLib;
#endif

namespace UniverseLib.Runtime.Il2Cpp
{
    /// <summary>
    /// Helper class for using Unity ICalls (internal calls).
    /// </summary>
    public static class ICallManager
    {
        // cache used by GetICall
        private static readonly Dictionary<string, Delegate> iCallCache = new();
        // cache used by GetICallUnreliable
        private static readonly Dictionary<string, Delegate> unreliableCache = new();

        /// <summary>
        /// Helper to get and cache an iCall by providing the signature (eg. "UnityEngine.Resources::FindObjectsOfTypeAll").
        /// Fixed the issue where the iCall signature parsing failed for U6000.
        /// </summary>
        /// <typeparam name="T">The Type of Delegate to provide for the iCall.</typeparam>
        /// <param name="signature">The signature of the iCall you want to get.</param>
        /// <returns>The <typeparamref name="T"/> delegate if successful.</returns>
        /// <exception cref="MissingMethodException" />
        public static T GetICall<T>(string signature) where T : Delegate
        {
            if (iCallCache.TryGetValue(signature, out var sig))
            {
                return (T)sig;
            }
            // In Unity 6000, most iCall signatures have been renamed from xxx to xxx_Injected.
            if (!(
                    TryResolveICall(signature, out var ptr) ||
                    TryResolveICall($"{signature}_Injected", out ptr)
                ))
            {
                throw new MissingMethodException($"Could not find any iCall with the signature '{signature}'!");
            }

            Delegate iCall = Marshal.GetDelegateForFunctionPointer(ptr, typeof(T));
            iCallCache.Add(signature, iCall);

            return (T)iCall;
        }

        /// <summary>
        /// Get an iCall which may be one of multiple different signatures (ie, the name changed in different Unity versions).
        /// Each possible signature must have the same Delegate type, it can only vary by name.
        /// Fixed the issue where the iCall signature parsing failed for U6000.
        /// </summary>
        public static T GetICallUnreliable<T>(params string[] possibleSignatures) where T : Delegate
        {
            // use the first possible signature as the 'key'.
            string key = possibleSignatures.First();

            if (unreliableCache.TryGetValue(key, out var signature))
            {
                return (T)signature;
            }

            var loopSig = new List<string>(possibleSignatures);
            // In Unity 6000, most iCall signatures have been renamed from xxx to xxx_Injected.
            loopSig.Concat(possibleSignatures.Select(s => $"{s}_Injected"));

            foreach (string sig in loopSig)
            {
                if (TryResolveICall(sig, out var ptr))
                {
                    var iCall = (T)Marshal.GetDelegateForFunctionPointer(ptr, typeof(T));
                    unreliableCache.Add(key, iCall);
                    return iCall;
                }
            }

            throw new MissingMethodException($"Could not find any iCall from list of provided signatures starting with '{key}'!");
        }
        /// <summary>
        /// Use out parameter modifier, redundant value retrieval can be avoided.
        /// </summary>
        private static bool TryResolveICall(string signature, out IntPtr ptr)
        {
            ptr = IL2CPP.il2cpp_resolve_icall(signature);
            return ptr != IntPtr.Zero;
        }
    }
}
#endif
