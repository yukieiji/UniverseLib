#if CPP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniverseLib.Config;
using UniverseLib.Utility;
#if INTEROP
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine.Experimental.Rendering;

#else
using UnhollowerBaseLib;
#endif

namespace UniverseLib.Runtime.Il2Cpp
{
    internal class Il2CppTextureHelper : TextureHelper
    {
        internal delegate IntPtr d_EncodeToPNG(IntPtr tex);

        internal delegate void d_Blit2(IntPtr source, IntPtr dest);

        internal delegate IntPtr d_CreateSprite(IntPtr texture, ref Rect rect, ref Vector2 pivot, float pixelsPerUnit,
            uint extrude, int meshType, ref Vector4 border, bool generateFallbackPhysicsShape);

        internal delegate void d_CopyTexture_Region(IntPtr src, int srcElement, int srcMip, int srcX, int srcY,
            int srcWidth, int srcHeight, IntPtr dst, int dstElement, int dstMip, int dstX, int dstY);
        
        protected internal override Texture2D Internal_NewTexture2D(int width, int height)
        {
            return new(width, height, TextureFormat.RGBA32, false);
        }

        protected internal override Texture2D Internal_NewTexture2D(int width, int height, TextureFormat textureFormat, bool mipChain)
        {
            return new(width, height, textureFormat, mipChain);
        }

        protected internal override void Internal_Blit(Texture tex, RenderTexture rt)
        {
            if (ConfigManager.Bypass_UniverseLib_ICall)
            {
                Graphics.Blit(tex, rt);
            }
            else
            {
                ICallManager.GetICall<d_Blit2>("UnityEngine.Graphics::Blit2")
                    .Invoke(tex.ToIl2CppPointer(), rt.ToIl2CppPointer());
            }
        }

        protected internal override byte[] Internal_EncodeToPNG(Texture2D tex)
        {
            IntPtr arrayPtr = ICallManager.GetICall<d_EncodeToPNG>("UnityEngine.ImageConversion::EncodeToPNG")
                .Invoke(tex.ToIl2CppPointer());

            return arrayPtr == IntPtr.Zero ? null : new Il2CppStructArray<byte>(arrayPtr);
        }

        protected internal override Sprite Internal_CreateSprite(Texture2D texture)
        {
            var rect = new Rect(0, 0, texture.width, texture.height);
            return CreateSpriteImpl(texture, rect, Vector2.zero, 100f, 0u, SpriteMeshType.Tight, Vector4.zero, false);
        }

        protected internal override Sprite Internal_CreateSprite(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, Vector4 border)
        {
            return CreateSpriteImpl(texture, rect, pivot, pixelsPerUnit, extrude, SpriteMeshType.Tight, border, false);
        }

        internal static Sprite CreateSpriteImpl(Texture2D texture, 
            Rect rect, 
            Vector2 pivot, 
            float pixelsPerUnit,
            uint extrude, 
            SpriteMeshType meshtype,
            Vector4 border,
            bool generateFallbackPhysicsShape)
        {
            try
            {
                Sprite sprite = Sprite.Create(
                    texture, rect, pivot, pixelsPerUnit, extrude,
                    meshtype, border, generateFallbackPhysicsShape);
                if (sprite != null)
                    return sprite;
            }
            catch (Exception ex)
            {
                Universe.LogWarning(ex);
            }
            
            if (ConfigManager.Bypass_UniverseLib_ICall)
                return null;
            
            d_CreateSprite icall = ICallManager.GetICall<d_CreateSprite>("UnityEngine.Sprite::CreateSprite_Injected");
            if (icall == null)
                return null;
            
            IntPtr spritePtr = icall.Invoke(texture.ToIl2CppPointer(), ref rect, ref pivot, pixelsPerUnit, extrude, 1, ref border, false);
            return spritePtr == IntPtr.Zero ? null : new Sprite(spritePtr);
        }

        internal override bool Internal_CanForceReadCubemaps => true;

        internal override Texture Internal_CopyTexture(Texture src, int srcElement, int srcMip, int srcX, int srcY,
            int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY)
        {
            if (ConfigManager.Bypass_UniverseLib_ICall)
            {
                Graphics.CopyTexture(
                    src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight,
                    dst, dstElement, dstMip, dstX, dstY);
                return dst;
            }
            else
            {
                ICallManager.GetICall<d_CopyTexture_Region>("UnityEngine.Graphics::CopyTexture_Region")
                    .Invoke(src.ToIl2CppPointer(), srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, dst.ToIl2CppPointer(), dstElement, dstMip, dstX, dstY);
            }
            return dst;
        }
    }
}
#endif
