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
using Microsoft.Xna.Framework.Net;
using SkinnedModel;
using Primitives3D;
using System.IO;
using System.Text;

namespace FPSGame
{
    public class Player : AnimatedActor, AudioEmitterInterface
    {
        #region Fields
        public string playerName;
        Vector3 position;
        Vector3 forward;
        Vector3 up;
        int selector;
        Game1 gameReference;
        KeyboardInputState Input;
        //position,transform, scale and rotation info for camera
        public enum playerState { onGround, jumping, falling };
        public enum animationState
        {
            START_IDLE,
            START_RUNNING_FORWARD,
            START_RUNNING_BACKWARD,
            START_RUNNING_RIGHT,
            START_RUNNING_LEFT,
            START_RUNNING_RIGHT_BACKWARD,
            START_RUNNING_LEFT_BACKWARD,
            START_JUMPING,
            IDLE,
            RUNNING_FORWARD,
            RUNNING_BACKWARD,
            RUNNING_RIGHT,
            RUNNING_LEFT,
            RUNNING_RIGHT_BACKWARD,
            RUNNING_LEFT_BACKWARD,
            JUMPING,
            START_CROUCH_IDLE,
            START_CROUCHING_FORWARD,
            START_CROUCHING_BACKWARD,
            START_CROUCHING_RIGHT,
            START_CROUCHING_LEFT,
            START_CROUCHING_RIGHT_BACKWARD,
            START_CROUCHING_LEFT_BACKWARD,
            CROUCH_IDLE,
            CROUCHING_FORWARD,
            CROUCHING_BACKWARD,
            CROUCHING_RIGHT,
            CROUCHING_LEFT,
            CROUCHING_RIGHT_BACKWARD,
            CROUCHING_LEFT_BACKWARD,
            PLAYING_ALL_ANIMATIONS
        };
        public playerState motionState = playerState.onGround;
        public animationState playerAnimationState = animationState.IDLE;
        public animationState lastAnimationState = 0;
        Utils.Timer jumpCount = new Utils.Timer();
        Utils.Timer respawnTimer = new Utils.Timer();
        Utils.TimerDelegate jumpCountDelegate;
        Utils.TimerDelegate respawnTimerDelegate;

        public BoundingBox cameraBounds;
        bool thirdPersonMode = false;

        Vector3 OnSurface = Vector3.Zero;
        Vector3 netVelocity = Vector3.Zero;
        Vector3 netForce = Vector3.Zero;
        Vector3 jumpForce = new Vector3(0, 45000, 0);
        Vector3 netAcceleration = Vector3.Zero;
        Vector3 g = new Vector3(0, -800, 0);
        float mass = 90;

        int stepCounter = 0;

        const float runMoveSpeed = 60.0f;
        const float walkMoveSpeed = 50f;
        const float crouchMoveSpeed = 40f;
        const float rotateSpeed = 0.05f;
        float currentMoveSpeed = runMoveSpeed;

        protected SpherePrimitive spherePrimitive;
        protected List<SkinnedSphere> skinnedSpheres;
        protected BoundingSphereWrapper[] boundingSpheres;

        public int ammo = 10;
        public int health = 100;
        public int lastHealth = 100;
        public bool isDead = false;
        public bool soundPlayed = false;

        private Terrain terrain;

        List<Ray> bulletRayList;

        //input related
        MouseState originalMouse;

        //crosshair
        SpriteBatch spriteBatch;
        //determine whether can fire
        protected Model boundingBox;

        public Weapon currentWeapon;
        public Model alienRifle;           // temporary for demonstrating weapon-to-bone attachment; change this to a currentWeapon of type Weapon later
        //public List<BillboardSystem> billboards;
        //List<Beam> beams;

        Boolean drawBoundingSpheres = false;

        #endregion

        public Vector3 Position
        {
            get { return(position = pose.WorldTransform.Translation); }
        }
        public Vector3 Forward
        {
            get { return (forward = pose.WorldTransform.Forward); }
        }
        public Vector3 Up
        {
            get { return (up = pose.WorldTransform.Up); }
        }

        public Player(Game1 game): base(game)
        {
            this.nameOfMesh = "AssetCollection\\Characters\\Roland\\Roland_rifle_movement";
            this.DrawOrder = 999999;
            this.gameReference = game;
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
            Input = (KeyboardInputState)Game.Services.GetService(typeof(KeyboardInputState));
            Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            originalMouse = Mouse.GetState();
            Force = mass * g;
            jumpCountDelegate = new Utils.TimerDelegate(jumpCheck);
            respawnTimerDelegate = new Utils.TimerDelegate(respawn);
            bulletRayList = new List<Ray>();
        }

        public void parentInitialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            boundingBox = Game.Content.Load<Model>("AssetCollection\\Characters\\Roland\\Roland_bounding_box");
            derived_WorldPosition = new Vector3(0, 200, 0);
            //camera.Pose.WorldPosition = pose.WorldPosition + camera.EyeOffset;

