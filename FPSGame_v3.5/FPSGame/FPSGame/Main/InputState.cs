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
    /// 

    public enum MouseButtons
    {
        Left,
        Right,
    }



    public static class InputState
    {

        private static KeyboardState CurrentKeyboardState=new KeyboardState();
        private static KeyboardState LastKeyboardState = new KeyboardState();

        private static MouseState CurrentMouseState= new MouseState();
        private static MouseState LastMouseState = new MouseState();


        public static MouseState MouseState
        {
            get
            {
                return CurrentMouseState;
            }
        }

        public static KeyboardState KeyboardState
        {
            get
            {
                return CurrentKeyboardState;
            }
        }

        //Update Input State
        public static void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            LastMouseState = CurrentMouseState;
            CurrentMouseState = new MouseState();
        }

        //Detect Keyboard Input State
        public static bool IsNewKeyPress(Keys key)
        {

            return (CurrentKeyboardState.IsKeyDown(key) &&
                    LastKeyboardState.IsKeyUp(key));
        }

        public static bool IsKeyHeld(Keys key)
        {

            return (CurrentKeyboardState.IsKeyDown(key) &&
                    LastKeyboardState.IsKeyDown(key));
        }

        public static bool IsKeyReleased(Keys key)
        {

            return (CurrentKeyboardState.IsKeyUp(key) &&
                    LastKeyboardState.IsKeyDown(key));
        }
        
        //Detect Mouse Input State
        public static bool IsMousePressed(MouseButtons button)
        {

            switch (button)
            { 
                case MouseButtons.Left:
                    return CurrentMouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released;
                case MouseButtons.Right:
                    return CurrentMouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released;
                default:
                    return false;
            }
        }

        public static bool IsMouseHeld(MouseButtons button)
        {

            switch (button)
            {
                case MouseButtons.Left:
                    return CurrentMouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right:
                    return CurrentMouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Pressed;
                default:
                    return false;
            }
        }

        public static bool PlayerMoveForward
        {
            get
            {
                return IsKeyHeld(Keys.W);
            }
        }
        public static bool PlayerMoveBack
        {
            get
            {
                return IsKeyHeld(Keys.S);
            }
        }

        public static bool PlayerMoveLeft
        {
            get
            {
                return IsKeyHeld(Keys.A);
            }
        }

        public static bool PlayerMoveRight
        {
            get
            {
                return IsKeyHeld(Keys.D);
            }
        }

        public static bool PlayerCrouch
        {
            get
            {
                return IsKeyHeld(Keys.LeftControl);
            }
        }

        public static bool PlayerJump
        {
            get
            {
                return IsNewKeyPress(Keys.Space);
            }
        }

        public static bool PlayerWalk
        {
            get
            {
                return IsKeyHeld(Keys.LeftShift);
            }
        }

        public static float deltaMouseX
        {
            get
            {
                return CurrentMouseState.X - LastMouseState.X;
            }
        }

        public static float deltaMouseY
        {
            get
            {
                return CurrentMouseState.Y - LastMouseState.Y;
            }
        }

    }
}
