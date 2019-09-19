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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kyub.GI
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class GILight : GIDirtyBehaviour
    {
        #region Static Properties

        protected static List<GILight> s_sceneGILights = new List<GILight>();
        public static IList<GILight> SceneGILights
        {
            get
            {
                return s_sceneGILights.AsReadOnly();
            }
        }

        #endregion

        #region Helper Enums

        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
            Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
        }

        public enum LightShapeTypeEnum { Cube = 0, Quad = 1, Sphere = 2, Custom = 3 }

        public enum LightModeEnum { Realtime, Baked }

        #endregion

        #region Private Variables

        [Header("Runtime Light Fields")]
        [SerializeField]
        bool m_isOn = true;
        [SerializeField, ColorUsage(false, false)]
        Color m_color = Color.white;
        [SerializeField]
        float m_intensity = 1;

        [Header("Non-Runtime Light Fields")]
        [SerializeField]
        LightShapeTypeEnum m_lightShapeType = LightShapeTypeEnum.Cube;
        [SerializeField]
        Mesh m_customMesh = null;
        [SerializeField]
        Material m_customMaterial = null;

        [Header("Runtime Renderer Fields")]
        [SerializeField]
        bool m_showRenderer = false;
        [SerializeField]
        UnityEngine.Rendering.LightProbeUsage m_rendererLightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        [SerializeField]
        UnityEngine.Rendering.ReflectionProbeUsage m_rendererReflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        [SerializeField]
        LightProbeProxyVolume m_rendererProxyVolume = null;
        [SerializeField]
        UnityEngine.Rendering.ShadowCastingMode m_rendererShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        [SerializeField]
        bool m_rendererReceiveShadows = false;
        
        [Header("Non-Runtime Renderer Fields")]
        [SerializeField]
        Vector3 m_rendererOffsetPosition = Vector3.zero;
        [SerializeField]
        Vector3 m_rendererOffsetAngle = Vector3.zero;
        [SerializeField]
        Vector3 m_rendererOffsetScale = new Vector3(3,3,3);
        [SerializeField, Range(0, 10)]
        float m_scaleInLightMap = 0;

        [SerializeField]
        MeshRenderer m_giMeshRenderer = null;

        /*[SerializeField, HideInInspector]
        Mesh m_defaultCylinderMesh = null;
        [SerializeField, HideInInspector]
        Mesh m_defaultCapsuleMesh = null;*/

        [SerializeField, HideInInspector]
        Material m_defaultGILightMaterial = null;
        [SerializeField, HideInInspector]
        Mesh m_defaultSphereMesh = null;
        [SerializeField, HideInInspector]
        Mesh m_defaultCubeMesh = null;
        [SerializeField, HideInInspector]
        Mesh m_defaultQuadMesh = null;

        [SerializeField, HideInInspector]
        int m_customMaterialInstanceID = -1;
        [SerializeField, HideInInspector]
        int m_lightInstanceID = -1;

        #endregion

        #region Public Properties

        public UnityEngine.Rendering.ShadowCastingMode RendererShadowCastingMode
        {
            get
            {
                return m_rendererShadowCastingMode;
            }
            set
            {
                if (m_rendererShadowCastingMode == value)
                    return;
                m_rendererShadowCastingMode = value;
                SetDirty();
            }
        }


        public bool RendererReceiveShadows
        {
            get
            {
                return m_rendererReceiveShadows;
            }
            set
            {
                if (m_rendererReceiveShadows == value)
                    return;
                m_rendererReceiveShadows = value;
                SetDirty();
            }
        }

        public UnityEngine.Rendering.LightProbeUsage RendererLightProbeUsage
        {
            get
            {
                return m_rendererLightProbeUsage;
            }
            set
            {
                if (m_rendererLightProbeUsage == value)
                    return;
                m_rendererLightProbeUsage = value;
                SetDirty();
            }
        }

        public LightProbeProxyVolume RendererProxyVolume
        {
            get
            {
                return m_rendererProxyVolume;
            }
            set
            {
                if (m_rendererProxyVolume == value)
                    return;
                m_rendererProxyVolume = value;
                SetDirty();
            }
        }

        public UnityEngine.Rendering.ReflectionProbeUsage RendererReflectionProbeUsage
        {
            get
            {
                return m_rendererReflectionProbeUsage;
            }
            set
            {
                if (m_rendererReflectionProbeUsage == value)
                    return;
                m_rendererReflectionProbeUsage = value;
                SetDirty();
            }
        }


        public bool ShowRenderer
        {
            get
            {
                return m_showRenderer;
            }
            set
            {
                if (m_showRenderer == value)
                    return;
                m_showRenderer = value;
                SetDirty();
            }
        }

        public float ScaleInLightMap
        {
            get
            {
                return m_scaleInLightMap;
            }
            set
            {
                if (m_scaleInLightMap == value)
                    return;
                m_scaleInLightMap = value;
                SetDirty();
            }
        }

        public bool IsOn
        {
            get
            {
                return m_isOn;
            }
            set
            {
                if (m_isOn == value)
                    return;
                m_isOn = value;
                SetDirty();
            }
        }

        public Color Color
        {
            get
            {
                return m_color;
            }
            set
            {
                if (m_color == value)
                    return;
                m_color = value;
                SetDirty();
                SetIconDirty(true);
            }
        }

        public float Intensity
        {
            get
            {
                return m_intensity;
            }
            set
            {
                if (m_intensity == value)
                    return;
                m_intensity = value;
                SetDirty();
            }
        }

        public Color HDRColor
        {
            get
            {
                return new Color(m_color.r * m_intensity, m_color.g * m_intensity, m_color.b * m_intensity, m_color.a);
            }
        }

        public LightShapeTypeEnum LightShapeType
        {
            get
            {
                return m_lightShapeType;
            }
            set
            {
                if (m_lightShapeType == value)
                    return;
                m_lightShapeType = value;
                SetDirty();
            }
        }

        public Mesh CustomMesh
        {
            get
            {
                return m_customMesh;
            }
            set
            {
                if (m_customMesh == value)
                    return;
                m_customMesh = value;
                SetDirty();
            }
        }

        public Vector3 RendererOffsetPosition
        {
            get
            {
                return m_rendererOffsetPosition;
            }
            set
            {
                if (m_rendererOffsetPosition == value)
                    return;
                m_rendererOffsetPosition = value;
                SetDirty();
            }
        }

        public Vector3 RendererOffsetScale
        {
            get
            {
                return m_rendererOffsetScale;
            }
            set
            {
                if (m_rendererOffsetScale == value)
                    return;
                m_rendererOffsetScale = value;
                SetDirty();
            }
        }

        public Vector3 RendererOffsetAngle
        {
            get
            {
                return m_rendererOffsetAngle;
            }
            set
            {
                if (m_rendererOffsetAngle == value)
                    return;
                m_rendererOffsetAngle = value;
                SetDirty();
            }
        }

        #endregion

        #region Unity Functions

        protected virtual void Awake()
        {
            if (!s_sceneGILights.Contains(this))
                s_sceneGILights.Add(this);
        }

        protected override void OnEnable()
        {
            if (!s_sceneGILights.Contains(this))
                s_sceneGILights.Add(this);
            SetIconDirty();
            if(_started)
                Init();
            if (m_giMeshRenderer != null)
            {
                m_giMeshRenderer.sharedMaterials = new Material[] { m_giMeshRenderer.sharedMaterial };
                m_giMeshRenderer.gameObject.SetActive(true);
            }
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
            Init();
        }

        protected override void OnDisable()
        {
            if(m_giMeshRenderer != null)
                TryApply(true);
#if UNITY_EDITOR
            SetIconDirty();
#endif
            m_giMeshRenderer.gameObject.SetActive(false);
            base.OnDisable();
        }

        protected virtual void OnDestroy()
        {
            var v_index = s_sceneGILights.IndexOf(this);
            if (v_index >= 0)
                s_sceneGILights.RemoveAt(v_index);
#if UNITY_EDITOR
            SetIconDirty();
#endif
            if (m_giMeshRenderer != null)
            {
                if(!Application.isPlaying)
                    GameObject.DestroyImmediate(m_giMeshRenderer.gameObject);
                else
                    GameObject.Destroy(m_giMeshRenderer.gameObject);
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            SetDirty();
        }
#endif

        #endregion

        #region Helper Functions

        protected override void Apply()
        {
            if (m_giMeshRenderer == null)
                Init();
            TryRecreateCustomMaterial();
#if UNITY_EDITOR
            SetIconDirty();
            UnityEditor.GameObjectUtility.SetStaticEditorFlags(m_giMeshRenderer.gameObject, UnityEditor.StaticEditorFlags.ContributeGI | UnityEditor.StaticEditorFlags.BatchingStatic);

            UnityEditor.SerializedObject v_rendererSerialized = new UnityEditor.SerializedObject(m_giMeshRenderer);
            var v_scaleInLightMapProp = v_rendererSerialized.FindProperty("m_ScaleInLightmap");
            if(v_scaleInLightMapProp != null)
                v_scaleInLightMapProp.floatValue = Mathf.Max(0.000001f, m_scaleInLightMap);
            v_rendererSerialized.ApplyModifiedProperties();
#endif
            //Set Transform Properties
            var v_meshFilter = m_giMeshRenderer.GetComponent<MeshFilter>();
            v_meshFilter.sharedMesh = GetGIMesh();
            v_meshFilter.gameObject.layer = this.gameObject.layer;
            v_meshFilter.gameObject.transform.localPosition = m_rendererOffsetPosition;
            v_meshFilter.gameObject.transform.localEulerAngles = m_rendererOffsetAngle;
            v_meshFilter.gameObject.transform.localScale = m_rendererOffsetScale;

            //Set Renderer Properties
            m_giMeshRenderer.shadowCastingMode = m_rendererShadowCastingMode;
            m_giMeshRenderer.receiveShadows = m_rendererReceiveShadows;
            m_giMeshRenderer.lightProbeUsage = m_rendererLightProbeUsage;
            m_giMeshRenderer.reflectionProbeUsage = m_rendererReflectionProbeUsage;
            m_giMeshRenderer.lightProbeProxyVolumeOverride = m_rendererProxyVolume != null? m_rendererProxyVolume.gameObject : null;

            var v_isOn = m_isOn && enabled && gameObject.activeInHierarchy && gameObject.activeSelf;
            m_giMeshRenderer.enabled = v_isOn || m_showRenderer; // we only deactivate renderer when light is off and dont want to show renderer
            MaterialPropertyBlock v_block = new MaterialPropertyBlock();
            m_giMeshRenderer.GetPropertyBlock(v_block);
            v_block.Clear();
            if (!m_showRenderer)
            {
                v_block.SetColor("_Color", new Color(m_giMeshRenderer.sharedMaterial.color.r, m_giMeshRenderer.sharedMaterial.color.b, m_giMeshRenderer.sharedMaterial.color.g, 0));
                v_block.SetFloat("_Cutoff", 1);
            }
            v_block.SetColor("_EmissionColor", v_isOn? HDRColor : Color.clear);
            m_giMeshRenderer.SetPropertyBlock(v_block);
            RendererExtensions.UpdateGIMaterials(m_giMeshRenderer);
        }

        Material GetDefaultGILightMaterial()
        {
            if (m_defaultGILightMaterial == null)
            {
                m_defaultGILightMaterial = Resources.Load<Material>("DefaultGILightMaterial");
                if (m_defaultGILightMaterial == null)
                {
                    m_defaultGILightMaterial = new Material(Shader.Find("Standard"));
                    m_defaultGILightMaterial.enableInstancing = true;
                    m_defaultGILightMaterial.SetFloat("_Cutoff", 0);
                    m_defaultGILightMaterial.EnableKeyword("_EMISSION");
                    SetupMaterialWithBlendMode(m_defaultGILightMaterial, BlendMode.Cutout);
#if UNITY_EDITOR
                    m_defaultGILightMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    UnityEditor.MaterialEditor.FixupEmissiveFlag(m_defaultGILightMaterial);
#endif
                }
            }
            return m_defaultGILightMaterial;
        }

        protected virtual void TryRecreateCustomMaterial()
        {
            if (m_giMeshRenderer != null)
            {
                var v_currentMaterialID = m_giMeshRenderer.sharedMaterial != null ? m_giMeshRenderer.sharedMaterial.GetInstanceID() : -1;
                if (v_currentMaterialID != m_customMaterialInstanceID || v_currentMaterialID < 0)
                {
                    m_giMeshRenderer.sharedMaterial = m_customMaterial != null ? m_customMaterial : GetDefaultGILightMaterial();
                    m_customMaterialInstanceID = m_giMeshRenderer.sharedMaterial.GetInstanceID();
                    //Material must be Culout and must have Emission Enabled
                    m_giMeshRenderer.sharedMaterial.EnableKeyword("_EMISSION");
                    //SetupMaterialWithBlendMode(m_giMeshRenderer.sharedMaterial, BlendMode.Cutout);
                    //Unload old Resources
                    Resources.UnloadUnusedAssets();
                }
            }
        }

        protected virtual void Init()
        {
            var v_thisInstanceID = this.GetInstanceID();
            var v_needCreate = m_giMeshRenderer == null || m_lightInstanceID != v_thisInstanceID;
            if (v_needCreate)
            {
                m_lightInstanceID = v_thisInstanceID;
                GameObject v_lightObject = m_giMeshRenderer == null || m_giMeshRenderer.transform.parent != this.transform? new GameObject("GILightRenderer") : m_giMeshRenderer.gameObject;
                v_lightObject.hideFlags = HideFlags.HideInHierarchy;
                v_lightObject.transform.SetParent(this.transform);
                v_lightObject.transform.localPosition = m_rendererOffsetPosition;
                v_lightObject.transform.localEulerAngles = m_rendererOffsetAngle;
                v_lightObject.transform.localScale = m_rendererOffsetScale;
                m_giMeshRenderer = v_lightObject.GetComponent<MeshRenderer>();
                if(m_giMeshRenderer == null)
                    m_giMeshRenderer = v_lightObject.AddComponent<MeshRenderer>();
                m_giMeshRenderer.shadowCastingMode = m_rendererShadowCastingMode;
                m_giMeshRenderer.receiveShadows = m_rendererReceiveShadows;
                m_giMeshRenderer.lightProbeUsage = m_rendererLightProbeUsage;
                m_giMeshRenderer.reflectionProbeUsage = m_rendererReflectionProbeUsage;
                m_giMeshRenderer.lightProbeProxyVolumeOverride = m_rendererProxyVolume != null ? m_rendererProxyVolume.gameObject : null;
                var v_meshFilter = v_lightObject.GetComponent<MeshFilter>();
                if (v_meshFilter == null)
                    v_meshFilter = v_lightObject.AddComponent<MeshFilter>();
                v_meshFilter.sharedMesh = GetGIMesh();
                SetDirty();
            }
#if UNITY_EDITOR
            UnityEditor.GameObjectUtility.SetStaticEditorFlags(m_giMeshRenderer.gameObject, UnityEditor.StaticEditorFlags.ContributeGI | UnityEditor.StaticEditorFlags.BatchingStatic);
#endif
            TryRecreateCustomMaterial();
        }
            
        public virtual Mesh GetGIMesh()
        {
            if (m_lightShapeType == LightShapeTypeEnum.Sphere)
            {
                if (m_defaultSphereMesh == null)
                {
                    var v_gameObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    m_defaultSphereMesh = v_gameObj.GetComponent<MeshFilter>().sharedMesh;
                    GameObject.DestroyImmediate(v_gameObj);
                }
                return m_defaultSphereMesh;
            }
            else if (m_lightShapeType == LightShapeTypeEnum.Cube)
            {
                if (m_defaultCubeMesh == null)
                {
                    var v_gameObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    m_defaultCubeMesh = v_gameObj.GetComponent<MeshFilter>().sharedMesh;
                    GameObject.DestroyImmediate(v_gameObj);
                }
                return m_defaultCubeMesh;
            }
            else if (m_lightShapeType == LightShapeTypeEnum.Quad)
            {
                if (m_defaultQuadMesh == null)
                {
                    var v_gameObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    m_defaultQuadMesh = v_gameObj.GetComponent<MeshFilter>().sharedMesh;
                    GameObject.DestroyImmediate(v_gameObj);
                }
                return m_defaultQuadMesh;
            }
            return m_customMesh;
        }

        public MeshRenderer GetGIMeshRenderer()
        {
            if (m_giMeshRenderer == null)
                Init();
            return m_giMeshRenderer;
        }

        public static BlendMode GetMaterialBlendMode(Material material)
        {
            return material != null && material.HasProperty("_Mode") ? (BlendMode)material.GetFloat("_Mode") : (BlendMode)0;
        }

        public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", "");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    material.SetInt("_ZWrite", 1);
                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    material.SetInt("_ZWrite", 1);
                    break;
                case BlendMode.Fade:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.SetInt("_ZWrite", 0);
                    break;
                case BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.SetInt("_ZWrite", 0);
                    break;
            }
            material.SetFloat("_Mode", (float)blendMode);
