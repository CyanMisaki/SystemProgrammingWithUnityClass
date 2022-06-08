using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HW8
{
    public class MyRenderPipeline : RenderPipeline
    {
        private CameraRenderer _cameraRenderer = new CameraRenderer();


        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            CamerasRender(context, cameras);
        }
        

        private void CamerasRender(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                _cameraRenderer.Render(context, camera);
            }
        }
        
    }
}