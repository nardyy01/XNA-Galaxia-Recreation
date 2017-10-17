using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GalaxiaGameXna
{
    public class StarSprite : Sprite
    {
        private TimeSpan? startTime;
        private TimeSpan journeyTime;
        private Vector2 startPosition;

        public StarSprite(Texture2D texture, Main game, TimeSpan journeyTime)
            :base(texture,game)
        {
            this.journeyTime = journeyTime;
        }

        public override void Initialize()
        {

        }


        public override void Draw(GameTime gameTime)
        {
            this.Game.SpriteBatch.Draw(this.Texture,
                new Rectangle((int)this.Position.X,
                    (int)this.Position.Y, 2, 2), Color.White);
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
                float distanceToTravel = Main.ScreenHeight;
                float percentProgress = (float)(gameTime.TotalGameTime - this.startTime.Value).TotalMilliseconds / (float)this.journeyTime.TotalMilliseconds;
                this.Position = new Vector2(this.Position.X,this.startPosition.Y + (percentProgress * distanceToTravel));
            }

            if (this.Position.Y > Main.ScreenHeight)
                this.Game.Components.Remove(this);

        }


    }
}
