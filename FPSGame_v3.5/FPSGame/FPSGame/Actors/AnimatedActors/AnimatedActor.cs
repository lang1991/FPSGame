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
using SkinnedModel;


namespace FPSGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AnimatedActor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Model info
        public Model actorModel;
        protected string nameOfMesh;
        public Matrix[] actorBones;
        protected AnimationPlayer animationPlayer;
        protected SkinningData skinningData;
        protected Dictionary<string,AnimationClip> animationClips = new Dictionary<string,AnimationClip>();

        //Position info
        protected Pose pose;
        protected Quaternion rotation = Quaternion.Identity;

        //view and proj
        protected FPSCamera camera;

        public AnimatedActor(Game game): base(game)
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
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
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
            skinningData = actorModel.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");
            animationPlayer = new AnimationPlayer(skinningData);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public void Draw(GameTime gameTime, Matrix worldTransform)
        {
            base.Draw(gameTime);
            if (animationPlayer != null)
            {
                Matrix[] bones = animationPlayer.GetSkinTransforms();
                if (bones != null)
                {
                    // Render the skinned mesh.
                    foreach (ModelMesh mesh in actorModel.Meshes)
                    {
                        foreach (SkinnedEffect effect in mesh.Effects)
                        {
                            effect.SetBoneTransforms(bones);
                            effect.View = camera.ViewMatrix;
                            effect.World = worldTransform;
                            effect.Projection = camera.ProjMatrix;
                            effect.EnableDefaultLighting();
                            effect.SpecularColor = new Vector3(0.25f);
                            effect.SpecularPower = 16;
                        }
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
