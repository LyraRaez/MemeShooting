using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeShooting
{
    public class HUD
    {
        public int playerScore, screenWidht, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePosition;
        public bool showHUD;

        public HUD()
        {
            playerScore = 0;
            showHUD = true;
            screenHeight = 950;
            screenWidht = 800;
            playerScoreFont = null;
            playerScorePosition = new Vector2(screenWidht / 2, 50);

        }

        //Load content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("Score");
        }

        //Update fontion
        public void Update(GameTime gameTime)
        {
            //avoir l'état de clavier
            KeyboardState keyState = Keyboard.GetState();
        }

        //draw
        public void Draw (SpriteBatch spriteBatch)
        {
            //si noux pouvons voir notre HUD (if showHUD==true) il affiche notre HUD
            if (showHUD)
                spriteBatch.DrawString(playerScoreFont, "Score= "+playerScore,  playerScorePosition, Color.Green);
        }

    }
}
