using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sourcery
{
  /// <summary>Элемент пути</summary>
  public class PathNode : IPathNode<Object>
  {
    #region Поля

    /// <summary>Возвращает или задает координату X</summary>
    public Int32 X { get; set; }

    /// <summary>Возвращает или задает координату Y</summary>
    public Int32 Y { get; set; }
    
    /// <summary>Возвращает или задает номер ячейки</summary>
    public Int32 i { get; set; }
    
    /// <summary>Возвращает или задает номер ячейки</summary>
    public Int32 j { get; set; }

    /// <summary>Возвращает или задает проходимость ячейки</summary>
    public Boolean IsWall { get; set; }

    /// <summary>Возвращает или задает тип ячейки</summary>
    public CellType Type;

    /// <summary>Область ячейки</summary>
    public Rectangle Rect { get { return new Rectangle(X * Width, Y * Height, Width, Height); } }

    /// <summary>Ширина ячейки</summary>
    public int Width;

    /// <summary>Высота ячейки</summary>
    public int Height;
    #endregion


    public bool IsWalkable(Object unused)
    {
      return !IsWall;
    }
  }
}
