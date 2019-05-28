using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Экран разработчкиков</summary>
  class DeveloperScreen:Screen
  {
    #region Поля

    /// <summary>Прозрачность</summary>
    int mAlphaValue = 1;

    /// <summary>Количество обновлений</summary>
    int Cycle = 0;

    /// <summary>Приращение прозрачности</summary>
    int mFadeIncrement = 5;

    /// <summary>Начальный цвет затемнения</summary>
    int fadeOut = 150;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="DeveloperScreen"/>.</summary>
    /// <param name="game">Ссылка на игру.</param>
    public DeveloperScreen()
    {
      Settings = Helper.Settings.Screens(ScreenType.DeveloperScreen);
    }
    #endregion

    #region Методы

    /// <summary>Загружает контент</summary>
    /// <param name="textureName">Имя текстуры задника.</param>
    public override void LoadContent()
    {

    }

    /// <summary>Реагирует на изменения</summary>
    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      Cycle++;
      if (Cycle > 100)
        fadeOut -= 5;
      if (mAlphaValue >= 255 || mAlphaValue <= 0)
      {
        mAlphaValue = 0;
      }
      else
        mAlphaValue += mFadeIncrement;
    }

    /// <summary>Отрисовывает</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      if (mAlphaValue != 0)
        spriteBatch.Draw(Background, Rect, new Color(255, 255, 255, MathHelper.Clamp(mAlphaValue, 0, 255)));
      else if (Cycle > 100)
        spriteBatch.Draw(Background, Rect, new Color(fadeOut, 0, 0));
      else
        spriteBatch.Draw(Background, Rect, Color.White);
      spriteBatch.End();
    }
    #endregion
  }
}
