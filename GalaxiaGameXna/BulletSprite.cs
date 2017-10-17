using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GalaxiaGameXna
{
    public class BulletSprite : Sprite
    {
        private TimeSpan? startTime;
        private TimeSpan journeyTime;
        private Vector2 startPosition;
        private bool goingUp;
        private Texture2D bullet;
        private int length = 4, height = 18;

        public BulletSprite(Texture2D texture, Main game, bool goingUp, TimeSpan journeyTime, int length = 4, int height = 18)
            : base(texture, game)
        {
            this.journeyTime = journeyTime;
            this.goingUp = goingUp;

            this.bullet = game.Content.Load<Texture2D>("pump");
            this.length = length;
            this.height = height;

        }

        public override void Initialize()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.SpriteBatch.Draw(this.Texture,
                new Rectangle((int)this.Position.X,
                    (int)this.Position.Y, length, height), Color.White);
        }

        public override void Update(GameTime gameTime)
        {


            if (!this.startTime.HasValue)
            {
                this.startTime = gameTime.TotalGameTime;
                this.startPosition = this.Position;
            }
            else
            {
                float distanceToTravel = 0;
                if (this.goingUp)
                {
                    if (checkForBulletHit(true) == true) 
                    {
                        //player killed mob
                        this.Game.Sound = this.Game.Content.Load<SoundEffect>("kill");
                        this.Game.Sound.Play();

                        //score handling
                        if (this.Game.EnemyConductor.monster < 3 && this.Game.EnemyConductor.bossOrNot == 2)
                            this.Game.Score += 400;
                        else if (this.Game.EnemyConductor.monster > 3 && this.Game.EnemyConductor.bossOrNot == 2)
                            this.Game.Score += 250;
                        else
                            this.Game.Score += 100;

                        this.Game.Components.Remove(this);
                        return;
                    }

                    distanceToTravel = this.startPosition.Y;
                }
                else
                {
                    if (isPlayerStupid() || (checkForBulletHit(false) == true && this.Game.Ship.Enabled && !this.Game.Ship.PlayerBeenShot))
                    {
                        //mob killed player
                        this.Game.Ship.TimePlayerIsKilled = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(1));
                        this.Game.Ship.PlayerBeenShot = true;
                        this.Game.Ship.Texture = this.Game.ShipExplodingTexture;
                        this.Game.Sound = this.Game.Content.Load<SoundEffect>("Explode_snd");
                        this.Game.Sound.Play();
                        this.Game.Lives -=1;
                        this.Game.Components.Remove(this);
                        if (isPlayerStupid())
                        {
                            this.Game.Ship.Texture = this.Game.ShipExplodingTexture;
                            this.Game.Sound = this.Game.Content.Load<SoundEffect>("Explode_snd");
                            this.Game.Sound.Play();
                            this.Game.Ship.Position = new Vector2(this.Game.Ship.Position.X, this.Game.Graphics.PreferredBackBufferHeight - this.Game.Ship.Texture.Height);
                        }
                        return;
                    }

                    distanceToTravel = Main.ScreenHeight - this.startPosition.Y;
                }

                float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                if (this.goingUp)
                {
                    this.Position = new Vector2(this.Position.X, this.startPosition.Y - (percentProgress * distanceToTravel));
                }
                else
                {
                    this.Position = new Vector2(this.Position.X, this.startPosition.Y + (percentProgress * distanceToTravel));
                }

                if (this.Position.Y < 0 || this.Position.Y > Main.ScreenHeight)
                {
                    this.Game.Components.Remove(this);
                }
            }

        }

        private bool checkForBulletHit(bool friendBullet)
        {

            if (friendBullet)
            {
                for (int i = 0; i < this.Game.EnemyConductor.enemyList.Count; ++i)
                {
                    EnemySprite enemy = this.Game.EnemyConductor.enemyList[i];

                    if (this.Position.X > enemy.Position.X && this.Position.X < (enemy.Position.X + enemy.Texture.Width))
                    {
                        if (this.Position.Y > enemy.Position.Y && this.Position.Y < (enemy.Position.Y + enemy.Texture.Height))
                        {
                            //Take 1 health from enemy
                            this.Game.EnemyConductor.enemyList[i].Health -= 1;

                            //check if it is dead or not
                            if (this.Game.EnemyConductor.enemyList[i].Health == 0)
                            {
                                this.Game.EnemyConductor.enemyList.RemoveAt(i);
                                this.Game.Components.Remove(enemy);
                                return true;
                            }
                            else if (this.Game.EnemyConductor.enemyList[i].Health != 0)
                            {
                                this.Game.Components.Remove(this);                               
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.Position.X + length > this.Game.Ship.Position.X && this.Position.X < (this.Game.Ship.Position.X + this.Game.Ship.Texture.Width))
                {
                    if (this.Position.Y > this.Game.Ship.Position.Y && this.Position.Y < (this.Game.Ship.Position.Y + this.Game.Ship.Texture.Height))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool isPlayerStupid()
        {
            //TESTING AREA
            for (int i = 0; i < this.Game.EnemyConductor.enemyList.Count; ++i)
            {
                EnemySprite enemy = this.Game.EnemyConductor.enemyList[i];

                if (this.Game.Ship.Position.X > enemy.Position.X && this.Game.Ship.Position.X < (enemy.Position.X + enemy.Texture.Width))
                {
                    if (this.Game.Ship.Position.Y > enemy.Position.Y && this.Game.Ship.Position.Y < (enemy.Position.Y + enemy.Texture.Height))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }


}
