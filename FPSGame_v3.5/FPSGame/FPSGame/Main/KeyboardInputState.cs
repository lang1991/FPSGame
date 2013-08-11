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
    class KeyboardInputState
    {
        KeyboardState CurrentInputState;
        KeyboardState LastInputState;

        public KeyboardInputState()
        {
            CurrentInputState = new KeyboardState();
            LastInputState = CurrentInputState;
        }

        public void Update()
        {
            LastInputState=CurrentInputState;
            CurrentInputState = Keyboard.GetState();
        }

        public bool IsKeyHeld(Keys key)
        {
            return CurrentInputState.IsKeyDown(key) && LastInputState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return CurrentInputState.IsKeyDown(key) && LastInputState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return CurrentInputState.IsKeyUp(key) && LastInputState.IsKeyDown(key);
        }

    }
}
