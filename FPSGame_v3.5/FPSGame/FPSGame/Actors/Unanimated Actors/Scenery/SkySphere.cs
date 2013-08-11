using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FPSGame
{
    public class SkySphere : IRenderable
    {
        CModel model;
        Effect effect;
        GraphicsDevice graphics;

        public SkySphere(ContentManager Content,
        GraphicsDevice GraphicsDevice, TextureCube Texture)
        {
            model = new CModel(Content.Load<Model>("AssetCollection\\Sky\\skySphereMesh"),
                Vector3.Zero, Vector3.Zero, new Vector3(100), GraphicsDevice);
            effect = Content.Load<Effect>("AssetCollection\\Effects\\skyEffect");
            effect.Parameters["CubeMap"].SetValue(Texture);
            model.SetModelEffect(effect, false);
            this.graphics = GraphicsDevice;
        }
        public void Draw(Matrix View, Matrix Projection,
            Vector3 CameraPosition)
        {
            // Disable the depth buffer
            graphics.DepthStencilState = DepthStencilState.None;
            // Move the model with the sphere
            model.position = CameraPosition;
            model.Draw(View, Projection, CameraPosition);
            graphics.DepthStencilState = DepthStencilState.Default;
        }
        public void SetClipPlane(Vector4? Plane)
        {
            effect.Parameters["ClipPlaneEnabled"].SetValue(Plane.HasValue);
            if (Plane.HasValue)
                effect.Parameters["ClipPlane"].SetValue(Plane.Value);
        }
    }
}