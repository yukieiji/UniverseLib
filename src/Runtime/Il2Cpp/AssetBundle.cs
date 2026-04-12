#if CPP
using System;
using System.Runtime.InteropServices;
using UniverseLib.Runtime.Il2Cpp;
using UniverseLib.Utility;
#if INTEROP
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using IL2CPPType = Il2CppInterop.Runtime.Il2CppType;
#else
using UnhollowerRuntimeLib;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Attributes;
using IL2CPPType = UnhollowerRuntimeLib.Il2CppType;
#endif

namespace UniverseLib
{
    /// <summary>
    /// Replacement class for AssetBundles in case they were stripped by the game.
    /// </summary>
    public class AssetBundle : UnityEngine.Object
    {
        static AssetBundle()
        {
            ClassInjector.RegisterTypeInIl2Cpp<AssetBundle>();
        }

        // ~~~~~~~~~~~~ Static ~~~~~~~~~~~~

        // AssetBundle.LoadFromFile(string path)

        private delegate IntPtr d_LoadFromFile(IntPtr path, uint crc, ulong offset);
        private delegate IntPtr d_LoadFromFile_Injected(ref ManagedSpanWrapper path, uint crc, ulong offset);

        [HideFromIl2Cpp]
        public static AssetBundle LoadFromFile(string path) => LoadFromFile(path, 0u, 0UL);
        [HideFromIl2Cpp]
        public static AssetBundle LoadFromFile(string path, uint crc) => LoadFromFile(path, crc, 0UL);
        [HideFromIl2Cpp]
        public static AssetBundle LoadFromFile(string path, uint crc, ulong offset)
        {
            IntPtr ptr = IntPtr.Zero;
            d_LoadFromFile_Injected icall_Injected = ICallManager.GetICallInternal<d_LoadFromFile_Injected>(
                "UnityEngine.AssetBundle::LoadFromFile_Internal_Injected");
            if (icall_Injected != null)
            {
                IntPtr gcHandle = ManagedSpanWrapper.Invoke(path, span => icall_Injected(ref span, crc, offset));
                ptr = ((gcHandle != IntPtr.Zero) ? Il2CppProvider.GCHandleGetTarget(gcHandle) : IntPtr.Zero);
            }
            else
            {
                ptr = ICallManager.GetICallUnreliableInternal<d_LoadFromFile>(
                        "UnityEngine.AssetBundle::LoadFromFile_Internal",
                        "UnityEngine.AssetBundle::LoadFromFile")
                    ?.Invoke(IL2CPP.ManagedStringToIl2Cpp(path), crc, offset) ?? IntPtr.Zero;
            }

            return ptr != IntPtr.Zero ? new AssetBundle(ptr) : null;
        }

        [HideFromIl2Cpp]
        public static AssetBundle LoadFromMemory(Il2CppStructArray<byte> binary) => LoadFromMemory(binary, 0u);
        [HideFromIl2Cpp]
        public static AssetBundle LoadFromMemory(Il2CppStructArray<byte> binary, uint crc)
        {
            Il2CppSystem.IO.MemoryStream il2CppStream = new();
            il2CppStream.Write(binary, 0, binary.Length);
            il2CppStream.Flush();
            return LoadFromStream(il2CppStream);
        }
        
        private delegate void d_ValidateLoadFromStream(IntPtr stream);
        private delegate IntPtr d_LoadFromStream(IntPtr stream, uint crc, uint managedReadBufferSize);
        
        [HideFromIl2Cpp]
        public static AssetBundle LoadFromStream(Il2CppSystem.IO.Stream stream) => LoadFromStream(stream, 0u, 0u);
        [HideFromIl2Cpp]
        public static AssetBundle LoadFromStream(Il2CppSystem.IO.Stream stream, uint crc) => LoadFromStream(stream, crc, 0u);
        [HideFromIl2Cpp]
        public static AssetBundle LoadFromStream(Il2CppSystem.IO.Stream stream, uint crc, uint managedReadBufferSize)
        {
            ICallManager.GetICallInternal<d_ValidateLoadFromStream>(
                "UnityEngine.AssetBundle::ValidateLoadFromStream"
            )?.Invoke(stream.ToIl2CppPointer());

            IntPtr ptr = ICallManager.GetICallUnreliableInternal<d_LoadFromStream>(
                    "UnityEngine.AssetBundle::LoadFromStreamInternal",
                    "UnityEngine.AssetBundle::LoadFromStream")
                ?.Invoke(stream.ToIl2CppPointer(), crc, 0) ?? IntPtr.Zero;

            return ptr != IntPtr.Zero ? new AssetBundle(ptr) : null;
        }

        // AssetBundle.GetAllLoadedAssetBundles()

        public delegate IntPtr d_GetAllLoadedAssetBundles_Native();

