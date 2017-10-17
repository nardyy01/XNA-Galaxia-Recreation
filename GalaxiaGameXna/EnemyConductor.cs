using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalaxiaGameXna
{
    public class EnemyConductor : IGameComponent, IUpdateable
    {
        private int updateOrder;
        private bool enabled;
        private List<EnemySprite> enemies;
        private Main game;
        private Texture2D shipImage1;
        private Texture2D shipImage2;
        private Texture2D shipImage3;
        private Texture2D shipImage4;
        private Texture2D shipImage5;
        private Texture2D shipImage6;
        private Texture2D shipImage7;
        private Texture2D shipImage8;
        private Texture2D bossImage6;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        private TimeSpan lastFireTimeEnemy, bossGetsMad;

        private TimeSpan? startTime;
        private TimeSpan journeyTime;
        private float distanceToTravel;
        private bool enemiesGoingLeft;
        public int bossOrNot, monster;
        private int timer = 0;

        public EnemyConductor(Main game)
        {
            this.game = game;
            this.enemies = new List<EnemySprite>();
            this.Enabled = true;

            journeyTime = TimeSpan.FromSeconds(3);
            distanceToTravel = 250;
            enemiesGoingLeft = false;

            this.shipImage1 = game.Content.Load<Texture2D>("bat");
            this.shipImage2 = game.Content.Load<Texture2D>("pump");
            this.shipImage3 = game.Content.Load<Texture2D>("smallPumpkin");
            this.shipImage4 = game.Content.Load<Texture2D>("enemy1");
            this.shipImage5 = game.Content.Load<Texture2D>("enemy2");
            this.shipImage6 = game.Content.Load<Texture2D>("enemy3");
            this.shipImage7 = game.Content.Load<Texture2D>("minion1");
            this.shipImage8 = game.Content.Load<Texture2D>("minion2");
            this.bossImage6 = game.Content.Load<Texture2D>("bossAlien");
        }


        public void Initialize()
        {
            
        }
        public void Initialize(int x)
        {
            //Create ships here
            createEnemyShips(x);
            bossOrNot = x;
        }

        private void createEnemyShips(int x)
        {
            //if not a boss do this...
            if(x == 1)
            {
                //Halloween mobs (change mobs @ X level)
                if (game.level < 6)
                {
                    for (int i = 0; i <= 14; ++i)
                    {
                        int left = 50 + (i * 45);
                        createEnemy(shipImage1, left, 240);
                        createEnemy(shipImage1, left, 200);
                        createEnemy(shipImage1, left, 160);
                    }

                    for (int i = 0; i <= 12; ++i)
                    {
                        int left = 95 + (i * 45);
                        createEnemy(shipImage2, left, 120);
                        createEnemy(shipImage2, left, 80);
                    }

                    for (int i = 0; i <= 7; ++i)
                    {
                        int left = 160 + (i * 55);
                        createEnemy(shipImage3, left, 25);
                    }
                }
                //Change the mobs here
                else if (game.level > 5)
                {
                    for (int i = 0; i <= 17; ++i)
                    {
                        
                        if (i % 3 == 0) //skip this iteration
                            continue;

                        int left = -30 + (i * 45);
                        createEnemy(shipImage4, left, 240);
                        createEnemy(shipImage4, left, 200);
                        createEnemy(shipImage4, left, 160);
                    }

                    for (int i = 0; i <= 14; ++i)
                    {
                        int left = 55 + (i * 45);
                        createEnemy(shipImage5, left, 120);
                        createEnemy(shipImage5, left, 80);
                    }

                    for (int i = 0; i <= 10; ++i)
                    {
                        int left = 20 + (i * 70);
                        createEnemy(shipImage6, left, 25);
                    }
                }
            }
            //if boss spawns do this...
            else if (x != 1)
            {
                //Generate a random boss monster
                monster = generateRandom(0, 11);

                if (monster < 3) //Alien Boss
                {
                    for (int i = 0; i <= 0; ++i)
                    {
                        int left = 30 + (i * 100);
                        createEnemy(bossImage6, 1024/2 - bossImage6.Width, 25, 150);
                    }
                }
                else if (monster < 6)
                {
                    for (int i = 0; i <= 12; ++i)
                    {
                        int left = 40 + (i * 60);
                        createEnemy(shipImage4, left, 120,2);
                        createEnemy(shipImage5, left, 80,2);
                    }
                    for (int i = 0; i <= 7; ++i)
                    {
                        int left = 30 + (i * 100);
                        createEnemy(shipImage4, left, 25, 3);
                    }
                }
                else if (monster < 8)
                {
                    for (int i = 0; i <= 12; ++i)
                    {
                        int left = 40 + (i * 60);
                        createEnemy(shipImage5, left, 120);
                        createEnemy(shipImage6, left, 80);
                    }
                    for (int i = 0; i <= 7; ++i)
                    {
                        int left = 30 + (i * 100);
                        createEnemy(shipImage5, left, 25, 3);
                    }
                }
                else if (monster < 12)
                {
                    for (int i = 0; i <= 12; ++i)
                    {
                        int left = 40 + (i * 60);
                        createEnemy(shipImage6, left, 120);
                        createEnemy(shipImage4, left, 80);
                    }
                    for (int i = 0; i <= 7; ++i)
                    {
                        int left = 30 + (i * 100);
                        createEnemy(shipImage6, left, 25, 3);
                    }
                }
            }
        }


        public void createEnemy(Texture2D imageName, int left, int top , int health = 1)
        {
            EnemySprite enemy = new EnemySprite(imageName, this.game);
            enemy.Position = new Vector2(left, top);

            enemy.StartPosition = enemy.Position;
            enemy.Health = health;
            this.enemies.Add(enemy);
            this.game.Components.Add(enemy);
        }

        public void Update(GameTime gameTime)
        {
            switch (this.game.CurrentGameState)
            {
                case Main.GameState.MainMenu:

                    break;

                case Main.GameState.Playing: 

                    //IF NOT BOSS DO THIS...
                    if (bossOrNot == 1)
                    {
                        //Logic for manuevouring
                        if (!this.startTime.HasValue)
                        {
                            this.startTime = gameTime.TotalGameTime;
                        }
                        else
                        {

                            if (enemiesGoingLeft)
                            {
                                float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                                for (int i = 0; i < enemyList.Count; ++i)
                                    enemyList[i].Position = new Vector2(enemyList[i].StartPosition.X - (percentProgress * distanceToTravel), enemyList[i].StartPosition.Y);

                                if (percentProgress > 1)
                                {
                                    this.startTime = gameTime.TotalGameTime;
                                    for (int i = 0; i < enemyList.Count; ++i)
                                        enemyList[i].StartPosition = new Vector2(enemyList[i].Position.X, enemyList[i].Position.Y);
                                    enemiesGoingLeft = false;
                                }
                            }
                            else
                            {
                                float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                                for (int i = 0; i < enemyList.Count; ++i)
                                    enemyList[i].Position = new Vector2(enemyList[i].StartPosition.X + (percentProgress * distanceToTravel), enemyList[i].StartPosition.Y);

                                if (percentProgress > 1)
                                {
                                    this.startTime = gameTime.TotalGameTime;
                                    for (int i = 0; i < enemyList.Count; ++i)
                                        enemyList[i].StartPosition = new Vector2(enemyList[i].Position.X, enemyList[i].Position.Y);
                                    enemiesGoingLeft = true;
                                }
                            }
                        }

                        //fire enemy bullets
                        if (lastFireTimeEnemy < gameTime.TotalGameTime && enemyList.Count > 0)
                        {
                            BulletSprite bullet = new BulletSprite(shipImage2, game, false, TimeSpan.FromSeconds(game.level), 34, 26);
                            bullet.Position = new Vector2(game.EnemyConductor.enemyList[generateRandom(0, game.EnemyConductor.enemyList.Count)].Position.X + 17,
                                                        game.EnemyConductor.enemyList[generateRandom(0, game.EnemyConductor.enemyList.Count)].Position.Y + 32);
                            game.Components.Add(bullet);

                            lastFireTimeEnemy = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(0.5));
                        }
                    }

                    //IF BOSS DO THIS...
                    else if (bossOrNot != 1)
                    {
                        //Anything but the alien
                        if (monster >= 3)
                        {
                            //Logic for manuevouring
                            if (!this.startTime.HasValue)
                            {
                                this.startTime = gameTime.TotalGameTime;
                            }
                            else
                            {

                                if (enemiesGoingLeft)
                                {
                                    float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                                    for (int i = 0; i < enemyList.Count; ++i)
                                        enemyList[i].Position = new Vector2(enemyList[i].StartPosition.X - (percentProgress * distanceToTravel), enemyList[i].StartPosition.Y);

                                    if (percentProgress > 1)
                                    {
                                        this.startTime = gameTime.TotalGameTime;
                                        for (int i = 0; i < enemyList.Count; ++i)
                                            enemyList[i].StartPosition = new Vector2(enemyList[i].Position.X, enemyList[i].Position.Y);
                                        enemiesGoingLeft = false;
                                    }
                                }
                                else
                                {
                                    float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                                    for (int i = 0; i < enemyList.Count; ++i)
                                        enemyList[i].Position = new Vector2(enemyList[i].StartPosition.X + (percentProgress * distanceToTravel), enemyList[i].StartPosition.Y);

                                    if (percentProgress > 1)
                                    {
                                        this.startTime = gameTime.TotalGameTime;
                                        for (int i = 0; i < enemyList.Count; ++i)
                                            enemyList[i].StartPosition = new Vector2(enemyList[i].Position.X, enemyList[i].Position.Y);
                                        enemiesGoingLeft = true;
                                    }
                                }
                            }
                            //fire boss bullets
                            if (lastFireTimeEnemy < gameTime.TotalGameTime && enemyList.Count > 0)
                            {
                                for (int i = 1; i <= 3; i++)
                                {
                                    float posX = game.EnemyConductor.enemyList[generateRandom(0, game.EnemyConductor.enemyList.Count)].Position.X;
                                    float posY = game.EnemyConductor.enemyList[generateRandom(0, game.EnemyConductor.enemyList.Count)].Position.Y;
                                    BulletSprite bullet = new BulletSprite(shipImage2, game, false, TimeSpan.FromSeconds(generateRandom(1, 5)), 34, 26);
                                    bullet.Position = new Vector2((posX + i) + (17), (posY + i) + (32));
                                    game.Components.Add(bullet);

                                    lastFireTimeEnemy = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(0.5));
                                }
                            }
                        }


                        //if boss is alien
                        else if (monster < 3)
                        {
                            //Logic for manuevouring
                            if (!this.startTime.HasValue)
                            {
                                this.startTime = gameTime.TotalGameTime;
                            }
                            else
                            {

                                if (enemiesGoingLeft)
                                {
                                    float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                                    for (int i = 0; i < enemyList.Count; ++i)
                                        enemyList[i].Position = new Vector2(enemyList[i].StartPosition.X - (percentProgress * distanceToTravel), enemyList[i].StartPosition.Y);

                                    if (percentProgress > 1)
                                    {
                                        this.startTime = gameTime.TotalGameTime;
                                        for (int i = 0; i < enemyList.Count; ++i)
                                            enemyList[i].StartPosition = new Vector2(enemyList[i].Position.X, enemyList[i].Position.Y);
                                        enemiesGoingLeft = false;
                                    }
                                }
                                else
                                {
                                    float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                                    for (int i = 0; i < enemyList.Count; ++i)
                                        enemyList[i].Position = new Vector2(enemyList[i].StartPosition.X + (percentProgress * distanceToTravel), enemyList[i].StartPosition.Y);

                                    if (percentProgress > 1)
                                    {
                                        this.startTime = gameTime.TotalGameTime;
                                        for (int i = 0; i < enemyList.Count; ++i)
                                            enemyList[i].StartPosition = new Vector2(enemyList[i].Position.X, enemyList[i].Position.Y);
                                        enemiesGoingLeft = true;
                                    }
                                }

                                if (timer == 0)
                                {
                                    bossGetsMad = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(15));
                                    timer += 1;
                                }

                                //fire boss bullets
                                if (lastFireTimeEnemy < gameTime.TotalGameTime && enemyList.Count > 0)
                                {
                                    for (int i = 1; i <= 3; i++)
                                    {
                                        float posX = game.EnemyConductor.enemyList[generateRandom(0, game.EnemyConductor.enemyList.Count)].Position.X;
                                        float posY = game.EnemyConductor.enemyList[generateRandom(0, game.EnemyConductor.enemyList.Count)].Position.Y;
                                        BulletSprite bullet = new BulletSprite(shipImage2, game, false, TimeSpan.FromSeconds(generateRandom(1, 5)), 34, 26);
                                        bullet.Position = new Vector2((generateRandom(0,1024)), (posY + i) + (32));
                                        game.Components.Add(bullet);

                                        lastFireTimeEnemy = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(0.5));
                                    }
                                }

                                if (bossGetsMad < gameTime.TotalGameTime && timer == 1)
                                {
                                    for (int i = 0; i <= 14; ++i)
                                    {
                                        int left = 50 + (i * 45);

                                        createEnemy(shipImage6, left, 240, 2);
                                        createEnemy(shipImage5, left, 200, 2);
                                
                                    }

                                    timer = 3;
                                }
                            }
                        }
                    }
                    break;
                //End of Second Case
                case Main.GameState.Ending:
                    break;
            }
           
    }




        private int generateRandom(int startFrom, int endAt)
        {
            //function to return a random number
            int randomNumber = game.Rnd.Next(startFrom, endAt);
            return randomNumber;
        }


        public GameTime StartTime
        {
            set
            {
                startTime = value.TotalGameTime;
            }
        }

        public bool EnemiesGoingLeft
        {
            set
            {
                enemiesGoingLeft = value;
            }
        }


        public List<EnemySprite> enemyList
        {
            get
            {
                return this.enemies;
            }
        }


        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    if (this.EnabledChanged != null)
                    {
                        this.EnabledChanged(this, EventArgs.Empty);
                    }
                }
            }
        }



        public int UpdateOrder
        {
            get
            {
                return this.updateOrder;
            }
            set
            {
                if (this.updateOrder != value)
                {
                    this.updateOrder = value;
                    if (this.UpdateOrderChanged != null)
                    {
                        this.UpdateOrderChanged(this, EventArgs.Empty);
                    }
                }
            }
        }



    }
}
