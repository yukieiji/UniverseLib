using System;
using System.Runtime.InteropServices;

namespace UniverseLib
{
    // New struct needed for Unity 6 function calls.
    [StructLayout(LayoutKind.Sequential)]
    public struct ManagedSpanWrapper
    {
        public unsafe void* begin;
        public int length;
        
        /// <summary>
        /// Pins a string and creates a ManagedSpanWrapper using the pinned pointer
        /// </summary>
        internal static unsafe void Pin(string str, 
            Action<ManagedSpanWrapper> act)
        {
            fixed (char* charPtr = str) // Pins the string to char*
            {
                // Create the new ManagedSpanWrapper
                var span = new ManagedSpanWrapper
                {
                    begin = charPtr,
                    length = str.Length
                };
                
                // Run Action while string is still pinned
                act(span);
            }
        }
    }
}