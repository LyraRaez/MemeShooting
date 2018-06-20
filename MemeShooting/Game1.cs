using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeShooting
{

    public class Game1 : Game
    {
        public enum State
        {
            Menu,
            Playing,
            GameOver
        }

        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage;

        Player p = new Player();
        Starfield sf = new Starfield();
        HUD Hud = new HUD();
        SoundManager sm = new SoundManager();

        //List
        List<Enemy> enemyList = new List<Enemy>();
        List<Asteroid> asteroidsList = new List<Asteroid>();
        List<Explosion> explosionList = new List<Explosion>();

        //Image pour gameState
        public Texture2D menuImage;
        public Texture2D gameOverImage;

        //Mise en palce du premier State
        State gameState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            this.Window.Title = "Meme Shoot";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 10;
            menuImage = null;
            gameOverImage = null;
        }

        protected override void Initialize()
        {
           
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p.LoadContent(Content);
            sf.LoadContent(Content);
            Hud.LoadContent(Content);
            sm.LoadContent(Content);

            menuImage = Content.Load<Texture2D>("space");
            gameOverImage = Content.Load<Texture2D>("GameOver");
           
            
        }

        
        protected override void UnloadContent()
        {
            
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //mise à jour playing state
            switch(gameState)
            {
                case State.Playing:
                    {


                        //Mise à jour des ennemis et vérification de la collision
                      
                        sf.speed = 5;
                        foreach (Enemy e in enemyList)
                        {
                            sf.speed = 5;
                            //Vérifier si le vaisseau ennemi entre en collision avec le joueur
                            if (e.boundingBox.Intersects(p.BoundingBox))
                            {
                                p.health -= 40;
                                e.isVisible = false;
                            }

                            //vérifie la balle ennemi entre en collision avec le joueur
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (p.BoundingBox.Intersects(e.bulletList[i].BoundingBox))
                                {
                                    p.health -= enemyBulletDamage;
                                    e.bulletList.ElementAt(i).isVisible = false;
                                }
                            }

                            //verifie si la balle du joue entre en collision avec le vaisseau ennemi
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (p.bulletList[i].BoundingBox.Intersects(e.boundingBox))
                                {
                                    sm.explosionSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                    Hud.playerScore += 20;
                                    p.bulletList[i].isVisible = false;
                                    e.isVisible = false;
                                }
                            }

                            e.Update(gameTime);
                        }

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        //Pour chaque asteroids dans l'asteroidList, update it verfier la collision
                        foreach (Asteroid a in asteroidsList)
                        {
                            if (a.BoundingBox.Intersects(p.BoundingBox))
                            {
                                a.isVisible = false;
                                p.health -= 15;
                            }

                            // iterate to our bulletList if any asteroids comme in contact with these bullet, destroy the bullet and asteroids
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (a.BoundingBox.Intersects(p.bulletList[i].BoundingBox))
                                {
                                    sm.explosionSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                    Hud.playerScore += 5;
                                    a.isVisible = false;
                                    p.bulletList.ElementAt(i).isVisible = false;
                                }
                            }

                            a.Update(gameTime);

                        }


                        // Hud.Update(gameTime);

                        // si playerhealth vient à être à 0 alors il va au gameover state
                        if (p.health<=0)
                        {
                            gameState = State.GameOver;
                        }

                        p.Update(gameTime);
                        sf.Update(gameTime);
                        ManageExplosion();
                        LoadAsteroids();
                        LoadEnemy();
                        break;
                    }


                //mise à jour du menu state
                case State.Menu:
                    {
                        
                        //avoir l'état du clavier
                        KeyboardState keyState = Keyboard.GetState();

                        if(keyState.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        sf.Update(gameTime);
                        MediaPlayer.Play(sm.bgMusic);
                        sf.speed = 1;
                        //sm.gameOverSound.Play();

                        break;
                    }

                //mise à jour du gameover state
                case State.GameOver:
                    {
                        //avoit l'état du clavier 
                        KeyboardState keyState = Keyboard.GetState();

                        if (keyState.IsKeyDown(Keys.E))
                        {
                            enemyList.Clear();
                            asteroidsList.Clear();
                            p.health = 200;
                            Hud.playerScore = 0;
                            gameState = State.Menu;
                        }

                        MediaPlayer.Stop();
                       
                        break;
                    }
            }

           

            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

          switch(gameState)
            {
                //Dessine playing state
                case State.Playing:
                    {

                        sf.Draw(spriteBatch);
                        p.Draw(spriteBatch);

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }


                        foreach (Asteroid a in asteroidsList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }

                        Hud.Draw(spriteBatch);
                        break;
                    }


                //dessine menu state
                case State.Menu:
                    {
                        sf.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                        break;
                    }
                
                //dessine gameover state
                case State.GameOver:
                    {
                        spriteBatch.Draw(gameOverImage, new Vector2(280, 283), Color.White);
                        spriteBatch.DrawString(Hud.playerScoreFont, "Final Score: " + Hud.playerScore.ToString(), new Vector2(280, 525), Color.Red);
                        break;
                    }
            }
            




            spriteBatch.End();
           
            base.Draw(gameTime);
        }

        //Chargement des ennemi
        public void LoadEnemy()
        {
            //Création de position x et y pour nos asteroids
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);

            //Si il y a moins de 3 ennemis à l'écran il en créer 5 en plus 
            if (enemyList.Count()<10)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("trollface"), new Vector2(randX, randY), Content.Load<Texture2D>("EnemyBullet")));
            }


            // si n'impporte le quel des ennemis a été détruit, il sera retirer de l'asteroidsList
            for (int i=0;  i< enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }

        }


        public void LoadAsteroids()//Chargement des asteroides
        {
            //Création de position x et y pour nos asteroids
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);

            //Si il y a moins de 3 ennemis à l'écran il en créer 5 en plus 
            if (asteroidsList.Count() < 15)
            {
                asteroidsList.Add(new Asteroid(Content.Load<Texture2D>("Asteroids"), new Vector2(randX, randY)));
            }


            // si n'impporte le quel des asteroid a été détruit, il sera retirer de l'asteroidsList
            for (int i = 0; i < asteroidsList.Count; i++)
            {
                if (!asteroidsList[i].isVisible)
                {
                    asteroidsList.RemoveAt(i);
                    i--;
                }
            }

        }

        //Manage Expllosion
        public void ManageExplosion()
        {
            for (int i =0; i<explosionList.Count(); i++)
            {
                if(!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;

                }
            }
        }


    }
}