            cameraBounds = UpdateBoundingBox(boundingBox, Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f) * WorldTransform);
            // Code auto-generated by AnimationDefinitionFileGenerator_v1
            animationClips.Add("crouch_back", skinningData.AnimationClips["crouch_back"]);
            animationClips.Add("crouch_fire_01", skinningData.AnimationClips["crouch_fire_01"]);
            animationClips.Add("crouch_forward", skinningData.AnimationClips["crouch_forward"]);
            animationClips.Add("crouch_idle", skinningData.AnimationClips["crouch_idle"]);
            animationClips.Add("crouch_left_b", skinningData.AnimationClips["crouch_left_b"]);
            animationClips.Add("crouch_left_f", skinningData.AnimationClips["crouch_left_f"]);
            animationClips.Add("crouch_reload_01", skinningData.AnimationClips["crouch_reload_01"]);
            animationClips.Add("crouch_right_b", skinningData.AnimationClips["crouch_right_b"]);
            animationClips.Add("crouch_right_f", skinningData.AnimationClips["crouch_right_f"]);
            animationClips.Add("crouch_rotate_left", skinningData.AnimationClips["crouch_rotate_left"]);
            animationClips.Add("crouch_rotate_right", skinningData.AnimationClips["crouch_rotate_right"]);
            animationClips.Add("crouch_throw_grenade", skinningData.AnimationClips["crouch_throw_grenade"]);
            animationClips.Add("crouch_weapon_switch_down", skinningData.AnimationClips["crouch_weapon_switch_down"]);
            animationClips.Add("crouch_weapon_switch_up", skinningData.AnimationClips["crouch_weapon_switch_up"]);
            animationClips.Add("jump_idle", skinningData.AnimationClips["jump_idle"]);
            animationClips.Add("run_back", skinningData.AnimationClips["run_back"]);
            animationClips.Add("run_forward", skinningData.AnimationClips["run_forward"]);
            animationClips.Add("run_left_b", skinningData.AnimationClips["run_left_b"]);
            animationClips.Add("run_left_f", skinningData.AnimationClips["run_left_f"]);
            animationClips.Add("run_right_b", skinningData.AnimationClips["run_right_b"]);
            animationClips.Add("run_right_f", skinningData.AnimationClips["run_right_f"]);
            animationClips.Add("sprint_foward", skinningData.AnimationClips["sprint_foward"]);
            animationClips.Add("sprint_left_f", skinningData.AnimationClips["sprint_left_f"]);
            animationClips.Add("sprint_right_f", skinningData.AnimationClips["sprint_right_f"]);
            animationClips.Add("stand_fire_01", skinningData.AnimationClips["stand_fire_01"]);
            animationClips.Add("stand_idle", skinningData.AnimationClips["stand_idle"]);
            animationClips.Add("stand_melee", skinningData.AnimationClips["stand_melee"]);
            animationClips.Add("stand_reload_01", skinningData.AnimationClips["stand_reload_01"]);
            animationClips.Add("stand_rotate_left", skinningData.AnimationClips["stand_rotate_left"]);
            animationClips.Add("stand_rotate_right", skinningData.AnimationClips["stand_rotate_right"]);
            animationClips.Add("stand_throw_grande", skinningData.AnimationClips["stand_throw_grande"]);
            animationClips.Add("stand_weapon_switch_down", skinningData.AnimationClips["stand_weapon_switch_down"]);
            animationClips.Add("stand_weapon_switch_up", skinningData.AnimationClips["stand_weapon_switch_up"]);
            animationClips.Add("walk_back", skinningData.AnimationClips["walk_back"]);
            animationClips.Add("walk_forward", skinningData.AnimationClips["walk_forward"]);
            animationClips.Add("walk_left_b", skinningData.AnimationClips["walk_left_b"]);
            animationClips.Add("walk_left_f", skinningData.AnimationClips["walk_left_f"]);
            animationClips.Add("walk_right_b", skinningData.AnimationClips["walk_right_b"]);
            animationClips.Add("walk_right_f", skinningData.AnimationClips["walk_right_f"]);
            animationClips.Add("all_animations", skinningData.AnimationClips["all_animations"]);
            animationPlayer.StartClip(animationClips["stand_idle"]);
            currentWeapon = new SniperRifleHandsFP(Game, this);
            Game.Components.Add(currentWeapon);

            // Load the bounding spheres.
            skinnedSpheres = new List<SkinnedSphere>();
            populateCollisionSpheres(skinnedSpheres);
            boundingSpheres = new BoundingSphereWrapper[skinnedSpheres.Count];
            spherePrimitive = new SpherePrimitive(GraphicsDevice, 1, 12);

