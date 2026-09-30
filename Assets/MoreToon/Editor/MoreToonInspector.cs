#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace lilToon
{
    public class MoreToonInspector : lilToonInspector
    {
        private const string shaderName = "MoreToon";

        private MaterialProperty moreToonEnabled;
        private MaterialProperty moreToonStrength;
        private MaterialProperty moreToonShadowSteps;
        private MaterialProperty moreToonShadowSharpness;
        private MaterialProperty moreToonLightDirectionSteps;
        private MaterialProperty moreToonLightSteps;
        private MaterialProperty moreToonMinLight;

        private static bool showMoreToon = true;

        protected override void LoadCustomProperties(MaterialProperty[] props, Material material)
        {
            isCustomShader = true;
            ReplaceToCustomShaders();

            // v0.1 deliberately supports the opaque variants only.
            isShowRenderMode = false;

            moreToonEnabled = FindProperty("_MoreToonEnabled", props);
            moreToonStrength = FindProperty("_MoreToonStrength", props);
            moreToonShadowSteps = FindProperty("_MoreToonShadowSteps", props);
            moreToonShadowSharpness = FindProperty("_MoreToonShadowSharpness", props);
            moreToonLightDirectionSteps = FindProperty("_MoreToonLightDirectionSteps", props);
            moreToonLightSteps = FindProperty("_MoreToonLightSteps", props);
            moreToonMinLight = FindProperty("_MoreToonMinLight", props);
        }

        protected override void DrawCustomProperties(Material material)
        {
            showMoreToon = Foldout("MoreToon", "Anime-style lighting controls", showMoreToon);
            if(!showMoreToon) return;

            EditorGUILayout.BeginVertical(boxOuter);
            m_MaterialEditor.ShaderProperty(moreToonEnabled, "Enable MoreToon");

            if(moreToonEnabled.floatValue > 0.5f)
            {
                EditorGUILayout.BeginVertical(boxInner);
                m_MaterialEditor.ShaderProperty(moreToonStrength, "Strength");
                m_MaterialEditor.ShaderProperty(moreToonShadowSteps, "Shadow Steps");
                m_MaterialEditor.ShaderProperty(moreToonShadowSharpness, "Shadow Quantization");
                m_MaterialEditor.ShaderProperty(moreToonLightDirectionSteps, "Light Direction Steps");
                m_MaterialEditor.ShaderProperty(moreToonLightSteps, "Diffuse Light Steps");
                m_MaterialEditor.ShaderProperty(moreToonMinLight, "Minimum Light");
                EditorGUILayout.EndVertical();

                EditorGUILayout.HelpBox(
                    "v0.1 prototype: Opaque only. Light Direction Steps = 0 disables direction quantization.",
                    MessageType.Info
                );
            }

            EditorGUILayout.EndVertical();
        }

        protected override void ReplaceToCustomShaders()
        {
            // Keep unsupported variants on lilToon's initialized defaults.
            // v0.1 only replaces the opaque and opaque+outline variants.
            lts = Shader.Find(shaderName + "/lilToon");
            ltso = Shader.Find("Hidden/" + shaderName + "/OpaqueOutline");
        }
    }
}
#endif
