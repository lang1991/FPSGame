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
    public class Weapon : AnimatedActor
    {
        public enum WeaponStates {
            START_IDLE,
            START_SHOOTING,
            START_RELOADING,
            IDLE,
            SHOOTING,
            RELOADING
        };
        public WeaponStates weaponState = WeaponStates.IDLE;

        public Matrix weaponWorldTransform;
        public float weaponForwardOffset;
        public float weaponDownOffset;
        public float weaponRightOffset;
        public int ammo_left;
        public bool canFire = true;
        public Utils.Timer recoilTimer = new Utils.Timer();
        public Utils.TimerDelegate recoilDelegate;
        public Utils.TimerDelegate canFireDelegate;
        public float forwardOffset;
        public float downOffset;
        public float rightOffset;

        public float forwardOffset_zoom;
        public float downOffset_zoom;
        public float rightOffset_zoom;

        //how unstable the gun currently is
        public float recoil;
        //how fast the gun recovers from firing
        public float recoilRate;
        //fire rate of the gun
        public float fireRate;
        //how fast the gun will lost control
        public float backlashForce;
        //the maximum extent the gun can lose contorl
        public float recoilLimit;

        protected List<GunFire> gunFires;
        protected GunFire gunFire;
        protected Effect gunFireEffect;
        protected List<Texture2D> gunFireTextures;
        protected Random rnd;

        public Weapon(Game game)
            : base(game)
        {
            recoilDelegate = new Utils.TimerDelegate(recoverFromRecoil);
            canFireDelegate = new Utils.TimerDelegate(changeFireState);
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            gunFireEffect = Game.Content.Load<Effect>("AssetCollection\\Effects\\GunShot");
            gunFireEffect.CurrentTechnique = gunFireEffect.Techniques["Technique1"];
            gunFireTextures = new List<Texture2D>();
            gunFire = new GunFire(Game.GraphicsDevice, Vector3.Zero, gunFireEffect);
            rnd = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            base.Update(gameTime);
            if (weaponState == WeaponStates.START_SHOOTING)
            {
                weaponState = WeaponStates.SHOOTING;
                animationPlayer.StartClip(animationClips["fire_01"]);
            }
            else if (weaponState == WeaponStates.START_IDLE)
            {
                weaponState = WeaponStates.IDLE;
                animationPlayer.StartClip(animationClips["idle_01"]);
            }
            else if (weaponState == WeaponStates.START_RELOADING)
            {
                weaponState = WeaponStates.RELOADING;
                animationPlayer.StartClip(animationClips["Reload_01"]);
            }
            UpdateGunFire(gameTime);
        }

        public Matrix WeaponWorldTransform
        {
            get
            {
                return this.weaponWorldTransform;
            }
            set
            {
                this.weaponWorldTransform = value;
            }
        }

        public float Recoil
        {
            get
            {
                return this.recoil;
            }
        }

        private void recoverFromRecoil()
        {
            if (recoil <= 1)
                recoilTimer.RemoveTimer("recoilTimer");
            else
                recoil -= recoilRate;
        }

        private void changeFireState()
        {
            canFire = true;
            weaponState = WeaponStates.START_IDLE;
        }

        public virtual void fire()
        {
            if (recoil + backlashForce < recoilLimit)
                recoil += backlashForce;
            recoilTimer.AddTimer("recoilTimer", 0.1f, recoilDelegate, true);
            recoilTimer.AddTimer("fireRateTimer", fireRate, canFireDelegate, false);
            canFire = false;
            weaponState = WeaponStates.START_SHOOTING;
            gunFire.reset();
        }

        //for AWP, pass the start and end point of the attack
        //for other guns, pass Vector3.Zero for both start and end
        public virtual void createShootingEffect(Vector3 start,Vector3 end)
        { 
            
        }

        protected void UpdateGunFire(GameTime gameTime)
        {
            gunFire.Update(gameTime);
        }

    }
}
