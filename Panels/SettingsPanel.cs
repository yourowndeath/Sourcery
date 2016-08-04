using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Sourcery
{

  /// <summary>
  /// Панель настроек
  /// </summary>
  class SettingsPanel
  {
    #region Поля

    /// <summary>Окно родитель</summary>
    private GameScreen _Parent;

    private int _Delay = 0;

    /// <summary>Ссылка на игру</summary>
    private SourceryGame _Game;

    /// <summary>Игровое меню</summary>
    private Menu _Menu;

    /// <summary>Текстура панели</summary>
    private Texture2D _PanelTexture;

    /// <summary>Шрифт для заголовков</summary>
    private SpriteFont _Font;

    /// <summary>Текущий набор пунктов меню</summary>
    private List<MenuItem> _CurrentMenu;

    /// <summary>Текущие настройки</summary>
    private Settings _Settings;

    /// <summary>Диалоговое окно</summary>
    private Dialog _CurrentDialog;
    #endregion

    #region Конструкторы
    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="SettingsPanel"/>.
    /// </summary>
    /// <param name="game">The game.</param>
    public SettingsPanel(SourceryGame game,Settings settings,GameScreen parent)
    {
      _Game = game;
      _Parent = parent;
      _Settings = settings;
      _Menu = new Menu("Settings/GameMenu.xml",game);
      _PanelTexture = game.Panel;
      _Font = game.Font;
      _CurrentMenu = _Menu.Items;
      LoadSettings(_Menu.Items);
    }
    #endregion

    #region Методы

    /// <summary>
    /// Обновляемся
    /// </summary>
    /// <param name="state">Состояние мыши</param>
    public void Update(MouseState state)
    {
      if (_CurrentDialog != null)
        _CurrentDialog.Update(state);
      if (_CurrentMenu == null)
        return;
      if (_Delay != 0)
      {
        _Delay--;
        return;
      }
      foreach (MenuItem item in _CurrentMenu)
        item.ItemControl.ChangeState(state);
    }


    /// <summary>
    /// Совершаем действие предусмотренное элементом
    /// </summary>
    /// <param name="item">Элемент</param>
    public void DoAction(MenuItem item)
    {
      switch (item.Action)
      {
        case ActionType.SubMenu:
          {
            LoadSettings(item.Items);
            _CurrentMenu = item.Items;
            if (item.Name == "Звук")
              _Delay = 2;
            _Delay = 15;
            break;
          }
        case ActionType.PreviousMenu:
          {
            _Delay = 15;
            if (item.Parent.Parent != null)
            {
              LoadSettings(item.Parent.Parent.Items);
              _CurrentMenu = item.Parent.Parent.Items;
            }
            else
            {
              LoadSettings(_Menu.Items);
              _CurrentMenu = _Menu.Items;
            }
            break;
          }
        case ActionType.ReturnToGame:
          {
            _Parent.CloseSettings();
            break;
          }
        case ActionType.LeaveToSystem:
          {
            _CurrentDialog = new Dialog(_Game,"Вы дейтсвительно хотите выйти из системы?");
            _CurrentDialog.OnDialogClose += OnLeaveToSystem;
     
            break;
          }
        case ActionType.Leave:
          {
            _Game.ChangeScreen(ScreenType.TitleScreen);
            break;

          }
      }
    }


    /// <summary>
    /// Обрабатываем выход в систему
    /// </summary>
    /// <param name="result">Результат модального окна</param>
    private void OnLeaveToSystem(ModalResult result)
    {
      if (result == ModalResult.Ok)
        _Game.CloseGame();
      else
      {
        _CurrentDialog.OnDialogClose -= OnLeaveToSystem;
        _CurrentDialog = null;
      }
    }

    /// <summary>
    /// Рисуем меню в прямоугольнике
    /// </summary>
    /// <param name="Rect">Прямоугольник</param>
    public void Draw(Rectangle Rect)
    {
      _Game.spriteBatch.Draw(_PanelTexture,Rect,Color.White);

      foreach (MenuItem item in _CurrentMenu)
        {
          Rectangle destinationRectangle;
          switch (item.Control)
          {
            case ControlType.Button: { destinationRectangle = new Rectangle(((Rect.Right - Rect.Left) / 2) - 150, (Rect.Top + 50) + 60 * item.Position, 300, 60); break; }
          }
          if (item.Control != ControlType.ComboBox)
          {
            if (item.Action != ActionType.PreviousMenu && item.Action!=ActionType.ReturnToGame)
            {
              if (item.Control != ControlType.Button)
                item.Rect = new Rectangle(Rect.Left + 30, (Rect.Top + 50) + 60 * item.Position, Rect.Width - 40, 30);
              else
                item.Rect = new Rectangle(Rect.Left + 5, (Rect.Top + 50) + 60 * item.Position, 300, 60);
              item.ItemControl.Draw(_Game.spriteBatch, item.Rect);
            }
            else
            {
              item.Rect = new Rectangle(Rect.Left + 5, (Rect.Bottom - 100), 300, 60);
              item.ItemControl.Draw(_Game.spriteBatch, item.Rect);
            }
          }
        }

      //Рисуем комбобоксы в самом конце!
      foreach (MenuItem item in _CurrentMenu)
      {
        if (item.Control == ControlType.ComboBox)
        {
          item.Rect = new Rectangle(Rect.Left + 40, (Rect.Top + 50) + 35 * item.Position, Rect.Width - 80, 30);
          item.ItemControl.Draw(_Game.spriteBatch, item.Rect);
        }
      }
      if (_CurrentDialog != null)
        _CurrentDialog.Draw(_Game.spriteBatch);
    }


    /// <summary>
    /// Подгружаем текущее меню
    /// </summary>
    /// <param name="items">Элементы меню</param>
    private void LoadSettings(List<MenuItem> items)
    {
      foreach (MenuItem mitem in _CurrentMenu)
        mitem.OnItemChanged -= DoAction;

      foreach (MenuItem item in items)
      {
        item.OnItemChanged += DoAction;
        switch (item.Action)
        {
          case ActionType.ChangeCursor:
            {
              (item.ItemControl as CheckBox).Checked = _Settings.ChangeCursor;
              item.ItemControl.OnValueChanged += OnChangeCursor;
              break;
            }
          case ActionType.FullScreen:
            {
              (item.ItemControl as CheckBox).Checked = _Settings.FullScreen;
              item.ItemControl.OnValueChanged += OnFullScreenChange;
              break;
            }
          case ActionType.MusicVolume:
            {
              (item.ItemControl as TrackBar).Value = _Settings.MusicVolume;
              item.ItemControl.OnValueChanged += OnMusicVolumeChange;
              break;
            }
          case ActionType.SoundVolume:
            {
              (item.ItemControl as TrackBar).Value = _Settings.SoundVolume;
              item.ItemControl.OnValueChanged += OnSoundVolumeChange;
              break;
            }
          case ActionType.ScreenResolution:
            {
              item.ItemControl.Caption = _Settings.ScreenResolution;
              item.ItemControl.OnValueChanged += OnScreenResolutionChange;
              break;
            }
        }
      }
    }

    /// <summary>
    /// Реагируем на смену расширения экрана
    /// </summary>
    private void OnScreenResolutionChange()
    {
      foreach (MenuItem item in _CurrentMenu)
        if (item.Action == ActionType.ScreenResolution)
          _Settings.ScreenResolution = item.ItemControl.Caption;
      _Game.ChangeResolution(_Settings.ScreenResolution);
    }

    /// <summary>
    /// Устанавливаем полноэкранный режим
    /// </summary>
    private void OnFullScreenChange()
    {
      foreach (MenuItem item in _CurrentMenu)
        if (item.Action == ActionType.FullScreen)
          _Settings.FullScreen = (item.ItemControl as CheckBox).Checked;
      _Game.ChangeFullScreen(_Settings.FullScreen);
    }

    /// <summary>
    /// Меняем курсор
    /// </summary>
    private void OnChangeCursor()
    {
      foreach (MenuItem item in _CurrentMenu)
        if (item.Action == ActionType.ChangeCursor)
          _Settings.ChangeCursor = (item.ItemControl as CheckBox).Checked;
      _Game.ChangeCursor(_Settings.ChangeCursor);
    }

    /// <summary>
    /// Меняем громкость звуков
    /// </summary>
    private void OnMusicVolumeChange()
    {
      foreach (MenuItem item in _CurrentMenu)
        if (item.Action == ActionType.MusicVolume)
          _Settings.MusicVolume = (item.ItemControl as TrackBar).Value;
      float vol = 12 - _Settings.MusicVolume;
      if (vol == 12)
        MediaPlayer.Volume = 0;
      else
      MediaPlayer.Volume = 1 / vol; 
    }


    /// <summary>
    /// Меняем громкость музыки
    /// </summary>
    private void OnSoundVolumeChange()
    {
      foreach (MenuItem item in _CurrentMenu)
        if (item.Action == ActionType.SoundVolume)
          _Settings.SoundVolume = (item.ItemControl as TrackBar).Value;
    }
    #endregion
  }
}
