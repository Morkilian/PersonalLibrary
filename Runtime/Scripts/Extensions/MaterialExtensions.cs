namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class MaterialExtensions
    {
        public static void SetFloat(this Material[] entity, string name, float value)
        {
            for (int i = 0; i < entity.Length; i++)
            {
                entity[i].SetFloat(name, value);
            }
        }

        public static void SetVector(this Material[] entity, string name, Vector4 value)
        {
            for (int i = 0; i < entity.Length; i++)
            {
                entity[i].SetVector(name, value);
            }
        }

        public static void SetTexture(this Material[] entity, string name, Texture value)
        {
            for (int i = 0; i < entity.Length; i++)
            {
                entity[i].SetTexture(name, value);
            }
        }
        /// <summary>
        /// Sets the float property "_Alpha" value, usually a saturated value
        /// </summary>
        public static void SetAlpha(this Material mat, float value)
        {
            mat.SetFloat("_Alpha", value);
        }
        /// <summary>
        /// Sets the float property "_Value" value, usually a saturated  value
        /// </summary>
        public static void SetValue(this Material mat, float value)
        {
            mat.SetFloat("_Value", value);
        }

        #region BALIO PBR / BALIO CHARACTERS
        public const string SHADER_ALPHATESTON_NAME = "_AlphaClip"; //Float proprtty for the alpha test (inspector)
        public const string SHADER_ALPHATESTON_KEYWORD = "_ALPHATEST_ON"; //Keyword for the alpha test

        /// <summary>
        /// Sets the blinking value of the PBR Balio material, for interactable items.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="value"></param>
        public static void SetBlinkValue(this Material mat, float value)
        {
            mat.SetFloat("_Blink", value);
        }

        public static float GetBlinkValue(this Material mat)
        {
            return mat.GetFloat("_Blink");
        }
        public static void SetHighlightValue(this Material mat, bool enabled) => mat.SetFloat("_Highlight", enabled ? 1 : 0);
        public static bool IsHighlighted(this Material mat) => mat.GetFloat("_Highlight") == 1f;

        #region Base Color
        public static Color GetBaseColor(this Material mat) => mat.GetColor("_BaseColor");
        public static void SetBaseColor(this Material mat, Color color) => mat.SetColor("_BaseColor", color);
        #endregion

        public static void EnableAlphaClip(this Material mat, bool value)
        {
            if (value)
            {
                mat.EnableKeyword(SHADER_ALPHATESTON_KEYWORD);
            }
            else
            {
                mat.DisableKeyword(SHADER_ALPHATESTON_KEYWORD);
            }
            mat.SetFloat(SHADER_ALPHATESTON_NAME, value == false ? 0f : 1f);
        }
        public static bool IsAlphaClipToggled(this Material mat)
        {
            return mat.IsKeywordEnabled(SHADER_ALPHATESTON_KEYWORD);
        }
        #endregion
    }

}