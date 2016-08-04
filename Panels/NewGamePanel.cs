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

  /// <summary>Панель создания новой игры</summary>
  class NewGamePanel:Panel
  {
    #region Поля
    /// <summary>Текстура панели</summary>
    private Texture2D _Texture;

    /// <summary>Класс игры</summary>
    private SourceryGame _Game;

    /// <summary>Начать игру</summary>
    private Button _StartGame;

    /// <summary>Начинаем игру</summary>
    public bool Start;
    #endregion

    #region Конструкторы
    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="NewGamePanel"/>.
    /// </summary>
    /// <param name="texture">The texture.</param>
    public NewGamePanel(SourceryGame game)
    {
      Type = PanelTypes.NewGame;
      _Game = game;
      _Texture = game.Panel;
      _StartGame = new Button(game,"Новая игра");
      _StartGame.OnValueChanged += OnStartGame;
      Items = new List<Control>();
      LoadLevels();
    }
    #endregion

    #region Методы

    /// <summary>Нажали "Начать игру"</summary>
    private void OnStartGame()
    {
      Start = true;
      foreach (PanelItem item in Items)
        if (item.Selected)
          _Game.StartGame(item.Caption);
    }

    /// <summary>Выделили элемент списка</summary>
    private void OnSelectItem()
    {
      foreach (PanelItem item in Items)
        item.Selected = false;
    }

    /// <summary>Загружаем список доступных уровней</summary>
    private void LoadLevels()
    {
      var dir = new DirectoryInfo("data/levels"); // папка с файлами
      foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список
        Items.Add(new PanelItem(_Game, Path.GetFileNameWithoutExtension(file.FullName)));
      foreach (Control item in Items)
        item.OnValueChanged += OnSelectItem;
    }
    
    /// <summary>
    /// Отрисовывает панель по вектору
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">The location.</param>
    public override void Draw(SpriteBatch spriteBatch, Vector2 location)
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
    public override void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
    {
      spriteBatch.Draw(_Texture, rectangle, Color.White);
      _StartGame.Rect = new Rectangle(rectangle.Right - 280, rectangle.Bottom + 20, 300, 60);
      _StartGame.Draw(spriteBatch);
      int i = 1;
      foreach (PanelItem item in Items)
      {
        item.Rect = new Rectangle(rectangle.Left + 50, rectangle.Top + 30 * i, rectangle.Width - 100, 30);
        item.Draw(spriteBatch);
        i++;
      }
    }

    /// <summary>
    /// Реагируем на изменения в игре
    /// </summary>
    /// <param name="state">The state.</param>
    public override void ChangeState(Microsoft.Xna.Framework.Input.MouseState state)
    {
      _StartGame.ChangeState(state);
      foreach (PanelItem item in Items)
        item.ChangeState(state);
    }
    #endregion

  }
}
