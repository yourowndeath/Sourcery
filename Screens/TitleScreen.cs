using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{
  class TitleScreen:Screen
  {

    #region Поля

    /// <summary>Ссылка на игру хозяина</summary>
    private SourceryGame _Game;

    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Класс работы с меню</summary>
    private Menu _Menu;

    /// <summary>Текущие отображаемые элементы меню</summary>
    private List<MenuItem> _CurrentMenu;

    /// <summary>Тип экрана</summary>
    public ScreenType _Type;

    /// <summary>Текущая панель</summary>
    private Panel _CurrentPanel;

    /// <summary>Задержка для того чтобы показать подменю</summary>
    private int _Delay = 0;

    private bool Start;
    private int fadeOut = 150;
    private Edit _Edit;

    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="TitleScreen"/>.</summary>
    public TitleScreen(SourceryGame game)
    {
      _Game = game;
      Settings = Helper.Settings.Screens(ScreenType.TitleScreen);
    }
    #endregion

    #region Методы

    /// <summary>Загружаем контент для экрана</summary>
    public override void LoadContent()
    {

      _Menu = new Menu("Settings/MainMenu.xml",_Game);
      _CurrentMenu = _Menu.Items;
      foreach (MenuItem item in _CurrentMenu)
        item.OnItemChanged += DoAction;
      _Font = _Game.Font;
      _Edit = new Edit(_Game);
    }

    /// <summary>Реагируем на апдейт</summary>
    public override void Update(GameTime gameTime)
    {
      if (_CurrentPanel != null && _CurrentPanel.Type == PanelTypes.NewGame)
        Start = (_CurrentPanel as NewGamePanel).Start;
      if (Start)
        fadeOut -= 5;
      MouseState st = Mouse.GetState();
      KeyboardState keystate = Keyboard.GetState();
      _Edit.Update(st,keystate);
      if (_Delay!=0)
      {
        --_Delay;
        return;
      }
      if (_CurrentPanel != null)
        _CurrentPanel.ChangeState(st);
      foreach (MenuItem item in _CurrentMenu)
        item.ItemControl.ChangeState(st);
    }

    /// <summary>
    /// Выполняем действие
    /// </summary>
    /// <param name="item">Элемент</param>
    private void DoAction(MenuItem item)
    {
      switch (item.Action)
      {
        case ActionType.NewGame:
          {
            _CurrentPanel = new NewGamePanel(_Game);
            break;
          }
        case ActionType.Load:
          {
            _CurrentPanel = new SaveLoadPanel(_Game, false);
            break;
          }
        case ActionType.Save:
          {
            _CurrentPanel = new SaveLoadPanel(_Game, true);
            break;
          }
        case ActionType.SubMenu:
          {
            ReloadMenu(item.Items);
            _CurrentMenu = item.Items;
            _CurrentPanel = null;
            _Delay = 15;
            break;
          }
        case ActionType.PreviousMenu:
          {
            if (item.Parent.Parent != null)
            {
              ReloadMenu(item.Parent.Parent.Items);
              _CurrentMenu = item.Parent.Parent.Items;
            }
            else
            {
              ReloadMenu(_Menu.Items);
              _CurrentMenu = _Menu.Items;
            }
            _CurrentPanel = null;
            break;
          }
        case ActionType.Help:
          {
            _CurrentPanel = new HelpPanel(_Game);
            break;
          }
        case ActionType.exit:
          {
            _Game.CloseGame();
            break;
          }
      }
    }

    /// <summary>Перезагружаем меню</summary>
    /// <param name="items">Элементы меню.</param>
    private void ReloadMenu(List<MenuItem> items)
    {
      foreach (MenuItem item in _CurrentMenu)
        item.OnItemChanged -= DoAction;
      foreach (MenuItem item in items)
        item.OnItemChanged += DoAction;
    }
    /// <summary>Отрисовываем сцену</summary>
    public override void Draw(SpriteBatch spriteBatch)
    {
      DrawMenu(_CurrentMenu);
      if (_CurrentPanel != null)
        DrawPanel(_CurrentPanel);
    }

    /// <summary>Отрисовываем панель</summary>
    /// <param name="panel">панель</param>
    private void DrawPanel(Panel panel)
    {
      if (panel == null)
        return;
      _Game.spriteBatch.Begin();

      var width = _Game.graphics.GraphicsDevice.Viewport.Width;
      var height = _Game.graphics.GraphicsDevice.Viewport.Height;
      panel.Draw(_Game.spriteBatch, new Rectangle(10, 10, width - 350, height-100));
      _Game.spriteBatch.End();
    }

    /// <summary>Отрисовываем меню</summary>
    /// <param name="lst">Список элементов</param>
    private void DrawMenu(List<MenuItem> lst)
    {
      if (lst == null)
        return;
      _Game.spriteBatch.Begin();

      var width = _Game.graphics.GraphicsDevice.Viewport.Width;
      var height = _Game.graphics.GraphicsDevice.Viewport.Height;
      if (!Start) {
        _Game.spriteBatch.Draw(Background, new Rectangle(0, 0, width, height), Color.White);
      _Edit.Draw(_Game.spriteBatch, new Rectangle(width - 310,20,300,60));
      foreach (MenuItem item in lst)
      {
        item.Rect = new Rectangle(width - 310, 50+ 60 * (item.Position + 1), 300, 60);
        item.ItemControl.Draw(_Game.spriteBatch, item.Rect);
      }
    }
      else
        _Game.spriteBatch.Draw(Background, new Rectangle(0, 0, width, height), new Color(fadeOut,0,0));
      _Game.spriteBatch.End();
    }
    #endregion
  }
}
