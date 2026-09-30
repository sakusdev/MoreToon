#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace MoreToon.Tests
{
    public class ShaderCompileTests
    {
        private readonly struct ShaderCase
        {
            public readonly string AssetPath;
            public readonly string ShaderName;

            public ShaderCase(string assetPath, string shaderName)
            {
                AssetPath = assetPath;
                ShaderName = shaderName;
            }
        }

        private static readonly ShaderCase[] ShaderCases =
        {
            new ShaderCase(
                "Assets/MoreToon/Shader/lts.lilcontainer",
                "MoreToon/lilToon"
            ),
            new ShaderCase(
                "Assets/MoreToon/Shader/lts_o.lilcontainer",
                "Hidden/MoreToon/OpaqueOutline"
            ),
            new ShaderCase(
                "Assets/MoreToon/Shader/ltspass_opaque.lilcontainer",
                "Hidden/MoreToon/ltspass_opaque"
            )
        };

        [Test]
        public void MoreToonShadersExistAndCompileWithoutErrors()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            var failures = new List<string>();

            foreach(var shaderCase in ShaderCases)
            {
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderCase.AssetPath);

                if(shader == null)
                {
                    failures.Add(
                        $"Shader importer did not produce a Shader at {shaderCase.AssetPath}."
                    );
                    continue;
                }

                if(shader.name != shaderCase.ShaderName)
                {
                    failures.Add(
                        $"{shaderCase.AssetPath}: expected shader name '{shaderCase.ShaderName}', " +
                        $"but importer produced '{shader.name}'."
                    );
                }

                var registeredShader = Shader.Find(shaderCase.ShaderName);
                if(registeredShader == null)
                {
                    failures.Add(
                        $"{shaderCase.ShaderName}: Shader.Find could not resolve the imported shader."
                    );
                }

                var messages = ShaderUtil.GetShaderMessages(shader);
                foreach(var message in messages)
                {
                    Debug.Log(
                        $"[MoreToon Shader Compiler] {shader.name}: " +
                        $"[{message.severity}] {message.message}"
                    );
                }

                if(ShaderUtil.ShaderHasError(shader))
                {
                    var details = string.Join(
                        "\n",
                        messages.Select(message =>
                            $"[{message.severity}] {message.message}"
                        )
                    );

                    failures.Add(
                        $"{shader.name} has shader compiler errors:\n{details}"
                    );
                }
            }

            Assert.That(
                failures,
                Is.Empty,
                "MoreToon shader compile validation failed:\n" +
                string.Join("\n\n", failures)
            );
        }
    }
}
#endif
