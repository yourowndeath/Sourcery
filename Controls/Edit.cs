using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Редактор текста</summary>
  class Edit
  {
    #region Поля
    /// <summary>Текстура блока</summary>
    private AnimateSprite _Texture;

    /// <summary>Курсор редактирования</summary>
    private Texture2D _Cursor;

    /// <summary>Последнее состояние мыши</summary>
    private MouseState _LastMouseState;

    /// <summary>Последнее состояние клавиатуры</summary>
    private KeyboardState _LastKeyboardState;

    /// <summary>Текст внутри</summary>
    private string _Text;

    /// <summary>Шрифт отрисовки</summary>
    private SpriteFont _Font;

    /// <summary>Рисовать ли курсор</summary>
    private bool _Draw;

    /// <summary>Курсор не виден</summary>
    private int _Delay = 25;

    /// <summary>Курсор виден</summary>
    private int _DrawDelay = 25;

    /// <summary>Нажатая кнопка</summary>
    private Keys _LastKey;

    /// <summary>Ссылка на игру</summary>
    private SourceryGame _Game;

    /// <summary>Предыдущий прямоугольник для курсора</summary>
    private Rectangle _LastRectangle;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="Edit"/>.</summary>
    /// <param name="game">Ссылка на игру.</param>
    /// <param name="text">Текст.</param>
    public Edit(SourceryGame game)
    {
      _Texture = new AnimateSprite(game.Edit, 2, 1);
      _Font = game.Font;
      _Text = game.PlayerName;
      _Cursor = game.Editing;
      _Draw = false;
      _Game = game;
    }
    #endregion

    #region Методы
    /// <summary>Рисуем компонент</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rect">The rect.</param>
    public void Draw(SpriteBatch spriteBatch, Rectangle rect)
    {
      _Texture.Draw(spriteBatch, rect);
      Vector2 FontOrigin = _Font.MeasureString(_Text) / 2;
      spriteBatch.DrawString(_Font, _Text, new Vector2(rect.Right - rect.Width / 2, rect.Bottom - rect.Height / 2), Color.White, 0, FontOrigin, 1, SpriteEffects.None, 1);
      if (_Texture.CurrentFrame == 1 && _Draw)
      {
        Rectangle rc;
        if (_Text != "")
        {
          rc = new Rectangle((int)Math.Round(rect.Right - rect.Width / 2 + FontOrigin.X) - 5, (int)Math.Round(rect.Bottom - rect.Height / 2 - FontOrigin.Y) - 3, 15, 20);
          _LastRectangle = rc;
        }
        else
          rc = _LastRectangle;
        spriteBatch.Draw(_Cursor, rc, Color.White);
      }
    }

    /// <summary>Обновляем состояние</summary>
    /// <param name="mouseState">Состояние мыши</param>
    /// <param name="keyboardState">Состояние клавиатуры</param>
    public void Update(MouseState mouseState, KeyboardState keyboardState)
    {

      if (mouseState.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton == ButtonState.Released)
      {
        if (_Texture.Contains(mouseState.X, mouseState.Y))
          _Texture.CurrentFrame = 1;
        else
        {
          _Texture.CurrentFrame = 0;
          _Game.PlayerName = _Text;
        }
      }
      if (keyboardState.IsKeyUp(Keys.Back) && (_LastKeyboardState.IsKeyDown(Keys.Back)) && _Texture.CurrentFrame == 1)
      {
        if (_Text.Length>0)
        _Text = _Text.Substring(0, _Text.Length - 1);
        else
          _Text="";
      }
      var keys = keyboardState.GetPressedKeys();
      foreach (Keys key in keys)
        if (key >= Keys.A && key <= Keys.Z)
          _LastKey = key;

      if (keyboardState.IsKeyUp(_LastKey) && _LastKeyboardState.IsKeyDown(_LastKey))
        _Text = _Text + _LastKey.ToString();

      _LastMouseState = mouseState;
      _LastKeyboardState = keyboardState;

      if (_Texture.CurrentFrame == 1)
      {
        if (_Delay != 0 && _DrawDelay == 25)
        {
          _Delay--;
          _Draw = false;
          return;
        }
        else
        {
          if (_DrawDelay != 0)
          {
            _DrawDelay--;
            _Draw = true;
            _Delay = 25;
            return;
          }
          else { _DrawDelay = 25; }
        }
      }

    }
    #endregion
  }
}
