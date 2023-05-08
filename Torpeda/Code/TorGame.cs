using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;



namespace Torpeda
{
    class TorGame
    {
        public static int Wight, Height;
        public static Random rnd = new Random();
        static public SpriteBatch SpriteBatch { get; set; }
        public static Texture2D backgroundGame { get; set; }
        static List<Fire> fires = new List<Fire>();
        static List<Ship> ships = new List<Ship>();



        static public Scope Scope { get; set; }

        static public int GetIntRnd(int min, int max)
        {
            return rnd.Next(min, max);
        }

        static public void ShipFire()
        {
            fires.Add(new Fire(Scope.GetPosForFire));
        }

        static public void Init(SpriteBatch SpriteBatch, int Wight, int Height)
        {
            TorGame.Wight = Wight;
            TorGame.Height = Height;
            TorGame.SpriteBatch = SpriteBatch;
            Scope = new Scope(new Vector2(Wight / 2 - 145, Height / 2 - 85));
            for (int i = 1; i < 10; i++)
                ships.Add(new Ship());
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundGame, new Rectangle(0, 0, 1920, 1080), Color.White);
            foreach (Ship ship in ships)
                ship.Draw();
            foreach (Fire fire in fires)
                fire.Draw();
            Scope.Draw();
        }
        public static void Update()
        {
            foreach (Ship ship in ships)
                ship.Update();
            for (int i = 0; i < fires.Count; i++)
            {
                fires[i].Update();
                Ship shipCrash = fires[i].Crash(ships);
                if (shipCrash != null)
                {
                    ships.Remove(shipCrash);
                    fires.RemoveAt(i);
                    i--;
                    continue;
                }
                if (fires[i].Hidden)
                {
                    fires.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    class Ship
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;
        float scale;
        Point size;

        public static Texture2D Texture2D { get; set; }
        public bool IsIntersect(Rectangle rectangle)
        {
            return rectangle.Intersects(new Rectangle((int)Pos.X, (int)Pos.Y, size.X, size.Y));
        }

        public Ship(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }
        public Ship(Vector2 Dir)
        {
            this.Dir = Dir;
        }
        public Ship()
        {
            Position();
        }
        public void Update()
        {
            Pos += Dir;
            if (Pos.X < -Texture2D.Width)
                Position();
        }
        public void Position()
        {
            Pos = new Vector2(TorGame.GetIntRnd(TorGame.Wight, TorGame.Wight + 100), TorGame.GetIntRnd(300, 400));
            Dir = new Vector2(-(float)TorGame.rnd.NextDouble() * 2 + 0.1f, 0f);
            scale = (float)TorGame.rnd.NextDouble();
            color = Color.White;
        }
        public void Draw()
        {
            TorGame.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
    class Scope
    {
        Vector2 Pos;
        Color color = Color.White;
        public int Speed { get; set; } = 3;
        public static Texture2D Texture2D { get; set; }

        public Scope(Vector2 Pos)
        {
            this.Pos = Pos;
        }

        public Vector2 GetPosForFire => new Vector2(Pos.X + 30, Pos.Y + 30);

        public void Left()
        {
            if (this.Pos.X > 0) this.Pos.X -= Speed;
        }

        public void Right()
        {
            if (this.Pos.X < TorGame.Wight - Texture2D.Width) this.Pos.X += Speed;
        }

        public void Draw()
        {
            TorGame.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
    class Fire
    {
        const int speed = 3;
        public Vector2 Pos;
        Vector2 Dir;
        Color color = Color.White;
        public float Scale { get; private set; }
        static Vector2 center => new Vector2(-80, Fire.Texture2D.Height - 600);

        public static Texture2D Texture2D { get; set; }

        public float Rotation { get; set; } = 0;
        public float RotationSpeed { get; set; } = 0;

        public Fire(Vector2 Pos)
        {
            this.Pos = Pos;
            this.Dir = new Vector2(0, -speed);
            this.Rotation = Rotation;
            this.RotationSpeed = RotationSpeed;

        }

        public void SetRotation()
        {

            Scale = (float)TorGame.GetIntRnd(98, 101) / 100;
            RotationSpeed = (float)(1 / -10);

        }

        public Ship Crash(List<Ship> ships)
        {
            foreach (Ship ship in ships)
                if (ship.IsIntersect(new Rectangle((int)Pos.X, (int)Pos.Y, Texture2D.Width, Texture2D.Height))) return ship;
            return null;
        }

        public bool Hidden
        {
            get { return Dir.X > TorGame.Wight; }
        }
        public void Update()
        {

            Pos += Dir;
            Rotation += RotationSpeed;

            if (Pos.X < TorGame.Wight)
            {
                SetRotation();
                if (Pos.Y < TorGame.Height / 5)
                {
                    Scale = (float)TorGame.GetIntRnd(98, 101) / 100;
                }
            }


        }
        public void Draw()
        {
            if (Pos.Y > TorGame.Height / 20)
            {
                TorGame.SpriteBatch.Draw(Texture2D, Pos, null, color, Rotation, center, Scale, SpriteEffects.None, 0);
            }
        }
    }
}
