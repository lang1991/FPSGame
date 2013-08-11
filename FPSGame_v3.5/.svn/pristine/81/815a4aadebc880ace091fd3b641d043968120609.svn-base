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
    public class FPSCamera: Camera
    {
        //this setting for first person camera
        public Vector3 standingEyeOffset=new Vector3(0,10,0);
        //this setting for third-person view
        public Vector3 thirdPersonReference = new Vector3(0, 10, 30);


        Vector3 eyeOffset;
        public Matrix cameraRotation = Matrix.Identity;
        
        public FPSCamera(Game game): base(game)
        {
            
        }

        public override void Initialize()
        {
            eyeOffset = standingEyeOffset;
            pose.WorldPosition = Vector3.Zero + eyeOffset;
            viewMatrix = new Matrix();
            projMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi/3, Game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 10000.0f);
            this.updateViewMatrix();
            base.Initialize();
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
                updateViewMatrix();
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
                this.pose.Rotation = this.pose.Orientation;
                updateViewMatrix();
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
                this.pose.Rotation = this.pose.Orientation;
                updateViewMatrix();
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return this.Pose.WorldPosition;
            }
            set
            {
                this.pose.WorldPosition = value;
                updateViewMatrix();
            }
        }
        
        public void updateCameraPositionWithPlayer(Vector3 playerWorldPosition)
        {
            pose.WorldPosition = playerWorldPosition + EyeOffset;
        }

        public Vector3 EyeOffset
        {
            get
            {
                return this.eyeOffset;
            }
            set
            {
                this.eyeOffset = value;
            }
        }

        public override void updateViewMatrix()
        {
            cameraRotation = Matrix.CreateRotationX(pose.RotateUpDown) * Matrix.CreateRotationY(pose.RotateLeftRight);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = pose.WorldPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(pose.WorldPosition, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}
