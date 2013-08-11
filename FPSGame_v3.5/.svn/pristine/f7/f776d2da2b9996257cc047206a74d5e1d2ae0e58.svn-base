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


namespace FPSGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Obstacle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Model Info
        public Model actorModel;
        protected string nameOfMesh;
        public Matrix[] actorBones;
        public BoundingBox worldBoundsBox;
        public SixFacedBoundingBox boundingBoxGroup;


        //Position Info
        public Matrix worldTransform;
        protected float scale = 1.0f;
        protected Vector3 worldPosition=Vector3.Zero;
        protected Quaternion rotation = Quaternion.Identity;

        //Camera Info
        protected FPSCamera camera;


        public Obstacle(Game game, Vector3 worldPosition)
            : base(game)
        {
            // TODO: Construct any child components here
            this.worldPosition = worldPosition;
            camera = (FPSCamera)Game.Services.GetService(typeof(FPSCamera));
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            worldTransform = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(worldPosition);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            actorModel = Game.Content.Load<Model>(nameOfMesh);
            actorBones = new Matrix[actorModel.Bones.Count];
            actorModel.CopyAbsoluteBoneTransformsTo(actorBones);
            worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(worldPosition);
            worldBoundsBox = UpdateBoundingBox(actorModel, actorBones[actorModel.Meshes[0].ParentBone.Index] * worldTransform);
            boundingBoxGroup = new SixFacedBoundingBox(worldBoundsBox);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (ModelMesh mesh in actorModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjMatrix;
                    effect.World = actorBones[mesh.ParentBone.Index] * worldTransform;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
            //BoundingBoxRenderer.Render(Game.GraphicsDevice, camera.ViewMatrix, camera.ProjMatrix, worldBoundsBox);
        }

        private BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]),worldTransform);
                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            // Create and return bounding box
            return new BoundingBox(min, max);
        }

        public Vector3 WorldPosition
        {
            get
            {
                return this.worldPosition;
            }
            set
            {
                this.worldPosition = value;
                updateWorldTranform();
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
                updateWorldTranform();
            }
        }

        public float Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                updateWorldTranform();
            }
        }

        public void updateWorldTranform()
        {
            this.worldTransform = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(worldPosition);
        }
    }
}
