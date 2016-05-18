using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders_test.Menu;

namespace Space_Invaders_test
{
    class Rocket : BasicObject
    {
        protected GameTime gameTime=new GameTime();
        protected Rocket rocket;
        protected Alien[,] aliens;
        protected Rectangle objRect;
        private BossBomb[] bomb;
        private Boss boss;


        public Rocket(Texture2D sprite) : base (sprite)
        {
            IsAlive = false;
            Width = 5;
            Height = 31;
        }

        public void RocketFlight(Rocket rocket, Alien[,] aliens, BossBomb[] bomb,Boss boss)
        {
            this.rocket = rocket;
            this.aliens = aliens;
            this.bomb = bomb;
            this.boss = boss;

            rocket.IsAlive = true;
            objRect = new Rectangle((int)rocket.Position.X, (int)rocket.Position.Y, rocket.Width, rocket.Height);

            rocket.Position += rocket.Speed;

            //Обработка столкновений
            if (objRect.Y <= 0)
                rocket.IsAlive = false;
            if (objRect.Y >= Game1.gameScreenHeight - objRect.Height)
            {
                rocket.IsAlive = false;
            }
            // Столкновение с инопланетян
            for (int i = 0; i < Alien.aliensWidth; i++)
                for (int j = 0; j < Alien.aliensHeight; j++)
                {
                    if (objRect.Intersects(aliens[i, j].Border) && aliens[i, j].IsAlive && (!aliens[i,j].IsAnimated))
                    {
                        Game1.alienExpl.Play(1, 0, 0);
                        aliens[i, j].IsAnimated = true;
                        rocket.IsAlive = false;
                        Game1.getPressedSpace = false;
                    }
                }

            // Столкновение с бомбой
            foreach (BossBomb bossBomb in bomb)
            {

                if (objRect.Intersects(bossBomb.Border) && bossBomb.IsAlive)
                {
                    Game1.getPressedSpace = false;
                    Game1.bombExpl.Play(1, 0, 0);
                    bossBomb.IsAlive = false;
                    rocket.IsAlive = false;

                }
            }

            // Столкновение с монстром
            if (objRect.Intersects(boss.Border) && boss.IsAlive && rocket.IsAlive)
            {
                boss.IsAnimated = true;
                Game1.bossExpl.Play(1, 0, 0);
                rocket.IsAlive = false;
                Game1.getPressedSpace = false;

            }
        }
    }
}
