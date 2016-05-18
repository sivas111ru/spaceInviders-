using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders_test.Menu;
using LevelContent;

namespace Space_Invaders_test
{
    class BossBomb : BasicObject
    {
        private Rectangle objRect;
        private Boss boss;
        private int bombSpeed;
        private Random rand=new Random();
        

       public BossBomb(Texture2D sprite,Boss boss) : base (sprite)
       {
            Height = 20;
            Width = 45;
            this.boss = boss;
            IsAlive = true;
            frameSize = new Point(64, 64); 
            currentFrame = new Point(0, 0);
            sheetSize = new Point(1, 0); 
            Position = new Vector2(boss.Position.X-10, boss.Height+10);
            bombSpeed = Game1.levels[Game1.currentlevel].bombVelocity;
       }

        public void Movement (Spaceship spaceship,BossBomb enemyBomb)
        {
            Position.Y += bombSpeed;
            
            //Обработка столкновений
            objRect = new Rectangle((int)enemyBomb.Position.X, (int)enemyBomb.Position.Y, enemyBomb.Width, enemyBomb.Height);

            if (objRect.Intersects(spaceship.Border) && IsAlive)
            {
                spaceship.IsAnimated = true;
                Game1.shipExpl.Play(1, 0, 0);
                enemyBomb.IsAlive = false;
            }

            if (objRect.Y >= Game1.gameScreenHeight - objRect.Height)
            {
                enemyBomb.IsAlive = false;
            }
        }
    }
}
