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
    }

}