#if UNITY_EDITOR
            UnityEditor.MaterialEditor.FixupEmissiveFlag(material);
#endif
        }

        #endregion

        #region Editor Icon Functions

#if UNITY_EDITOR
        static HashSet<GameObject> s_objectsToRemoveIcon = new HashSet<GameObject>();
        protected static void EditorUpdate()
        {
            if (!UnityEditor.EditorApplication.isCompiling)
            {
                foreach (var v_gameObject in s_objectsToRemoveIcon)
                {
                    if (v_gameObject != null)
                    {
                        var v_giLight = v_gameObject.GetComponent<GILight>();
                        if (v_giLight == null)
                            KyubEditor.GI.GILightIconManager.RemoveIcon(v_gameObject);
                        else if(v_giLight != null)
                            v_giLight.TryRecalculateEditorIcon();
                    }
                }
                s_objectsToRemoveIcon.Clear();
                UnregisterEditorUpdate();
            }
        }

        protected static void RegisterEditorUpdate()
        {
            UnregisterEditorUpdate();
            UnityEditor.EditorApplication.update += EditorUpdate;
        }

        protected static void UnregisterEditorUpdate()
        {
            UnityEditor.EditorApplication.update -= EditorUpdate;
        }
#endif

        protected bool _isIconDirty = false;
        protected void SetIconDirty(bool p_value = true)
        {
#if UNITY_EDITOR
            _isIconDirty = p_value;
            if (!s_objectsToRemoveIcon.Contains(this.gameObject))
                s_objectsToRemoveIcon.Add(this.gameObject);
            if (_isIconDirty)
                RegisterEditorUpdate();
#endif
        }

        protected Texture2D _editorGizmoTexture = null;
        protected virtual void TryRecalculateEditorIcon(bool p_force = false)
        {
#if UNITY_EDITOR
            if (this.gameObject != null && (_isIconDirty || p_force))
            {
                _isIconDirty = false;
                if (this.enabled)
                    _editorGizmoTexture = KyubEditor.GI.GILightIconManager.SetIcon(this.gameObject, GetIconTexture(), this.IsOn ? this.Color : new Color(this.Color.r, this.Color.g, this.Color.b, 0.5f));
                else
                {
                    KyubEditor.GI.GILightIconManager.RemoveIcon(this.gameObject);
                    _editorGizmoTexture = null;
                }
            }
#endif
        }

        protected virtual Texture2D GetIconTexture()
        {
            if (m_lightShapeType == LightShapeTypeEnum.Sphere)
                return Resources.Load<Texture2D>("GILightGizmo_Sphere");
            else if (m_lightShapeType == LightShapeTypeEnum.Cube)
                return Resources.Load<Texture2D>("GILightGizmo_Cube");
            else if (m_lightShapeType == LightShapeTypeEnum.Quad)
                return Resources.Load<Texture2D>("GILightGizmo_Quad");
            return Resources.Load<Texture2D>("GILightGizmo_Custom");
        }

#endregion
    }
}
