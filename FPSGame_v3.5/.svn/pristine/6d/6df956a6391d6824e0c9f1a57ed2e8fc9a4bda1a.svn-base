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
    public class EffectManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<GeometricPrimitive> lasers;
        GraphicsDevice graphicsDevice;
        BasicEffect effect;
        FPSCamera camera;

        public EffectManager(Game1 game,GraphicsDevice graphicsDevice,BasicEffect effect)
            : base(game)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;
            this.effect.EnableDefaultLighting();
            lasers = new List<GeometricPrimitive>();
            camera = (FPSCamera)Game.Services.GetService(typeof(FPSCamera));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            updateLaserBeams(gameTime);
        }

        private void updateLaserBeams(GameTime gameTime)
        {
            for (int i = 0; i < lasers.Count; i++)
            {
                lasers[i].update(gameTime);
                if (lasers[i].delete)
                {
                    lasers.Remove(lasers[i]);
                    break;
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            drawLaserBeams();
        }

        private void drawLaserBeams()
        {
            foreach (GeometricPrimitive gp in lasers)
            {
                gp.Draw(camera.ViewMatrix, camera.ProjMatrix);
            }
        }

        public void addNewLaser(Vector3 startPoint,Vector3 endPoint,Color c)
        {
            Console.WriteLine(startPoint+"    "+endPoint);
            lasers.Add(new CylinderPrimitive(this.graphicsDevice, this.effect, startPoint, endPoint, c));
        }
    }
}
