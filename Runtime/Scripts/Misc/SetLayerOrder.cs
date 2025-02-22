using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Morkilian.Helper
{    
    public class SetLayerOrder : MultiObjectsEditing
    {
        [Tooltip("Leave empty for default. Other examples are \"UI\"")]
        public string m_sortingLayerName = "";
        [Tooltip("Leave empty to set on default")]
        public int m_orderInLayer = 0;
        [Tooltip("Do NOT modify this unless you know what you're doing.")]
        public int m_renderingLayerMask = 1;
        [Tooltip("If empty, it will apply the settings to the first child renderer.")]
        public Renderer m_rendererOverride;

        private void Awake()
        {
            if (m_rendererOverride == null)
            {
                m_rendererOverride = GetComponentInChildren<Renderer>();
            }
            m_rendererOverride.sortingOrder = m_orderInLayer;
            m_rendererOverride.sortingLayerName = m_sortingLayerName;
            m_rendererOverride.renderingLayerMask = (uint)m_renderingLayerMask;
        }
        [ContextMenu("Force Update")]
        public void ForceUpdate()
        {
            Awake();
        }
    } 
}
