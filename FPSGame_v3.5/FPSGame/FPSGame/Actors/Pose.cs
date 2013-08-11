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
    public class Pose
    {
        Vector3 worldPosition;
        Vector3 preWorldPosition;
        Quaternion orientation;
        Quaternion rotation;
        float scale;
        Matrix worldTransform;
        float rotateLeftRight;
        float rotateUpDown;

        public Pose()
        {
            worldPosition=Vector3.Zero;
            preWorldPosition = worldPosition;
            orientation = Quaternion.Identity;
            scale = 1;
            updateWorldTransform();
            rotateLeftRight = 0;
            rotateUpDown = 0;
        }

        public Pose(Vector3 worldPosition,Vector3 preWorldPosition,float rotateLeftRight,float rotateUpDown)
        {
            this.worldPosition = worldPosition;
            this.preWorldPosition = preWorldPosition;
            this.orientation = Quaternion.CreateFromYawPitchRoll(rotateLeftRight,rotateUpDown,0);
            scale = 1;
            updateWorldTransform();
            this.rotateLeftRight = rotateLeftRight;
            this.rotateUpDown = rotateUpDown;
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
                updateWorldTransform();
            }
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
                updateWorldTransform();
            }
        }

        public Vector3 PreWorldPosition
        {
            get
            {
                return this.preWorldPosition;
            }
            set
            {
                this.preWorldPosition = value;
            }
        }

        public Matrix WorldTransform
        {
            get
            {
                return this.worldTransform;
            }
            set
            {
                this.worldTransform = value;
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
                updateWorldTransform();
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return this.orientation;
            }

            set 
            {
                this.orientation = value;
            }
        }

        public float RotateLeftRight
        {
            get
            {
                return this.rotateLeftRight;
            }
            set
            {
                this.rotateLeftRight = value;
                orientation = Quaternion.CreateFromYawPitchRoll(rotateLeftRight,rotateUpDown,0);
            }
        }

        public float RotateUpDown
        {
            get
            {
                return this.rotateUpDown;
            }
            set
            {
                this.rotateUpDown = value;
                orientation = Quaternion.CreateFromYawPitchRoll(rotateLeftRight, rotateUpDown, 0);
            }
        }

        private void updateWorldTransform()
        {
            worldTransform = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(worldPosition);
        }
    }
}
