using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Класс уровня. Отвечает за отрисовку уровня</summary>
  class Level
  {
    #region Поля
    /// <summary>Имя карты</summary>
    private string _Name;

    /// <summary>Плавающая зона</summary>
    private FloatingZone _FloatingZone;

    /// <summary>Ширина доски</summary>
    private int _BoardWidth;

    /// <summary>Высота доски</summary>
    private int _BoardHeight;

    /// <summary>Карта уровня</summary>
    private Texture2D _Map;

    /// <summary>Список строений</summary>
    private List<Building> _Buildings;

    /// <summary>Массив игроков</summary>
    private List<Player> _Players;

    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Предыдущее состояние кнопки</summary>
    private MouseState _LastMouseState;

    /// <summary>Магии</summary>
    private Magics _Magics;

    /// <summary>Доска игры</summary>
    private Cell[,] _Board;

    /// <summary>Выбранная магия</summary>
    private Magic _SelectedMagic;

    /// <summary>Выбранный замок</summary>
    private Building _TargetBuilding;

    /// <summary>Выбранный герой</summary>
    private Player _TargetPlayer;

    /// <summary>Предыдущая цель</summary>
    private Cell _LastTagret;

    /// <summary>Текстура для отображения количества магии</summary>
    private Texture2D _AllMagic;

    /// <summary>Ждем пока дойдет, чтобы ударить магией</summary>
    private bool _Wait;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="Level"/>.</summary>
    /// <param name="name">The name.</param>
    /// <param name="game">The game.</param>
    public Level(string name, SourceryGame game)
    {
      _Font = game.BigFont;
      _AllMagic = game.AllMagic;
      _Buildings = new List<Building>();
      LoadXmlAttributes(name);
      _FloatingZone = new FloatingZone(game,_Players);
      _Magics = new Magics("Settings/Magic.xml",game);
    }
    #endregion

    #region Методы

    #region Загрузка

    /// <summary>Возвращает значение свойства XML-документа</summary>
    /// <param name="doc">XML-документ.</param>
    /// <param name="propertyName">Имя свойства.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">Если свойство не найдено в документе, значит он не соответствует стандарту</exception>
    private string GetXmlValue(XmlDocument doc,string propertyName)
    {
      var node = doc.SelectSingleNode(propertyName);
      if (node == null)
        throw new ArgumentException("Уровень "+_Name+" поврежден");
      return node.InnerText;
    }

    /// <summary>Загружаем уровень из XML</summary>
    /// <param name="name">Имя файла.</param>
    /// <exception cref="System.ArgumentException">
    /// Уровень  не найден или Уровень задан некорректно
    /// </exception>
    private void LoadXmlAttributes(string name)
    {
      //Проверим существование файла
      string path = "data/levels/" + name + ".lvl";
      if (!File.Exists(path))
        throw new ArgumentException("Уровень "+name+" не найден");
      
      //Создадим документ
      XmlDocument document;
      document = new XmlDocument();
      document.Load(path);

      //Получаем свойства
      _Name = GetXmlValue(document,"Level/Name");
      var mapName = GetXmlValue(document, "Level/Texture");
      _Map = Helper.Game.Content.Load<Texture2D>("maps/" + mapName);
      if (_Map==null)
        throw new ArgumentException("Уровень " + name + " задан некорректно");

      int width = Convert.ToInt32(GetXmlValue(document,"Level/Width"));
      int height = Convert.ToInt32(GetXmlValue(document,"Level/Height"));
      if (width == 0 || height == 0)
        throw new ArgumentException("Уровень " + name + " задан некорректно");
      _BoardWidth = width;
      _BoardHeight = height;
      //Загружаем доску
      _Board = new Cell[width,height];
      var lines = document.SelectNodes("Level/Board/Line");
      if (lines==null)
        throw new ArgumentException("Уровень " + name + " задан некорректно");
      int i = 0;
      CellType type = CellType.Passable;
      foreach (XmlNode node in lines)
      {
        var st = node.InnerText;
        for (int j = 0; j < width; j++)
        {
          int k = int.Parse(st[j].ToString());
          switch (k)
          {
            case 0: { type = CellType.Impassable; break; }
            case 1: { type = CellType.Passable; break; }
            case 2: { type = CellType.Town; break; }
            case 3: { type = CellType.Castle; break; }
          }
          var Cell = new Cell(new Point(j, i), type, Helper.ScreenWidth / width, Helper.ScreenHeight / height);
          _Board[j, i] = Cell;
        }
        i++;
      }

      Helper.Board = _Board;

      LoadCastles(document);
      LoadTowers(document);
      LoadPlayers(document);
      LoadPlayersCastles();
    }

    /// <summary>Загружаем игроков</summary>
    private void LoadPlayers(XmlDocument document)
    {
      var players = document.SelectNodes("Level/Players/Player");
      if (players == null)
        throw new ArgumentException("Ошибка в описании героев");
      
      _Players = new List<Player>();
      foreach (XmlNode node in players)
        _Players.Add(new Player(Helper.Game,node));
    }

    /// <summary>Загружаем точки респауна игроков</summary>
    private void LoadTowers(XmlDocument document)
    {
      var towers = document.SelectNodes("Level/Towers/Tower");
      if (towers == null)
        throw new ArgumentException("Ошибка в описании башен героев");
      foreach (XmlNode node in towers)
        _Buildings.Add(new Tower(node));
    }

    /// <summary>Загружаем нейтральные города</summary>
    private void LoadCastles(XmlDocument document)
    {
      var castles = document.SelectNodes("Level/Castles/Castle");
      if (castles == null)
        throw new ArgumentException("Уровень " + _Name + " задан некорректно");

      foreach (XmlNode node in castles)
        _Buildings.Add(new Castle(node));
    }

    /// <summary>Загружаем соответствие между героями и башнями</summary>
    public void LoadPlayersCastles()
    {
      foreach (Building building in _Buildings)
        if (building.OwnerName == "")
          building.Owner = null;
        else foreach (Player player in _Players)
            if (player.Name == building.OwnerName)
              player.AddBuilding(building);
    }

    #endregion

    #region Отрисовка
    /// <summary>Рисуем элементы карты</summary>
    private void DrawMap(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_Map, new Rectangle(0, 0, Helper.ScreenWidth, Helper.ScreenHeight), Color.White);
      foreach (Player player in _Players)
        player.Draw(spriteBatch);
      foreach (Building tower in _Buildings)
        tower.Draw(spriteBatch);
    }

    /// <summary>Рисуем пиктограммы магии</summary>
    private void DrawMagic(SpriteBatch spriteBatch)
    {
      int pos = 1;
      int left = 1;
      foreach (Magic magic in _Magics.MagicList)
        if (magic.Type == MagicType.agressive)
        {
          magic.Draw(spriteBatch, new Rectangle(Helper.ScreenWidth - 60, pos * 60, 50, 50));
          pos++;
        }
        else
        {
          magic.Draw(spriteBatch, new Rectangle(5, left * 60, 50, 50));
          left++;
      }
    }

    /// <summary>Рисуем</summary>
    public void Draw(SpriteBatch spriteBatch)
    {
      DrawMap(spriteBatch);
      DrawMagic(spriteBatch);
      var text = _Name;
      spriteBatch.DrawString(_Font, text, new Vector2(Helper.ScreenWidth - 200, 10), Color.Black);
      _FloatingZone.Draw(spriteBatch);
    }

    #endregion

    /// <summary>Обновляем состояние магии</summary>
    private void GetMagic(MouseState state)
    {
      //Магия равна нулю выходим
      if (_SelectedMagic == null)
        return;
      //Магия запущена убираем все выделения
      if (_SelectedMagic.Finished)
      {
        if (_SelectedMagic.Targets.Contains(TargetType.town))
          foreach (Building build in _Buildings)
          {
             build.DrawEnemySelector = false;
             build.DrawOurSelector = false;
          }

        if (_SelectedMagic.Targets.Contains(TargetType.hero))
          foreach (Player player in _Players)
          {
            player.DrawOurSelector = false;
            player.DrawEnemySelector = false;
          }
        _Players[0].CurrentMagic -= _SelectedMagic.Cost;

        if (_TargetBuilding != null)
        {
          if (_TargetBuilding.CurrentResource > _SelectedMagic.Effect)
            _TargetBuilding.CurrentResource -= _SelectedMagic.Effect;
          else
          {
            _TargetBuilding.CurrentResource = _TargetBuilding.MaxResource;
            _Players[0].AddBuilding(_TargetBuilding);
          }
        }

        else if (_TargetPlayer != null)
        {
          _TargetPlayer.CurrentMagic -= _SelectedMagic.Effect;
        }
        _TargetBuilding = null;
        _TargetPlayer = null;
        _SelectedMagic = null;
        return;
      }

      if (_SelectedMagic.Executed|_Wait)
        return;

      //Агрессивная магия
      if (_SelectedMagic.Type == MagicType.agressive)
      {
        //Удар по городу
        if (_SelectedMagic.Targets.Contains(TargetType.town))
        {
          foreach (Building castle in _Buildings)
            if (castle.Contains(state.X, state.Y) && castle.Owner != _Players[0])
            {
               castle.DrawEnemySelector = true;
               _TargetBuilding = castle;
              _TargetPlayer = null;
            }
          else
            castle.DrawEnemySelector = false;
        }

        //Удар по герою
        if (_SelectedMagic.Targets.Contains(TargetType.hero))
        {
          foreach (Player player in _Players)
            if (player.Contains(state.X, state.Y) && player != _Players[0] && !player.Dead)
            {
              player.DrawEnemySelector = true;
              _TargetPlayer = player;
              _TargetBuilding = null;
            }
            else
              player.DrawEnemySelector = false;
        }

      }
      //Мирная магия
      else
      {
        if (_SelectedMagic.Targets.Contains(TargetType.town))
        {
          foreach (Building castle in _Buildings)
            if (castle.Contains(state.X, state.Y) && castle.Owner == _Players[0])
            {
               castle.DrawOurSelector = true;
              _TargetBuilding = castle;
            }
           else
           castle.DrawOurSelector = false;
        }

        if (_SelectedMagic.Targets.Contains(TargetType.hero))
        {
          if (_Players[0].Contains(state.X, state.Y))
          {
            _Players[0].DrawOurSelector = true;
            _TargetPlayer = _Players[0];
          }
          else
            _Players[0].DrawOurSelector = false;
        }
      }

    }

    /// <summary>Убираем выделение со всех магий кроме current</summary>
    /// <param name="current">Выделенная магия.</param>
    private void Unselect(Magic current)
    {
      foreach (Magic magic in _Magics.MagicList)
        if (magic != current)
          magic.Selected = false;
    }

    /// <summary>Обновляем состояние всех объектов на карте</summary>
    private void CurrentUpdate(MouseState state)
    {
      foreach (Player player in _Players)
        player.Update(state);
      foreach (Building tower in _Buildings)
        tower.Update(state);
      foreach (Magic magic in _Magics.MagicList)
        magic.Update(state);
    }

    /// <summary>Меняем активную магию</summary>
    private void ChangeSelectedMagic(MouseState state)
    {
      foreach (Magic magic in _Magics.MagicList)
        if (magic._Back.Contains(state.X, state.Y) && magic.Active&&magic.Cost<=_Players[0].CurrentMagic)
        {
          magic.Selected = true;
          _SelectedMagic = magic;
          _SelectedMagic.Finished = false;
          Unselect(magic);
          break;
       }
    }

    /// <summary>Получаем проходимую клетку под курсором</summary>
    /// <param name="state">Положение курсора</param>
    /// <returns>Возвращает клетку под курсором, если клетка проходима и null, если нет</returns>
    private Cell GetTargetPassable(MouseState state)
    {
      Cell target = null;
      for (int i = 0; i < _BoardWidth; i++)
        for (int j = 0; j < _BoardHeight; j++)
          if ((_Board[i, j].Rect.Contains(state.X, state.Y)) && (_Board[i, j].Type != CellType.Impassable))
          {
            target = _Board[i, j];
            break;
          }
      return target;
    }


    /// <summary>Получаем центр цели</summary>
    private Cell GetTargetCenter()
    {
      Cell cell=null;
      int x = 0, y = 0;

      //Находим центр цели
      if (_TargetBuilding != null)
      {
        x = _TargetBuilding.Rect.Center.X;
        y = _TargetBuilding.Rect.Center.Y;
      }
      else if (_TargetPlayer != null)
      {
        x = _TargetPlayer.Rect.Center.X;
        y = _TargetPlayer.Rect.Center.Y;
      }

      //Находим ячейку в центре
      for (int i = 0; i < _BoardWidth; i++)
        for (int j = 0; j < _BoardHeight; j++)
          if (_Board[i, j].Rect.Contains(x, y))
            return _Board[i, j];
      return cell;
    }

    /// <summary>Применить магию</summary>
    private void ExecuteMagic(Cell target)
    {
      //Нет цели выходим
      if (_TargetBuilding == null && _TargetPlayer == null)
      return;

      Cell centerTarget=GetTargetCenter();
      if (centerTarget == null)
        centerTarget = target;

      int di = _Players[0].Position.i;
      int dj = _Players[0].Position.j;
      var dx = Math.Abs(di-centerTarget.i);
      var dy = Math.Abs(dj-centerTarget.j);
      int max = _SelectedMagic.Radius / (Helper.ScreenWidth / _BoardWidth);
        if (dx > max+1 || dy > max+1)
        {
          if (!_Wait)
          {
            _Players[0].MoveTo(GetNearestCell(centerTarget));
            _Wait = true;
            _LastTagret = centerTarget;
          }
        }
        else
        {
          _Players[0].Stop();
          if (_SelectedMagic.Direction==DirectionType.fromHero)
          _SelectedMagic.Execute(_Players[0].Position, centerTarget);
          else
            _SelectedMagic.Execute(centerTarget, centerTarget);
          _Wait = false;
          _LastTagret = null;
        }
    }


    /// <summary>Возвращает ближайшую клетку</summary>
    private Cell GetNearestCell(Cell current)
    {
      if (_SelectedMagic == null)
        return null;
      int Radius = _SelectedMagic.Radius;
      int max = Radius / (Helper.ScreenWidth / _BoardWidth);
      Point PlayerCell = new Point(_Players[0].Position.i, _Players[0].Position.j);
      Point TargetCell = new Point(current.i, current.j);
      int deltaX = Math.Abs(PlayerCell.X - TargetCell.X);
      int deltaY =Math.Abs( PlayerCell.Y - TargetCell.Y);
      int delta = 0;
      int newDelta = 0;
      while (max > 0)
      {

        if (deltaX > max & deltaY < max)
        {
          delta = deltaX - max;
          if ((PlayerCell.X > TargetCell.X) && (_Board[PlayerCell.X - max, PlayerCell.Y] != null) && _Board[PlayerCell.X - max, PlayerCell.Y].Type!=CellType.Impassable)
            return _Board[PlayerCell.X - delta, PlayerCell.Y];
          else if ((PlayerCell.X < TargetCell.X) && (_Board[PlayerCell.X + max, PlayerCell.Y] != null) && _Board[PlayerCell.X + max, PlayerCell.Y].Type != CellType.Impassable)
            return _Board[PlayerCell.X + delta, PlayerCell.Y];
        }

        if (deltaY > max & deltaX < max)
        {
          delta = deltaY - max;
          if ((PlayerCell.Y > TargetCell.Y) & (_Board[PlayerCell.X, PlayerCell.Y - max] != null) && _Board[PlayerCell.X, PlayerCell.Y-max].Type != CellType.Impassable)
          return _Board[PlayerCell.X, PlayerCell.Y - delta];
          else if ((PlayerCell.Y < TargetCell.Y) & (_Board[PlayerCell.X, PlayerCell.Y + max] != null) && _Board[PlayerCell.X, PlayerCell.Y+max].Type != CellType.Impassable)
          return _Board[PlayerCell.X, PlayerCell.Y + delta];
        }

        if (deltaX > max & deltaY > max)
        {
          delta = deltaX - max;
          newDelta = deltaY - max;
          if ((PlayerCell.X > TargetCell.X) && (PlayerCell.Y > TargetCell.Y) && (_Board[PlayerCell.X - max, PlayerCell.Y - max] != null) && _Board[PlayerCell.X - max, PlayerCell.Y-max].Type != CellType.Impassable)
            return _Board[PlayerCell.X - delta, PlayerCell.Y - newDelta];
          else if ((PlayerCell.X > TargetCell.X) && (PlayerCell.Y < TargetCell.Y) && (_Board[PlayerCell.X - max, PlayerCell.Y + max] != null) && _Board[PlayerCell.X - max, PlayerCell.Y + max].Type != CellType.Impassable)
            return _Board[PlayerCell.X - delta, PlayerCell.Y + newDelta];
          else if ((PlayerCell.X < TargetCell.X) && (PlayerCell.Y > TargetCell.Y) && (_Board[PlayerCell.X + max, PlayerCell.Y - max] != null) && _Board[PlayerCell.X + max, PlayerCell.Y - max].Type != CellType.Impassable)
            return _Board[PlayerCell.X + delta, PlayerCell.Y - newDelta];
          else if ((PlayerCell.X < TargetCell.X) && (PlayerCell.Y < TargetCell.Y) && (_Board[PlayerCell.X + max, PlayerCell.Y + max] != null) && _Board[PlayerCell.X + max, PlayerCell.Y + max].Type != CellType.Impassable)
            return _Board[PlayerCell.X + delta, PlayerCell.Y + newDelta];
        }
        --max;
      }

      return null;
    }

    /// <summary>Обновляем уровень</summary>
    public void Update(MouseState state)
    {
      CurrentUpdate(state);

      if (state.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton == ButtonState.Released)
        ChangeSelectedMagic(state);

      GetMagic(state);
      if (state.RightButton == ButtonState.Pressed && _LastMouseState.RightButton == ButtonState.Released)
      {
        var target = GetTargetPassable(state);
        if (target == null)
          return;

        if (_SelectedMagic != null && !_SelectedMagic.Executed)
          ExecuteMagic(target);
        else
          _Players[0].MoveTo(target);
      }
      if (_Wait&&_LastTagret!=null)
        ExecuteMagic(_LastTagret);
      _FloatingZone.Update();
      _LastMouseState = state;
    }
    #endregion

  }
}
