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

  /// <summary>Экран игры</summary>
  class GameScreen:Screen
  {
    #region Поля

    /// <summary>Кнопка меню</summary>
    private Button _MenuButton;

    /// <summary>Меню настроек</summary>
    private Menu _Settings;

    /// <summary>Панель настроек</summary>
    private SettingsPanel _SettingsPanel;

    /// <summary>Текущий уровень</summary>
    private Level _Level;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="GameScreen"/>.</summary>
    /// <param name="game">The game.</param>
    public GameScreen(string lvlName)
    {
      _Level = new Level(lvlName, Helper.Game);
      Settings = Helper.Settings.Screens(ScreenType.GameScreen);
    }
    #endregion

    #region Методы

    /// <summary>Закрываем панель настроек</summary>
    public void CloseSettings()
    {
      _SettingsPanel = null;
      Helper.Settings.Save();
    }

    /// <summary>Загружает все необходимое</summary>
    public override void LoadContent()
    {
      _Settings = new Menu("Settings/GameMenu.xml",Helper.Game);
      _MenuButton = new Button(Helper.Game, "Меню");
      _MenuButton.OnValueChanged += ShowMenu;
    }

    /// <summary>Обработчик события нажатия на кнопку меню</summary>
    private void ShowMenu()
    {
      if (_SettingsPanel == null)
        _SettingsPanel = new SettingsPanel(Helper.Game, Helper.Settings, this);
      else
        CloseSettings();
    }


    /// <summary>
    /// Обновляемся
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      MouseState st = Mouse.GetState();
      if (_SettingsPanel != null)
        _SettingsPanel.Update(st);
      _MenuButton.ChangeState(st);
      _Level.Update(st);
    }

    /// <summary>Рисуем</summary>
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      _Level.Draw(spriteBatch);
      _MenuButton.Draw(spriteBatch, new Rectangle(5, 5, 150, 30));
      if (_SettingsPanel != null)
      {

        _SettingsPanel.Draw(new Rectangle(Helper.ScreenWidth / 2 - 150, Helper.ScreenHeight/2-250, 300, 500));
      }
    spriteBatch.End();
    }
    #endregion
  }
}
