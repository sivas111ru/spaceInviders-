using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders_test.Menu;

namespace  Space_Invaders_test
{
    class BasicObject
    {
        public Texture2D Sprite;  // Спрайт
        public Vector2 Position;  // Положение
        public Vector2 Speed;     // Скорость
        public bool IsAlive;      // Жив ли обьект
        public bool IsAnimated;   // Анимирован ли объект
        public int Width;         // Ширина
        public int Height;        // Высота


        protected Point frameSize;     // Размер фрейма отрисовки
        protected Point currentFrame; // Положение текущего фрейма
        protected Point sheetSize;   // Размер спрайт-листа
        protected int framestart;   // Начальный фрейм
        protected int fps = 30;    // Скорость отрисовки фреймов
        protected int count;
        protected int maxcount;

        public Rectangle Border //Границы обьекта
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        public BasicObject(Texture2D sprite)
        {
            Sprite = sprite;
            Width = sprite.Width;
            Height = sprite.Height;
            IsAlive = true;
            Position = Vector2.Zero;
            Speed = Vector2.Zero;
            framestart = 0;
            count = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position,
            new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }


        public void Animate(GameTime gameTime)
        {
            framestart += gameTime.ElapsedGameTime.Milliseconds; //начиная с 0, инкрементируем кадры

            if (framestart > fps) //Если превышаем fps отрисовывем следующий фрейм
            {
                count++;
                framestart -= fps;
                currentFrame.X++;
            }
            // Проверка отрисовки анимации
            if (count > maxcount)  IsAlive = false;
       }

    }
}
