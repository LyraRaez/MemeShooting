﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MemeShooting
{
    public class Starfield
    {
        public Texture2D texture;
        public Vector2 bgPos1, bgPos2;
        public int speed;
        public Starfield()
        {
            texture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -950);
            speed = 5;
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("BackGround");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            //met en place la vitesse du background
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;

            //Scrolling background (Répété)
            if(bgPos1.Y>=950)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -950;
            }


        }
    }
}
