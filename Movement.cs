using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Класс для движеня объекта вдоль траектории</summary>
  class Movement
  {
    #region Поля
    /// <summary>Маршрут</summary>
    private List<Point> _Route;

    /// <summary>Текущая позиция в маршруте</summary>
    private int _CurrentPosition;

    /// <summary>Цель маршрута</summary>
    private Cell _Target;

    /// <summary>Текущее положение объекта</summary>
    private Cell _Current;

    /// <summary>Скорость перемещения объекта</summary>
    private int _Speed;

    /// <summary>Область отрисовки объекта</summary>
    private Rectangle _CurrentState;

    /// <summary>Задержка для бега</summary>
    private int _Delay = 1;

    /// <summary>Анимация при движении</summary>
    private AnimateSprite _Tile;

    /// <summary>Параметры анимации</summary>
    private PlayerAnimation _Animation;

    private PathNode[,] _Field;

    public bool IsEnd;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="Movement"/>.</summary>
    /// <param name="board">Вся доска.</param>
    /// <param name="state">Прямоугольник отрисовки.</param>
    /// <param name="current">Текущая ячейка.</param>
    /// <param name="speed">Скорость перемещения.</param>
    /// <param name="sprite">Анимация объекта.</param>
    /// <param name="animation">Параметры анимации объекта.</param>
    public Movement(Rectangle state, Cell current, int speed, AnimateSprite sprite, PlayerAnimation animation, bool IsMagic)
    {
      IsEnd = true;
      _CurrentState = state;
      _Current = current;
      _Speed = speed;
      _Tile = sprite;
      _Animation = animation;
      _Field = Helper.Field;
      _Field = new PathNode[Helper.Board.GetLength(0), Helper.Board.GetLength(1)];
      bool pass;
      for (int i = 0; i < Helper.Board.GetLength(0); i++)
        for (int j = 0; j < Helper.Board.GetLength(1); j++)
        {
          if (IsMagic)
            pass = false;
          else
            pass = (Helper.Board[i, j].Type != CellType.Passable); 
          _Field[i, j] = new PathNode()
          {
            IsWall = pass ,
            X = Helper.Board[i, j].Rect.X,
            Y = Helper.Board[i, j].Rect.Y,
            i = i,
            j = j,
          };
        }
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает текущий прямоугольник отрисовки</summary>
    public Rectangle CurrentState
    {
      get { return _CurrentState; }
      set { _CurrentState = value; }
    }

    /// <summary>Возвращает текущую ячейку</summary>
    public Cell CurrentCell
    {
      get { return _Current; }
      set { _Current = value; }
    }
    #endregion

    #region Методы

    #endregion
    /// <summary>Двигаемся по маршруту от клетки к клетке</summary>
    /// <param name="route">Маршрут</param>
    public void MoveToRoute(List<Point> route)
    {
      _Route = route;
      _Target = Helper.Board[route[1].X, route[1].Y];
      _CurrentPosition = 1;
    }

    public void Move(Cell from,Cell to,Rectangle state)
    {
      _Current = from;
      _CurrentState = state;
      Solver<PathNode, Object> aStar = new Solver<PathNode, Object>(_Field);
      _Route = new List<Point>();
      var cell = from;
      var start = new Point(cell.i, cell.j);
      var finish = new Point(to.i, to.j);
      IEnumerable<PathNode> path = aStar.Search(start, finish, null);
      if (path != null)
      {
        foreach (PathNode node in path)
          _Route.Add(new Point(node.i, node.j));
        MoveToRoute(_Route);
        IsEnd = false;
      }
      else
        IsEnd = true;
    }

    /// <summary>Идем в следующую клетку по маршруту</summary>
    /// <returns></returns>
    private bool MoveNext()
    {
      if (_Route == null)
        return false;

      ++_CurrentPosition;
      if (_CurrentPosition < 0 || _CurrentPosition >= _Route.Count)
      {
        IsEnd = true;
        return false;
      }
      _Target = Helper.Board[_Route[_CurrentPosition].X, _Route[_CurrentPosition].Y];
      return true;
    }

    public void Stop()
    {
      _Target = null;
    }
    /// <summary>Проверяет окончено ли перемещение</summary>
    private bool IsOver
    {
      get{return _Target.Rect.Contains(_CurrentState.Center.X, _CurrentState.Center.Y);}
    }

    /// <summary>Двигаемся вперед по прямой</summary>
    private void MoveForward()
    {
      _CurrentState.X = _CurrentState.X + _Speed;
      _Tile.UpdateInRange(_Animation.Forward.Start, _Animation.Forward.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.Forward.State;
        }
      }
    }

    /// <summary>Двигаемся наза по прямой</summary>
    private void MoveBackward()
    {
      _CurrentState.X = _CurrentState.X - _Speed;
      _Tile.UpdateInRange(_Animation.Backward.Start, _Animation.Backward.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.Backward.State;
        }
      }
    }

    /// <summary>Двигаемся вверх по прямой</summary>
    private void MoveTop()
    {
      _CurrentState.Y = _CurrentState.Y - _Speed;
      _Tile.UpdateInRange(_Animation.Top.Start, _Animation.Top.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.Top.State;
        }
      }
    }

    /// <summary>Двигаемся вниз по прямой</summary>
    private void MoveBottom()
    {
      _CurrentState.Y = _CurrentState.Y + _Speed;
      _Tile.UpdateInRange(_Animation.Bottom.Start, _Animation.Bottom.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.Bottom.State;
        }
      }
    }

    /// <summary>Двигаемся вверх-влево по диагонали</summary>
    private void MoveTopLeft()
    {
      if (_CurrentState.Y != _Target.Rect.Y)
        _CurrentState.Y = _CurrentState.Y - _Speed;
      if (_CurrentState.X != _Target.Rect.X)
        _CurrentState.X = _CurrentState.X - _Speed;
      _Tile.UpdateInRange(_Animation.TopLeft.Start, _Animation.TopLeft.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.TopLeft.State;
        }
      }
    }

    /// <summary>Двигаемся вверх-вправо по диагонали</summary>
    private void MoveTopRight()
    {
      if (_CurrentState.Y != _Target.Rect.Y)
        _CurrentState.Y = _CurrentState.Y - _Speed;
      if (_CurrentState.X != _Target.Rect.X)
        _CurrentState.X = _CurrentState.X + _Speed;
      _Tile.UpdateInRange(_Animation.TopRight.Start, _Animation.TopRight.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.TopRight.State;
        }
      }
    }

    /// <summary>Двигаемся вправо-вниз по диагонали</summary>
    private void MoveBottomRight()
    {
      if (_CurrentState.Y != _Target.Rect.Y)
        _CurrentState.Y = _CurrentState.Y + _Speed;
      if (_CurrentState.X != _Target.Rect.X)
        _CurrentState.X = _CurrentState.X + _Speed;
      _Tile.UpdateInRange(_Animation.BottomRight.Start, _Animation.BottomRight.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.BottomRight.State;
        }
      }
    }

    /// <summary>Двигаемся влево-вниз</summary>
    private void MoveBottomLeft()
    {
      if (_CurrentState.Y != _Target.Rect.Y)
        _CurrentState.Y = _CurrentState.Y + _Speed;
      if (_CurrentState.X != _Target.Rect.X)
        _CurrentState.X = _CurrentState.X - _Speed;
      _Tile.UpdateInRange(_Animation.BottomLeft.Start, _Animation.BottomLeft.Stop);
      if (IsOver)
      {
        _Current = _Target;
        if (!MoveNext())
        {
          _Target = null;
          _Tile.CurrentFrame = _Animation.BottomLeft.State;
        }
      }
    }
   
    /// <summary>Двигает персонажа в клетку target</summary>
    /// <param name="target">Пункт назначения</param>
    public void MoveTo(Cell target)
    {
      if (target == null)
        return;
      _Target = target;
    }

    /// <summary>Обновляемся</summary>
    public void Update()
    {
      if (_Target == null)
        return;
      if (_Current != _Target)
      {
        if (_Delay != 0)
        {
          _Delay--;
          return;
        }
        else
          _Delay = 1;

        if ((_Current.j == _Target.j) && (_Target.i > _Current.i))
          MoveForward();
        else if (((_Current.j == _Target.j) && (_Target.i < _Current.i)))
          MoveBackward();
        else if ((_Current.i == _Target.i) && (_Target.j < _Current.j))
          MoveTop();
        else if ((_Current.i == _Target.i) && (_Target.j > _Current.j))
          MoveBottom();
        else if ((_Current.i > _Target.i) && (_Target.j < _Current.j))
          MoveTopLeft();
        else if ((_Current.i < _Target.i) && (_Target.j < _Current.j))
          MoveTopRight();
        else if ((_Current.i < _Target.i) && (_Target.j > _Current.j))
          MoveBottomRight();
        else if ((_Current.i > _Target.i) && (_Target.j > _Current.j))
          MoveBottomLeft();
      }
      else
      {
        _Target = null;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      //if (_Route != null)
     //   foreach (Point point in _Route)
       //   spriteBatch.Draw(new Texture2D(spriteBatch.GraphicsDevice,2,2,false,SurfaceFormat.Color), new Rectangle(point.X * 2, point.Y * 2, 2, 2), Color.Black);
    }
  }
}
