using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using The_apple_catcher;
namespace AppleCatcher
{
    class Apples
    {
        public static int Width, Height;
        public static Random random = new();
        static public SpriteBatch SpriteBatch { get; set; }
        static Sheets[] sheets;
        static public Basket Basket { get; set; }
        public static int Score { get; set; } = 0;
        public static int Lives { get; set; } = 3;
        public static int Combo { get; set; } = 0;
        public static int MaxCombo { get; set; } = 0;
        static List<Apple> apples = new();
        static TimeSpan appleSpawnTime = TimeSpan.FromSeconds(1.0f);
        static TimeSpan lastAppleSpawnTime = TimeSpan.Zero;

        static public int GetIntRnd(int min, int max)
        {
            return random.Next(min, max);
        }
        static public void Init(SpriteBatch SpriteBatch, int width, int height)
        {
            Apples.Width = width;
            Apples.Height = height;
            Apples.SpriteBatch = SpriteBatch;
            sheets = new Sheets[60];
            for (int i = 0; i < sheets.Length; i++)
            {
                sheets[i] = new Sheets(new Vector2(-random.Next(4, 9), 0));
            }
            Basket = new Basket(new Vector2(Width / 2 - 180, Width / 2 - 180));
        }
        static public void Draw()
        {
            foreach (var sheet in sheets)
                sheet.Draw();

            foreach (var apple in apples)
                apple.Draw();

            Basket.Draw();
            SpriteBatch.DrawString(SplashScreen.Font, "Score: " + Score, new Vector2(10, 10), Color.White);
            SpriteBatch.DrawString(SplashScreen.Font, "Lives: " + Lives, new Vector2(10, 50), Color.White);
            SpriteBatch.DrawString(SplashScreen.Font, "Combo: " + Combo, new Vector2(10, 90), Color.White);
        }
        static public void Update(GameTime gameTime)
        {
            foreach (var sheet in sheets)
                sheet.Update();

            // Создание новых яблок
            if (gameTime.TotalGameTime - lastAppleSpawnTime > appleSpawnTime)
            {
                SpawnApple();
                lastAppleSpawnTime = gameTime.TotalGameTime;
            }

            // Обновление существующих яблок
            for (int i = apples.Count - 1; i >= 0; i--)
            {
                apples[i].Update();

                // Проверка столкновения с корзиной
                if (apples[i].IsCollidingWith(Basket))
                {
                    OnAppleCatched(apples[i]);
                    apples.RemoveAt(i);
                }
                // Удаление яблок, ушедших за пределы экрана
                else if (apples[i].Pos.Y > Height)
                {
                    // Обнуляем комбо, если хорошее яблоко не поймано
                    if (apples[i].Type != AppleType.Rotten)
                    {
                        Combo = 0;
                    }
                    apples.RemoveAt(i);
                }
            }
        }

        static void SpawnApple()
        {
            // Тип яблока (случайный)
            Array appleTypes = Enum.GetValues(typeof(AppleType));
            AppleType randomAppleType = (AppleType)appleTypes.GetValue(random.Next(appleTypes.Length));
            int x = random.Next(Width);
            apples.Add(new Apple(randomAppleType, new Vector2(x, -Apple.Texture2D.Height)));
        }

        static void OnAppleCatched(Apple apple)
        {
            switch (apple.Type)
            {
                case AppleType.Green:
                    Score += 5;
                    Combo++;
                    break;
                case AppleType.Yellow:
                    Score += 10;
                    Combo++;
                    break;
                case AppleType.Red:
                    Score += 15;
                    Combo++;
                    break;
                case AppleType.Rotten:
                    Lives--;
                    Combo = 0;
                    break;
            }

            MaxCombo = Math.Max(Combo, MaxCombo);
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
            color = Color.FromNonPremultiplied(Apples.GetIntRnd(0, 256), Apples.GetIntRnd(0, 256), Apples.GetIntRnd(0, 256), 255);
        }
        public void Draw()
        {
            Apples.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
    class Basket
    {
        Vector2 Pos;
        public int Speed { get; set; } = 17;

        Color color = Color.Brown;
        public static Texture2D Texture2D { get; set; }
        public Basket(Vector2 Pos)
        {
            this.Pos = Pos;

        }
        public void Left()
        {
            this.Pos.X = Math.Max(this.Pos.X - Speed, 0);
        }
        public void Right()
        {
            this.Pos.X = Math.Min(this.Pos.X + Speed, Apples.Width - Texture2D.Width);
        }

        public void Draw()
        {
            Apples.SpriteBatch.Draw(Texture2D, Pos, color);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Pos.X, (int)Pos.Y, Texture2D.Width, Texture2D.Height);
        }
    }
    enum AppleType
    {
        Green,
        Yellow,
        Red,
        Rotten
    }
    class Apple
    {
        public Vector2 Pos { get; private set; }
        private Vector2 dir;
        public Vector2 Dir
        {
            get
            { return dir; }
            set { dir = value; }

        }
        Color color;
        public float Scale { get; private set; }
        static Vector2 Center => new(Apple.Texture2D.Width / 2, Apple.Texture2D.Height / 2);
        public static Texture2D Texture2D { get; set; }
        public static Texture2D GreenAppleTexture { get; set; }
        public static Texture2D YellowAppleTexture { get; set; }
        public static Texture2D RottenAppleTexture { get; set; }

        public float RotationSpeed { get; set; } = 0;
        public float Rotation { get; set; } = 0;

        public AppleType Type { get; private set; }

        public Apple(AppleType type, Vector2 pos)
        {
            this.Type = type;
            RandomSet(pos);
        }

        public Apple(Vector2 pos, Vector2 dir, float scale, float rotation, float rotationSpeed)
        {
            this.Pos = pos;
            this.Dir = dir;
            this.Scale = scale;
            this.RotationSpeed = rotationSpeed;
            this.Rotation = rotation;
        }



        public Apple(Vector2 Dir)
        {
            this.Dir = Dir;
        }

        public void Update()
        {
            Pos += Dir;
        }

        public void RandomSet(Vector2 pos)
        {
            Pos = pos;
            Dir = new Vector2(0, Apples.GetIntRnd(4, 8));
            Scale = 1;
            RotationSpeed = (float)(Apples.random.NextDouble() - 0.5) / 2;
            color = Color.White;
        }
        public void Draw()
        {
            Texture2D texture;

            switch (Type)
            {
                case AppleType.Green:
                    texture = GreenAppleTexture;
                    break;
                case AppleType.Yellow:
                    texture = YellowAppleTexture;
                    break;
                case AppleType.Rotten:
                    texture = RottenAppleTexture;
                    break;
                default:
                    texture = Texture2D;
                    break;
            }

            Apples.SpriteBatch.Draw(texture, Pos, null, color, Rotation, Center, Scale, SpriteEffects.None, 0);
        }

        public bool IsCollidingWith(Basket basket)
        {
            Rectangle appleRect = new((int)Pos.X, (int)Pos.Y, Texture2D.Width, Texture2D.Height);
            return appleRect.Intersects(basket.GetBounds());
        }
    }
}