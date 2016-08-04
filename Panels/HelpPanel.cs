using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Панель справки</summary>
  class HelpPanel:Panel
  {
    #region Поля
    /// <summary>Текстура панели</summary>
    private Texture2D _Texture;

    /// <summary>Шрифт отрисовки текста</summary>
    private SpriteFont _Font;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="HelpPanel"/>.
    /// </summary>
    /// <param name="game">The game.</param>
    public HelpPanel(SourceryGame game)
    {
      _Texture = game.Panel;
      _Font = game.BigFont;
      Items = new List<Control>();
      Type = PanelTypes.Help;
    }
    #endregion

    #region Методы
    /// <summary>
    /// Рисуем панель по вектору
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">Вектор</param>
    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 location)
    {
      int width = _Texture.Width;
      int height = _Texture.Height;
      Rectangle sourceRectangle = new Rectangle(width, height, width, height);
      Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
      spriteBatch.Draw(_Texture, destinationRectangle, sourceRectangle, Color.White);
    }

    /// <summary>
    /// Рисуем панель в прямоугольнике
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rectangle">The rectangle.</param>
    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle rectangle)
    {
      spriteBatch.Draw(_Texture, rectangle, Color.White);
      spriteBatch.DrawString(_Font, "Здесь будет справка", new Vector2(rectangle.Left + 50, rectangle.Height / 2), Color.Black);
    }

    /// <summary>
    /// Реагируем на изменение мыши
    /// </summary>
    /// <param name="state">Состояние мыши</param>
    public override void ChangeState(Microsoft.Xna.Framework.Input.MouseState state)
    {

    }
    #endregion
  }
}
