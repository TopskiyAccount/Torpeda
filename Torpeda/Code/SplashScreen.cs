using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Torpeda;

static class SplashScreen
{
    public static Texture2D menu { get; set; }
    static int TimeCounter = 0;
    static Color color;
    public static SpriteFont Font { get; set; }
    static Vector2 TextPosition = new Vector2(900, 100);

    static public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(menu, new Rectangle(0, 0, 1920, 1080), Color.White);
        spriteBatch.DrawString(Font, "Торпеда!", TextPosition, color);
        spriteBatch.DrawString(Font, "Нажмите 'Space', чтобы начать игру", new(100, 600), color);
        spriteBatch.DrawString(Font, "Нажмите 'ContolLeft', чтобы стрелять", new(100, 800), color);

    }

    static public void Update()
    {
        color = Color.FromNonPremultiplied(255, 255, 255, TimeCounter % 256);
        TimeCounter++;
    }
}
