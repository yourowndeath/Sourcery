using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Абстрактный класс экрана</summary>
  abstract public class Screen
  {

    /// <summary>Тип экрана</summary>
    public ScreenType Type { get { return Settings.Type; } }

    /// <summary>Настройки экрана</summary>
    public static ScreenSettings Settings;

    /// <summary>Текстура задника</summary>
    public Texture2D Background {get {
      if (Settings.Texture != "")
        return Helper.Game.Content.Load<Texture2D>(Settings.Texture);
      else
        return null;
    }
    }

    /// <summary>Область отрисовки</summary>
    public Rectangle Rect
    { get { return new Rectangle(0, 0, Helper.ScreenWidth, Helper.ScreenHeight);} }

    /// <summary>Загружает контент экрана</summary>
    /// <param name="textureName">имя текстуры задника.</param>
    public abstract void LoadContent();

    /// <summary>Обновляет экран</summary>
    /// <param name="gameTime">Игровое время.</param>
    public abstract void Update(GameTime gameTime);

    /// <summary>Отрисовывает экран</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public abstract void Draw(SpriteBatch spriteBatch);
  }
}
