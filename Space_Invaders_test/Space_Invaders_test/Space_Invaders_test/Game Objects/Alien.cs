using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders_test.Menu;

namespace Space_Invaders_test
{
    class Alien : BasicObject
    {
        public static int aliensWidth;    // Количество инопланетян в ряд
        public static int aliensHeight;   // Количество инопланетян в высоту
        protected bool ok;

        public Alien(Texture2D sprite) : base (sprite)
        {
            Width = 65;
            Height = 65;
            framestart = 0;
            fps = 30;
            frameSize = new Point(65, 65); 
            currentFrame = new Point(0, 0);
            sheetSize = new Point(25, 0);
            maxcount = sheetSize.X - 1;
            ok = true;
        }

        public void Animate(GameTime gameTime)
        {
            framestart += gameTime.ElapsedGameTime.Milliseconds;

            if (framestart > fps)
            {
                count++;
                framestart -= fps;
                currentFrame.X++;
            }

            if (count > maxcount)
            {
                IsAlive = false;
                if (ok) Game1.aliencount++;
                ok = false;

            }

        }
    }
}
