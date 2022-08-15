using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LiquidVolumeFX {

    public class LiquidVolumeDepthPrePassRenderFeature : ScriptableRendererFeature {

        enum Pass {
            BackBuffer = 0,
            FrontBuffer = 1
        }

        public readonly static List<LiquidVolume> lvBackRenderers = new List<LiquidVolume>();
        public readonly static List<LiquidVolume> lvFrontRenderers = new List<LiquidVolume>();

        public static void AddLiquidToBackRenderers(LiquidVolume lv) {
            if (lv == null || lv.topology != TOPOLOGY.Irregular || lvBackRenderers.Contains(lv)) return;
            lvBackRenderers.Add(lv);
        }

        public static void RemoveLiquidFromBackRenderers(LiquidVolume lv) {
            if (lv == null || !lvBackRenderers.Contains(lv)) return;
            lvBackRenderers.Remove(lv);
        }

        public static void AddLiquidToFrontRenderers(LiquidVolume lv) {
            if (lv == null || lv.topology != TOPOLOGY.Irregular || lvFrontRenderers.Contains(lv)) return;
            lvFrontRenderers.Add(lv);
        }

        public static void RemoveLiquidFromFrontRenderers(LiquidVolume lv) {
            if (lv == null || !lvFrontRenderers.Contains(lv)) return;
            lvFrontRenderers.Remove(lv);
        }

        class DepthPass : ScriptableRenderPass {

            static class ShaderParams {
                public static int RTBackBuffer = Shader.PropertyToID("_VLBackBufferTexture");
                public static int RTFrontBuffer = Shader.PropertyToID("_VLFrontBufferTexture");
                public static int FlaskThickness = Shader.PropertyToID("_FlaskThickness");
                public const string SKW_FP_RENDER_TEXTURE = "LIQUID_VOLUME_FP_RENDER_TEXTURES";
            }

            const string profilerTag = "LiquidVolumeDepthPrePass";

            Material mat;
            int targetId;
            int passId;
            List<LiquidVolume> lvRenderers;

            public DepthPass(Material mat, Pass pass) {
                this.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
                this.mat = mat;
                switch (pass) {
                    case Pass.BackBuffer:
                        targetId = ShaderParams.RTBackBuffer;
                        passId = (int)Pass.BackBuffer;
                        lvRenderers = lvBackRenderers;
                        break;
                    case Pass.FrontBuffer:
                        targetId = ShaderParams.RTFrontBuffer;
                        passId = (int)Pass.FrontBuffer;
                        lvRenderers = lvFrontRenderers;
                        break;
                }
            }


            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
                cameraTextureDescriptor.colorFormat = LiquidVolume.useFPRenderTextures ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32;
                cameraTextureDescriptor.sRGB = false;
                cameraTextureDescriptor.depthBufferBits = 16;
                cmd.GetTemporaryRT(targetId, cameraTextureDescriptor);
                ConfigureTarget(targetId);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

                if (lvRenderers == null) return;

                CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
                cmd.Clear();
                if (LiquidVolume.useFPRenderTextures) {
                    Camera cam = renderingData.cameraData.camera;
                    cmd.ClearRenderTarget(true, true, new Color(cam.farClipPlane, 0, 0, 0), 1f);
                    cmd.EnableShaderKeyword(ShaderParams.SKW_FP_RENDER_TEXTURE);
                } else {
                    cmd.ClearRenderTarget(true, true, new Color(0.9882353f, 0.4470558f, 0.75f, 0f), 0f);
                    cmd.DisableShaderKeyword(ShaderParams.SKW_FP_RENDER_TEXTURE);
                }
                lvRenderers.ForEach((LiquidVolume lv) => {
                    if (lv != null) {
                        if (lv.isActiveAndEnabled) {
                            cmd.SetGlobalFloat(ShaderParams.FlaskThickness, 1.0f - lv.flaskThickness);
                        } else {
                            cmd.SetGlobalFloat(ShaderParams.FlaskThickness, 1.0f);
                        }
                        cmd.DrawRenderer(lv.mr, mat, lv.subMeshIndex >= 0 ? lv.subMeshIndex : 0, passId);
                    }
                });

                context.ExecuteCommandBuffer(cmd);

                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd) {
                cmd.ReleaseTemporaryRT(targetId);
            }
        }


        [SerializeField, HideInInspector]
        Shader shader;

        public static bool installed;
        Material mat;
        DepthPass backPass, frontPass;


        private void OnDestroy() {
            CoreUtils.Destroy(mat);
        }

        public override void Create() {
            name = "Liquid Volume Depth PrePass";
            shader = Shader.Find("LiquidVolume/DepthPrePass");
            if (shader == null) {
                return;
            }
            mat = CoreUtils.CreateEngineMaterial(shader);
            backPass = new DepthPass(mat, Pass.BackBuffer);
            frontPass = new DepthPass(mat, Pass.FrontBuffer);
        }

        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
            installed = true;
            if (backPass != null && lvBackRenderers.Count > 0) {
                renderer.EnqueuePass(backPass);
            }
            if (frontPass != null && lvFrontRenderers.Count > 0) {
                renderer.EnqueuePass(frontPass);
            }
        }
    }
}
