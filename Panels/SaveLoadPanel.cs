using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Панель Сохранения/Загрузки</summary>
  class SaveLoadPanel:Panel
  {
    #region Поля

    /// <summary>Ссылка на игру</summary>
    private SourceryGame _Game;

    /// <summary>Текстура панели</summary>
    private Texture2D _Texture;

    /// <summary>Кнопка Сохранить/Загрузить</summary>
    private Button _SaveLoadButton;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="SaveLoadPanel"/>.
    /// </summary>
    /// <param name="game">Ссылка на игру</param>
    /// <param name="IsSave">if set to <c>true</c> [is save].</param>
    public SaveLoadPanel(SourceryGame game, bool IsSave)
    {
      _Game = game;
      _Texture = game.Panel;
      Items = new List<Control>();
      var _ButtonFont = game.Font;
      if (IsSave)
        _SaveLoadButton = new Button(game, "Сохранить");
      else
        _SaveLoadButton = new Button(game, "Загрузить");
      LoadGames();
    }
    #endregion

    #region Методы

    /// <summary>Загружает список сохраненных игр</summary>
    private void LoadGames()
    {
      var dir = new DirectoryInfo("Save"); // папка с файлами
      foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список
        Items.Add(new PanelItem(_Game, Path.GetFileNameWithoutExtension(file.FullName)));
      foreach (Control item in Items)
        item.OnValueChanged += OnSelectItem;
    }
    private void OnSelectItem()
    {
      foreach (PanelItem item in Items)
        item.Selected = false;
    }
    /// <summary>
    /// Отрисовывает панель по вектору
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">The location.</param>
    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 location)
    {
      int width = _Texture.Width;
      int height = _Texture.Height;
      Rectangle sourceRectangle = new Rectangle(width, height, width, height);
      Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
      spriteBatch.Draw(_Texture, destinationRectangle, sourceRectangle, Color.White);
    }

    /// <summary>
    /// Отрисовывает панель в прямоугольнике
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rectangle">The rectangle.</param>
    public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle rectangle)
    {
      spriteBatch.Draw(_Texture, rectangle, Color.White);
      _SaveLoadButton.Rect = new Rectangle(rectangle.Right - 220, rectangle.Bottom + 20, 200, 40);
      _SaveLoadButton.Draw(spriteBatch);
      int i = 1;
      foreach (PanelItem item in Items)
      {
        item.Rect = new Rectangle(rectangle.Left + 50, rectangle.Top + 30 * i, rectangle.Width - 100, 30);
        item.Draw(spriteBatch);
        i++;
      }
    }


    /// <summary>
    /// Реагируем на апдейт игры
    /// </summary>
    /// <param name="state">The state.</param>
    public override void ChangeState(Microsoft.Xna.Framework.Input.MouseState state)
    {
      _SaveLoadButton.ChangeState(state);
      foreach (PanelItem item in Items)
        item.ChangeState(state);
    }
    #endregion

  }
}
