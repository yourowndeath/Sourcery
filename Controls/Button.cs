using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Кнопка</summary>
  class Button:Control
  {
    #region Поля

    /// <summary>Спрайт для кнопки</summary>
    private AnimateSprite _Sprite;

    /// <summary>Шрифт отрисовки</summary>
    private SpriteFont _Font;

    /// <summary>Предыдущее состояние мыши</summary>
    private MouseState _LastMouseState;

    /// <summary>Звук нажатия</summary>
    private SoundEffect _Sound;

    private SourceryGame _Game;
    #endregion

    #region События

    /// <summary>Нажали на кнопку</summary>
    public override event Control.MethodChange OnValueChanged;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="Button"/>.
    /// </summary>
    /// <param name="texture">Текстура кнопки</param>
    /// <param name="text">Текст кнопки</param>
    /// <param name="font">Шрифт кнопки</param>
    public Button(SourceryGame game, string text)
    {
      _Sprite = new AnimateSprite(game.Button, 3, 1);
      Caption = text;
      _Font = game.Font;
      _Sound = game.ClickSound;
      _Game = game;
    }
    #endregion

    #region Методы

    public override void Draw(SpriteBatch spriteBatch, Vector2 location)
    {
      throw new NotImplementedException();
    }

    public override void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
    {
      Rect = rectangle;
      _Sprite.Draw(spriteBatch, rectangle);
      Vector2 FontOrigin = _Font.MeasureString(Caption) / 2;
      spriteBatch.DrawString(_Font, Caption, new Vector2(rectangle.Right - rectangle.Width / 2, rectangle.Bottom - rectangle.Height / 2), Color.White, 0, FontOrigin, 1, SpriteEffects.None, 1);
    }

    public override void ChangeState(Microsoft.Xna.Framework.Input.MouseState state)
    {
      if (state.LeftButton != ButtonState.Pressed && state.RightButton != ButtonState.Pressed)
      {
        if (_Sprite.Contains(state.X, state.Y))
          _Sprite.CurrentFrame = 1;
        else
          _Sprite.CurrentFrame = 0;
      }
      if (state.LeftButton == ButtonState.Pressed && _Sprite.Contains(state.X, state.Y) && _LastMouseState.LeftButton==ButtonState.Released)
      {
        _Sprite.CurrentFrame = 2;
        if (OnValueChanged!=null)
         OnValueChanged();
        float snd = 12 - _Game.SoundVolume();
        if (snd != 12)
          _Sound.Play(1 / snd, 0, 0);
      }
      _LastMouseState = state;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (Rect != null)
        Draw(spriteBatch, Rect);
    }
    #endregion

  }
}
