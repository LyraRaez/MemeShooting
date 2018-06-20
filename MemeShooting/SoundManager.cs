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
    public class SoundManager
    {
        public SoundEffect playerShootSound;
        public SoundEffect explosionSound;
        public SoundEffect gameOverSound;
        public Song bgMusic;

        public SoundManager()
        {
            playerShootSound = null;
            explosionSound = null;
            bgMusic = null;
        }

        public void LoadContent(ContentManager Content)
        {
            playerShootSound = Content.Load<SoundEffect>("shoot1");
            explosionSound = Content.Load<SoundEffect>("ExplosionSound1");
            gameOverSound = Content.Load<SoundEffect>("GameOverSound");
            bgMusic = Content.Load<Song>("Theme1");
        }


    }
}
