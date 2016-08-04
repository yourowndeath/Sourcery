using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sourcery
{
  class Helper
  {

    /// <summary>Возвращает или задает путевой массив</summary>
    public static PathNode[,] Field {get;set; }

    /// <summary>Возвращает или задает путевой массив для магии</summary>
    public static PathNode[,] MagicField { get; set; }

    /// <summary>Возвращает или задает игровую доску</summary>
    public static Cell[,] Board { get; set; }

    /// <summary>Возвращает или задает ссылку на игру</summary>
    public static SourceryGame Game { get; set; }

    /// <summary>Возвращает или задает ширину экрана</summary>
    public static int ScreenWidth { get {return Game.GraphicsDevice.Viewport.Width;}}

    /// <summary>Возвращает или задает высоту экрана</summary>
    public static int ScreenHeight { get { return Game.GraphicsDevice.Viewport.Height; }}

    /// <summary>Возвращает или задает настройки игры</summary>
    public static Settings Settings { get; set; }
    
  }

}