            alienRifle = Game.Content.Load<Model>("AssetCollection/Weapons/gestalt_alien_rifle_maya");
        }

        protected void populateCollisionSpheres(List<SkinnedSphere> skinnedSpheres)
        {
            String fullPath = Path.GetFullPath(@"Content\AssetCollection\Characters\Roland\HitDetection\CollisionSpheres.txt");
            var boneCollisionSphere = File.ReadAllText(fullPath, Encoding.ASCII).Split(new[] { '\n' });
            for (int i = 0; i < boneCollisionSphere.Length; i++)
            {
                var collisionSphereData = boneCollisionSphere[i].Split(' ');
                for (int j = 0; j < collisionSphereData.Length; j++)
                    Console.WriteLine(collisionSphereData[j]);
                skinnedSpheres.Add(new SkinnedSphere());
                skinnedSpheres[i].BoneName = collisionSphereData[0];
                skinnedSpheres[i].Radius = Convert.ToSingle(collisionSphereData[1]);
                skinnedSpheres[i].XOffset = Convert.ToSingle(collisionSphereData[2]);
                skinnedSpheres[i].YOffset = Convert.ToSingle(collisionSphereData[3]);
                skinnedSpheres[i].ZOffset = Convert.ToSingle(collisionSphereData[4]);
                skinnedSpheres[i].DamageValue = Convert.ToSingle(collisionSphereData[5]);
                skinnedSpheres[i].highlighted = false;
                
            }
        }

        public void parentLoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(motionState);
            Console.WriteLine("Force"+Force);
            Console.WriteLine("Acceleration" + Acceleration);
            Console.WriteLine("Velocity" + Velocity);
            adjustSpheres(skinnedSpheres);
            if (playerAnimationState == animationState.START_IDLE)
            {
                playerAnimationState = animationState.IDLE;
                animationPlayer.StartClip(animationClips["stand_idle"]);
            }
            else if (playerAnimationState == animationState.START_CROUCH_IDLE)
            {
                playerAnimationState = animationState.CROUCH_IDLE;
                animationPlayer.StartClip(animationClips["crouch_idle"]);
            }
            else if (playerAnimationState == animationState.START_RUNNING_FORWARD)
            {
                playerAnimationState = animationState.RUNNING_FORWARD;
                animationPlayer.StartClip(animationClips["run_forward"]);
            }
            else if (playerAnimationState == animationState.START_RUNNING_BACKWARD)
            {
                playerAnimationState = animationState.RUNNING_BACKWARD;
                animationPlayer.StartClip(animationClips["run_back"]);
            }
            else if (playerAnimationState == animationState.START_RUNNING_LEFT)
            {
                playerAnimationState = animationState.RUNNING_LEFT;
                animationPlayer.StartClip(animationClips["run_left_f"]);
            }
            else if (playerAnimationState == animationState.START_RUNNING_RIGHT)
            {
                playerAnimationState = animationState.RUNNING_RIGHT;
                animationPlayer.StartClip(animationClips["run_right_f"]);
            }
            else if (playerAnimationState == animationState.START_RUNNING_LEFT_BACKWARD)
            {
                playerAnimationState = animationState.RUNNING_LEFT_BACKWARD;
                animationPlayer.StartClip(animationClips["run_left_b"]);
            }
            else if (playerAnimationState == animationState.START_RUNNING_RIGHT_BACKWARD)
            {
                playerAnimationState = animationState.RUNNING_RIGHT_BACKWARD;
                animationPlayer.StartClip(animationClips["run_right_b"]);
            }
            else if (playerAnimationState == animationState.START_JUMPING)
            {
                playerAnimationState = animationState.JUMPING;
                animationPlayer.StartClip(animationClips["jump_idle"]);
            }
            else if (playerAnimationState == animationState.START_CROUCH_IDLE)
            {
                playerAnimationState = animationState.CROUCH_IDLE;
                animationPlayer.StartClip(animationClips["crouch_idle"]);
            }
            else if (playerAnimationState == animationState.START_CROUCHING_FORWARD)
            {
                playerAnimationState = animationState.CROUCHING_FORWARD;
                animationPlayer.StartClip(animationClips["crouch_forward"]);
            }
            else if (playerAnimationState == animationState.START_CROUCHING_BACKWARD)
            {
                playerAnimationState = animationState.CROUCHING_BACKWARD;
                animationPlayer.StartClip(animationClips["crouch_back"]);
            }
            else if (playerAnimationState == animationState.START_CROUCHING_RIGHT)
            {
                playerAnimationState = animationState.CROUCHING_RIGHT;
                animationPlayer.StartClip(animationClips["crouch_right_f"]);
            }
            else if (playerAnimationState == animationState.START_CROUCHING_LEFT)
            {
                playerAnimationState = animationState.CROUCHING_LEFT;
                animationPlayer.StartClip(animationClips["crouch_left_f"]);
            }
            else if (playerAnimationState == animationState.START_CROUCHING_RIGHT_BACKWARD)
            {
                playerAnimationState = animationState.CROUCHING_RIGHT_BACKWARD;
                animationPlayer.StartClip(animationClips["crouch_right_b"]);
            }
            else if (playerAnimationState == animationState.START_CROUCHING_LEFT_BACKWARD)
            {
                playerAnimationState = animationState.CROUCHING_LEFT_BACKWARD;
                animationPlayer.StartClip(animationClips["crouch_left_b"]);
            }
            else if (playerAnimationState == animationState.PLAYING_ALL_ANIMATIONS)
            {
                animationPlayer.StartClip(animationClips["all_animations"]);
            }

            if (!isDead)
            {
                updateAltitudeFromHeightMap();
                processInput(gameTime);
                checkForGravity(gameTime);
                jumpCount.Update(gameTime);
                camera.updateCameraPositionWithPlayer(derived_WorldPosition);
                currentWeapon.weaponWorldTransform = Matrix.Invert(camera.ViewMatrix);
                UpdateBoundingSpheres();
                //do footstep stuff
                if (Game1.gameState == Game1.GameState.PlayingGame)
                {
                    if (playerAnimationState == animationState.RUNNING_BACKWARD ||
                        playerAnimationState == animationState.RUNNING_FORWARD ||
                        playerAnimationState == animationState.RUNNING_LEFT ||
                        playerAnimationState == animationState.RUNNING_LEFT_BACKWARD ||
                        playerAnimationState == animationState.RUNNING_RIGHT ||
                        playerAnimationState == animationState.RUNNING_RIGHT_BACKWARD)
                    {
                        if (stepCounter >= 30)
                        {
                            Random chooser = new Random();
                            int i = chooser.Next(1, 5);
                            gameReference.AudioUpdate(gameReference.audioManager.soundNames[i], false);
                            gameReference.audioManager.Play3DSound(gameReference.audioManager.soundNames[i], false, this);
                            stepCounter = 0;
                        }
                        stepCounter++;
                    }
                    else if (playerAnimationState == animationState.CROUCHING_BACKWARD ||
                        playerAnimationState == animationState.CROUCHING_FORWARD ||
                        playerAnimationState == animationState.CROUCHING_LEFT ||
                        playerAnimationState == animationState.CROUCHING_LEFT_BACKWARD ||
                        playerAnimationState == animationState.CROUCHING_RIGHT ||
                        playerAnimationState == animationState.CROUCHING_RIGHT_BACKWARD)
                    {
                        if (stepCounter >= 60)
                        {
                            Random chooser = new Random();
                            int i = chooser.Next(1, 5);
                            gameReference.AudioUpdate(gameReference.audioManager.soundNames[i], false);
                            gameReference.audioManager.Play3DSound(gameReference.audioManager.soundNames[i], false, this);
                            stepCounter = 0;
                        }
                        stepCounter++;
                    }
                    else if (playerAnimationState == animationState.START_JUMPING)
                    {
                        gameReference.AudioUpdate(gameReference.audioManager.soundNames[4], false);
                        gameReference.audioManager.Play3DSound(gameReference.audioManager.soundNames[4], false, this);
                    }
                }
            }

            //send animation data
            if (Game1.gameState == Game1.GameState.PlayingGame)
            {
                respawnTimer.Update(gameTime);
                NetworkSession temp = (NetworkSession)Game.Services.GetService(typeof(NetworkSession));
                if (playerAnimationState != lastAnimationState)
                {
                    gameReference.AnimationUpdate();
                }
                lastAnimationState = playerAnimationState;
                if (health < lastHealth)
                {
                    Random chooser = new Random();
                    int i = chooser.Next(5, 8);
                    gameReference.audioManager.Play3DSound(gameReference.audioManager.soundNames[i], false, this);
                    gameReference.AudioUpdate(gameReference.audioManager.soundNames[i], false);
                }
                if (isDead && !soundPlayed)
                {
                    playerAnimationState = animationState.START_IDLE;
                    respawnTimer.AddTimer("respawnTimer", 1f, respawnTimerDelegate, false);
                    Random chooser = new Random();
                    int i = chooser.Next(8, 11);
                    gameReference.audioManager.Play3DSound(gameReference.audioManager.soundNames[i], false, this);
                    gameReference.AudioUpdate(gameReference.audioManager.soundNames[i], false);
                    soundPlayed = true;
                }
                lastHealth = health;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the boundingSpheres array to match the current animation state.
        /// </summary>
        protected void UpdateBoundingSpheres()
        {
            // Look up the current world space bone positions.
            Matrix[] worldTransforms = animationPlayer.GetWorldTransforms();

            for (int i = 0; i < skinnedSpheres.Count; i++)
            {
                // Convert the SkinnedSphere description to a BoundingSphere.
                SkinnedSphere source = skinnedSpheres[i];
                Vector3 center = new Vector3(source.XOffset, source.YOffset, source.ZOffset);
                BoundingSphereWrapper sphere = new BoundingSphereWrapper(gameReference, new BoundingSphere(center, source.Radius),this,source);

                // Transform the BoundingSphere by its parent bone matrix,
                // and store the result into the boundingSpheres array.
                int boneIndex = skinningData.BoneIndices[source.BoneName];

                // THIS CODE SO BAD.
                boundingSpheres[i] = new BoundingSphereWrapper(gameReference, 
                    (sphere.getBoundingSphere).Transform(worldTransforms[boneIndex] * pose.WorldTransform),this,source);
                //Matrix.CreateScale(sphere.Radius) * Matrix.CreateTranslation(sphere.Center);
            }
        }

        public void adjustSpheres(List<SkinnedSphere> skinnedSpheres)
        {
            if (Input.IsKeyPressed(Keys.Q))
            {
                selector++;
            }
            if (Input.IsKeyPressed(Keys.E))
            {
                selector--;
            }
            if (Input.IsKeyPressed(Keys.U) || Input.IsKeyHeld(Keys.U))
            {
                skinnedSpheres[selector].XOffset += 0.25f;
            }
            if (Input.IsKeyPressed(Keys.J) || Input.IsKeyHeld(Keys.J))
            {
                skinnedSpheres[selector].XOffset -= 0.25f;
            }
            if (Input.IsKeyPressed(Keys.I) || Input.IsKeyHeld(Keys.I))
            {
                skinnedSpheres[selector].YOffset += 0.25f;
            }
            if (Input.IsKeyPressed(Keys.K) || Input.IsKeyHeld(Keys.K))
            {
                skinnedSpheres[selector].YOffset -= 0.25f;
            }
            if (Input.IsKeyPressed(Keys.O) || Input.IsKeyHeld(Keys.O))
            {
                skinnedSpheres[selector].ZOffset += 0.25f;
            }
            if (Input.IsKeyPressed(Keys.L) || Input.IsKeyHeld(Keys.L))
            {
                skinnedSpheres[selector].ZOffset -= 0.25f;
            }
            if (Input.IsKeyPressed(Keys.Tab))
            {
                Console.WriteLine(skinnedSpheres[selector].BoneName);
                Console.WriteLine(skinnedSpheres[selector].Radius);
                Console.WriteLine(skinnedSpheres[selector].XOffset);
                Console.WriteLine(skinnedSpheres[selector].YOffset);
                Console.WriteLine(skinnedSpheres[selector].ZOffset);
            }
            UpdateBoundingSpheres();
        }

        public void parentUpdate(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //BoundingBoxRenderer.Render(Game.GraphicsDevice, camera.ViewMatrix, camera.ProjMatrix, cameraBounds);
            if (thirdPersonMode)
            {
                // Render the skinned mesh.
                drawCurrentWeapon();
                base.Draw(gameTime, pose.WorldTransform);
                if(drawBoundingSpheres)
                    DrawBoundingSpheres(camera.ViewMatrix, camera.ProjMatrix);
            }
            else
            {
                if (currentWeapon is SniperRifleHandsFP)
                {
                    SniperRifleHandsFP tempDraw = (SniperRifleHandsFP)currentWeapon;
                    tempDraw.Draw(gameTime);
                }
            }
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        /// <summary>
        /// Draws the animated bounding spheres.
        /// </summary>
        protected void DrawBoundingSpheres(Matrix view, Matrix projection)
        {
            //Console.WriteLine(boundingSpheres.Length);
            GraphicsDevice.RasterizerState = Wireframe;

            for (int i = 0; i < boundingSpheres.Length; i++)
            {
                BoundingSphere sphere = boundingSpheres[i].getBoundingSphere;

                Matrix world = Matrix.CreateScale(sphere.Radius) *
                               Matrix.CreateTranslation(sphere.Center);

                if(skinnedSpheres[i].highlighted)
                    spherePrimitive.Draw(world, view, projection, Color.Red);
                else
                    spherePrimitive.Draw(world, view, projection, Color.White);
            }

            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        }

        static RasterizerState Wireframe = new RasterizerState
        {
            FillMode = FillMode.WireFrame
        };

        public void parentDraw(GameTime gameTime,Matrix worldTransform)
        {
            base.Draw(gameTime,pose.WorldTransform);
        }

        protected void drawCurrentWeapon()
        {
            int handIndex = skinningData.BoneIndices["Weapon"];

            Matrix[] worldTransforms = animationPlayer.GetWorldTransforms();

            // Adjust weapon offsets
            Matrix rootBone = actorBones[0];
            Matrix weaponWorldTransform = Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateTranslation(0, -8, 0) * worldTransforms[handIndex] * WorldTransform * rootBone;

            foreach (ModelMesh mesh in alienRifle.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = weaponWorldTransform;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjMatrix;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        private void processInput(GameTime gameTime)
        {
            if (!Game.IsActive)
            {
                return;
            }

            MouseState currentMouse = Mouse.GetState();
            if (currentMouse != originalMouse)
            {
                camera.RotateLeftRight -= (currentMouse.X - originalMouse.X) * rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                camera.RotateUpDown -= (currentMouse.Y - originalMouse.Y) * rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                camera.RotateUpDown = MathHelper.Clamp(camera.RotateUpDown, -MathHelper.PiOver2, MathHelper.PiOver2);
                //if (thirdPersonMode)
                //{
                    derived_Rotation = Quaternion.CreateFromYawPitchRoll(camera.RotateLeftRight, 0, 0);
                    camera.WorldPosition = Vector3.Transform(camera.WorldPosition, Matrix.CreateFromAxisAngle(WorldTransform.Up, camera.RotateLeftRight));
                //}
                Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            }
            if (currentMouse.LeftButton == ButtonState.Pressed && currentWeapon.canFire && Game1.gameState == Game1.GameState.PlayingGame)
            {
                rayCasting();
                currentWeapon.fire();
                ammo--;
                if (ammo == 0)
                    ammo = 10;
                NetworkSession temp = (NetworkSession)Game.Services.GetService(typeof(NetworkSession));
                gameReference.AudioUpdate("AssetCollection/Audio/AwpSound", false);
                gameReference.audioManager.Play3DSound("AssetCollection/Audio/AwpSound", false, this);
            }
            if (currentMouse.RightButton == ButtonState.Pressed)
            {
                currentWeapon.weaponDownOffset = currentWeapon.downOffset_zoom;
                currentWeapon.weaponRightOffset = currentWeapon.rightOffset_zoom;
                currentWeapon.weaponForwardOffset = currentWeapon.forwardOffset_zoom;
                camera.ProjMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, Game.GraphicsDevice.Viewport.AspectRatio, 0.3f, 100000.0f);
            }
            else
            {
                currentWeapon.weaponDownOffset = currentWeapon.downOffset;
                currentWeapon.weaponRightOffset = currentWeapon.rightOffset;
                currentWeapon.weaponForwardOffset = currentWeapon.forwardOffset;
                camera.ProjMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi/2, Game.GraphicsDevice.Viewport.AspectRatio, 0.3f, 100000.0f);
            }

            Vector3 deltaMoveVector = Vector3.Zero;
            if (playerAnimationState == animationState.PLAYING_ALL_ANIMATIONS)
            {
            }
            else if (motionState == playerState.onGround)
            {
                if (Input.IsKeyHeld(Keys.A))
                {
                    deltaMoveVector.X += -1;
                    if (Input.IsKeyHeld(Keys.S))
                    {
                        deltaMoveVector.Z += 1;
                        if (Input.IsKeyHeld(Keys.LeftControl))
                        {
                            if (playerAnimationState != animationState.CROUCHING_LEFT_BACKWARD)
                                playerAnimationState = animationState.START_CROUCHING_LEFT_BACKWARD;
                        }
                        else
                        {
                            if (playerAnimationState != animationState.RUNNING_LEFT_BACKWARD)
                                playerAnimationState = animationState.START_RUNNING_LEFT_BACKWARD;
                        }
                    }
                    else
                    {
                        if (Input.IsKeyHeld(Keys.W))
                            deltaMoveVector.Z += -1;
                        if (Input.IsKeyHeld(Keys.LeftControl))
                        {
                            if (playerAnimationState != animationState.CROUCHING_LEFT)
                                playerAnimationState = animationState.START_CROUCHING_LEFT;
                        }
                        else
                        {
                            if (playerAnimationState != animationState.RUNNING_LEFT)
                                playerAnimationState = animationState.START_RUNNING_LEFT;
                        }
                    }
                }
                else if (Input.IsKeyHeld(Keys.D))
                {
                    deltaMoveVector.X += 1;
                    if (Input.IsKeyHeld(Keys.S))
                    {
                        deltaMoveVector.Z += 1;
                        if (Input.IsKeyHeld(Keys.S))
                        {
                            deltaMoveVector.Z += 1;
                            if (Input.IsKeyHeld(Keys.LeftControl))
                            {
                                if (playerAnimationState != animationState.CROUCHING_RIGHT_BACKWARD)
                                    playerAnimationState = animationState.START_CROUCHING_RIGHT_BACKWARD;
                            }
                            else
                            {
                                if (playerAnimationState != animationState.RUNNING_RIGHT_BACKWARD)
                                    playerAnimationState = animationState.START_RUNNING_RIGHT_BACKWARD;
                            }
                        }
                    }
                    else
                    {
                        if (Input.IsKeyHeld(Keys.W))
                            deltaMoveVector.Z += -1;
                        if (Input.IsKeyHeld(Keys.LeftControl))
                        {
                            if (playerAnimationState != animationState.CROUCHING_RIGHT)
                                playerAnimationState = animationState.START_CROUCHING_RIGHT;
                        }
                        else
                        {
                            if (playerAnimationState != animationState.RUNNING_RIGHT)
                                playerAnimationState = animationState.START_RUNNING_RIGHT;
                        }
                    }
                }
                else if (Input.IsKeyHeld(Keys.W))
                {
                    deltaMoveVector.Z += -1;
                    if (Input.IsKeyHeld(Keys.LeftControl))
                    {
                        if (playerAnimationState != animationState.CROUCHING_FORWARD)
                            playerAnimationState = animationState.START_CROUCHING_FORWARD;
                    }
                    else
                    {
                        if (playerAnimationState != animationState.RUNNING_FORWARD)
                            playerAnimationState = animationState.START_RUNNING_FORWARD;
                    }
                }
                else if (Input.IsKeyHeld(Keys.S))
                {
                    deltaMoveVector.Z += 1;
                    if (Input.IsKeyHeld(Keys.LeftControl))
                    {
                        if (playerAnimationState != animationState.CROUCHING_BACKWARD)
                            playerAnimationState = animationState.START_CROUCHING_BACKWARD;
                    }
                    else
                    {
                        if (playerAnimationState != animationState.RUNNING_BACKWARD)
                            playerAnimationState = animationState.START_RUNNING_BACKWARD;
                    }
                }
                else if (Input.IsKeyHeld(Keys.RightShift))
                {
                    playerAnimationState = animationState.PLAYING_ALL_ANIMATIONS;
                }
                else
                {
                    if (Input.IsKeyHeld(Keys.LeftControl))
                    {
                        if (playerAnimationState != animationState.CROUCH_IDLE)
                            playerAnimationState = animationState.START_CROUCH_IDLE;
                    }
                    else
                    {
                        if (playerAnimationState != animationState.IDLE)
                            playerAnimationState = animationState.START_IDLE;
                    }
                }
                if (Input.IsKeyHeld(Keys.Space))
                {
                    playerAnimationState = animationState.START_JUMPING;
                    motionState = playerState.jumping;
                    Force += jumpForce;
                    jumpCount.AddTimer("jumpCounter", 1.0f/6, jumpCountDelegate, false);
                }
                // Camera and move speed adjustments
                if (Input.IsKeyHeld(Keys.LeftControl))
                {
                    currentMoveSpeed = crouchMoveSpeed;
                    camera.EyeOffset = camera.standingEyeOffset - Vector3.Up*5;
                }
                else
                {
                    camera.EyeOffset = camera.standingEyeOffset;
                    currentMoveSpeed = runMoveSpeed;
                }
            }
            else if (motionState == playerState.falling || motionState == playerState.jumping)
            {
                if (Input.IsKeyHeld(Keys.W))
                    deltaMoveVector.Z += -1;
                if (Input.IsKeyHeld(Keys.A))
                    deltaMoveVector.X += -1;
                if (Input.IsKeyHeld(Keys.S))
                    deltaMoveVector.Z += 1;
                if (Input.IsKeyHeld(Keys.D))
                    deltaMoveVector.X += 1;
                if (playerAnimationState != animationState.JUMPING)
                    playerAnimationState = animationState.IDLE;
            }
            if (Input.IsKeyReleased(Keys.T))
                drawBoundingSpheres = !drawBoundingSpheres;
            if (Input.IsKeyReleased(Keys.F))
                thirdPersonMode = !thirdPersonMode;
            if (deltaMoveVector != Vector3.Zero)
                deltaMoveVector = Vector3.Normalize(deltaMoveVector)*currentMoveSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds;

            updatePositionWithCamera(deltaMoveVector);
            camera.updateViewMatrix();
        }

        private void updatePositionWithCamera(Vector3 vectorToAdd)
        {
            Vector3 tempPrePosition = derived_WorldPosition;
            //derived_PreWorldPosition = derived_WorldPosition;
            Matrix cameraRotation = Matrix.CreateRotationY(camera.Pose.RotateLeftRight);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            rotatedVector.Y = 0;
            float previousY = cameraBounds.Min.Y;
            Vector3 previousWorldPosition = derived_WorldPosition;
            derived_WorldPosition += rotatedVector;
            
            //
            int MAP_DIMENSION = 256;
            int CELL_SIZE = 8;
            int arrayX = Convert.ToInt32(derived_WorldPosition.X) / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            int arrayZ = Convert.ToInt32(derived_WorldPosition.Z) / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            if (arrayX >= 0 && arrayX < MAP_DIMENSION && arrayZ >= 0 && arrayZ < MAP_DIMENSION)
            {
                if(motionState == playerState.onGround)
                {
                    float[,] heights = gameReference.getTerrain().getVertexHeights();
                    if(previousY == derived_WorldPosition.Y)
                    {
                    }
                    else if (previousY - getTerrainHeightAt(previousWorldPosition) > 5 &&
                        previousY - derived_WorldPosition.Y > 5)
                    {
                        motionState = playerState.falling;
                    }
                    else if (motionState != playerState.falling)
                    {
                        //Console.WriteLine("--------------------------");
                        derived_WorldPosition = new Vector3(derived_WorldPosition.X, getTerrainHeightAt(derived_WorldPosition) +
                        (cameraBounds.Max.Y - cameraBounds.Min.Y)-2, derived_WorldPosition.Z);
                    }
                }
            }
            else
            {
                //if(motionState != playerState.falling)
                //    motionState = playerState.falling;
            }
            //

            bool intersects = false;
            foreach (GameComponent gc in Game.Components)
            {
                if (gc.GetType().BaseType == typeof(Obstacle))
                {
                    Obstacle temp = (Obstacle)gc;
                    if (cameraBounds.Intersects(temp.worldBoundsBox))
                    {
                        intersects = true;
                        break;
                    }
                }
                else if (gc.GetType() == typeof(DummyPlayer))
                {
                    DummyPlayer temp = (DummyPlayer)gc;
                    if (cameraBounds.Intersects(temp.cameraBounds))
                    {
                        intersects = true;
                        break;
                    }
                }
            }
            if (intersects)
            {
                derived_WorldPosition = new Vector3(tempPrePosition.X, derived_WorldPosition.Y, tempPrePosition.Z);
            }
        }

        private void checkForGravity(GameTime gameTime)
        {
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds / 2.0f;
            derived_WorldPosition += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Acceleration = Force / mass;
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds / 2.0f;

            if (Velocity.Y < 0)
                motionState = playerState.falling;

            if (motionState == playerState.onGround)
            {
                BoundingBox test = UpdateBoundingBox(boundingBox,Matrix.CreateRotationX(MathHelper.ToRadians(90))*Matrix.CreateScale(0.1f)* Matrix.CreateTranslation(derived_WorldPosition + new Vector3(0, -10, 0)));
                List<GameComponent> platforms = new List<GameComponent>();
                foreach (GameComponent gc in Game.Components)
                {
                    if (gc.GetType().BaseType == typeof(Obstacle))
                    {
                        Obstacle temp = (Obstacle)gc;
                        if (test.Intersects(temp.worldBoundsBox))
                        {
                            platforms.Add(temp);
                        }
                    }
                }

                if (platforms.Count == 0 && cameraBounds.Min.Y > getTerrainHeightAt(derived_WorldPosition))
                {
                    //motionState = playerState.falling;
                }
            }
            else if (motionState == playerState.falling)
            {
                if (Acceleration == Vector3.Zero)
                    Acceleration += g;

                if (derived_WorldPosition.Y < getTerrainHeightAt(derived_WorldPosition))
                {
                    Console.WriteLine("Intersecting ground heightmap");
                    Force = Vector3.Zero;
                    Acceleration = Vector3.Zero;
                    Velocity = Vector3.Zero;
                    derived_WorldPosition = derived_PreWorldPosition;
                    derived_WorldPosition = new Vector3(derived_WorldPosition.X, getTerrainHeightAt(derived_WorldPosition), derived_WorldPosition.Z+2);
                    motionState = playerState.onGround;
                    playerAnimationState = animationState.IDLE;
                    animationPlayer.StartClip(animationClips["stand_idle"]);
                }

                foreach (GameComponent gc in Game.Components)
                {
                    if (gc.GetType().BaseType == typeof(Obstacle))
                    {
                        Obstacle temp = (Obstacle)gc;
                        for (int i = 0; i < temp.boundingBoxGroup.getBoxes().Length; i++)
                        {
                            if(temp.boundingBoxGroup.getBoxes()[i].Intersects(cameraBounds))
                            {
                                if (i == 3)
                                {
                                    Console.WriteLine("Intersecting ground");
                                    Force = Vector3.Zero;
                                    Acceleration = Vector3.Zero;
                                    Velocity = Vector3.Zero;
                                    derived_WorldPosition = derived_PreWorldPosition;
                                    derived_WorldPosition = new Vector3(derived_WorldPosition.X, temp.worldBoundsBox.Max.Y + (cameraBounds.Max.Y - cameraBounds.Min.Y) / 2 + 1, derived_WorldPosition.Z);
                                    motionState = playerState.onGround;
                                    playerAnimationState = animationState.IDLE;
                                    animationPlayer.StartClip(animationClips["stand_idle"]);
                                    break;
                                }
                                else if (i == 2)
                                {
                                    Force -= jumpForce;
                                    Velocity = Vector3.Zero;
                                    derived_WorldPosition = derived_PreWorldPosition;
                                    derived_WorldPosition = new Vector3(derived_WorldPosition.X, temp.worldBoundsBox.Min.Y - (cameraBounds.Max.Y - cameraBounds.Min.Y) -1, derived_WorldPosition.Z);
                                    motionState = playerState.falling;
                                    playerAnimationState = animationState.IDLE;
                                    animationPlayer.StartClip(animationClips["stand_idle"]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void jumpCheck()
        {
            motionState = playerState.falling;
            Force -= jumpForce;
            Acceleration = g;
        }

        public void respawn()
        {
            Vector3[] spawnPoints = new Vector3[4];
            spawnPoints[0] = new Vector3(500f, 80f, 460f);
            spawnPoints[1] = new Vector3(-270f, 90f, 550f);
            spawnPoints[2] = new Vector3(-400f, 105f, -500f);
            spawnPoints[3] = new Vector3(600f, 40f, -750f);
            Random chooser = new Random();
            int j = chooser.Next(0, 4);
            bool canSpawn = true;
            bool spawned = false;
     /*   loop: while (!spawned)
            {
                for (int i = 0; i < Game.Components.Count; i++)
                {
                    Player tempPlayer;
                    Console.WriteLine(Game.Components[i].GetType().BaseType == typeof(Player) && Game.Components[i] != this);
                    if (Game.Components[i].GetType().BaseType == typeof(Player) && Game.Components[i] != this)
                    {
                        tempPlayer = (Player)Game.Components[i];
                        if (Vector3.DistanceSquared(spawnPoints[j], tempPlayer.WorldTransform.Translation) < 900)
                        {
                            goto loop;
                            /*
                            canSpawn = false;
                            break;
                             * 
                        }
                    }
                }*/
                if (canSpawn)
                {
                    //spawn the player
                    this.pose.WorldPosition = spawnPoints[j];
                    this.isDead = false;
                    this.health = 100;
                    gameReference.HealUpdate(100);
                    this.ammo = 10;
                    this.soundPlayed = false;
                    playerAnimationState = animationState.IDLE;
                    motionState = playerState.onGround;
                    spawned = true;
                }
                else
                {
                    j = chooser.Next(0, 3);
                }
            }
        //}

        private void rayCasting()
        {
            Ray bulletRay = new Ray(currentWeapon.WeaponWorldTransform.Translation, currentWeapon.WeaponWorldTransform.Forward);
            Vector3 startPoint = WorldTransform.Translation + 5 * WorldTransform.Forward + 7 * WorldTransform.Up - 0.5f * WorldTransform.Left;

            SortedDictionary<float, GameComponent> victims = new SortedDictionary<float, GameComponent>();

            bool isHit = false;

            for (int i = 0; i < Game.Components.Count; i++)
            {
                Obstacle tempObstacle;
                Player tempPlayer;

                if (Game.Components[i].GetType().BaseType == typeof(Obstacle))
                {
                    tempObstacle = (Obstacle)(Game.Components[i]);
                    if (bulletRay.Intersects(tempObstacle.worldBoundsBox) != null)
                    {
                        isHit = true;
                        float distance = (float)bulletRay.Intersects(tempObstacle.worldBoundsBox);
                        while (victims.Keys.Contains(distance))
                        {
                            distance += 0.001f;
                        }
                        victims.Add(distance, tempObstacle);
                    }
                }
                else if (Game.Components[i].GetType().BaseType == typeof(Player) && Game.Components[i] != this)
                {
                    tempPlayer = (Player)Game.Components[i];
                    for(int j = 0; j < tempPlayer.boundingSpheres.Length; j++)
                    {
                        BoundingSphere bs = tempPlayer.boundingSpheres[j].getBoundingSphere;
                        if (bulletRay.Intersects(bs) != null)
                        {
                            isHit = true;
                            float distance = (float)bulletRay.Intersects(bs);

                            /*
                            tempPlayer.skinnedSpheres[j].highlighted = true;
                            tempPlayer.health -= (int)tempPlayer.skinnedSpheres[j].DamageValue;
                            if (tempPlayer.health <= 0)
                            {
                                tempPlayer.health = 0;
                                tempPlayer.isDead = true;
                            }
                            */
                            while (victims.Keys.Contains(distance))
                            {
                                distance += 0.001f;
                            }
                            victims.Add(distance, tempPlayer.boundingSpheres[j]);
                        }
                    }
                }
            }

            
            
            if (isHit)
            {

                if (victims[victims.Keys.First()].GetType() == typeof(BoundingSphereWrapper))
                {
                    BoundingSphereWrapper bsw = (BoundingSphereWrapper)victims[victims.Keys.First()];
                    //Game.Components.Add(new Crate(Game, bulletRay.Position + bulletRay.Direction * victims.Keys.First()));
                    bsw.getSkinnedSphere.highlighted = true;
                    bsw.getPlayer.health -= (int)bsw.getSkinnedSphere.DamageValue;
                    if (bsw.getPlayer.health <= 0)
                    {
                        bsw.getPlayer.health = 0;
                        bsw.getPlayer.isDead = true;
                    }
                    gameReference.DamageUpdate(bsw.getPlayer.health, bsw.getPlayer.playerName);
                }
                else if (victims[victims.Keys.First()].GetType().BaseType == typeof(Obstacle))
                {
                    Random chooser = new Random();
                    int i = chooser.Next(12, 16);
                    Obstacle tempObstacle = (Obstacle)(victims[victims.Keys.First()]);
                    SoundEmitter tempEmitter = new SoundEmitter(tempObstacle.WorldPosition);
                    gameReference.soundEmitters.Add(tempEmitter);
                    gameReference.audioManager.Play3DSound(gameReference.audioManager.soundNames[i], false, tempEmitter);
                    gameReference.RemoteAudioUpdate(gameReference.audioManager.soundNames[i], false, tempObstacle.WorldPosition);
                }

                if (!thirdPersonMode)
                {
                    gameReference.getEffectManager.addNewLaser(currentWeapon.weaponWorldTransform.Translation + 8f * currentWeapon.weaponWorldTransform.Forward - 2f * currentWeapon.weaponWorldTransform.Up + 2.3f * currentWeapon.weaponWorldTransform.Right, currentWeapon.weaponWorldTransform.Translation + victims.Keys.First() * currentWeapon.weaponWorldTransform.Forward, Color.Red);
                    Console.WriteLine(currentWeapon.weaponWorldTransform.Translation + victims.Keys.First() * currentWeapon.weaponWorldTransform.Forward);
                }
                else
                {
                    Matrix startPointMatrix = Matrix.CreateTranslation(startPoint);
                    gameReference.getEffectManager.addNewLaser(startPoint, currentWeapon.weaponWorldTransform.Translation + victims.Keys.First() * currentWeapon.weaponWorldTransform.Forward, Color.Red);
                }
                //send laser effect data
                if (Game1.gameState == Game1.GameState.PlayingGame)
                {
                    NetworkSession temp = (NetworkSession)Game.Services.GetService(typeof(NetworkSession));
                    gameReference.EffectUpdate(startPoint, currentWeapon.weaponWorldTransform.Translation + victims.Keys.First() * currentWeapon.weaponWorldTransform.Forward, Color.Red);
                }
            }
            else
            {
                if (!thirdPersonMode)
                {
                    gameReference.getEffectManager.addNewLaser(currentWeapon.weaponWorldTransform.Translation + 8f * currentWeapon.weaponWorldTransform.Forward - 2f * currentWeapon.weaponWorldTransform.Up + 2.3f * currentWeapon.weaponWorldTransform.Right, currentWeapon.weaponWorldTransform.Translation + 1000 * currentWeapon.weaponWorldTransform.Forward, Color.Red);   
                }
                else
                {
                    Matrix startPointMatrix = Matrix.CreateTranslation(startPoint);
                    gameReference.getEffectManager.addNewLaser(startPoint, currentWeapon.weaponWorldTransform.Translation + 1000 * currentWeapon.weaponWorldTransform.Forward, Color.Red);
                }
                //send laser effect data
                if (Game1.gameState == Game1.GameState.PlayingGame)
                {
                    NetworkSession temp = (NetworkSession)Game.Services.GetService(typeof(NetworkSession));
                    gameReference.EffectUpdate(startPoint, (currentWeapon.weaponWorldTransform.Translation + 1000 * currentWeapon.weaponWorldTransform.Forward), Color.Red);

                }
            }
        }

        protected BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
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
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), actorBones[mesh.ParentBone.Index] * worldTransform);
                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            // Create and return bounding box
            return new BoundingBox(min, max);
        }


        public Matrix WorldTransform
        {
            get
            {
                return pose.WorldTransform;
            }
            set
            {
                this.pose.WorldTransform = value;
                cameraBounds = UpdateBoundingBox(boundingBox, Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(pose.WorldTransform.Translation)); 
            }
        }

        public Quaternion derived_Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                base.Pose.Rotation = value;
                cameraBounds = UpdateBoundingBox(boundingBox, Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(pose.WorldTransform.Translation));
            }
        }

        public Vector3 derived_WorldPosition
        {
            get
            {
                return this.pose.WorldPosition;
            }
            set
            {
                base.Pose.PreWorldPosition = base.pose.WorldPosition;
                base.Pose.WorldPosition = value;
                if (thirdPersonMode)
                {
                    Vector3 transformedReference = Vector3.Transform(camera.thirdPersonReference, Matrix.CreateFromAxisAngle(WorldTransform.Up, camera.RotateLeftRight));
                    camera.WorldPosition = transformedReference + this.derived_WorldPosition;
                    //camera.WorldPosition = new Vector3(camera.Pose.WorldPosition.X, this.pose.WorldPosition.Y + camera.EyeOffset.Y, camera.Pose.WorldPosition.Z + camera.EyeOffset.Z);
                }
                else
                {
                    camera.WorldPosition = this.derived_WorldPosition + camera.EyeOffset;//camera.standingEyeOffset;
                }

                cameraBounds = UpdateBoundingBox(boundingBox, Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(pose.WorldTransform.Translation));
            }
        }

        public Vector3 derived_PreWorldPosition
        {
            get
            {
                return this.pose.PreWorldPosition;
            }
            set
            {
                this.pose.PreWorldPosition = value;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return netVelocity;
            }
            set
            {
                this.netVelocity = value;
            }
        }

        public Vector3 Force
        {
            get
            {
                return netForce;
            }
            set
            {
                this.netForce = value;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return netAcceleration;
            }
            set
            {
                this.netAcceleration = value;
            }
        }

        public List<Ray> BulletRayList
        {
            get
            {
                return this.bulletRayList;
            }
            set
            {
                this.bulletRayList = value;
            }
        }

        public float RotateLeftRight
        {
            get
            {
                return this.pose.RotateLeftRight;
            }
            set
            {
                this.pose.RotateLeftRight = value;
            }
        }

        public float RotateUpDown
        {
            get
            {
                return this.pose.RotateUpDown;
            }
            set
            {
                this.pose.RotateUpDown = value;
            }
        }

        public playerState MotionState
        {
            get
            {
                return this.motionState;
            }
            set
            {
                this.motionState = value;
            }
        }

        private float getTerrainHeightAt(Vector3 position)
        {
            int MAP_DIMENSION = 256;
            int CELL_SIZE = 8;

            int arrayX = Convert.ToInt32(derived_WorldPosition.X) / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            int arrayZ = Convert.ToInt32(derived_WorldPosition.Z) / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            float[,] heights = gameReference.getTerrain().getVertexHeights();
            if (arrayX >= 0 && arrayX < MAP_DIMENSION && arrayZ >= 0 && arrayZ < MAP_DIMENSION)
                return heights[arrayX, arrayZ];
            else
                return -1000;
        }

        private void updateAltitudeFromHeightMap()
        {
            //int MAP_DIMENSION = 256;
            //int CELL_SIZE = 8;
            /*
            float arrayX1f = derived_WorldPosition.X / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            float arrayZ1f = derived_WorldPosition.Z / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            int arrayX1 = Convert.ToInt32(derived_WorldPosition.X) / CELL_SIZE + (MAP_DIMENSION - 1) / 2;
            int arrayZ1 = Convert.ToInt32(derived_WorldPosition.Z) / CELL_SIZE + (MAP_DIMENSION-1) / 2;
            int arrayX2 = arrayX1 + 8;
            int arrayZ2 = arrayZ1;
            int arrayX3 = arrayX1;
            int arrayZ3 = arrayZ1 + 8;
            int arrayX4 = arrayX2;
            int arrayZ4 = arrayZ3;
            float[,] heights = gameReference.getTerrain().getVertexHeights();
            if (arrayX1 >= 0 && arrayX1 < MAP_DIMENSION && arrayZ1 >= 0 && arrayZ1 < MAP_DIMENSION &&
                arrayX2 >= 0 && arrayX2 < MAP_DIMENSION && arrayZ2 >= 0 && arrayZ2 < MAP_DIMENSION &&
                arrayX3 >= 0 && arrayX3 < MAP_DIMENSION && arrayZ3 >= 0 && arrayZ3 < MAP_DIMENSION &&
                arrayX4 >= 0 && arrayX4 < MAP_DIMENSION && arrayZ4 >= 0 && arrayZ4 < MAP_DIMENSION)
            {
                float upperLeft = heights[arrayX1, arrayZ1];
                float upperRight = heights[arrayX2, arrayZ2];
                float lowerLeft = heights[arrayX3, arrayZ3];
                float lowerRight = heights[arrayX4, arrayZ4];
                float leftBias = (arrayX1f - arrayX1) / (arrayX2 - arrayX1);
                float upperBias = (arrayZ1f - arrayZ1) / (arrayZ3 - arrayZ1);
                float lerpTop = leftBias * (upperRight - upperLeft) + upperLeft;
                float lerpRight = upperBias * (lowerRight - upperRight) + upperRight;
                float lerpBottom = leftBias * (lowerRight - lowerLeft) + lowerLeft;
                float lerpLeft = upperBias * (lowerLeft - upperLeft) + upperLeft;
                float lerpAverage = (lerpTop + lerpRight + lerpBottom + lerpLeft) / 4;
                derived_WorldPosition = new Vector3(derived_WorldPosition.X, lerpAverage, derived_WorldPosition.Z);
                Console.WriteLine(lerpTop + " " + lerpRight + " " + lerpBottom + " " + lerpBottom + " " + lerpAverage);
            }
            Console.WriteLine("Nothing");
            */
            //derived_WorldPosition = new Vector3(derived_WorldPosition.X, getTerrainHeightAt(derived_WorldPosition), derived_WorldPosition.Z);
        }
    }
}
