using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FPSGame
{
    class Water
    {
        CModel waterMesh;
        Effect waterEffect;
        ContentManager content;
        RenderTarget2D reflectionTarg;
        public List<IRenderable> Objects = new List<IRenderable>();
        Game gameReference;

        public Water(ContentManager content, Game game,
            Vector3 position, Vector2 size)
        {
            this.content = content;
            this.gameReference = game;
            waterMesh = new CModel(content.Load<Model>("AssetCollection\\Water\\waterPlaneMesh"), position,
                Vector3.Zero, new Vector3(size.X, 1, size.Y), gameReference.GraphicsDevice);
            waterEffect = content.Load<Effect>("AssetCollection\\Effects\\WaterEffect");
            waterMesh.SetModelEffect(waterEffect, false);
            waterEffect.Parameters["viewportWidth"].SetValue(
                gameReference.GraphicsDevice.Viewport.Width);
            waterEffect.Parameters["viewportHeight"].SetValue(
                gameReference.GraphicsDevice.Viewport.Height);
            reflectionTarg = new RenderTarget2D(gameReference.GraphicsDevice, gameReference.GraphicsDevice.Viewport.Width,
                gameReference.GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color,
                DepthFormat.Depth24);
            waterEffect.Parameters["WaterNormalMap"].SetValue(
                content.Load<Texture2D>("AssetCollection\\Water\\ripplesNormalMap"));
        }

        public void renderReflection(TempCamera camera)
        {
            // Reflect the camera's properties across the water plane
            Vector3 reflectedCameraPosition = ((FreeCamera)camera).Position;
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y +
            waterMesh.position.Y * 2;
            Vector3 reflectedCameraTarget = ((FreeCamera)camera).Target;
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y
            + waterMesh.position.Y * 2;
            // Create a temporary camera to render the reflected scene
            TempCamera reflectionCamera = new TargetCamera(
            reflectedCameraPosition, reflectedCameraTarget, gameReference.GraphicsDevice);
            reflectionCamera.Update();
            // Set the reflection camera's view matrix to the water effect
            waterEffect.Parameters["ReflectedView"].SetValue(
            reflectionCamera.View);
            // Create the clip plane
            Vector4 clipPlane = new Vector4(0, 1, 0, -waterMesh.position.Y);
            // Set the render target
            gameReference.GraphicsDevice.SetRenderTarget(reflectionTarg);
            gameReference.GraphicsDevice.Clear(Color.Black);
            // Draw all objects with clip plane
            foreach (IRenderable renderable in Objects)
            {
                renderable.SetClipPlane(clipPlane);
                renderable.Draw(reflectionCamera.View, reflectionCamera.Projection,
                reflectedCameraPosition);
                renderable.SetClipPlane(null);
            }
            gameReference.GraphicsDevice.SetRenderTarget(null);
            // Set the reflected scene to its effect parameter in
            // the water effect
            waterEffect.Parameters["ReflectionMap"].SetValue(reflectionTarg);
        }

        public void PreDraw(TempCamera camera, GameTime gameTime)
        {
            renderReflection(camera);
            waterEffect.Parameters["Time"].SetValue(
                (float)gameTime.TotalGameTime.TotalSeconds);
        }

        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            waterMesh.Draw(View, Projection, CameraPosition);
        }
    }
}
