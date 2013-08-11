using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FPSGame
{
    public static class BoundingBoxRenderer
    {
        static VertexBuffer vertBuffer;
        static BasicEffect effect;
        static VertexPositionColor[] verts;

        public static void InitializeGraphics(GraphicsDevice device)
        {
            effect = new BasicEffect(device);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = false;
            verts = new VertexPositionColor[24];

            for (int i = 0; i < verts.Length; i++)
            {
                verts[i].Color = Color.Red;
            }

            vertBuffer = new VertexBuffer(device, typeof(VertexPositionColor), verts.Length, BufferUsage.None);
            vertBuffer.SetData(verts);
        }

        public static void Render(GraphicsDevice device, Matrix view, Matrix proj, BoundingBox box)
        {
            if (vertBuffer == null)
                InitializeGraphics(device);

            Vector3 min = box.Min;
            Vector3 max = box.Max;
            verts[0].Position = new Vector3(min.X, min.Y, min.Z);

            verts[1].Position = new Vector3(max.X, min.Y, min.Z);

            verts[2].Position = new Vector3(min.X, min.Y, max.Z);

            verts[3].Position = new Vector3(max.X, min.Y, max.Z);

            verts[4].Position = new Vector3(min.X, min.Y, min.Z);

            verts[5].Position = new Vector3(min.X, min.Y, max.Z);

            verts[6].Position = new Vector3(max.X, min.Y, min.Z);

            verts[7].Position = new Vector3(max.X, min.Y, max.Z);

            verts[8].Position = new Vector3(min.X, max.Y, min.Z);

            verts[9].Position = new Vector3(max.X, max.Y, min.Z);

            verts[10].Position = new Vector3(min.X, max.Y, max.Z);

            verts[11].Position = new Vector3(max.X, max.Y, max.Z);

            verts[12].Position = new Vector3(min.X, max.Y, min.Z);

            verts[13].Position = new Vector3(min.X, max.Y, max.Z);

            verts[14].Position = new Vector3(max.X, max.Y, min.Z);

            verts[15].Position = new Vector3(max.X, max.Y, max.Z);

            verts[16].Position = new Vector3(min.X, min.Y, min.Z);

            verts[17].Position = new Vector3(min.X, max.Y, min.Z);

            verts[18].Position = new Vector3(max.X, min.Y, min.Z);

            verts[19].Position = new Vector3(max.X, max.Y, min.Z);

            verts[20].Position = new Vector3(min.X, min.Y, max.Z);

            verts[21].Position = new Vector3(min.X, max.Y, max.Z);

            verts[22].Position = new Vector3(max.X, min.Y, max.Z);

            verts[23].Position = new Vector3(max.X, max.Y, max.Z);

            effect.World = Matrix.CreateScale(1);
            effect.View = view;
            effect.Projection = proj;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, verts, 0, 12);
                pass.Apply();
            }
        }
    }
}
