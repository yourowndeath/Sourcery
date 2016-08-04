using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Элемент списка</summary>
  public class PanelItem:Control
  {
    #region Поля
    /// <summary>Спрайт выделения элемента</summary>
    private AnimateSprite _Texture;

    /// <summary>Шрифт отрисовки</summary>
    private SpriteFont _Font;

    /// <summary>Выделение элемента</summary>
    public bool Selected;

    /// <summary>Предыдущее состояние мыши</summary>
    private MouseState _LastMouseState;

    /// <summary>Элемент выбран</summary>
    public bool Chosen = false;

    #endregion

    #region События

    /// <summary>Выбрали элемент</summary>
    public override event Control.MethodChange OnValueChanged;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="PanelItem"/>.
    /// </summary>
    /// <param name="game">The game.</param>
    /// <param name="text">The text.</param>
    public PanelItem(SourceryGame game, string text,string fontName="")
    {
      Caption = text;
      _Texture = new AnimateSprite(game.ItemButton, 3, 1);
      if (fontName == "")
        _Font = game.BigFont;
      else
        _Font = game.Content.Load<SpriteFont>("Fonts/"+fontName);
      _Texture.CurrentFrame = 2;
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
      _Texture.Draw(spriteBatch, rectangle);
      spriteBatch.DrawString(_Font, Caption, new Vector2(rectangle.Left + 5, rectangle.Top), Color.Black);
    }

    /// <summary>
    /// Реакция на изменения мыши
    /// </summary>
    /// <param name="state">The state.</param>
    public override void ChangeState(Microsoft.Xna.Framework.Input.MouseState state)
    {
      if (state.LeftButton != ButtonState.Pressed && state.RightButton != ButtonState.Pressed)
      {
        if (Rect.Contains(state.X, state.Y))
          _Texture.CurrentFrame = 0;
        else if (!Selected)
          _Texture.CurrentFrame = 2;
      }
      if (state.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton == ButtonState.Released && Rect.Contains(state.X, state.Y))
      {
        Chosen = true;
        if (OnValueChanged != null)
          OnValueChanged();
        _Texture.CurrentFrame = 1;
        Selected = true;
      }
      _LastMouseState = state;
    }
 
    #endregion
  }
}
