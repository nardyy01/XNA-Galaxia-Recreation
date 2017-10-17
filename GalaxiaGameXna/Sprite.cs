using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalaxiaGameXna
{
    public abstract class Sprite : IUpdateable, IGameComponent, IDrawable
    {
        private Texture2D texture;
        private Main game;
        private int drawOrder;
        private int updateOrder;
        private bool enabled;
        private bool visible;


        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public abstract void Update(GameTime gameTime);
        public abstract void Initialize();



        protected Sprite(Texture2D texture, Main game)
        {
            this.texture = texture;
            this.game = game;
            this.enabled = true;
            this.visible = true;
        }



        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }



        public Vector2 Position
        {
            get;
            set;
        }



        public int DrawOrder
        {
            get
            {
                return this.drawOrder;
            }
            set
            {
                if (value != this.drawOrder)
                {
                    this.drawOrder = value;
                    if (this.DrawOrderChanged != null)
                    {
                        this.DrawOrderChanged(this, EventArgs.Empty);
                    }
                }
            }
        }



        protected Main Game
        {
            get
            {
                return this.game;
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            this.game.SpriteBatch.Draw(this.texture, this.Position, Color.White);
        }



        public bool Visible
        {
            get 
            {
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    if (this.VisibleChanged != null)
                    {
                        this.VisibleChanged(this, EventArgs.Empty);
                    }
                }
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
