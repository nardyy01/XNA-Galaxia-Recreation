using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GalaxiaGameXna
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        public const int ScreenHeight = 768;
        public const int ScreenWidth = 1024;

        //game components
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SoundEffect sound;
        private Song song;
        private Color color;

        private Texture2D companionTexture;
        private Texture2D shipTexture;
        private Texture2D shipExplodingTexture;
        private Texture2D friendBullet;
        private Texture2D enemyBullet;
        private cButton buttonPlay;
        private cButton buttonQuit;
        private Texture2D startScreen;
        private List<Texture2D> star = new List<Texture2D>(5);

        private ShipSprite ship;
        private ShipCompanion shipC;
        private EnemyConductor enemyConductor;

        public Random rnd = new Random((int)DateTime.Now.Ticks);

        ////game variables
        private bool gameInProgress = false, musicPlaying = false;
        private int lives = 3;
        private int score = 0;
        public int level = 1, prevLevel = 1;
        private float goals = 10000;
        private int side = 1;
        private int c1=0, c2=0, c3=0, c4=0;

        /// <summary>
        /// GAME STATE CONTROL CENTER :)
        /// </summary>
        public enum GameState
        {
            MainMenu, // = 1
            Options, // = 2
            Playing, // = 3
            Ending,
        }
        public GameState CurrentGameState = GameState.MainMenu;
        /// <summary>
        /// END GAME STATE CC TRANSMISSION.
        /// </summary>
     

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        //This is how to expose inner stuff as a property

        public GraphicsDeviceManager Graphics
        {
            get
            {
                return this.graphics;
            }
        }


        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }

        public ShipSprite Ship
        {
            get
            {
                return this.ship;
            }
        }

        public ShipCompanion ShipCompanion
        {
            get
            {
                return this.shipC;
            }
        }

        public EnemyConductor EnemyConductor
        {
            get
            {
                return this.enemyConductor;
            }
            set
            {
                this.enemyConductor = value;
            }
        }

        public SoundEffect Sound
        {
            get
            {
                return sound;
            }
            set
            {
                sound = value;
            }
        }

        public bool GameInProgress
        {
            get
            {
                return gameInProgress;
            }
            set
            {
                gameInProgress = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }

        public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
            }
        }


        public Random Rnd
        {
            get
            {
                return rnd;
            }
        }

        public Texture2D ShipExplodingTexture
        {
            get
            {
                return shipExplodingTexture;
            }
        }

        public Texture2D ShipTexture
        {
            get
            {
                return shipTexture;
            }
        }


        public Texture2D FriendBullet
        {
            get
            {
                return this.friendBullet;
            }
        }

        public Texture2D EnemyBullet
        {
            get
            {
                return this.enemyBullet;
            }
        }



        protected override void Initialize()
        {
            base.Initialize();
        }


        private Color starColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 1:
                    return Color.White;
                case 2:
                    return Color.Red;
                case 3:
                    return Color.Magenta;
                case 4:
                    return Color.Lime;
                case 5:
                    return Color.Cyan;
                default:
                    return Color.Turquoise;
            }
        }




        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load player ship textures
            this.shipTexture = Content.Load<Texture2D>("ship");
            this.shipExplodingTexture = Content.Load<Texture2D>("Explode");

            this.ship = new ShipSprite(this.shipTexture, this);
            this.ship.Position = new Vector2(500, 700);
            this.Components.Add(this.ship);

            //Load companion
            this.companionTexture = Content.Load<Texture2D>("shipC");
            this.shipC = new ShipCompanion(this.companionTexture, this);
            this.shipC.Position = this.ship.Position + new Vector2(100,0);
            


            //load the sound here to prevent a stutter when use fires first bullet
            sound = Content.Load<SoundEffect>("fire");

            //load song (background music)
            song = Content.Load<Song>("music");
            

            // Create friend bullet
            this.friendBullet = new Texture2D(this.GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Yellow;
            this.friendBullet.SetData(data);


            // Create enemy bullet
            this.enemyBullet = new Texture2D(this.GraphicsDevice, 1, 1);
            data = new Color[1 * 1];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            this.enemyBullet.SetData(data);


            //create 6 coloured stars
            for (int i = 0; i < 6; ++i)
            {
                Texture2D newStar;

                newStar = new Texture2D(this.GraphicsDevice, 1, 1);
                data = new Color[1 * 1];
                for (int a = 0; a < data.Length; ++a) data[a] = starColor(i);
                newStar.SetData(data);
                star.Add(newStar);
            }

            //load enemies
            this.enemyConductor = new EnemyConductor(this);
            this.enemyConductor.Initialize(1);
            this.Components.Add(this.enemyConductor);

            //load buttons
            buttonPlay = new cButton(Content.Load<Texture2D>("playButton"), graphics.GraphicsDevice);

            buttonQuit = new cButton(Content.Load<Texture2D>("quitButton"), graphics.GraphicsDevice);


            //load background
            startScreen = Content.Load<Texture2D>("MainMenu");
            color = new Color(c1, c2, c3, c4);
        }


        protected override void UnloadContent()
        {

        }


        private int generateRandom(int startFrom, int endAt)
        {
            //function to return a random number
            int randomNumber = rnd.Next(startFrom, endAt);
            return randomNumber;
        }


        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    //Menu Actions
                    if (buttonPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    if (buttonQuit.isClicked == true) Exit();
                    buttonQuit.Update(mouse);
                    buttonPlay.Update(mouse);
                    break;

                case GameState.Playing:

                    //Game actions
                    if (musicPlaying == true) { }
                    else if (musicPlaying == false)
                    {
                        MediaPlayer.Play(song);
                        musicPlaying = true;
                    }

                    KeyboardState keyboardState = Keyboard.GetState();

                    //generate stars
                    for (int i = 0; i < 4; ++i)
                    {
                        StarSprite star = new StarSprite(this.star[generateRandom(0, 6)], this, TimeSpan.FromSeconds(2));
                        star.Position = new Vector2(generateRandom(0, ScreenWidth), 0);
                        this.Components.Add(star);
                    }




                    //start & continue game
                    if (keyboardState.IsKeyDown(Keys.Enter) && !ship.Enabled)
                    {
                        if (lives == 0)
                        {
                            for (int i = 0; i < enemyConductor.enemyList.Count; ++i)
                                this.Components.Remove(enemyConductor.enemyList[i]);

                            //enemyConductor.enemyList.Clear();
                            this.enemyConductor.StartTime = gameTime;
                            this.enemyConductor.EnemiesGoingLeft = false;
                            this.enemyConductor.Initialize(1);

                            score = 0;
                            lives = 3;
                            level = 1;
                            goals = 10000;
                            ship.Enabled = true;
                            ship.Visible = true;

                            CurrentGameState = GameState.Ending;
                        }
                        else
                        {
                            goals = score + goals;
                            ship.Enabled = true;
                            ship.Visible = true;
                        }
                    }

                    if(score >= goals)
                    {
                        //Load companion
                        this.shipC = new ShipCompanion(this.companionTexture, this);
                        this.shipC.Position = this.ship.Position + new Vector2(side * 50, 0);
                        this.Components.Add(this.shipC);
                        this.shipC.Counter = 1000;
                        goals *= 1.5f;
                        side *= -1;
                    }

                    //respawn mobs if player wipes them all out
                    if (EnemyConductor.enemyList.Count == 0 && level % 2 != 0)
                    {
                        level++;
                        enemyConductor.enemyList.Clear();
                        this.enemyConductor.EnemiesGoingLeft = false;
                        this.enemyConductor.StartTime = gameTime;
                        this.enemyConductor.Initialize(1);
                    }
                    else if(EnemyConductor.enemyList.Count == 0 && level % 2 == 0)
                    {
                        level++;
                        enemyConductor.enemyList.Clear();
                        this.enemyConductor.EnemiesGoingLeft = false;
                        this.enemyConductor.StartTime = gameTime;
                        this.enemyConductor.Initialize(2);
                    }
                    //Change background color every level...
                    
                    /*
                    if(level > prevLevel)
                    {
                        if (c4 < 100)
                        {
                            c1 += 25; c2 += 25; c3 += 25; c4 += 25;
                            color = new Color(c1, c2, c3);
                        }
                        else
                        {
                            c1 -= 25; c2 -= 25; c3 -= 25; c4 += 25;
                            color = new Color(c1, c2, c3);
                            if (c4 == 200) c4 = 0;
                        }

                        prevLevel = level;
                    }
                    */
                    break;
                case GameState.Ending:
                    KeyboardState kb = Keyboard.GetState();
                    if (kb.IsKeyDown(Keys.Space))
                    {
                        enemyConductor.enemyList.Clear();
                        CurrentGameState = GameState.MainMenu;
                    }
                    break;
              
            }

            base.Update(gameTime);
        }






        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color);
            spriteBatch.Begin();
            base.Draw(gameTime);
            ////////////////////
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(startScreen, new Rectangle(0,0,1024,768), Color.White);
                    buttonPlay.Draw(spriteBatch, new Rectangle(125, 570, 175, 47));
                    buttonQuit.Draw(spriteBatch, new Rectangle(115, 630, 175, 40));

                    break;
                case GameState.Playing:
                    SpriteFont font;
                    font = Content.Load<SpriteFont>("textLives");


                    //score
                    spriteBatch.DrawString(font, "Score", new Vector2(5, 0), Color.Red);
                    spriteBatch.DrawString(font, score.ToString(), new Vector2(5, 20), Color.White);

                    //lives
                    spriteBatch.DrawString(font, "Lives", new Vector2(5, 700), Color.White);
                    spriteBatch.DrawString(font, lives.ToString(), new Vector2(5, 720), Color.White);

                    //level
                    spriteBatch.DrawString(font, "Level:", new Vector2(900, 700), Color.White);
                    spriteBatch.DrawString(font, level.ToString(), new Vector2(980, 700), Color.White);

                    break;
                case GameState.Ending:
                    SpriteFont font1;
                    font1 = Content.Load<SpriteFont>("textLives");

                    spriteBatch.DrawString(font1, "Leaderboard", new Vector2(graphics.PreferredBackBufferWidth/2, 0), Color.White);
                    
                    break;
            }

            //////////////////////
            spriteBatch.End();

        }
    }
}
