using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Чек бокс</summary>
  class CheckBox:Control
  {
    #region Поля
    /// <summary>Текстура чекбокса</summary>
    private AnimateSprite _Sprite;

    /// <summary>Шрифт заголовка</summary>
    private SpriteFont _Font;

    /// <summary>Выбран</summary>
    private bool _Checked;

    /// <summary>Предыдущее состояние мыши</summary>
    private MouseState _LastMouseState;
    #endregion

    #region События

    /// <summary>Щелкнули по чекбоксу</summary>
    public override event Control.MethodChange OnValueChanged;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="CheckBox"/>.
    /// </summary>
    /// <param name="game">Ссылка на игру</param>
    /// <param name="caption">Заголовок</param>
    public CheckBox(SourceryGame game, string caption)
    {
      Caption = caption;
      _Sprite = new AnimateSprite(game.CheckBox,4,1);
      _Font = game.Font;
      _Checked = false;
    }
    #endregion

    #region Свойства

    /// <summary>
    /// Возвращает или задает свойство  <see cref="CheckBox"/> включен.
    /// </summary>
    public bool Checked 
    {
      get { return _Checked; }
      set 
      {
        if (value)
          _Sprite.CurrentFrame = 3;
        else
          _Sprite.CurrentFrame = 0;
        _Checked = value;
      }
    }
    #endregion

    #region Методы

    /// <summary>
    /// Рисовать по текущей области
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (Rect != null)
        Draw(spriteBatch,Rect);
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
    public override void Draw(SpriteBatch spriteBatch,Rectangle rectangle)
    {
      Rect = rectangle;
      spriteBatch.DrawString(_Font, Caption, new Vector2(rectangle.Left + 40, rectangle.Top + 5), Color.Black);
      _Sprite.Draw(spriteBatch, new Rectangle(rectangle.Left, rectangle.Top, 30, 30));

    }

    /// <summary>
    /// Реакция на изменения мыши
    /// </summary>
    /// <param name="state">The state.</param>
    public override void ChangeState(MouseState state)
    {
      if (state.LeftButton != ButtonState.Pressed && state.RightButton != ButtonState.Pressed)
      {
        if (_Sprite.Contains(state.X, state.Y))
        {
          if (_Checked)
            _Sprite.CurrentFrame = 2;
          else
            _Sprite.CurrentFrame = 1;
        }
        else
        {
          if (_Checked)
            _Sprite.CurrentFrame = 3;
          else
            _Sprite.CurrentFrame = 0;
        }
      }
      if (state.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton == ButtonState.Released && _Sprite.Contains(state.X, state.Y))
      { 
          Checked = !_Checked;
          if (OnValueChanged != null)
            OnValueChanged();        
      }
      _LastMouseState = state;
    }
    #endregion
  }
}
