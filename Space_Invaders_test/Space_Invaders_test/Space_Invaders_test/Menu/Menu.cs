using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders_test.Menu
{
    class Menu
    {
        public List<MenuItem> Items { get; set; } // Список элементов меню
        private SpriteFont font;

        public int currentItem;           //Номер текущего элемента меню
        private KeyboardState prevState;  // Предыдущее состояние клавиатуры
        private Texture2D background;     //Фон 
        private Texture2D background2;     
        private Texture2D background3;
        int y;                           // Переменная определяющая положение строки меню по Y

        public Menu()
        {
            Items = new List<MenuItem>();
        }

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            bool ok = false;

            if (state.IsKeyDown(Keys.Enter)) Items[currentItem].OnClick();

            int x = 0; // Ход меню

            if (state.IsKeyDown(Keys.Up) && prevState.IsKeyUp(Keys.Up)) x = -1;
            if (state.IsKeyDown(Keys.Down) && prevState.IsKeyUp(Keys.Down)) x = 1;
            currentItem += x;

            while (!ok)
            {
                if (currentItem < 0)
                    currentItem = Items.Count - 1;
                else if (currentItem > Items.Count - 1)
                    currentItem = 0;
                else if (Items[currentItem].Active == false)
                    currentItem += x;
                else ok = true;
            }

            prevState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            y = 300;
            spriteBatch.Begin();

            if (Game1.endGame) 
                spriteBatch.Draw(background2, new Rectangle(0, 0, Game1.gameScreenWidth, Game1.gameScreenHeight),Color.White);
            else if (Game1.gameCompleted) 
                spriteBatch.Draw(background3, new Rectangle(0, 0, Game1.gameScreenWidth, Game1.gameScreenHeight), Color.White);
            else 
                spriteBatch.Draw(background, new Rectangle(0, 0, Game1.gameScreenWidth, Game1.gameScreenHeight), Color.White);

            foreach (MenuItem item in Items)
            {
                Color color = Color.White;
                if (item.Active == false) color = Color.Gray;
                if (item == Items[currentItem]) color = Color.OrangeRed;
                spriteBatch.DrawString(font, item.Name, new Vector2(Game1.gameScreenWidth/2-140, y), color);
                y += 50;
            }
            spriteBatch.End();
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>(@"Fonts/MenuFont");
            background = Content.Load<Texture2D>(@"Textures/Background/Menu/menuBack5");
            background2 = Content.Load<Texture2D>(@"Textures/Background/Menu/gameOverBack1");
            background3 = Content.Load<Texture2D>(@"Textures/Background/Menu/gameEndBack9");
        }
    }
}
