using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GalaxiaGameXna
{
    public class ShipCompanion : Sprite
    {

        private TimeSpan lastFireTimeFriend;
        private TimeSpan timePlayerIsKilled;
        private bool playerBeenShot;
        private int counter = 0;

        public ShipCompanion(Texture2D texture, Main game)
            : base(texture, game)
        {
            playerBeenShot = false;
        }


        public TimeSpan TimePlayerIsKilled
        {
            get
            {
                return timePlayerIsKilled;
            }
            set
            {
                timePlayerIsKilled = value;
            }
        }


        public bool PlayerBeenShot
        {
            get
            {
                return playerBeenShot;
            }
            set
            {
                playerBeenShot = value;
            }
        }
        public int Counter
        {
            get
            {
                return counter;
            }
            set
            {
                counter = value;
            }
        }



        public override void Initialize()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (playerBeenShot && this.Visible == true && timePlayerIsKilled < gameTime.TotalGameTime)
            {
                //player has been shot, reset
                this.Visible = false;
                this.Texture = this.Game.ShipTexture;
                playerBeenShot = false;                
                this.Enabled = false;
                return;
            }

            counter--;
            if(counter == 0)
            {
                this.Enabled = false;
                this.Visible = false;
            }

            //player ship input controls
            KeyboardState keyboardState = Keyboard.GetState();

            if (this.Enabled && !playerBeenShot && this.Visible)
            {
                if (keyboardState.IsKeyDown(Keys.Left)) this.Position = new Vector2(this.Position.X - 5, this.Position.Y);
                if (keyboardState.IsKeyDown(Keys.Right)) this.Position = new Vector2(this.Position.X + 5, this.Position.Y);
                if (this.Position.X < 0) this.Position = new Vector2(0, this.Position.Y);
                if (this.Position.X > this.Game.Graphics.PreferredBackBufferWidth - this.Texture.Width)
                {
                    this.Position = new Vector2(this.Game.Graphics.PreferredBackBufferWidth - this.Texture.Width, this.Position.Y);
                }
            }

            if (keyboardState.IsKeyDown(Keys.LeftControl) && 
                lastFireTimeFriend < gameTime.TotalGameTime && 
                !playerBeenShot && this.Visible)
            {

                BulletSprite bullet = new BulletSprite(this.Game.FriendBullet, this.Game, true, TimeSpan.FromSeconds(1));
                bullet.Position = new Vector2(this.Position.X + 17, this.Position.Y - 16);
                this.Game.Components.Add(bullet);

                this.Game.Sound = this.Game.Content.Load<SoundEffect>("fire");
                this.Game.Sound.Play();

                lastFireTimeFriend = gameTime.TotalGameTime.Add(TimeSpan.FromSeconds(0.2));
            }


        }
    }
}
