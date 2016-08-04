using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Sourcery
{
  /// <summary>Класс описания элемента меню</summary>
  class MenuItem
  {
    #region Поля

    /// <summary>Значения</summary>
    public List<String> Values;

    /// <summary>Контрол кнопки</summary>
    public Control ItemControl;

    /// <summary>Запись о пункте меню</summary>
    private XmlNode _Node;

    /// <summary>Наименование пункта меню</summary>
    private string _Name;

    /// <summary>Действие элемента</summary>
    private ActionType _Action;

    /// <summary>Область отрисовки кнопки</summary>
    private Rectangle _Rectangle;

    /// <summary>Расшифровка пункта меню</summary>
    private string _Description;

    /// <summary>Положение пункта меню</summary>
    private int _Position;

    /// <summary>Родитель элемента меню</summary>
    private MenuItem _Parent;

    /// <summary>Тип контрола</summary>
    private ControlType _ControlType;

    /// <summary>Список дочерних элементов</summary>
    public List<MenuItem> Items;
    #endregion

    #region События
    public delegate void ItemChanged(MenuItem item);
    public event ItemChanged OnItemChanged;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="MenuItem"/>. </summary>
    public MenuItem(XmlNode Node,MenuItem Parent,SourceryGame game)
    {
      _Node = Node;
      _Parent = Parent;
      _Name = _Node.SelectSingleNode("Name").InnerText;
      _Description = _Node.SelectSingleNode("Description").InnerText;
      _Position = Convert.ToInt32(_Node.SelectSingleNode("Position").InnerText);
      Values = new List<string>();
      var val = _Node.SelectSingleNode("Values");
      if (val != null)
      {
        var nodeSelection = val.SelectNodes("Value");
        foreach (XmlNode node in nodeSelection)
          Values.Add(node.InnerText);
      }
      var type = _Node.SelectSingleNode("Action").InnerText;
      _Action = (ActionType)Enum.Parse(typeof(ActionType), type);
      var controltype = _Node.SelectSingleNode("Type").InnerText;
      _ControlType = (ControlType)Enum.Parse(typeof(ControlType), controltype);
      switch (_ControlType)
      {
        case ControlType.Button:
          {
            ItemControl = new Button(game, _Name);
            break;
          }
        case ControlType.CheckBox:
          {
            ItemControl = new CheckBox(game, _Name);
            break;
          }
        case ControlType.ComboBox:
          {
            ItemControl = new ComboBox(game, Values);
            break;
          }
        case ControlType.TrackBar:
          {
            ItemControl = new TrackBar(game,_Name);
            break;
          }
      }
      ItemControl.OnValueChanged += OnControlChanged;
      var selection = _Node.SelectNodes("Item");
      if (selection != null && selection.Count > 0)
      {
        Items = new List<MenuItem>();
        foreach (XmlNode node in selection)
          Items.Add(new MenuItem(node,this,game));
      }
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает или задает тип контрола</summary>
    public ControlType Control
    {
      get { return _ControlType; }
      set { _ControlType = value; }
    }
    /// <summary>Возвращает или задает родителя</summary>
    public MenuItem Parent 
    {
      get { return _Parent; }
      set { _Parent = value; }
    }
    /// <summary>Возвращает или задает действие кнопки</summary>
    public ActionType Action
    {
      get { return _Action; }
      set { _Action = value; }
    }
    /// <summary>Возвращает или задает область отрисовки кнопки</summary>
    public Rectangle Rect
    {
      get { return _Rectangle; }
      set { _Rectangle = value; }
    }

    /// <summary>Возвращает или задает имя элемента меню</summary>
    public string Name
    {
      get { return _Name; }
      set
      {
        if (value != _Name)
        {
          _Node.SelectSingleNode("Name").InnerText = value;
          _Name = value;
        }
      }
    }

    /// <summary>Возвращает или задает расшифровку элемента меню</summary>
    public string Description
    {
      get { return _Description; }
      set
      {
        if (value != _Description)
        {
          _Node.SelectSingleNode("Description").InnerText = value;
          _Description = value;
        }
      }
    }

    /// <summary>Возвращает или задает позицию элемента меню</summary>
    public int Position
    {
      get { return _Position; }
      set
      {
        if (value != _Position)
        {
          _Node.SelectSingleNode("Position").InnerText = _Position.ToString();
          _Position = value;
        }
      }
    }
    #endregion

    #region Методы

    private void OnControlChanged()
    {
      if (OnItemChanged != null)
        OnItemChanged(this);
    }
    #endregion
  }

  /// <summary>Класс описания меню</summary>
  class Menu
  {
    #region Поля
    /// <summary>Ссылка на документ главного меню</summary>
    private XmlDocument _Document;

    /// <summary>Элементы меню</summary>
    public List<MenuItem> Items;
    #endregion

    #region Конструкторы
    /// <summary> Создаёт новый экземпляр класса <see cref="Menu"/>.</summary>
    public Menu(string path, SourceryGame game)
    {
      _Document = new XmlDocument();
      _Document.Load(path);
      Items = new List<MenuItem>();

      var nodeSelection = _Document.SelectNodes("Menu/Item");
      foreach (XmlNode node in nodeSelection)
        Items.Add(new MenuItem(node, null, game));
    }
    #endregion
  }
}
