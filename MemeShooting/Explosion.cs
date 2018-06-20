using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeShooting
{
    public class Explosion
    {
        public Texture2D texture;
        public Vector2 position;
        public float timer;
        public float interval;
        public Vector2 origin;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;
        public bool isVisible;

        //Constructeur
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 100f;
            currentFrame = 1;
            spriteWidth = 120;
            spriteHeight = 120;
            isVisible = true;
        }

        public void LoadContent(ContentManager Content)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            //augmente le timer par le nombre de millisecondes depuis update a  eu le dernière appel
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Verifer que le timer est plus grand que l'interval choisi
            if (timer <interval)
            {
                //montre la prochaine frame
                currentFrame++;
                //reset le timer
                timer = 0f;
            }

            //si quand on est sur la dernière frame, rendre l'explosion invisible qnd remise à 0 de la currentFrame

            if (currentFrame==17)
            {
                isVisible = false;
                currentFrame = 0;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);

        }

        //Fonction de dessin
        public void Draw(SpriteBatch spriteBatch)
        {
            //si visible alors il est dessiner
            if(isVisible==true)
            {
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);

            }
        }

 
    }
}
