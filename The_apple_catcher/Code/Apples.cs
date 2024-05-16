using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AppleCatcher
{
    class Apples
    {
        public static int Width, Height;
        public static Random random = new Random();
        public static SpriteBatch SpriteBatch { get; set; }
        static Sheets[] sheets;
        static public Basket Basket { get; set; }
        static public int GetIntRnd(int min, int max)
        {
            return random.Next(min, max);
        }
        static public void Init(SpriteBatch SpriteBatch, int width, int height)
        {
            Apples.Width = width;
            Apples.Height = height;
            Apples.SpriteBatch = SpriteBatch;
            sheets=new Sheets[60];
            for (int i = 0;i < sheets.Length; i++)
            {
                sheets[i] = new Sheets(new Vector2(-random.Next(4, 9), 0));
            }
            Basket = new Basket(new Vector2(Width / 2-180, Width / 2 - 180));
        }
        static public void Draw()
        {
            foreach (var sheet in sheets)
                sheet.Draw();
            Basket.Draw();
        }
        static public void Update()
        {
            foreach (var sheet in sheets)
                sheet.Update();
        }

    }
    class Sheets
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;
        public static Texture2D Texture2D { get; set; }
        public Sheets(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }
        public Sheets(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }
        public void Update()
        {
            Pos += Dir;
            if (Pos.X < 0)
            {
                RandomSet();
            }
        }

        private void RandomSet()
        {
            Pos = new Vector2(Apples.GetIntRnd(Apples.Width, Apples.Width + 300), Apples.GetIntRnd(0, Apples.Height));
            color = Color.FromNonPremultiplied(Apples.GetIntRnd(0, 256), Apples.GetIntRnd(0, 256),Apples.GetIntRnd(0, 256),255);
        }
        public void Draw()
        {
            Apples.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
    class Basket
    {
        Vector2 Pos;
        public int Speed { get; set; } = 8;

        Color color=Color.Brown;
        public static Texture2D Texture2D { get; set; }
        public Basket(Vector2 Pos)
        {
            this.Pos = Pos;

        }
        public void Left()
        { 
        this.Pos.X -= Speed;
        }
        public void Right()
        {
            this.Pos.X += Speed;
        }

        public void Draw()
        {
            Apples.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
}