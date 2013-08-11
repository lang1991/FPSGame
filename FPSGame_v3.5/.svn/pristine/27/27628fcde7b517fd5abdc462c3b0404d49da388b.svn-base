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
    public class UnanimatedActor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Model info
        public Model actorModel;
        protected string nameOfMesh;
        public Matrix[] actorBones;


        //Position info
        protected Pose pose;
        protected Quaternion rotation = Quaternion.Identity;

        //view and proj
        protected FPSCamera camera;

        public UnanimatedActor(Game game)
            : base(game)
        {
            camera = (FPSCamera)Game.Services.GetService(typeof(FPSCamera));
            pose = new Pose();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
       
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            base.Update(gameTime);
        }

        public Pose Pose
        {
            get
            {
                return this.pose;
            }
            set
            {
                this.pose = value;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            actorModel = Game.Content.Load<Model>(nameOfMesh);
            actorBones = new Matrix[actorModel.Bones.Count];
            actorModel.CopyAbsoluteBoneTransformsTo(actorBones);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.RasterizerState.CullMode = CullMode.None;
            //actorModel.CopyAbsoluteBoneTransformsTo(actorBones);
            foreach (ModelMesh mesh in actorModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjMatrix;
                    effect.World = actorBones[mesh.ParentBone.Index] * pose.WorldTransform;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }
    }
}