        [HideFromIl2Cpp]
        public static AssetBundle[] GetAllLoadedAssetBundles()
        {
            IntPtr ptr = ICallManager.GetICall<d_GetAllLoadedAssetBundles_Native>("UnityEngine.AssetBundle::GetAllLoadedAssetBundles_Native")
                .Invoke();

            return ptr != IntPtr.Zero ? (AssetBundle[])new Il2CppReferenceArray<AssetBundle>(ptr) : null;
        }

        // ~~~~~~~~~~~~ Instance ~~~~~~~~~~~~

        public readonly IntPtr m_bundlePtr = IntPtr.Zero;
        public readonly IntPtr m_bundlePtr_Injected = IntPtr.Zero;

        public AssetBundle(IntPtr ptr) : base(ptr)
        {
            m_bundlePtr = ptr;
            m_bundlePtr_Injected = Marshal.ReadIntPtr(m_bundlePtr + 0x10); // Skip the Il2CppObject header + read the REAL object ptr from there.
        }

        // LoadAllAssets()

        private delegate IntPtr d_LoadAssetWithSubAssets_Internal(IntPtr _this, IntPtr name, IntPtr type);
        private delegate IntPtr d_LoadAssetWithSubAssets_Internal_Injected(IntPtr _this, ref ManagedSpanWrapper name, IntPtr type);

        [HideFromIl2Cpp]
        public UnityEngine.Object[] LoadAllAssets()
        {
            IntPtr ptr = IntPtr.Zero;
            d_LoadAssetWithSubAssets_Internal_Injected icall_Injected = ICallManager.GetICallInternal<d_LoadAssetWithSubAssets_Internal_Injected>("UnityEngine.AssetBundle::LoadAssetWithSubAssets_Internal_Injected");
            if (icall_Injected != null)
            {
                IntPtr gcHandle = ManagedSpanWrapper.Invoke("", span => icall_Injected(m_bundlePtr_Injected, ref span, IL2CPPType.Of<UnityEngine.Object>().Pointer));
                ptr = ((gcHandle != IntPtr.Zero) ? Marshal.ReadIntPtr(gcHandle) : IntPtr.Zero);
            }
            else
            {
                ptr = ICallManager
                    .GetICallInternal<d_LoadAssetWithSubAssets_Internal>("UnityEngine.AssetBundle::LoadAssetWithSubAssets_Internal")
                    ?.Invoke(m_bundlePtr, IL2CPP.ManagedStringToIl2Cpp(""),
                        IL2CPPType.Of<UnityEngine.Object>().Pointer) ?? IntPtr.Zero;
            }

            return ptr != IntPtr.Zero ? (UnityEngine.Object[])new Il2CppReferenceArray<UnityEngine.Object>(ptr) : new UnityEngine.Object[0];
        }

        // LoadAsset<T>(string name, Type type)

        private delegate IntPtr d_LoadAsset_Internal(IntPtr _this, IntPtr name, IntPtr type);
        private delegate IntPtr d_LoadAsset_Internal_Injected(IntPtr _this, ref ManagedSpanWrapper name, IntPtr type);

        [HideFromIl2Cpp]
        public T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
            IntPtr ptr = IntPtr.Zero;
            d_LoadAsset_Internal_Injected icall_Injected = ICallManager.GetICallInternal<d_LoadAsset_Internal_Injected>("UnityEngine.AssetBundle::LoadAsset_Internal_Injected");
            if (icall_Injected != null)
            {
                IntPtr gcHandle = ManagedSpanWrapper.Invoke(name, span => icall_Injected(m_bundlePtr_Injected, ref span, IL2CPPType.Of<T>().Pointer));
                ptr = ((gcHandle != IntPtr.Zero) ? Marshal.ReadIntPtr(gcHandle) : IntPtr.Zero);
            }
            else
            {
                ptr = ICallManager.GetICallInternal<d_LoadAsset_Internal>("UnityEngine.AssetBundle::LoadAsset_Internal")
                          ?.Invoke(m_bundlePtr, IL2CPP.ManagedStringToIl2Cpp(name), IL2CPPType.Of<T>().Pointer) ??
                      IntPtr.Zero;
            }

            return ptr != IntPtr.Zero ? new UnityEngine.Object(ptr).TryCast<T>() : null;
        }

        // Unload(bool unloadAllLoadedObjects);

        private delegate void d_Unload(IntPtr _this, bool unloadAllLoadedObjects);

        [HideFromIl2Cpp]
        public void Unload(bool unloadAllLoadedObjects)
        {
            d_LoadAsset_Internal_Injected icall_Injected = ICallManager.GetICallInternal<d_LoadAsset_Internal_Injected>("UnityEngine.AssetBundle::LoadAsset_Internal_Injected");
            IntPtr bundlePtr = (icall_Injected != null) ? m_bundlePtr_Injected : m_bundlePtr;
            ICallManager.GetICall<d_Unload>("UnityEngine.AssetBundle::Unload")
                .Invoke(bundlePtr, unloadAllLoadedObjects);
        }
    }
}
#endif