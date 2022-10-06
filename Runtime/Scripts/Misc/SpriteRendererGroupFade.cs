using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Morkilian.Helper
{
    //KMS
    /// <summary>
    /// Component that fades in or out all the sprite renderers that are children of this object.
    /// </summary>
    public class SpriteRendererGroupFade : MonoBehaviour
    {
        [System.Serializable]
        protected class TMProColors
        {
            public Color BaseColor;
            public Color OutlineColor;
            public Color UnderlayColor;
            //Add more colors as necessary
            public TMProColors(Color baseColor, Color outline, Color underlay)
            {
                BaseColor = baseColor;
                OutlineColor = outline;
                UnderlayColor = underlay;
            }
        }
        [SerializeField] protected float timeFade = 0.5f;
        [SerializeField] protected bool startHidden = false;
        [HelpBox("This material [M_SpriteUnlit] is needed for the script to work. It will be applied to every Sprite Renderer without a material. Sprite Renderers with a Material should have a property named _Alpha in order to work." +
            "\n TextMeshPro cases are internally handled." +
            "\n LineRenderers should have a material assigned."+
            "\n Transparencies will be assumed to either 0 or 1.")]
        [SerializeField] Material materialToApplyToSpriteRenderers;
        protected Material[] spriteMaterials;
        private Material[] _spriteMaterials
        {
            get
            {
                if (spriteMaterials == null)
                    ManualStart();
                return spriteMaterials;
            }
        }
        protected TMPro.TextMeshPro[] tmproTexts;
        protected MeshRenderer[] tmproRenderers;
        protected Material[] tmproMaterials;
        protected TMProColors[] tmproColors;
        protected Material[] lineMaterials;
        private bool alreadyHidden = false;
        private float currentAlpha = -1;
        private const string SPRITE_DEFAULT_MATERIAL_NAME = "Sprites-Default";

        private void Start()
        {
            if (startHidden && alreadyHidden == false)
            {
                SetAlpha(0);
                alreadyHidden = true;
            }
        }
        private void Update()
        {
            if(currentAlpha!=-1)
            for (int i = 0; i < tmproMaterials.Length; i++)
            {
                tmproRenderers[i].material = tmproMaterials[i]; //Need to reassign materials because tmpro checks and reassigns them

                tmproMaterials[i].SetColor("_FaceColor", tmproColors[i].BaseColor);
                tmproMaterials[i].SetColor("_OutlineColor", tmproColors[i].OutlineColor);
                tmproMaterials[i].SetColor("_UnderlayColor", tmproColors[i].UnderlayColor);
            }
        }
        public void ManualStart()
        {
            //sprites
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
            spriteMaterials = new Material[sprites.Length];
            for (int i = 0; i < spriteMaterials.Length; i++)
            {
                //If default material, assign ours
                if(sprites[i].sharedMaterial.name == SPRITE_DEFAULT_MATERIAL_NAME)
                    sprites[i].sharedMaterial = materialToApplyToSpriteRenderers;

                spriteMaterials[i] = sprites[i].material;
            }

            //Line renderers
            LineRenderer[] lines = GetComponentsInChildren<LineRenderer>();
            lineMaterials = new Material[lines.Length];
            for (int i = 0; i < lineMaterials.Length; i++)
            {
                if (lines[i].sharedMaterial != null)
                    lineMaterials[i] = lines[i].material;
                else //Empty slot
                    lineMaterials[i] = new Material(materialToApplyToSpriteRenderers); 
            }

            tmproTexts = GetComponentsInChildren<TMPro.TextMeshPro>();
            tmproMaterials = new Material[tmproTexts.Length];
            tmproColors = new TMProColors[tmproTexts.Length];
            tmproRenderers = new MeshRenderer[tmproTexts.Length];
            for (int i = 0; i < tmproTexts.Length; i++)
            {
                tmproRenderers[i] = tmproTexts[i].GetComponent<MeshRenderer>();
                Material sharedMat = tmproRenderers[i].material;
                tmproColors[i] = new TMProColors(sharedMat.GetColor("_FaceColor"), sharedMat.GetColor("_OutlineColor"), sharedMat.GetColor("_UnderlayColor"));
                tmproMaterials[i] = sharedMat;
                tmproRenderers[i].material = tmproMaterials[i];
            }


            if (startHidden && alreadyHidden == false)
            {
                SetAlpha(0);
                alreadyHidden = true;
            }
        }

        public void SetAlpha(float alpha)
        {
            currentAlpha = Mathf.Clamp01(alpha);
            for (int i = 0; i < _spriteMaterials.Length; i++)
            {
                spriteMaterials[i].SetFloat("_Alpha", currentAlpha);
            }
            for (int i = 0; i < tmproMaterials.Length; i++)
            {
                tmproColors[i].BaseColor.a = currentAlpha;
                tmproColors[i].OutlineColor.a = currentAlpha;
                tmproColors[i].UnderlayColor.a = currentAlpha;

                tmproRenderers[i].material = tmproMaterials[i]; //Need to reassign materials because tmpro checks and reassigns them

                tmproMaterials[i].SetColor("_FaceColor", tmproColors[i].BaseColor);
                tmproMaterials[i].SetColor("_OutlineColor", tmproColors[i].OutlineColor);
                tmproMaterials[i].SetColor("_UnderlayColor", tmproColors[i].UnderlayColor);
            }
            for (int i = 0; i < lineMaterials.Length; i++)
            {
                lineMaterials[i].SetFloat("_Alpha", currentAlpha);
            }
        }

        public void StartFade(bool fadeIn, System.Action todoAfterFade = null)
        {
            StopAllCoroutines();
            if (this.gameObject.activeSelf)
                StartCoroutine(CRFade(fadeIn, todoAfterFade));
        }

        private IEnumerator CRFade(bool fadeIn, System.Action todoAfterFade)
        {
            float elapsedTime = 0;

            //float[] baseAlphasSprites = new float[spriteMaterials.Length];
            //for (int i = 0; i < spriteMaterials.Length; i++)
            //    baseAlphasSprites[i] = spriteMaterials[i].GetFloat("_Alpha");

            //float[] baseAlphasTexts = new float[tmproMaterials.Length];
            //for (int i = 0; i < tmproMaterials.Length; i++)
            //    baseAlphasTexts[i] = tmproMaterials[i].GetColor("_FaceColor").a;

            while (elapsedTime < timeFade)
            {
                yield return null;
                elapsedTime += Time.unscaledDeltaTime;
                float progress = Mathf.Clamp01(elapsedTime / timeFade);
                if (fadeIn == false) progress = 1 - progress;
                SetAlpha(progress);
                //for (int i = 0; i < spriteMaterials.Length; i++)
                //{
                //    tempColorsSprites[i].a = Mathf.Lerp(baseAlphasSprites[i], fadeIn ? startingColorsSprites[i].a : 0, progress);
                //    sprites[i].color = tempColorsSprites[i];
                //}
                //for (int i = 0; i < _textsLength; i++)
                //{
                //    tempColorsTexts[i].a = Mathf.Lerp(baseAlphasTexts[i], fadeIn ? startingColorsTexts[i].a : 0, progress);
                //    texts[i].color = tempColorsTexts[i];
                //}
            }
            todoAfterFade?.Invoke();
        }        
    } 
}
