using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders_test.Menu;

namespace Space_Invaders_test
{
    class Boss : BasicObject
    {
        private Random rand = new Random();

         public Boss(Texture2D sprite) : base (sprite)
         {
            Height = 60;
            Width = 60;
            framestart = 0;
            fps = 70;
            Position = new Vector2(rand.Next(0, Game1.gameScreenWidth - Width),30);
            frameSize = new Point(66, 64);
            currentFrame = new Point(0, 0);
            sheetSize = new Point(25, 0);
            maxcount = sheetSize.X - 1;
        }

    }
}
