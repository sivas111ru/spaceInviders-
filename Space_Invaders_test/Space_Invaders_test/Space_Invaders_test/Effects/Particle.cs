using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders_test
{
    public class Particle
    {

        public Texture2D Texture { get; set; }        // Текстура частички
        public Vector2 Position { get; set; }        // Позиция частички
        public Vector2 Speed { get; set; }       // Скорость частички
        public float Angle { get; set; }           // Угол поворота частички
        public float AngularSpeed { get; set; }// Угловая скорость
        public Vector4 Color { get; set; }       // Цвет частички
        public float Size { get; set; }         // Размер
        public float SizeVel { get; set; }	   // Скорость уменьшения размера
        public float AlphaVel { get; set; }	  // Скорость уменьшения альфы
        public int TTL { get; set; }         // Время жизни частички

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularSpeed, Vector4 color, float size, int ttl, float sizeVel, float alphaVel) // конструктор
        {
            Texture = texture;
            Position = position;
            Speed = velocity;
            Angle = angle;
            Color = color;
            AngularSpeed = angularSpeed;
            Size = size;
            SizeVel = sizeVel;
            AlphaVel = alphaVel;
            TTL = ttl;
        }

        public void Update()
        {
            TTL--;

            // Меняем параметры в соответствии со скоростями
            Position += Speed;
            Angle += AngularSpeed;
            Size += SizeVel;

            Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel);

        }

        public void Draw(SpriteBatch spriteBatch,Texture2D textue)
        {

            Rectangle sourceRectangle = new Rectangle(0, 0, textue.Width, textue.Height); // вся область из текстуры
            Vector2 origin = new Vector2(textue.Width / 2, textue.Height / 2); // центр

            spriteBatch.Draw(textue, Position, sourceRectangle, new Color(Color), Angle, origin, Size, SpriteEffects.None, 0); // акт прорисовки

        }

    }
}