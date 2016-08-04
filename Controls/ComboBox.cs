using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Комбобокс</summary>
  class ComboBox:Control
  {
    #region Поля

    /// <summary>Список элементов</summary>
    private List<Control> _Values;

    /// <summary>Спрайт для контрола</summary>
    private AnimateSprite _Sprite;

    /// <summary>Полотно для элементов</summary>
    private Texture2D _Panel;

    /// <summary>Раскрыть список</summary>
    private Boolean _Expand = false;

    /// <summary>Шрифт заголовка</summary>
    private SpriteFont _Font;

    /// <summary>Предыдущее состояние мыши</summary>
    private MouseState _LastMouseState;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="ComboBox"/>.
    /// </summary>
    /// <param name="game">The game.</param>
    /// <param name="values">The values.</param>
    public ComboBox(SourceryGame game, List<String> values)
    {
      _Values = new List<Control>();
      foreach (string str in values)
        _Values.Add(new PanelItem(game, str,"Font"));
      foreach (Control item in _Values)
        item.OnValueChanged += OnElementSelected;
      _Sprite = new AnimateSprite(game.ComboBox, 3, 1);
      _Panel = game.ItemPanel;
      _Font = game.Font;
    }
    #endregion

    #region События
    public override event Control.MethodChange OnValueChanged;
    #endregion

    #region Методы

    /// <summary>Выделение элемента</summary>
    private void OnElementSelected()
    {
      if (!_Expand)
      {
        foreach (PanelItem item in _Values)
        {
          item.Selected = false;
          item.Chosen = false;
        }
        return;
      }
      foreach (PanelItem item in _Values)
        if (item.Chosen)
        {
          Caption = item.Caption;
          if (OnValueChanged != null)
            OnValueChanged();
          _Expand = false;
        }
      foreach (PanelItem item in _Values)
      {
        item.Selected = false;
        item.Chosen = false;
      }
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
      _Sprite.Draw(spriteBatch, rectangle);
      spriteBatch.DrawString(_Font, Caption, new Vector2(rectangle.Left + 5, rectangle.Top), Color.Black);
      if (_Expand)
      {
        spriteBatch.Draw(_Panel, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 20 * _Values.Count), new Color(255, 255, 255, (byte)MathHelper.Clamp(70, 0, 255)));
        int i = 0;
        foreach (Control item in _Values)
        {
          item.Draw(spriteBatch, new Rectangle(rectangle.Left, rectangle.Bottom + 20 * i, rectangle.Width, 20));
          i++;
        }
      }
    }

    /// <summary>
    /// Реакция на изменения мыши
    /// </summary>
    /// <param name="state">The state.</param>
    public override void ChangeState(MouseState state)
    {
      foreach (Control item in _Values)
        item.ChangeState(state);
      if (state.LeftButton != ButtonState.Pressed && state.RightButton != ButtonState.Pressed)
      {
        if (_Sprite.Contains(state.X, state.Y))
          _Sprite.CurrentFrame = 1;
        else
          _Sprite.CurrentFrame = 0;
      }
      if (state.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton == ButtonState.Released && _Sprite.Contains(state.X, state.Y))
      {
        _Sprite.CurrentFrame = 2;
        _Expand = !_Expand;
      }
      _LastMouseState = state;
    }
    #endregion
  }
}
