using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GalaxiaGameXna
{
    public class EnemySprite : Sprite
    {
        private Vector2 startPosition;
        private int health;

        public EnemySprite(Texture2D texture, Main game)
            : base(texture, game)
        {
        }


        public Vector2 StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }


        public override void Initialize()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

    }
}
