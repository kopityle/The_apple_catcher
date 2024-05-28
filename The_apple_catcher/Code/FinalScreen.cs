using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppleCatcher
{
    internal class FinalScreen
    {
        public static Texture2D Background { get; set; }
        static int timeCounter = 0;
        static Color color;
        static Vector2 textPosition = new(100, 150);
        public static SpriteFont Font { get; set; }
        static public void Draw(SpriteBatch spriteBatch, int score, int maxCombo) 
        {
            spriteBatch.Draw(Background, new Rectangle(0, 0, 1920, 1020), color);
            spriteBatch.DrawString(Font, "Game Over", textPosition, color);
            spriteBatch.DrawString(Font, $"Score: {score}", new Vector2(100, 200), color);
            spriteBatch.DrawString(Font, $"Max Combo: {maxCombo}", new Vector2(100, 250), color);
        }
        static public void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 256);
            timeCounter++;
        }
    }
}
