namespace Sourcery
{

  /// <summary>Перечисление действий по нажатию кнопки</summary>
  public enum ActionType
  {
    /// <summary>Новая игра</summary>
    NewGame = 0,

    /// <summary>Открыть вложенное меню</summary>
    SubMenu = 1,

    /// <summary>Сохранить игру </summary>
    Save = 2,

    /// <summary>Загрузить игру</summary>
    Load = 3,

    /// <summary>Открыть предыдущее меню</summary>
    PreviousMenu = 4,

    /// <summary>Вернуться к игре</summary>
    ReturnToGame = 5,

    /// <summary>Справка</summary>
    Help = 6,

    /// <summary>Подтверждение</summary>
    ok = 7,

    /// <summary>Выход</summary>
    exit =8,

    /// <summary>Смена вида курсора</summary>
    ChangeCursor = 9,

    /// <summary>Громкость фоновоф музыки</summary>
    MusicVolume = 10,

    /// <summary>Громкость звуков</summary>
    SoundVolume =11,

    /// <summary>Разрешение экрана</summary>
    ScreenResolution = 12,

    /// <summary>Полноэкранный режим</summary>
    FullScreen = 13,

    /// <summary>Выйти из боя</summary>
    Leave =14,

    /// <summary>Выйти в систему</summary>
    LeaveToSystem =15,

  }
}
