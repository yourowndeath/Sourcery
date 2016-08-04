using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sourcery
{

  /// <summary>
  /// 
  /// </summary>
  public class Cell
  {

    /// <summary>Номер ячейки</summary>
    private Point _Number;

    /// <summary>Тип ячейки</summary>
    private CellType _Type;

    /// <summary>Область ячейки</summary>
    public Rectangle Rect;

    public Cell(Point number,CellType type,int width,int height)
    {
      _Number = number;
      _Type = type;
      Rect = new Rectangle(number.X * width, number.Y * height,width,height);
    }

    public int i 
    {
      get { return _Number.X; }
    }

    public int j
    {
      get { return _Number.Y; }
    }

    public CellType Type
    {
      get { return _Type; }
    }

  }
}
