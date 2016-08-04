using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace Sourcery
{

  /// <summary>Основной класс игры</summary>
  public class SourceryGame : Game
  {
    #region Поля

    #region Текстуры

    /// <summary>Текстура для Чекбокса</summary>
    public Texture2D CheckBox;

    /// <summary>Текстура для кнопки</summary>
    public Texture2D Button;

    /// <summary>Текстура для комбобокса</summary>
    public Texture2D ComboBox;

    /// <summary>Лента для трекбара</summary>
    public Texture2D TrackLent;

    /// <summary>Кнопка увеличения для трекбара</summary>
    public Texture2D PlusButton;

    /// <summary>Кнопка уменьшения для трекбара</summary>
    public Texture2D MinusButton;

    /// <summary>Панель для элементов комбобокса</summary>
    public Texture2D ItemPanel;

    /// <summary>Элемент списка</summary>
    public Texture2D ItemButton;

    /// <summary>Панель вывода информации</summary>
    public Texture2D Panel;

    /// <summary>Курсор</summary>
    public Texture2D Cursor;

    /// <summary>Ожидание загрузки</summary>
    public Texture2D Loader;

    /// <summary>Текстовый редактор</summary>
    public Texture2D Edit;

    /// <summary>Текстовый указатель</summary>
    public Texture2D Editing;

    /// <summary>Подписи к замкам</summary>
    public Texture2D ColorStripe;

    /// <summary>Текстура игрока</summary>
    public Texture2D Player;

    /// <summary>Кнопка с кулдауном</summary>
    public Texture2D ReloadingButton;

    /// <summary>Список селекторов цели</summary>
    public List<Texture2D> Selectors;

    /// <summary>Обновление замка</summary>
    public Texture2D Upgrade;

    /// <summary>Нижний контрол для отображения маны героев</summary>
    public Texture2D AllMagic;
    #endregion

    #region Шрифты
    /// <summary>Обычный шрифт</summary>
    public SpriteFont Font;

    /// <summary>Большой шрифт</summary>
    public SpriteFont BigFont;

    /// <summary>Маленький шрифт</summary>
    public SpriteFont SmallFont;
    #endregion

    #region Звуки

    /// <summary>Фоновая музыка</summary>
    public Song BackSound;

    /// <summary>Звук нажатия</summary>
    public SoundEffect ClickSound;

    #endregion

    #region FPS
    /// <summary>Всего кадров</summary>
    private int _total_frames = 0;

    /// <summary>Прошедшее время</summary>
    private float _elapsed_time = 0.0f;

    /// <summary>Кадров в секунду</summary>
    private int _fps = 0;
    #endregion

    #region Состояние игры
    /// <summary>Текущий уровень</summary>
    private string _CurrentLevel;

    /// <summary>Текущий экран</summary>
    private Screen _CurrentScreen;

    /// <summary>Пакет спрайтов</summary>
    public SpriteBatch spriteBatch;

    /// <summary>Настройки приложения</summary>
    private Settings _Settings;

    /// <summary>Устройстов вывода</summary>
    public GraphicsDeviceManager graphics;

    /// <summary>Показываем сплеш</summary>
    private int _SplashDelay = 200;

    /// <summary>Показываем разработчика</summary>
    private int _DeveloperDelay = 300;

    /// <summary>Прямоугольник для отрисовки курсора</summary>
    private Rectangle _CursorRect;

    #endregion

    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="SourceryGame"/>.</summary>
    public SourceryGame()
    {
      Helper.Game = this;
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content/Source";
      _Settings = new Settings("Settings/Settings.xml");
      ChangeCursor(_Settings.ChangeCursor);
      ChangeFullScreen(_Settings.FullScreen);
      ChangeResolution(_Settings.ScreenResolution);
      Helper.Settings = _Settings;
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает или задает имя игрока</summary>
    public string PlayerName
    {
      get { return _Settings.PlayerName; }
      set { _Settings.PlayerName = value; }
    }

    /// <summary>Возвращает громкость звука</summary>
    public int SoundVolume()
    {
      return _Settings.SoundVolume;
    }

    /// <summary>Возвращает громкость музыки</summary>
    /// <returns></returns>
    public int MusicVolume()
    {
      return _Settings.MusicVolume;
    }

    #endregion

    #region Методы

    #region Контент
    /// <summary>Загружаем сплеш окно</summary>
    private void LoadSplash()
    {
      Cursor = Content.Load<Texture2D>("cursor/Cursor");
      Loader = Content.Load<Texture2D>("animation/Loader");
      LoadFonts();
    }

    /// <summary>Загружаем шрифты</summary>
    private void LoadFonts()
    {
      Font = Content.Load<SpriteFont>("fonts/Font");
      BigFont = Content.Load<SpriteFont>("fonts/BigFont");
      SmallFont = Content.Load<SpriteFont>("fonts/SmallFont");
    }

    /// <summary>Загружаем звук</summary>
    private void LoadSounds()
    {
      BackSound = Content.Load<Song>("sounds/Back");
      ClickSound = Content.Load<SoundEffect>("sounds/Click");
    }

    /// <summary>Загружаем тайлы для карты (игры)</summary>
    private void LoadTiles()
    {
      ColorStripe = Content.Load<Texture2D>("game/strips");
      Player = Content.Load<Texture2D>("animation/hero");
      AllMagic = Content.Load<Texture2D>("game/AllMagic");
      Upgrade = Content.Load<Texture2D>("game/Update");
      Selectors = new List<Texture2D>();
      Selectors.Add(Content.Load<Texture2D>("game/enemy_selector"));
      Selectors.Add(Content.Load<Texture2D>("game/our_selector"));
      Selectors.Add(Content.Load<Texture2D>("game/magic_selector"));
    }

    /// <summary>Загружаем контролы</summary>
    private void LoadControls()
    {
      CheckBox = Content.Load<Texture2D>("controls/CheckBox");
      ComboBox = Content.Load<Texture2D>("controls/ComboBox");
      Button = Content.Load<Texture2D>("controls/Button");
      TrackLent = Content.Load<Texture2D>("controls/TrackLent");
      PlusButton = Content.Load<Texture2D>("controls/Increment");
      MinusButton = Content.Load<Texture2D>("controls/Decrement");
      ItemPanel = Content.Load<Texture2D>("controls/ItemPanel");
      ItemButton = Content.Load<Texture2D>("controls/ItemButton");
      Panel = Content.Load<Texture2D>("controls/Panel");
      Edit = Content.Load<Texture2D>("controls/Edit");
      Editing = Content.Load<Texture2D>("controls/EditCursor");
      ReloadingButton = Content.Load<Texture2D>("controls/ReloadingButton");
    }

    /// <summary>Загрузка контента XNA</summary>
    protected override void LoadContent()
    {
      _CurrentScreen.LoadContent();
      if (_Settings.ChangeCursor)
        this.IsMouseVisible = true;
    }

    /// <summary>Выгрузка контента XNA</summary>
    protected override void UnloadContent()
    {
      Content.Unload();
    }
    #endregion

    /// <summary>Инициализация</summary>
    protected override void Initialize()
    {
      spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
      ChangeScreen(ScreenType.DeveloperScreen);
      LoadSplash();

      LoadSounds();
      LoadFonts();
      LoadControls();

      base.Initialize();
      MediaPlayer.Play(BackSound);
      float vol = 12 - _Settings.MusicVolume;
      MediaPlayer.Volume = 1 / vol;
    }

    /// <summary>Обновление сцены XNA</summary>
    /// <param name="gameTime">Игровое время</param>
    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();
      
      if (_CurrentScreen.Type == ScreenType.DeveloperScreen)
      {
        if (_DeveloperDelay == 0)
        {
          ChangeScreen(ScreenType.SplashScreen);
          _DeveloperDelay = -1;
        }
        else 
          --_DeveloperDelay;
      }


      if (_DeveloperDelay == 0)
      {
        ChangeScreen(ScreenType.SplashScreen);
        _DeveloperDelay = -1;
      }
      else if (_CurrentScreen.Type==ScreenType.DeveloperScreen)
        --_DeveloperDelay;
      
      if (_DeveloperDelay==-1)

      if (_SplashDelay == 0)
      {
        ChangeScreen(ScreenType.TitleScreen);
        _SplashDelay = -1;
      }
      else if (_CurrentScreen.Type == ScreenType.SplashScreen)
        --_SplashDelay;

      MouseState st = Mouse.GetState();
      if (!_Settings.ChangeCursor)
        _CursorRect = new Rectangle(st.X, st.Y, 35, 45);
      _CurrentScreen.Update(gameTime);
      _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
      base.Update(gameTime);
    }

    /// <summary>Перерисовка</summary>
    /// <param name="gameTime">Игровое время</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _total_frames++;
      _CurrentScreen.Draw(spriteBatch);
      spriteBatch.Begin();
      if (!_Settings.ChangeCursor)
        spriteBatch.Draw(Cursor, _CursorRect, Color.White);
      spriteBatch.DrawString(Font, string.Format("FPS={0}", _fps),
            new Vector2(10.0f, 20.0f), Color.White);
      spriteBatch.End();
      base.Draw(gameTime);
    }

    /// <summary>Закрыть игру</summary>
    public void CloseGame()
    {
      this.Exit();
    }

    /// <summary>Меняем разрешение</summary>
    public void ChangeResolution(string value)
    {
      if (string.IsNullOrEmpty(value))
        return;
      if (!value.Contains("x"))
        return;
      int p = value.IndexOf("x");
      var res = value.Substring(0, p);
      int width = Convert.ToInt32(value.Substring(0, p));
      res = value.Substring(p+1, value.Length - p-1);
      int height = Convert.ToInt32(value.Substring(p + 1, value.Length - p - 1));
      graphics.PreferredBackBufferWidth = width;
      graphics.PreferredBackBufferHeight = height;

      graphics.ApplyChanges();
    }

    /// <summary>Переход между полноэкранным и отображением в окне</summary>
    public void ChangeFullScreen(bool value)
    {
      if (graphics.IsFullScreen!=value)
       graphics.IsFullScreen = value;
      graphics.ApplyChanges();
    }

    /// <summary>Меняем отображение курсора</summary>
    public void ChangeCursor(bool value)
    {
      _Settings.ChangeCursor = value;
      this.IsMouseVisible = value;

    }

    /// <summary>Запускает игровой процесс</summary>
    /// <param name="lvlName">Имя уровня игры.</param>
    public void StartGame(string lvlName)
    {
      if (String.IsNullOrEmpty(lvlName))
        return;
      _CurrentLevel = lvlName;
      ChangeScreen(ScreenType.GameScreen);
    }

    /// <summary>Осуществляет смену типа экрана</summary>
    /// <param name="type">Тип экрана</param>
    public void ChangeScreen(ScreenType type)
    {
      switch (type)
      {
        case ScreenType.GameScreen:
          {
            if (string.IsNullOrEmpty(_CurrentLevel))
              return;
            LoadTiles();
            _CurrentScreen = new GameScreen(_CurrentLevel);
            _CurrentScreen.LoadContent();
            break;
          }
        case ScreenType.TitleScreen:
          {
            _CurrentScreen = new TitleScreen(this);
            _CurrentScreen.LoadContent();
            break;
          }
        case ScreenType.SplashScreen:
          {
            _CurrentScreen = new SplashScreen();
            _CurrentScreen.LoadContent();
            break;
          }
        case ScreenType.DeveloperScreen:
          {
            _CurrentScreen = new DeveloperScreen();
            _CurrentScreen.LoadContent();
            break;
          }
      }
    }
   
    #endregion
  }

}
