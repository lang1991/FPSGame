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
    public class Sender : Microsoft.Xna.Framework.GameComponent
    {

        Player fpc_object;

        /// <summary>
        /// We may just use a string instead to store the input.
        /// Think about this for later
        /// </summary>
        internal KeyboardInputState p_Input;

        internal List<Ray> p_bulleyRayList;

        internal Matrix p_worldTranform;

        internal Player.playerState p_motionState;

        internal Vector3 p_preWorldPosition;
        internal Vector3 p_netVelocity;
        internal Vector3 p_netForce;
        internal Vector3 p_netAcceleration;

        internal float p_rotateLeftAndRight;
        internal float p_rotateUpAndDown;

        public Sender(Game game, Player fpc)
            : base(game)
        {
            fpc_object = fpc;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            //p_bulleyRayArray = 
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // PULLS DATA FROM PLAYER
            p_bulleyRayList         = fpc_object.BulletRayList;
            p_worldTranform         = fpc_object.WorldTransform;
            p_motionState           = fpc_object.MotionState;
            p_preWorldPosition      = fpc_object.derived_PreWorldPosition;
            p_netVelocity           = fpc_object.Velocity;
            p_netForce              = fpc_object.Force;
            p_netAcceleration       = fpc_object.Acceleration;
            p_rotateLeftAndRight    = fpc_object.RotateLeftRight;
            p_rotateUpAndDown       = fpc_object.RotateUpDown;
            // SEND TO SERVER HERE
            base.Update(gameTime);
        }

    }
}
