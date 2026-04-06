using System.Runtime.InteropServices;

namespace UniverseLib
{
    // New struct needed for Unity 6 function calls.
    [StructLayout(LayoutKind.Sequential)]
    public struct ManagedSpanWrapper
    {
        public unsafe void* begin;
        public int length;
    }
}