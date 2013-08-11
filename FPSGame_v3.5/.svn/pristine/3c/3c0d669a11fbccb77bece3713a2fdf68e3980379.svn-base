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
    public class SniperRifleHandsFP : Weapon
    {
        Player player;
        Model sniperRifle;
        public Matrix[] sniperRifleBones;

        public SniperRifleHandsFP(Game game, Player player)
            : base(game)
        {
            nameOfMesh = "AssetCollection\\Characters\\Roland\\Hands\\Hands_sniperRifle";  
            //nameOfMesh = "AssetCollection/Characters/Roland/Roland_rifle_movement";
            this.player = player;
            forwardOffset = -8.5f;
            downOffset = 30f;
            rightOffset = 0f;

            forwardOffset_zoom = 15f;
            downOffset_zoom = 4.8f;
            rightOffset_zoom = -0.21f;
            DrawOrder = int.MaxValue;
        }

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Add your initialization code here
            //initialize gun world transform info
            weaponWorldTransform = Matrix.Identity;
            weaponForwardOffset = forwardOffset;
            weaponDownOffset = downOffset;
            weaponRightOffset = rightOffset;

            //how unstable the gun currently is
            recoil = 1.0f;
            //how fast the gun recovers from firing
            recoilRate = 1f;
            //fire rate of the gun
            fireRate = 1.6f;
            //how fast the gun will lost control
            backlashForce = 10f;
            //the maximum extent the gun can lose contorl
            recoilLimit = 100f;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            animationClips.Add("Draw_01", skinningData.AnimationClips["Draw_01"]);
            animationClips.Add("Draw_02", skinningData.AnimationClips["Draw_02"]);
            animationClips.Add("Draw_Slow_01", skinningData.AnimationClips["Draw_Slow_01"]);
            animationClips.Add("Draw_Slow_02", skinningData.AnimationClips["Draw_Slow_02"]);
            animationClips.Add("fire_01", skinningData.AnimationClips["fire_01"]);
            animationClips.Add("fire_02", skinningData.AnimationClips["fire_02"]);
            animationClips.Add("fire_Slow_01", skinningData.AnimationClips["fire_Slow_01"]);
            animationClips.Add("fire_Slow_02", skinningData.AnimationClips["fire_Slow_02"]);
            animationClips.Add("grenade_throw_01", skinningData.AnimationClips["grenade_throw_01"]);
            animationClips.Add("Holster_01", skinningData.AnimationClips["Holster_01"]);
            animationClips.Add("Holster_Slow_01", skinningData.AnimationClips["Holster_Slow_01"]);
            animationClips.Add("idle_01", skinningData.AnimationClips["idle_01"]);
            animationClips.Add("Reload_01", skinningData.AnimationClips["Reload_01"]);
            animationClips.Add("RELOAD_02", skinningData.AnimationClips["RELOAD_02"]);
            animationClips.Add("Reload_Slow_01", skinningData.AnimationClips["Reload_Slow_01"]);
            animationClips.Add("RELOAD_Slow_02", skinningData.AnimationClips["RELOAD_Slow_02"]);
            animationClips.Add("Sprint", skinningData.AnimationClips["Sprint"]);
            animationPlayer.StartClip(animationClips["idle_01"]);
            gunFireTextures.Add(Game.Content.Load<Texture2D>("AssetCollection\\Effects\\gunFire"));
            gunFireTextures.Add(Game.Content.Load<Texture2D>("AssetCollection\\Effects\\gunFire2"));
            gunFireEffect.Parameters["myTexture"].SetValue(gunFireTextures[rnd.Next(2)]);
            sniperRifle = Game.Content.Load<Model>("AssetCollection/Weapons/gestalt_alien_rifle_maya");
            sniperRifleBones = new Matrix[sniperRifle.Bones.Count];
            sniperRifle.CopyAbsoluteBoneTransformsTo(sniperRifleBones);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            base.Update(gameTime);
            recoilTimer.Update(gameTime);
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            if (animationPlayer.finishedFirstAnimationCycle)
            {
                animationPlayer.StartClip(animationClips["idle_01"]);
            }
        }

        public void parentUpdate(GameTime gameTime)
        {
            base.Update(gameTime);
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
        }

        public void Draw(GameTime gameTime)
        {
            base.Draw(gameTime, weaponWorldTransform*Matrix.CreateTranslation(30f*weaponWorldTransform.Down + -10f*weaponWorldTransform.Forward + -12f*weaponWorldTransform.Right));
            //gunFire.Draw(camera, weaponWorldTransform * Matrix.CreateTranslation(5f*weaponWorldTransform.Forward + 0.7f*weaponWorldTransform.Left + -1f*weaponWorldTransform.Up), gunFireTextures[rnd.Next(2)]);
            drawCurrentWeapon();
        }

        protected void drawCurrentWeapon()
        {
            int handIndex = skinningData.BoneIndices["Weapon"];

            Matrix[] worldTransforms = animationPlayer.GetWorldTransforms();

            // Adjust weapon offsets
            Matrix rootBone = actorBones[0];
            Matrix myWeaponWorldTransform = sniperRifleBones[0] * worldTransforms[handIndex] *
                weaponWorldTransform * Matrix.CreateTranslation(38 * weaponWorldTransform.Down + -7f *
                weaponWorldTransform.Forward + -12f * weaponWorldTransform.Right);// *worldTransforms[handIndex];
            //weaponWorldTransform * Matrix.CreateTranslation(30f * weaponWorldTransform.Down + -10f * weaponWorldTransform.Forward + -12f * weaponWorldTransform.Right) * worldTransforms[handIndex];// *rootBone;

            foreach (ModelMesh mesh in sniperRifle.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = myWeaponWorldTransform;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjMatrix;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        public void parentDraw(GameTime gameTime)
        {
            base.Draw(gameTime, weaponWorldTransform);
        }

        public override void fire()
        {
            base.fire();
        }

        public override void createShootingEffect(Vector3 direction, Vector3 position)
        {
            base.createShootingEffect(direction,position);
        }
    }
}
