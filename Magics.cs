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

  /// <summary>Класс магии</summary>
  class Magic
  {
    #region Поля
    /// <summary>Запись во входном файле</summary>
    private XmlNode _Node;

    public bool Finished;

    private int _UpdateDelay = 5;

    /// <summary>Используем</summary>
    private bool _Executed;

    /// <summary>Выбрана</summary>
    public bool Selected;

    /// <summary>Кулдаун</summary>
    private int _Reload;

    /// <summary>Имя магии</summary>
    public string Name;

    /// <summary>Время анимации</summary>
    public int Animation;

    /// <summary>Задержка перед применением</summary>
    public int Delay;

    /// <summary>Стоимость</summary>
    public int Cost;

    /// <summary>Эффект магии</summary>
    public int Effect;

    /// <summary>Направление</summary>
    public DirectionType Direction;

    /// <summary>Тип магии</summary>
    public MagicType Type;

    /// <summary>Радиус действия</summary>
    public int Radius;

    /// <summary>Радиус действия эффекта</summary>
    public int EffectRadius;

    /// <summary>Пост-эффект магии</summary>
    public PostEffectType PostEffect;

    /// <summary>Время действия пост эффекта</summary>
    public int PostEffectTime;

    /// <summary>Возможные цели</summary>
    public List<TargetType> Targets;

    /// <summary>Анимация магии</summary>
    public AnimateSprite _Sprite;

    /// <summary>Задник для магии</summary>
    public AnimateSprite _Back;

    /// <summary>Начало анимации</summary>
    private int From;

    /// <summary>Конец анимации</summary>
    private int To;

    /// <summary>Иконка</summary>
    private int Static;

    /// <summary>Показываем описание</summary>
    private bool _ShowText;

    /// <summary>Область текста</summary>
    private Vector2 _TextVector;

    /// <summary>Шрифт</summary>
    private SpriteFont _Font;

    /// <summary>Большой шрифт</summary>
    private SpriteFont _BigFont;

    /// <summary>Предыдущее состояние мыши</summary>
    private MouseState _LastMousestate;

    /// <summary>Откуда начинается</summary>
    private Cell _FromCell;

    /// <summary>Куда направлена</summary>
    private Cell _ToCell;

    /// <summary>Ширина текстуры</summary>
    private int _Width;

    /// <summary>Высота текстуры</summary>
    private int _Height;

    /// <summary>Прямоугольник для летящей магии</summary>
    private Rectangle _CurrentRectangle;

    /// <summary>Класс перемещений</summary>
    private Movement _Movement;

    /// <summary>Параметры анимации</summary>
    private PlayerAnimation _Animation;
    private int _Speed;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="Magic"/>.</summary>
    /// <param name="node">Xml-запись</param>
    /// <param name="game">Ссылка на игру.</param>
    public Magic(XmlNode node, SourceryGame game)
    {
      _Node = node;
      LoadAttributes(node);
      _Executed = false;
      LoadSprites(game);
      _ShowText = false;
      _Movement = new Movement(new Rectangle(0,0,0,0),null,_Speed,_Sprite,_Animation,true);
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает используется ли магия</summary>
    public bool Executed
    {
      get { return _Executed; }
      set { _Executed = value; }
    }

    /// <summary>Возвращает активна магия или нет в данный момент</summary>
    public bool Active
    {
      get { return _Reload == 0; }
    }

    #endregion

    #region Методы 

    #region Загрузка 
    /// <summary>Загружаем все необходимое для отрисовки</summary>
    private void LoadSprites(SourceryGame game)
    {
      _Font = game.Font;
      _BigFont = game.BigFont;
      _Back = new AnimateSprite(game.Content.Load<Texture2D>("controls/ReloadingButton"), 3, 1);
      LoadSprite(game);
    }

    /// <summary>Загружаем атрибуты магии</summary>
    private void LoadAttributes(XmlNode node)
    {
      Name = node.SelectSingleNode("Name").InnerText;
      _Animation = new PlayerAnimation(node.SelectSingleNode("MovementAnimation"));
      _Width = int.Parse(node.SelectSingleNode("width").InnerText);
      _Speed = int.Parse(node.SelectSingleNode("MovementAnimation/speed").InnerText);
      _Height = int.Parse(node.SelectSingleNode("height").InnerText);
      double res = Convert.ToDouble(node.SelectSingleNode("Animation").InnerText);
      _Reload = Convert.ToInt32(node.SelectSingleNode("Reload").InnerText) * 100;
      Animation = (int)((double)res * 100);
      res = Convert.ToDouble(node.SelectSingleNode("Delay").InnerText);
      Delay = (int)((double)res * 100);
      Cost = Convert.ToInt32(node.SelectSingleNode("Cost").InnerText);
      Effect = Convert.ToInt32(node.SelectSingleNode("Effect").InnerText);
      var type = node.SelectSingleNode("Direction").InnerText;
      Direction = (DirectionType)Enum.Parse(typeof(DirectionType), type);
      type = node.SelectSingleNode("Type").InnerText;
      Type = (MagicType)Enum.Parse(typeof(MagicType), type);
      Radius = Convert.ToInt32(node.SelectSingleNode("Radius").InnerText);
      EffectRadius = Convert.ToInt32(node.SelectSingleNode("EffectRadius").InnerText);
      type = node.SelectSingleNode("PostEffect").InnerText;
      PostEffect = (PostEffectType)Enum.Parse(typeof(PostEffectType), type);
      PostEffectTime = Convert.ToInt32(node.SelectSingleNode("PostEffectTime").InnerText);
      Targets = new List<TargetType>();
      var lst = node.SelectNodes("Targets/Target");
      
      foreach (XmlNode current in lst)
        Targets.Add((TargetType)Enum.Parse(typeof(TargetType), current.InnerText));
    }

    //Загрузим соответствующий спрайт
    private void LoadSprite(SourceryGame game)
    {
       string name = _Node.SelectSingleNode("Animations/Texture").InnerText;
       int start = int.Parse(_Node.SelectSingleNode("Animations/Start").InnerText);
       int stop = int.Parse(_Node.SelectSingleNode("Animations/Stop").InnerText);
       int current = int.Parse(_Node.SelectSingleNode("Animations/Current").InnerText);
       _Sprite = new AnimateSprite(game.Content.Load<Texture2D>("animation/"+name),8,8);
       From = start;
       To = stop;
       Static = current;
      _Sprite.CurrentFrame = Static;
    }
    #endregion

    /// <summary>Отрисовываем магию</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="rect">Область рисования</param>
    public void Draw(SpriteBatch spriteBatch, Rectangle rect)
    {
      if (_Executed)
        _Sprite.Draw(spriteBatch,_CurrentRectangle);
      else
      {
        if (_Reload > 0)
          _Back.CurrentFrame = 1;
        else
          _Back.CurrentFrame = 0;
        if (Selected)
          _Back.CurrentFrame = 2;
        _Back.Draw(spriteBatch, rect);
        _Sprite.Draw(spriteBatch, new Rectangle(rect.X + 5, rect.Y + 5, rect.Width - 5, rect.Height - 5));
        if (_ShowText)
          spriteBatch.DrawString(_Font, Name, _TextVector, Color.Black);
        if (_Reload > 0)
        {
          var value = _Reload / 100;
          Vector2 target = _Font.MeasureString(value.ToString()) / 2;
          spriteBatch.DrawString(_BigFont, value.ToString(), new Vector2(rect.Right - rect.Width / 2, rect.Bottom - rect.Height / 2), Color.White, 0, target, 1, SpriteEffects.None, 1);
        }
      }
    }


    /// <summary>Применить магию</summary>
    /// <param name="From">От ячейки.</param>
    /// <param name="To">В ячейку.</param>
    public void Execute(Cell From, Cell To)
    {
      _Executed = true;
      _FromCell = From;
      _ToCell = To;
      Finished = false;
    }


    /// <summary>Показывать подсказку</summary>
    /// <param name="state">Положение курсора.</param>
    private void ShowHint(MouseState state)
    {
      if (_Back.Contains(state.X, state.Y))
      {
        _ShowText = true;
        _TextVector = new Vector2(state.X, state.Y);
      }
      else
        _ShowText = false; 
    }

    private void MoveMagic()
    {

    }
    /// <summary>Обновляем состояние выполняющейся магии</summary>
    private void UpdateExecuted()
    {
      bool IsEnd = false;
      //Если начало и конец одинаковые, то выполняем магию в текущей ячейке
      if (_FromCell == _ToCell)
      {
        _CurrentRectangle = new Rectangle(_ToCell.Rect.X + _ToCell.Rect.Width / 2 - _Width / 2, _ToCell.Rect.Y + _ToCell.Rect.Height / 2 - _Height / 2, _Width, _Height);
       if (_UpdateDelay!=0)
       {
         _UpdateDelay--;
         return;
       }
        _Sprite.UpdateInRange(From,To);
        _UpdateDelay = 5;
        IsEnd = (_Sprite.CurrentFrame == To);
      }
      else
      {
        if (_CurrentRectangle == null || _CurrentRectangle == new Rectangle(0, 0, 0, 0))
        {
          _CurrentRectangle = new Rectangle(_FromCell.Rect.X + _FromCell.Rect.Width / 2 - _Width / 2, _FromCell.Rect.Y + _FromCell.Rect.Height / 2 - _Height / 2, _Width, _Height);
          _Movement.Move(_FromCell, _ToCell, _CurrentRectangle);
        }
        _Movement.Update();
        _CurrentRectangle = _Movement.CurrentState;
        IsEnd = _Movement.IsEnd;
      }
      
 
      if (IsEnd)
      {
        _Executed = false;
        Finished = true;
        _Sprite.CurrentFrame = Static;
        _Reload = Convert.ToInt32(_Node.SelectSingleNode("Reload").InnerText) * 100;
        Selected = false;
        _CurrentRectangle = new Rectangle(0,0,0,0);
      }
    }
    /// <summary>Обновляемся</summary>
    /// <param name="state">Состояние мыши</param>
    public void Update(MouseState state)
    {
      if (_Reload != 0)
        _Reload--;
      if (state.LeftButton != ButtonState.Pressed && state.RightButton != ButtonState.Pressed)
        ShowHint(state);

      if (_Executed)
        UpdateExecuted();
      _LastMousestate = state;
    }
    #endregion


  }

  /// <summary>Все магии в игре</summary>
  class Magics
  {
    #region Поля
    /// <summary>Документ исходник</summary>
    private XmlDocument _Document;

    /// <summary>Список магий</summary>
    public List<Magic> MagicList;

    /// <summary>Ссылка на игру</summary>
    private SourceryGame _Game;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="Magics"/>.</summary>
    /// <param name="path">Путь до *.xml файла</param>
    public Magics(string path,SourceryGame game)
    {
      _Document = new XmlDocument();
      _Game = game;
      _Document.Load(path);
      MagicList = new List<Magic>();
      var nodeList = _Document.SelectNodes("Magics/Magic");
      foreach (XmlNode node in nodeList)
        MagicList.Add(new Magic(node,game));
    }
    #endregion
  }
}
