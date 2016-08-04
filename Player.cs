using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Класс игрока</summary>
  public class Player
  {
    #region Поля

    /// <summary>Текстура игрока</summary>
    private AnimateSprite _Tile;

    /// <summary>Текущая позиция игрока</summary>
    private Rectangle _CurrentState;

    /// <summary>Тип игрока</summary>
    private PlayerType _Type;

    private int RegenDelay = 100;

    /// <summary>Текущая ячейка</summary>
    private Cell _CurrentCell;

    /// <summary>Имя игрока</summary>
    public string Name;

    /// <summary>Цвет игрока</summary>
    private Color _Color;

    /// <summary>Собственность героя</summary>
    private List<Building> _Buildings;

    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Рисуем выделение</summary>
    public bool DrawEnemySelector;

    /// <summary>Рисуем мирное выделение</summary>
    public bool DrawOurSelector;

    /// <summary>Выделение атаки</summary>
    private Texture2D _EnemySelector;

    /// <summary>Мирный выделитель</summary>
    private Texture2D _OurSelector;
    /// <summary>Текущее количество магии</summary>
    public int CurrentMagic;

    public bool Dead;

    PlayerAnimation _Animation;

    private Movement _Movement;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="Player"/>.</summary>
    /// <param name="game">Ссылка на игру</param>
    /// <param name="type">Тип игрока</param>
    /// <param name="startPoint">Откуда начинает</param>
    public Player(SourceryGame game,XmlNode node)
    {
      
      _Buildings = new List<Building>();
      _EnemySelector = game.Selectors[0];
      _OurSelector = game.Selectors[1];
      _Font = game.Font;
      LoadPlayerParameters(node,game);
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает область отрисовки</summary>
    public Rectangle Rect
    {
      get { return _CurrentState; }
    }

    public int Regen
    {
      get {
        int Value = 0;
        foreach (Building castle in _Buildings)
          Value += castle.Reduction;
        return Value;
      }
    }
    
    public PlayerType Type
    {
      get { return _Type; }
    }
    public int MaxMagic
    {
      get 
      {
        int Value = 0;
        foreach (Building castle in _Buildings)
          Value += castle.CurrentResource;
        return Value;
      }
    }
    /// <summary>Текущее положение героя</summary>
    public Cell Position
    {
      get { return _CurrentCell; }
    }
    #endregion

    #region Методы

    #region Загрузка
    /// <summary>Загружает параметры игрока из XML-файла</summary>
    /// <param name="node">Запись об игроке.</param>
    /// <param name="game">Ссылка на игру.</param>
    public void LoadPlayerParameters(XmlNode node, SourceryGame game)
    {
      //Читаем параметры из *.lvl
      string playerName = node.SelectSingleNode("Name").InnerText;
      string fileName = node.SelectSingleNode("File").InnerText;
 
      int x = Convert.ToInt32(node.SelectSingleNode("x").InnerText);
      int y = Convert.ToInt32(node.SelectSingleNode("y").InnerText);
      int startAnimation = Convert.ToInt32(node.SelectSingleNode("StartAnimation").InnerText);
      var type = node.SelectSingleNode("Type").InnerText;
      PlayerType playerType = (PlayerType)Enum.Parse(typeof(PlayerType), type);
      type = node.SelectSingleNode("Color").InnerText;
      Colors color = (Colors)Enum.Parse(typeof(Colors), type);

      //Читаем параметры героя
      XmlDocument doc = new XmlDocument();
      doc.Load("data/players/" + fileName);
      string textureName = doc.SelectSingleNode("Player/Texture").InnerText;
      int width = int.Parse(doc.SelectSingleNode("Player/width").InnerText);
      int height = int.Parse(doc.SelectSingleNode("Player/height").InnerText);
      int speed = int.Parse(doc.SelectSingleNode("Player/speed").InnerText);
      _Animation = new PlayerAnimation(doc.SelectSingleNode("Player/Animation"));

      //Задаем начальные параметры
      _Tile = new AnimateSprite(game.Content.Load<Texture2D>("animation/" + textureName), 8, 24);
      _Tile.CurrentFrame = startAnimation;
      LoadColor(color);
      _CurrentState = new Rectangle(x-width/2, y-height/2, width, height);
      _Type = playerType;
      if (playerName == "")
        Name = game.PlayerName;
      else
        Name = playerName;
      foreach (Cell cell in Helper.Board)
        if (cell.Rect.Contains(x, y))
        {
          _CurrentCell = cell;
          break;
        }
      CurrentMagic = MaxMagic;
      _Movement = new Movement(_CurrentState, _CurrentCell, speed, _Tile, _Animation,false);
    }


    /// <summary>Загружает цвет игрока</summary>
    /// <param name="color">Цвет.</param>
    private void LoadColor(Colors color)
    {
      switch (color)
      {
        case Colors.Blue: { _Color = Color.Blue; break; }
        case Colors.Green: { _Color = Color.Green; break; }
        case Colors.Red: { _Color = Color.Red; break; }
        case Colors.Yellow: { _Color = Color.Yellow; break; }
      }
    }

    #endregion

    public void Stop()
    { 
      _Movement.Stop();
    }
    public void DeleteTower(Tower tower)
    {
      _Buildings.Remove(tower);
    }

    /// <summary>Добавляем строение герою</summary>
    /// <param name="building">Строение.</param>
    public void AddBuilding(Building building)
    {
      if (building == null)
        return;
      _Buildings.Add(building);
      CurrentMagic += building.MaxResource;
      building.Owner = this;
    }


    /// <summary>Двигаемся по маршруту от клетки к клетке</summary>
    /// <param name="route">Маршрут</param>
    public void MoveToRoute(List<Point> route)
    {
     _Movement.MoveToRoute(route);
    }

    public void MoveTo(Cell to)
    {
      _Movement.Move(_CurrentCell,to,_CurrentState);
    }
    public bool Contains(int x, int y)
    { 
      return _CurrentState.Contains(x,y);
    }

    /// <summary>Рисует персонажа</summary>
    public void Draw(SpriteBatch spriteBatch)
    {
      _CurrentState = _Movement.CurrentState;
      _CurrentCell = _Movement.CurrentCell;
      _Tile.Draw(spriteBatch, _CurrentState);
      spriteBatch.DrawString(_Font, Name, new Vector2(_CurrentState.X + 10, _CurrentState.Y + 10), _Color);

      if (DrawEnemySelector)
        spriteBatch.Draw(_EnemySelector, _CurrentState,Color.White);
      if (DrawOurSelector)
        spriteBatch.Draw(_OurSelector, _CurrentState, Color.White);
    }

    /// <summary>Обновляемся</summary>
    /// <param name="state">Состояние мышки</param>
    public void Update(MouseState state)
    {
      if (Dead)
        return;
      if (CurrentMagic <= 0)
      {
        Dead = true;
        _Tile.CurrentFrame += 23;
      }
      if (RegenDelay != 0)
      {
        RegenDelay--;
      }
      else
      {
        CurrentMagic += Regen;
        RegenDelay = 100;
      }

      if (CurrentMagic >= MaxMagic)
        CurrentMagic = MaxMagic;

      _Movement.Update();
    }
    #endregion
  }

}
