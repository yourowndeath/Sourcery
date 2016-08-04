using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Абстрактный класс для контрола</summary>
  abstract public class Control
  {

    /// <summary>Текст контрола</summary>
    public string Caption;

    /// <summary>Область отрисовки</summary>
    public Rectangle Rect;

    /// <summary>
    /// Рисовать по текущей области
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public abstract void Draw(SpriteBatch spriteBatch);

    /// <summary>
    /// Рисовать по вектору
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">Вектор</param>
    public abstract void Draw(SpriteBatch spriteBatch, Vector2 location);

    /// <summary>
    /// Рисовать в прямоугольнике
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rectangle">Прямоугольник</param>
    public abstract void Draw(SpriteBatch spriteBatch, Rectangle rectangle);

    /// <summary>
    /// Реакция на изменения мыши
    /// </summary>
    /// <param name="state">The state.</param>
    public abstract void ChangeState(MouseState state);

    /// <summary>Событие щелчка по контролу</summary>
    public delegate void MethodChange();
    public abstract event MethodChange OnValueChanged;
  }
}
