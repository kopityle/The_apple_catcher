using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace The_apple_catcher
{
    internal class SplashScreen
    {
        public static Texture2D Background { get; set; }
        static int timeCounter = 0;
        static Color color;
        static Vector2 textPosition = new Vector2(100, 100);
        public static SpriteFont Font { get; set; }
        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, new Rectangle(0, 0, 1920, 1020), color);
           spriteBatch.DrawString(Font, "Catcher", textPosition, color);
        }
        static public void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 256);
            timeCounter++;
        }
    }
}
