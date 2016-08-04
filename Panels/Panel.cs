using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Абстрактный класс панели</summary>
  abstract public class Panel
  {

    /// <summary>Тип панели</summary>
    public PanelTypes Type;

    /// <summary>Элементы панели</summary>
    public List<Control> Items;

    /// <summary>
    /// Рисуем панель по вектору
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">Вектор</param>
    public abstract void Draw(SpriteBatch spriteBatch, Vector2 location);

    /// <summary>
    ///  Рисуем панель в прямоугольнике
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rectangle">The rectangle.</param>
    public abstract void Draw(SpriteBatch spriteBatch, Rectangle rectangle);

    /// <summary>
    /// Реагируем на изменение мыши
    /// </summary>
    /// <param name="state">Состояние мыши</param>
    public abstract void ChangeState(MouseState state);
  }
}
