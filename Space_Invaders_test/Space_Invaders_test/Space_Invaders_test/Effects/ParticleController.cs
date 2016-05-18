using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders_test
{
    class ParticleController
    {

        public List<Particle> particles;

        private Texture2D texture; // текстура точки

        private Random random;

        public ParticleController(Texture2D texture)
        {
            this.particles = new List<Particle>();
            random = new Random();
            this.texture = texture;
        }


        public void EngineRocket(Vector2 position) //генерируем частицы
        {
            for (int a = 0; a < 2; a++) 
            {
                Vector2 speed = AngleToV2((float)(Math.PI * 2d * random.NextDouble()), 0.6f);
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                float size = 1f;
                int ttl = 40;
                float sizeVel = 0;
                float alphaVel = 0;


                GenerateNewParticle(texture, position, speed, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }

            for (int a = 0; a < 1; a++)
            {
                Vector2 speed = AngleToV2((float)(Math.PI * 2d * random.NextDouble()), .2f);
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(1.0f, 0.5f, 0.5f, 0.5f);
                float size = 1f;
                int ttl = 80;
                float sizeVel = 0;
                float alphaVel = .01f;


                GenerateNewParticle(texture, position, speed, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }

            for (int a = 0; a < 10; a++)
            {
                Vector2 speed = Vector2.Zero;
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(1.0f, 0.5f, 0.5f, 1f);
                float size = 0.1f + 1.8f * (float)random.NextDouble();
                int ttl = 10;
                float sizeVel = -.05f;
                float alphaVel = .01f;


                GenerateNewParticle(texture, position, speed, angle, angleVel, color, size, ttl, sizeVel, alphaVel);
            }
        }


        private Particle GenerateNewParticle(Texture2D texture, Vector2 position, Vector2 speed,
            float angle, float angularVelocity, Vector4 color, float size, int ttl, float sizeVel, float alphaVel) // генерация новой частицы
        {
            Particle particle = new Particle(texture, position, speed, angle, angularVelocity, color, size, ttl, sizeVel, alphaVel);
            particles.Add(particle);
            return particle;
        }

        public void Update(GameTime gameTime)
        {

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].Size <= 0 || particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive); 

            for (int index = 0; index < particles.Count; index++) // Отрисовывем частицы
            {
                particles[index].Draw(spriteBatch,texture);
            }

            spriteBatch.End();
        }

        public Vector2 AngleToV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle) * length;
            direction.Y = -(float)Math.Sin(angle) * length;
            return direction;
        }
    }
}