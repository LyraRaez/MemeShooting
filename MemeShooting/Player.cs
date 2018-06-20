using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeShooting
{
    public class Player
    {
        public Texture2D texture, bulletTexture, healthTexture;
        public Vector2 position, healthPosition;
        public int speed, health;
        public bool isColliding;
        public Rectangle BoundingBox, healthRectangle;
        public float bulletDelay;
        public List<Bullet> bulletList;

        SoundManager sm = new SoundManager();


        public Player()//Constructeur
        {
            texture = null;
            position = new Vector2(300,300);
            speed = 10;
            isColliding = false;
            bulletList = new List<Bullet>();
            bulletDelay = 20;
            health = 200;
            healthPosition = new Vector2(50, 50);
        }

        public void LoadContent(ContentManager Content)//chargement contenu
        {
            texture = Content.Load<Texture2D>("Player");
            bulletTexture = Content.Load<Texture2D>("playerbullet");
            healthTexture = Content.Load<Texture2D>("healthbar");
            sm.LoadContent(Content);
        }

        public void Draw(SpriteBatch spriteBatch)//fonction de dessin
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);


            foreach(Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)//Update
        {
            KeyboardState keyState = Keyboard.GetState();

            //hitbox pour le joueur
            BoundingBox = new Rectangle((int)position.X, (int) position.Y, texture.Width, texture.Height);

            //Mise en place du rectangle de la barre de vie
            healthRectangle = new Rectangle((int)healthPosition.X, (int) healthPosition.Y, health, 25);
            
            //Tir de la balle
            if(keyState.IsKeyDown(Keys.Space))
            {
                Shoot();
            }

            UpdateBullet();
           
            //Contrôle du vaisseau
            if (keyState.IsKeyDown(Keys.Z))
            {
                position.Y = position.Y - speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                position.Y = position.Y + speed;
            }
            if (keyState.IsKeyDown(Keys.Q))
            {
                position.X = position.X - speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X = position.X + speed;
            }

            //Garder le vaisseau dans l'écran
            if (position.X <= 0)
                position.X = 0;

            if (position.X >= 800-texture.Width)
                position.X = 800-texture.Width;

            if (position.Y <= 0)
                position.Y = 0;

            if (position.Y >= 950 - texture.Height)
                position.Y = 950 - texture.Height;


        }

        //Méthode de tir utilisé pour initialisé la position de la balle
        public void Shoot()
        {
            //tir seulment si la balle a été reset
            if (bulletDelay>=0)
            {
                bulletDelay--;
            }

            //si le bulletDelay est à 0 il crée une nouvelle balle visible à l'écran
            if (bulletDelay<=0)
            {
                sm.playerShootSound.Play();
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2, position.Y + 30);

                //fait que la balle soit visible
                newBullet.isVisible = true;

                if(bulletList.Count()<20)
                {
                    bulletList.Add(newBullet);
                }
            }

            //reset bulletDelay
            if(bulletDelay==0)
            {
                bulletDelay = 20;
            }

           
        }

        public void UpdateBullet()//met à jour la fonction bullet
        {
            
            
            //pour chaque balle dans votre bulletList update the mouvement et si la balle touche le haut de l'écran retirer la de la liste
            foreach (Bullet b in bulletList)
            {
                //hitbox pour la balle
                b.BoundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);
                //met en place les moyvement de la balle 
                b.position.Y = b.position.Y - b.speed;
                
                //si la balle touche le haut de l'écran celle ci disparait
                if(b.position.Y<=0)
                {
                    b.isVisible = false;
                }
            }


            //iterate throgh bulletList and see if any of bullet are not visible, if they arent then remove that bullet from dthe list
            for(int i =0; i< bulletList.Count; i++)
            {
                if(!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
