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

  /// <summary>Трекбар</summary>
  class TrackBar:Control
  {
    #region Поля
    /// <summary>Двигающаяся часть</summary>
    private AnimateSprite _Lent;

    /// <summary>Увеличиваем</summary>
    private AnimateSprite _Plus;

    /// <summary>Уменьшаем</summary>
    private AnimateSprite _Minus;

    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Ссылка на игру</summary>
    private SourceryGame _Game;

    /// <summary>Ссылка на звук</summary>
    private SoundEffect _Sound;

    /// <summary>Предыдущее состояние кнопки</summary>
    private MouseState _LastMouseState;

    #endregion

    #region События

    /// <summary>Поменялось состояние</summary>
    public override event MethodChange OnValueChanged;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="TrackBar"/>.
    /// </summary>
    /// <param name="game">The game.</param>
    /// <param name="caption">The caption.</param>
    public TrackBar(SourceryGame game, string caption)
    {
      Caption = caption;
      _Game = game;
      _Sound = game.Content.Load<SoundEffect>("Sounds/Click");
      _Font = game.Font;
      _Lent = new AnimateSprite(game.TrackLent, 11, 1);
      _Plus = new AnimateSprite(game.PlusButton, 2, 1);
      _Minus = new AnimateSprite(game.MinusButton, 2, 1);
    }

    #endregion

    #region Свойства

    /// <summary>Возвращает или задает позицию трекбара</summary>
    public int Value 
    {
      get { return _Lent.CurrentFrame; }
      set { _Lent.CurrentFrame = value; }
    }
    #endregion

    #region Методы

    private void PlaySound()
    {
      float snd = 12 - _Game.SoundVolume();
      if (snd != 12)
        _Sound.Play(1 / snd, 0, 0);
    }

    /// <summary>
    /// Рисовать по текущей области
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (Rect != null)
       Draw(spriteBatch, Rect);
    }

    /// <summary>
    /// Рисовать по вектору
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">Вектор</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Draw(SpriteBatch spriteBatch, Vector2 location)
    {
      throw new NotImplementedException();
    }


    /// <summary>
    /// Рисовать в прямоугольнике
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rectangle">Прямоугольник</param>
    public override void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
    {
      Rect = rectangle;
      Vector2 FontOrigin = _Font.MeasureString(Caption) / 2;
      Rectangle rect = new Rectangle(rectangle.Left+5, rectangle.Top, 23, 50);
      _Minus.Draw(spriteBatch, rect);
      Rectangle LentRect = new Rectangle(rect.Right, rect.Top+10,rectangle.Width-80,30);
      _Lent.Draw(spriteBatch,LentRect);
      spriteBatch.DrawString(_Font, Caption, new Vector2(LentRect.Right - LentRect.Width / 2, LentRect.Top+LentRect.Height/2), Color.Black, 0, FontOrigin, 1, SpriteEffects.None, 1);
      rect = new Rectangle(LentRect.Right, rectangle.Top, 23, 50);
      _Plus.Draw(spriteBatch, rect);
    }


    /// <summary>
    /// Реакция на изменения мыши
    /// </summary>
    /// <param name="state">The state.</param>
    public override void ChangeState(Microsoft.Xna.Framework.Input.MouseState state)
    {
      if (state.LeftButton != ButtonState.Pressed && state.RightButton != ButtonState.Pressed)
      {
        if (_Plus.Contains(state.X, state.Y))
          _Plus.CurrentFrame = 1;
        else
          _Plus.CurrentFrame = 0;
        if (_Minus.Contains(state.X, state.Y))
          _Minus.CurrentFrame = 1;
        else
          _Minus.CurrentFrame = 0;
      }
      if (state.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton==ButtonState.Released)
      {
        if (_Plus.Contains(state.X, state.Y) && (_Lent.CurrentFrame != 11))
        {
          _Lent.Update();
          PlaySound();
          if (OnValueChanged!=null)
            OnValueChanged();
        }
        if (_Minus.Contains(state.X, state.Y) && (_Lent.CurrentFrame != 0))
        {
          --_Lent.CurrentFrame;
          PlaySound();
          if (OnValueChanged != null)
           OnValueChanged();
        }

      }
     _LastMouseState = state;
    }
    #endregion
  }
}
