using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPSGame
{
    public class GunFire
    {
        // Vertex data
        VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;
        // Position
        Vector3 position;
        // Vertex and graphics info
        GraphicsDevice graphicsDevice;
        // Effect
        Effect gunFireEffect;
        //Spark duration
        float duration = -5;

        public GunFire(GraphicsDevice graphicsDevice, Vector3 position,Effect effect)
        {
            this.position = position;
            this.graphicsDevice = graphicsDevice;
            this.gunFireEffect = effect;
            InitializeParticleVertices();
        }

        private void InitializeParticleVertices()
        {
            // Initialize vertices
            verts = new VertexPositionTexture[4];
            verts[0] = new VertexPositionTexture(
            new Vector3(-1, 1, 0), new Vector2(0, 0));
            verts[1] = new VertexPositionTexture(
            new Vector3(1, 1, 0), new Vector2(1, 0));
            verts[2] = new VertexPositionTexture(
            new Vector3(-1, -1, 0), new Vector2(0, 1));
            verts[3] = new VertexPositionTexture(
            new Vector3(1, -1, 0), new Vector2(1, 1));
            // Set vertex data in VertexBuffer
            vertexBuffer = new VertexBuffer(this.graphicsDevice,
            typeof(VertexPositionTexture), verts.Length,
            BufferUsage.None);
            vertexBuffer.SetData(verts);
            duration = 0.05f;
        }

        public void Update(GameTime gameTime)
        {
            duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(FPSCamera camera, Matrix world, Texture2D texture)
        {
            if (duration >= 0)
            {
                graphicsDevice.SetVertexBuffer(vertexBuffer);

                gunFireEffect.Parameters["WorldViewProjection"].SetValue(world * camera.ViewMatrix * camera.ProjMatrix);
                gunFireEffect.Parameters["myTexture"].SetValue(texture);
                // Draw particles
                foreach (EffectPass pass in gunFireEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, verts, 0, 2);
                }
            }
        }

        public float Duration
        {
            get { return duration;}
        }

        public void reset()
        {
            duration = 0.1f;
        }
    }
}
