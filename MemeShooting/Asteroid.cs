using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeShooting
{
    public class Asteroid
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationAngle;
        public int speed;
        public bool isVisible; 
        public Rectangle BoundingBox;
        Random random = new Random();
        public float randX, randY;
        private Texture2D texture2D;

        public Asteroid(Texture2D newTexture, Vector2 newPosition)//Constructeur
        {
            position = newPosition;
            texture = newTexture;
            speed = 4;

            isVisible = true;
            randX = random.Next(0, 750);
            randY = random.Next(-600, -50);
        }

        public Asteroid(Texture2D texture2D)
        {
            this.texture2D = texture2D;
        }

        public void LoadContent(ContentManager Content)
        {
        }

        public void Update(GameTime gameTime)
        {
            
            
            //Mise en place d'une hitbox
            BoundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            //mise à jourde l'origin de la rotation
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            //Update Mouvement 
            position.Y = position.Y + speed;
            if(position.Y>=950)
            {
                position.Y = -50;
            }

            //rotation Asteroid
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //rotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            if(isVisible)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
