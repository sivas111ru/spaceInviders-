using System;
using LevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders_test.Menu
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static LevelConfig configuration;  //XML файл содержащий основные настройки игры
        public static int gameScreenWidth;
        public static int gameScreenHeight;

        private Texture2D background;               // Фон игрового поля
        private Texture2D alienSprite;              // Спрайт инопланетянина
        public static Texture2D bombSmoke;          // Текстура дыма бомбы
        public static Texture2D rocketSmoke;        // Текстура дыма ракеты

        public static SoundEffect shipFire;        
        public static SoundEffect dropBomb;
        public static SoundEffect bombExpl;
        public static SoundEffect alienExpl;
        public static SoundEffect shipExpl;
        public static SoundEffect bossExpl;

        private Menu menu;
        private Gamestate gameState = Gamestate.Menu;

        public static Level[] levels;
        private Spaceship spaceship;                
        private Rocket rocket;
        private Alien[,] aliens;      
        private Boss boss;      // Монстр
        private BossBomb[] bossBombs;
        private ParticleController particleEffects;
        private ParticleController particleEffects2;


        private int maxlevel;           //Количество описанных уровней в XML
        public static int currentlevel;  
        private double roundTime;      // Время игры с 0
        private double leftRoundTime;  // Время отведенное под раунд 
        private double roundTimeLeft;  // Время до окончания раунда 
        private int maxlives;          // Количество жизней (XML)
        public static int lives;       // Количество жизней
        private static int bosspeed;
        private static float alienSpeed; 
        public static int aliencount;  // Переменная для подсчета количества уничтоженных инопланетян за уровень
        private int alienTotal;        // Общее количество инопланетян на уровне (XML)
        private static int bombTotal;  // Количество скидываемых бомб
        private int bombcount;         // Переменная для учета количества бомб
        private int bombtimer;         // Переменная для учета интенсивности сбрасывания бомб 
        private int bombfreq;          // Интенсивность

        public static bool getPressedSpace { get; set; }   // Нажат ли space
        public static bool animate { get; set; }           // Активная ли анимация
        public static bool endGame;                        // Триггер окончания раунда  
        public static bool gameCompleted;                  // Триггер окончания игры
        private bool vector = false;                       // Вектор направления инопланетян (right/left) 
        private bool vector2 = false;                      // Вектор направления монстра (right/left)
        float pressedTime = 0;                             // Переменная для учета времени

        private Random rand;
        SpriteFont font;

        internal enum Gamestate
        {
            Game,
            Menu,
            End
        }
     

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            configuration = Content.Load<LevelConfig>(@"Levels/LevelsConfiguration");
            graphics.PreferredBackBufferWidth = configuration.PreferredBackBufferWidth;
            graphics.PreferredBackBufferHeight = configuration.PreferredBackBufferHeight;

        }

        protected override void Initialize()
        {
            gameScreenHeight = configuration.gameScreenHeight;
            gameScreenWidth = configuration.gameScreenWidth;
            maxlevel = configuration.maxlevel;
            maxlives = configuration.maxlives;
            levels = new Level[maxlevel];
            menu = new Menu();
            MenuItem newGame = new MenuItem("New Game");
            MenuItem resumeGame = new MenuItem("Continue");
            MenuItem exitGame = new MenuItem("Exit");
            resumeGame.Active = false;

            for (int i = 0; i < maxlevel; i++) // Загружаем описание уровней из XML файлов
            {

                levels[i] = Content.Load<Level>(@"Levels/Level" + (i + 1).ToString());
            }
 
            newGame.Click += new EventHandler(newGame_Click);
            resumeGame.Click += new EventHandler(resumeGame_Click);
            exitGame.Click += new EventHandler(exitGame_Click);

            menu.Items.Add(newGame);
            menu.Items.Add(resumeGame);
            menu.Items.Add(exitGame);

            base.Initialize();
        }

        void exitGame_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        void resumeGame_Click(object sender, EventArgs e)
        {
            gameState = Gamestate.Game;
        }

        void newGame_Click(object sender, EventArgs e)
        {

            gameCompleted = false;
            menu.Items[1].Active = true;
            gameState = Gamestate.Game;
            currentlevel = configuration.startLevel;
            alienSpeed = levels[currentlevel].alienVelocity;
            bombfreq = levels[currentlevel].bombFreq;
            lives = maxlives;
            bosspeed = (int)alienSpeed * 2;
            spaceship.IsAlive = true;

            LevelInit();
            InitObj();
            InitTexture();

            aliencount = 0;
            roundTime = 0;

        }

        protected override void LoadContent()
        {
            menu.LoadContent(Content);
            SoundInit();              
            LevelInit();             
            InitTexture();          
            InitObj();

        }

        private void SoundInit()
        {
            shipFire = Content.Load<SoundEffect>(@"Sounds/shipfire3");
            alienExpl = Content.Load<SoundEffect>(@"Sounds/alienExpl1");
            dropBomb = Content.Load<SoundEffect>(@"Sounds/bomb");
            bossExpl = Content.Load<SoundEffect>(@"Sounds/bossExpl3");
            bombExpl = Content.Load<SoundEffect>(@"Sounds/bombExpl1");
            shipExpl = Content.Load<SoundEffect>(@"Sounds/shipExpl2");
        }

        private void LevelInit()
        {
            endGame = false;
            background = Content.Load<Texture2D>(@"Textures/Background/Levels/"+levels[currentlevel].textureName);
            alienSpeed = levels[currentlevel].alienVelocity;
            bosspeed = (int)(alienSpeed * 2);
            bombTotal = levels[currentlevel].bombCount;
            bombfreq = levels[currentlevel].bombFreq;
            Alien.aliensWidth = levels[currentlevel].alienCount.X;
            Alien.aliensHeight = levels[currentlevel].alienCount.Y;
            alienTotal = Alien.aliensHeight * Alien.aliensWidth;
            leftRoundTime = levels[currentlevel].timeForLevel;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (gameState == Gamestate.Game)
                GameUpdate(gameTime);
            else menu.Update();
        }

        private void GameUpdate(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            pressedTime = gameTime.TotalGameTime.Milliseconds;
            bombtimer += gameTime.ElapsedGameTime.Milliseconds;

            if (currentlevel == maxlevel - 1 && spaceship.IsAlive && aliencount >= alienTotal && boss.IsAlive == false) // Проверка окончания игры
                gameCompleted = true;

            if (keyboardState.IsKeyDown(Keys.Escape)) 
                gameState = Gamestate.Menu;

            if (gameState == Gamestate.End)
            {
                menu.Items[1].Active = false; 
                gameState = Gamestate.Menu;
            }

            if (lives <= 0)
            {
                endGame = true;
                gameState = Gamestate.End;
                menu.currentItem = menu.Items.Count - 3;
                menu.Items[1].Active = false;
            }

            if (roundTimeLeft <= 0)
            {
                LevelInit();
                InitObj();
                lives--;
                alienSpeed = levels[currentlevel].alienVelocity;
                aliencount = 0;
                bosspeed = (int)alienSpeed * 2;
                spaceship.IsAlive = true;
                roundTime = 0;

            }

            if (spaceship.IsAlive && aliencount >= alienTotal && boss.IsAlive == false)  // Проверка окончания рануда (победа в раунде)
            {
                if (currentlevel == maxlevel - 1)
                {
                    endGame = true;
                    gameState = Gamestate.End;
                    menu.currentItem = menu.Items.Count - 3;
                    menu.Items[1].Active = false;
                }
                else currentlevel++;
            }


            if (spaceship.IsAlive && (!((boss.IsAlive == false && (aliencount >= alienTotal))))) // Проверка окончания раунда
            {

                spaceship.Move(keyboardState);
                alienSpeed = MathHelper.Clamp(alienSpeed, 2, 12);  // Ограничение скорости инопланетян

                if (!rocket.IsAlive)
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        getPressedSpace = true;
                        rocket.Position = new Vector2(spaceship.Position.X + 65,Game1.gameScreenHeight - spaceship.Height - rocket.Height + 40);
                        rocket.Speed = new Vector2(0, -14);
                        shipFire.Play(1, 0, 0);
                    }

                if (getPressedSpace == true && !spaceship.IsAnimated)
                {
                    rocket.RocketFlight(rocket, aliens, bossBombs, boss); 
                }

                if (spaceship.IsAnimated) spaceship.Animate(gameTime);

                rand = new Random((int)DateTime.Now.Ticks);

                if (bombcount >= bombTotal) bombcount = 0;

                if (bossBombs[bombcount].IsAlive == false && boss.IsAlive && !boss.IsAnimated) // Проверка на возможность сброса бомбы
                {
                    if (bombtimer > rand.Next(bombfreq, bombfreq * 50))
                    {
                        bossBombs[bombcount] = new BossBomb(Content.Load<Texture2D>(@"Textures/bomb-5"), boss);
                        dropBomb.Play(1, 0, 0);
                        bombtimer = 0;
                    }
                }

                bombcount++;

                if (aliencount == alienTotal)  // Если все инопланетяне убиты, ускоряем монстра
                {
                    bombfreq = bombfreq / (int)(bombfreq * 0.05);
                    bosspeed = bosspeed * 2;
                    bosspeed = (int)MathHelper.Clamp(bosspeed, 0, alienSpeed * 4);
                }



                if (pressedTime > 10) // Плавность игры
                {
                    roundTime += 1;
                    roundTimeLeft = leftRoundTime - roundTime/54.812;

                    for (int i = 0; i < bombTotal; i++)
                    {
                        bossBombs[i].Movement(spaceship, bossBombs[i]);
                    }

                    foreach (Alien al in aliens)
                    {
                        if (al.Position.X < 0 && al.IsAlive)
                        {
                            vector = true;
                            if (!(boss.IsAlive)) alienSpeed++;
                        }

                        if (al.Position.X > Game1.gameScreenWidth - al.Width && al.IsAlive) vector = false;
                    }



                    foreach (Alien al in aliens)  // Направление движения инопланетян
                    {
                        if (vector == true && !al.IsAnimated)  al.Position.X += alienSpeed; 
                        if (vector == false && !al.IsAnimated) al.Position.X -= alienSpeed;
                    }

                    //Направление движение монстра
                    if (boss.Position.X < 0 && boss.IsAlive) vector2 = true;
                    if (boss.Position.X > Game1.gameScreenWidth - boss.Width && boss.IsAlive) vector2 = false;
                    if (vector2 == true && !boss.IsAnimated) boss.Position.X += bosspeed;
                    if (vector2 == false && !boss.IsAnimated) boss.Position.X -= bosspeed;

                }
            }
            else if (boss.IsAlive == false && (aliencount >= alienTotal))
            {
                LevelInit();
                InitObj();
                spaceship.IsAlive = true;
                aliencount = 0;
                alienSpeed = levels[currentlevel].alienVelocity;
                bosspeed = (int)alienSpeed * 2;
                roundTime = 0;
            }
            else
            {
                LevelInit();
                InitObj();
                lives--;
                alienSpeed = levels[currentlevel].alienVelocity;
                aliencount = 0;
                bosspeed = (int)alienSpeed * 2;
                spaceship.IsAlive = true;
                roundTime = 0;

            }

            foreach (Alien al in aliens)  
                if (al.IsAnimated) al.Animate(gameTime);

            if (boss.IsAnimated) boss.Animate(gameTime);
       
            particleEffects.Update(gameTime);
            particleEffects2.Update(gameTime);

            if (rocket.IsAlive) particleEffects.EngineRocket(new Vector2(rocket.Position.X, rocket.Position.Y));

            foreach (BossBomb bossBomb in bossBombs)
            {
                if (bossBomb.IsAlive) particleEffects2.EngineRocket(new Vector2(bossBomb.Position.X + 55, bossBomb.Position.Y));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (gameState == Gamestate.Game) DrawGame();
            else if (gameState == Gamestate.Menu) menu.Draw(spriteBatch);
            else if (gameState == Gamestate.End) menu.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.gameScreenWidth, Game1.gameScreenHeight), Color.White);

            if (spaceship.IsAlive) spaceship.Draw(spriteBatch);

            foreach (var al in aliens)
                if (al.IsAlive) al.Draw(spriteBatch);

            if (rocket.IsAlive == true) spriteBatch.Draw(rocket.Sprite, rocket.Position, Color.White);

            foreach (BossBomb bossBomb in bossBombs)
                if (bossBomb.IsAlive) bossBomb.Draw(spriteBatch);
    
            if (boss.IsAlive) boss.Draw(spriteBatch);

            if (lives <=1) 
                spriteBatch.DrawString(font, "Ships: " + lives.ToString(), new Vector2(20, 3), Color.OrangeRed);
            else 
                spriteBatch.DrawString(font, "Ships: " + lives.ToString(), new Vector2(20, 3), Color.GhostWhite);

            if (roundTimeLeft > 10) 
                spriteBatch.DrawString(font, "Time: " + Convert.ToInt32(roundTimeLeft).ToString(), new Vector2(gameScreenWidth - 95, 3), Color.GhostWhite);
            else 
                spriteBatch.DrawString(font, "Time: " + Convert.ToInt32(roundTimeLeft).ToString(), new Vector2(gameScreenWidth - 95, 3), Color.OrangeRed);

            spriteBatch.DrawString(font, "Level: " + (currentlevel + 1).ToString(), new Vector2((int)(gameScreenWidth / 2) - 56, 4), Color.Gray);

            spriteBatch.End();

            particleEffects.Draw(spriteBatch);
            particleEffects2.Draw(spriteBatch);
        }


        public void InitTexture()
        {
            bombSmoke = Content.Load<Texture2D>(@"Textures/blueb");
            rocketSmoke = Content.Load<Texture2D>(@"Textures/orbz-fire");
            font = Content.Load<SpriteFont>(@"Fonts/font");
            alienSprite = Content.Load<Texture2D>(@"Textures/Sprites/alienanimfire");

        }

        public void InitObj()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            particleEffects2 = new ParticleController(bombSmoke);
            particleEffects = new ParticleController(rocketSmoke);
            spaceship = new Spaceship(Content.Load<Texture2D>(@"Textures/Sprites/spaceshpinfire"));
            spaceship.Position = new Vector2((int)(Game1.gameScreenWidth - spaceship.Width) / 2, Game1.gameScreenHeight - 130);
            rocket = new Rocket(Content.Load<Texture2D>(@"Textures/rocket"));
            boss = new Boss(Content.Load<Texture2D>(@"Textures/Sprites/bossanim2"));
            bossBombs = new BossBomb[bombTotal];

            for (int i = 0; i < bombTotal; i++)
                bossBombs[i] = new BossBomb(Content.Load<Texture2D>(@"Textures/bomb-5"), boss);

            aliens = new Alien[Alien.aliensWidth, Alien.aliensHeight];

            for (int i = 0; i < Alien.aliensWidth; i++)
                for (int j = 0; j < Alien.aliensHeight; j++)
                {
                    aliens[i, j] = new Alien(alienSprite);
                    aliens[i, j].Position = new Vector2(i * 85 + 150, j * 77 + 200);
                }

            roundTimeLeft = 1;

        }
    }


}
