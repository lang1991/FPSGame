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
using Utils;

namespace FPSGame
{
    public class SoundEmitter : AudioEmitterInterface
    {
        public Vector3 position;
        public bool dead = false;
        public Utils.Timer timer;
        public Utils.TimerDelegate timerDelegate;

        public SoundEmitter(Vector3 position)
        {
            this.position = position;
            timer = new Timer();
            timerDelegate = new TimerDelegate(setToDead);
            timer.AddTimer("soundTimer", 1, timerDelegate, false);
        }

        private void setToDead()
        {
            this.dead = true;
        }

        public Vector3 Position
        {
            get { return this.position; }
        }

        public Vector3 Forward
        {
            get { return Vector3.Forward; }
        }

        public Vector3 Up
        {
            get { return Vector3.Up; }
        }

        public Vector3 Velocity
        {
            get { return Vector3.Zero; }
        }
    }
}
