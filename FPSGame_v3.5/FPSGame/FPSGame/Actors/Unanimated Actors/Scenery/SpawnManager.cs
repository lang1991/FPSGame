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
    public class SpawnManager : Microsoft.Xna.Framework.GameComponent
    {
        public SpawnManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void SpawnGman()
        {
            for (int z = 0; z < 9; z++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Gman temp = new Gman(Game);
                    temp.DrawOrder = 1;
                    temp.Pose.WorldPosition = new Vector3(x*60,20,z*60);
                    temp.Pose.Scale = 1;
                    Game.Components.Add(temp);
                }
            }
        }
    }
}
