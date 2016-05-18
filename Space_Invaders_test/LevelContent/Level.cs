using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LevelContent
{
    public class Level
    {
        public Point alienCount;
        public float alienVelocity;

        public int bombCount;
        public int bombFreq;
        public int bombVelocity;

        public string textureName;
        public int timeForLevel;
    }
}
