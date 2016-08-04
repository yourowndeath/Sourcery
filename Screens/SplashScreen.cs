using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{
  /// <summary>Окно сплеша</summary>
  class SplashScreen:Screen
  {
    #region Поля
    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Текстура ожидания</summary>
    private AnimateSprite _Loader;
   
    /// <summary>Задержка</summary>
    private int Delay = 0;

    private int Cycle = 0;
    int fadeOut = 150;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="SplashScreen"/>.
    /// </summary>
    /// <param name="game">Ссылка на игру.</param>
    public SplashScreen()
    {
      Settings = Helper.Settings.Screens(ScreenType.SplashScreen);
    }
    #endregion

    #region Методы
    /// <summary>Загружаем необходиоме</summary>
    public override void LoadContent()
    {

      _Font = Helper.Game.BigFont;
      _Loader = new AnimateSprite(Helper.Game.Loader, 1, 11);
    }


    /// <summary>Обновляет текущее состояние</summary>
    public override void Update(GameTime gameTime)
    {
      Cycle++;
      if (Cycle > 100)
        fadeOut -= 5;
      if (Delay == 0)
      {
        _Loader.UpdateConstantly();
        Delay = 10;
      }
      else
        Delay--;
    }


    /// <summary>Рисуем</summary>
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      Vector2 FontOrigin = _Font.MeasureString("SOURCERY") / 2;
      if (Cycle < 100)
      {
        spriteBatch.Draw(Background, Rect, Color.White);

        spriteBatch.DrawString(_Font, "SOURCERY", new Vector2(Rect.Width / 2, Rect.Height / 2), Color.Black, 0, FontOrigin, 1, SpriteEffects.None, 1);
        _Loader.Draw(spriteBatch, new Vector2(Rect.Width / 2 - 50, (Rect.Height / 2) + 50));
      }
      else
        spriteBatch.Draw(Background, Rect, new Color(fadeOut, 0, 0));

      spriteBatch.End();
    }
    #endregion
  }
}
