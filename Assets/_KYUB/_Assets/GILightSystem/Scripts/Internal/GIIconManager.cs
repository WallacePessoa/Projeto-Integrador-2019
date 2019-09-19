/*
Copyright 2019 KYUB INTERACTIVE LTDA (http://www.kyubinteractive.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace KyubEditor.GI
{
    public class GILightIconManager
    {

        public enum LabelIcon
        {
            None = -1,
            Gray,
            Blue,
            Teal,
            Green,
            Yellow,
            Orange,
            Red,
            Purple
        }

        public enum Icon
        {
            None = -1,
            CircleGray,
            CircleBlue,
            CircleTeal,
            CircleGreen,
            CircleYellow,
            CircleOrange,
            CircleRed,
            CirclePurple,
            DiamondGray,
            DiamondBlue,
            DiamondTeal,
            DiamondGreen,
            DiamondYellow,
            DiamondOrange,
            DiamondRed,
            DiamondPurple
        }

        private static GUIContent[] labelIcons;
        private static GUIContent[] largeIcons;

        public static void SetIcon(GameObject gObj, LabelIcon icon)
        {
            if (labelIcons == null)
            {
                labelIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            }

            if (icon == LabelIcon.None)
                RemoveIcon(gObj);
            else
                Internal_SetIcon(gObj, labelIcons[(int)icon].image as Texture2D);
        }

        public static void SetIcon(GameObject gObj, Icon icon)
        {
            if (largeIcons == null)
            {
                largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
            }

            if (icon == Icon.None)
                RemoveIcon(gObj);
            else
                Internal_SetIcon(gObj, largeIcons[(int)icon].image as Texture2D);
        }

        public static void SetIcon(GameObject gObj, Texture2D texture)
        {
            Internal_SetIcon(gObj, texture);
        }

        public static Texture2D SetIcon(GameObject gObj, Texture2D texture, Color tintColor)
        {
            if (texture != null)
            {
                var tintTexture = new Texture2D(texture.width, texture.height);
                var pixels = texture.GetPixels();
                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = new Color(pixels[i].r * tintColor.r, pixels[i].g * tintColor.g, pixels[i].b * tintColor.b, pixels[i].a * tintColor.a);
                }
                tintTexture.SetPixels(pixels);
                tintTexture.Apply();
                SetIcon(gObj, tintTexture);
                return tintTexture;
            }
            else
            {
                var v_texture = new Texture2D(4, 4);
                SetIcon(gObj, v_texture);
                return v_texture;
            }
        }

        public static void RemoveIcon(GameObject gObj)
        {
            Internal_SetIcon(gObj, null);
        }

        private static void Internal_SetIcon(GameObject gObj, Texture2D texture)
        {
#if UNITY_EDITOR
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            mi.Invoke(null, new object[] { gObj, texture });
#endif
        }

        private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] guiContentArray = new GUIContent[count];
#if UNITY_EDITOR
#if UNITY_5_3_OR_NEWER
            for (int index = 0; index < count; index++)
            {
                guiContentArray[index] = EditorGUIUtility.IconContent(baseName + (startIndex + index) + postFix);
            }
#else
        var t = typeof( EditorGUIUtility );
        var mi = t.GetMethod( "IconContent", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof( string ) }, null );

        for ( int index = 0; index < count; ++index ) {
            guiContentArray[index] = mi.Invoke( null, new object[] { baseName + (object)( startIndex + index ) + postFix } ) as GUIContent;
        }
#endif
#endif
            return guiContentArray;
        }
    }
}