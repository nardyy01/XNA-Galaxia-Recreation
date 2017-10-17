using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GalaxiaGameXna
{
    class cButton
    {
        Texture2D texture;
        Rectangle rectangle;

        Color color = new Color(255, 255, 255, 255);

        public Vector2 size;

        public cButton(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;

            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);

        }

        public bool isClicked;
        public void Update(MouseState mouse)
        {


            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if(mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else
            {
                isClicked = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(texture, rect, color);
            this.rectangle = rect;
        }
    }
}
