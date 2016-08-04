using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Sourcery
{

  /// <summary>Класс настроек игры</summary>
  class Settings
  {
    #region Поля 

    /// <summary>Имя файла</summary>
    private string _FileName;

    /// <summary>Документ с настройками</summary>
    private XmlDocument _Document;

    /// <summary>Громкость музыки</summary>
    private int _MusicVolume;

    /// <summary>Громкость звука</summary>
    private int _SoundVolume;

    /// <summary>Разрешение экрана</summary>
    private string _ScreenResolution;

    /// <summary>Полноэкранный режим</summary>
    private bool _FullScreen;

    /// <summary>Игровой курсор</summary>
    private bool _ChangeCursor;

    /// <summary>Имя игрока</summary>
    private string _PlayerName;

    /// <summary>Имя игры</summary>
    private string _GameName;

    /// <summary>Настройки экрана</summary>
    private List<ScreenSettings> _ScreenSettings;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="Settings"/>.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    public Settings(string fileName)
    {
      _Document = new XmlDocument();
      _Document.Load(fileName);
      _MusicVolume = Convert.ToInt16(_Document.SelectSingleNode("Settings/MusicVolume").InnerText);
      _SoundVolume = Convert.ToInt16(_Document.SelectSingleNode("Settings/SoundVolume").InnerText);
      _ScreenResolution = _Document.SelectSingleNode("Settings/ScreenResolution").InnerText;
      _FullScreen = Convert.ToBoolean(_Document.SelectSingleNode("Settings/FullScreen").InnerText);
      _ChangeCursor = Convert.ToBoolean(_Document.SelectSingleNode("Settings/ChangeCursor").InnerText);
      _PlayerName = _Document.SelectSingleNode("Settings/PlayerName").InnerText;
      _GameName = _Document.SelectSingleNode("Settings/GameName").InnerText;
      var Selection = _Document.SelectNodes("Settings/Screens/Screen");
      _ScreenSettings = new List<ScreenSettings>();
      foreach (XmlNode node in Selection)
        _ScreenSettings.Add(new ScreenSettings(node));
      _FileName = fileName;
    }
    #endregion

    public ScreenSettings Screens(ScreenType type)
    {
      foreach (ScreenSettings settings in _ScreenSettings)
        if (settings.Type == type)
          return settings;
      return null;
    }


    #region Свойства

    /// <summary>Возвращает или задает имя игрока</summary>
    public string PlayerName
    {
      get { return _PlayerName; }
      set 
      {
        _PlayerName = value;
        _Document.SelectSingleNode("Settings/PlayerName").InnerText = value;
        Save();
      }
    }
    /// <summary>Возвращает или задает громкость музыки</summary>
    public int MusicVolume
    {
      get { return _MusicVolume; }
      set 
      {
        _MusicVolume = value;
        _Document.SelectSingleNode("Settings/MusicVolume").InnerText = value.ToString();
      }
    }


    /// <summary>Возвращает или задает громкость звука</summary>
    public int SoundVolume
    {
      get { return _SoundVolume; }
      set 
      {
        _SoundVolume = value;
        _Document.SelectSingleNode("Settings/SoundVolume").InnerText = value.ToString();
      }
    }

    /// <summary>Возвращает или задает разрешение экрана</summary>
    public string ScreenResolution
    {
      get { return _ScreenResolution; }
      set 
      {
        _ScreenResolution = value;
        _Document.SelectSingleNode("Settings/ScreenResolution").InnerText = value;
      }
    }

    /// <summary>Возвращает или задает полноэкранный режим</summary>
    public bool FullScreen
    {
      get { return _FullScreen; }
      set
      {
        _FullScreen = value;
        _Document.SelectSingleNode("Settings/FullScreen").InnerText = value.ToString();
      }
    }

    /// <summary>Возвращает или задает системный курсор</summary>
    public bool ChangeCursor
    {
      get { return _ChangeCursor; }
      set 
      {
        _ChangeCursor = value;
        _Document.SelectSingleNode("Settings/ChangeCursor").InnerText = value.ToString();
      }
    }

    #endregion

    #region Методы

    /// <summary>Сохраняем настройки</summary>
    public void Save()
    {
      _Document.Save(_FileName);
    }
    #endregion
  }
}
