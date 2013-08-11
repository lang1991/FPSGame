#region File Description
//-----------------------------------------------------------------------------
// GeometricPrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace FPSGame
{
    /// <summary>
    /// Base class for simple geometric primitive models. This provides a vertex
    /// buffer, an index buffer, plus methods for drawing the model. Classes for
    /// specific types of primitive (CubePrimitive, SpherePrimitive, etc.) are
    /// derived from this common base, and use the AddVertex and AddIndex methods
    /// to specify their geometry.
    /// </summary>
    public abstract class GeometricPrimitive : IDisposable
    {
        #region Fields


        // During the process of constructing a primitive model, vertex
        // and index data is stored on the CPU in these managed lists.
        List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
        List<ushort> indices = new List<ushort>();


        // Once all the geometry has been specified, the InitializePrimitive
        // method copies the vertex and index data into these buffers, which
        // store it on the GPU ready for efficient rendering.
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect basicEffect;
        ParticleSystem ps;
        Color color;
        Utils.Timer timer;
        Utils.TimerDelegate disposeDelegate;
        const float duration = 3.0f;
        float alpha;
        public bool delete = false;

        #endregion

        #region Initialization
        protected Matrix world = Matrix.Identity;

        /// <summary>
        /// Adds a new vertex to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        protected void AddVertex(Vector3 position, Vector3 normal)
        {
            vertices.Add(new VertexPositionNormal(position, normal));
        }


        /// <summary>
        /// Adds a new index to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            indices.Add((ushort)index);
        }


        /// <summary>
        /// Queries the index of the current vertex. This starts at
        /// zero, and increments every time AddVertex is called.
        /// </summary>
        protected int CurrentVertex
        {
            get { return vertices.Count; }
        }


        /// <summary>
        /// Once all the geometry has been specified by calling AddVertex and AddIndex,
        /// this method copies the vertex and index data into GPU format buffers, ready
        /// for efficient rendering.
        protected void InitializePrimitive(GraphicsDevice graphicsDevice, BasicEffect effect, Color c)
        {
            // Create a vertex declaration, describing the format of our vertex data.

            // Create a vertex buffer, and copy our vertex data into it.
            vertexBuffer = new VertexBuffer(graphicsDevice,
                                            typeof(VertexPositionNormal),
                                            vertices.Count, BufferUsage.None);

            vertexBuffer.SetData(vertices.ToArray());

            // Create an index buffer, and copy our index data into it.
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                          indices.Count, BufferUsage.None);

            indexBuffer.SetData(indices.ToArray());

            // Create a BasicEffect, which will be used to render the primitive.
            basicEffect = effect;
            basicEffect.EnableDefaultLighting();
            this.color = c;
            alpha = this.color.A / 255.0f;
            timer = new Utils.Timer();
            disposeDelegate = new Utils.TimerDelegate(remove);
            timer.AddTimer("dispose", duration, disposeDelegate, false);
        }


        /// <summary>
        /// Finalizer.
        /// </summary>
        ~GeometricPrimitive()
        {
            Dispose(false);
        }

        private void remove()
        {
            this.delete = true;
        }

        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (vertexBuffer != null)
                    vertexBuffer.Dispose();

                if (indexBuffer != null)
                    indexBuffer.Dispose();

                if (basicEffect != null)
                    basicEffect.Dispose();
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the primitive model, using the specified effect. Unlike the other
        /// Draw overload where you just specify the world/view/projection matrices
        /// and color, this method does not set any renderstates, so you must make
        /// sure all states are set to sensible values before you call it.
        /// </summary>
        public void Draw(Effect effect)
        {

            if (!vertexBuffer.IsDisposed)
            {
                GraphicsDevice graphicsDevice = effect.GraphicsDevice;

                // Set our vertex declaration, vertex buffer, and index buffer.
                graphicsDevice.SetVertexBuffer(vertexBuffer);

                graphicsDevice.Indices = indexBuffer;


                foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();

                    int primitiveCount = indices.Count / 3;

                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                         vertices.Count, 0, primitiveCount);
                }
            }
        }


        /// <summary>
        /// Draws the primitive model, using a BasicEffect shader with default
        /// lighting. Unlike the other Draw overload where you specify a custom
        /// effect, this method sets important renderstates to sensible values
        /// for 3D model rendering, so you do not need to set these states before
        /// you call it.
        /// </summary>
        public void Draw(Matrix view, Matrix projection)
        {
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.DiffuseColor = color.ToVector3();
            basicEffect.Alpha = alpha;
            DepthStencilState tempDepthStencilState = new DepthStencilState();
            tempDepthStencilState.DepthBufferEnable = true;
            //tempDepthStencilState.DepthBufferWriteEnable = false;
            basicEffect.GraphicsDevice.DepthStencilState = tempDepthStencilState;
            basicEffect.GraphicsDevice.BlendState = BlendState.Opaque;
            //basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Draw(basicEffect);
            basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }

        public void update(GameTime gameTime)
        {
            alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds / duration * 0.6f;
            timer.Update(gameTime);
        }
        #endregion
    }
}
