using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Контрол с затемнением</summary>
  class ColorZone
  {
    #region Поля
    /// <summary>Максимум</summary>
    private int _MaxResourse;

    /// <summary>Текущее положение</summary>
    private int _CurrentResourse;

    /// <summary>Разноцветные полоски</summary>
    private AnimateSprite _Sprite;

    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Затемнение</summary>
    private Texture2D _Back;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="ColorZone"/>.</summary>
    /// <param name="game">Ссылка на игру.</param>
    /// <param name="MaxResource">Максимум.</param>
    /// <param name="CurrentResourse">Текущее положение.</param>
    /// <param name="Position">Номер спрайта.</param>
    public ColorZone(SourceryGame game, int MaxResource, int CurrentResourse, int Position)
    {
      _MaxResourse = MaxResource;
      _CurrentResourse = CurrentResourse;
      _Sprite = new AnimateSprite(game.AllMagic, 1, 5);
      _Sprite.CurrentFrame = Position;
      _Font = game.BigFont;
      _Back = game.Content.Load<Texture2D>("Controls/Back");
    }
    #endregion
   
    #region Методы
    /// <summary>Рисуем</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rect">Прямоугольник.</param>
    public void Draw(SpriteBatch spriteBatch, Rectangle rect)
    {
      _Sprite.Draw(spriteBatch, rect);
      int position;
      if (_MaxResourse != 0)
        position = rect.Width * _CurrentResourse / _MaxResourse;
      else
        position = 0;
      var text = _CurrentResourse.ToString() + "/" + _MaxResourse.ToString();
      Vector2 FontOrigin = _Font.MeasureString(text) / 2;
      spriteBatch.DrawString(_Font, text, new Vector2(rect.Right - rect.Width / 2, rect.Bottom - rect.Height / 2), Color.White, 0, FontOrigin, 1, SpriteEffects.None, 1);
      spriteBatch.Draw(_Back, new Rectangle(rect.X, rect.Y, position, rect.Height), new Color(0, 0, 0, 175));
    }

    /// <summary>Обновляем</summary>
    /// <param name="currentResourse">Текущее положение.</param>
    /// <param name="MaxResourse">Максимум.</param>
    public void Update(int currentResourse, int MaxResourse)
    {
      _CurrentResourse = currentResourse;
      _MaxResourse = MaxResourse;
    }
    #endregion

  }
}
