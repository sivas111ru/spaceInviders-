using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Invaders_test.Menu;

namespace Space_Invaders_test
{
    class Spaceship: BasicObject
    {

        public Spaceship(Texture2D sprite) : base (sprite)
        {
            Width = 115;
            Height = 128;
            frameSize = new Point(128, 128); 
            currentFrame = new Point(0, 0);
            sheetSize = new Point(32, 0);
            maxcount = sheetSize.X - 1;
            fps = 35;
        }
      

        public void Move(KeyboardState keyboardState)
        {
            if (!IsAnimated)
            {
                // Двигаем корабль вправо
                if (keyboardState.IsKeyDown(Keys.Right)) Position.X += 7;
                
                // Двигаем корабль влево
                if (keyboardState.IsKeyDown(Keys.Left)) Position.X -= 7;
            }
            // Ограничиваем движение корабля игровым полем
            Position.X = MathHelper.Clamp(Position.X, 0, Game1.gameScreenWidth - Width);
        }

    }
}
