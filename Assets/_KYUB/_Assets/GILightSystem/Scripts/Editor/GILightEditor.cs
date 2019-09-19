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
using UnityEditor;
using Kyub.GI;

namespace KyubEditor.GI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GILight), true)]
    public class GILightEditor : Editor
    {
        #region Creator Menu

        [MenuItem("GameObject/Light/GI Emissive Light")]
        private static void CreateLightMenu()
        {
            GameObject v_gameObject = new GameObject("GI_Light");
            v_gameObject.AddComponent<GILight>();
            Selection.activeGameObject = v_gameObject;
        }

        #endregion

        #region Private Variables

        static Material _GiLightMaterial = null; 

        #endregion

        #region Unity Functions

        protected virtual void OnEnable()
        {
            SetAllIconsDirty();
            if (_GiLightMaterial == null)
                _GiLightMaterial = new Material(Shader.Find("Hidden/GILightWireframe"));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("GILight only works when created in editor with RealtimeGI Enabled (After Bake Scene)", MessageType.Warning);
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();

            if(GUI.changed)
                SetAllLightsDirty();
        }

        private void OnSceneGUI()
        {
            GILight target = (GILight)this.target;
            Color oldColor = Handles.color;
            Handles.color = target.Color;
            var v_renderer = target.GetGIMeshRenderer();
            var v_meshSize = v_renderer.bounds.size;
            var v_sizeRange = Mathf.Max(v_meshSize.x, v_meshSize.y, v_meshSize.z);
            var v_radius = v_sizeRange * target.Intensity;

            if (v_sizeRange > 0)
            {
                Handles.RadiusHandle(Quaternion.identity, v_renderer.transform.position, v_radius);
                float v_newRadius = Handles.RadiusHandle(Quaternion.identity, v_renderer.transform.position, v_radius, true);
                if (GUI.changed)
                {
                    Undo.RecordObject((Object)target, "Adjust GI Light Intensity");
                    target.Intensity = v_sizeRange == 0 ? 0 : v_newRadius / v_sizeRange;
                    target.TryApply();
                }
            }
            Handles.color = oldColor;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        protected static void DrawGizmosSelected(GILight scr, GizmoType gizmoType)
        {
            if (!scr.ShowRenderer)
            {
                UpdateMaterialColor(scr);

                var v_renderer = scr.GetGIMeshRenderer();
                Vector3 position = v_renderer.transform.position;
                Quaternion rotation = v_renderer.transform.rotation;
                Vector3 scale = v_renderer.transform.lossyScale;

                Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
                // set first shader pass of the material
                var v_mesh = scr.GetGIMesh();
                if (v_mesh != null)
                {
                    _GiLightMaterial.SetPass(0);
                    Graphics.DrawMeshNow(scr.GetGIMesh(), matrix);
                }
            }
        }

        #endregion

        #region Helper Functions

        protected static void UpdateMaterialColor(GILight p_light)
        {
            if (p_light != null && _GiLightMaterial != null && _GiLightMaterial.HasProperty("_WireColor"))
                _GiLightMaterial.SetColor("_WireColor", p_light.IsOn && p_light.enabled? new Color(p_light.Color.r, p_light.Color.g, p_light.Color.b, 0.3f) : Color.clear);
        }

        protected virtual void SetAllIconsDirty()
        {
            foreach (var v_target in targets)
            {
                GILight v_targetGI = v_target as GILight;
                if (v_targetGI != null && v_targetGI.enabled && v_targetGI.gameObject.activeInHierarchy)
                {
                    v_targetGI.SendMessage("SetIconDirty", true);
                }
            }
        }

        protected virtual void SetAllLightsDirty()
        {
            foreach (var v_target in targets)
            {
                GILight v_targetGI = v_target as GILight;
                if (v_targetGI != null && v_targetGI.enabled)
                {
                    v_targetGI.SetDirty();
                }
            }
        }

        #endregion
    }
}
