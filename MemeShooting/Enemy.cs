using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeShooting
{
    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int enemyHealth, speed, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;
        private Texture2D texture2D;
        private Vector2 vector2;

        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            enemyHealth = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            bulletDelay = 30;
            speed = 5;
            isVisible = true;

        }

        public Enemy(Texture2D texture2D, Vector2 vector2)
        {
            this.texture2D = texture2D;
            this.vector2 = vector2;
        }

        public void Update (GameTime gameTime)
        {
            //update Rectangle de collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Update Mouvement des ennemis
            position.Y += speed;

            //Mouvement des ennemis pour retourner au haut de l'écran si il vol au loin
            if (position.Y >= 950)
                position.Y = -75;
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            //Dessine le vaisseau ennemi
            spriteBatch.Draw(texture, position, Color.White);

            //Dessine les balle ennemis
            foreach(Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }

            EnemyShoot();
            UpdateBullet();
        }

        public void UpdateBullet()
        {

            //pour chaque balle dans votre bulletList update the mouvement et si la balle touche le haut de l'écran retirer la de la liste
            foreach (Bullet b in bulletList)
            {
                //hitbox pour la balle
                b.BoundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);
                
                //met en place les moyvement de la balle 
                b.position.Y = b.position.Y + b.speed;

                //si la balle touche le haut de l'écran celle ci disparait
                if (b.position.Y >= 950)
                {
                    b.isVisible = false;
                }
            }


            //iterate throgh bulletList and see if any of bullet are not visible, if they arent then remove that bullet from dthe list
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        //Foncion du tir ennemi
        public void EnemyShoot()
        {
            //Tir seulement si le bulletDelay est remis à 0
            if (bulletDelay>=0)
            {
                bulletDelay--;
            }

            if(bulletDelay<=0)
            {
                //Créer une nouvelle balle et la positionnner en avant et au centre du vaisseau ennemi 
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;

                if (bulletList.Count()<20)
                {
                    bulletList.Add(newBullet);
                }

                //Remise à zéro du bulletDelay
                if (bulletDelay==0)
                {
                    bulletDelay = 30;
                }
            }
        }


    }
}
