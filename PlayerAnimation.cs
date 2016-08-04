using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Sourcery
{

  /// <summary>Анимирование ходьбы</summary>
  class MovementAnimation
  {

    /// <summary>Начало передвижения</summary>
    public int Start;

    /// <summary>Конец передвижения</summary>
    public int Stop;

    /// <summary>Состояние по умолчанию</summary>
    public int State;

    /// <summary>Создаёт новый экземпляр класса <see cref="MovementAnimation"/>.</summary>
    /// <param name="start">Начальный кадр.</param>
    /// <param name="stop">Конечный кадр.</param>
    public MovementAnimation(int start, int stop,int state)
    {
      Start = start;
      Stop = stop;
      State = state;
    }
  }

  /// <summary>Работа с анимацией персонажа</summary>
  class PlayerAnimation
  {
    /// <summary>Скорость анимации</summary>
    public int AnimationSpeed;

    /// <summary>Анимация смерти</summary>
    public int DeadAnimation;

    /// <summary>Анимация хотьбы вперед</summary>
    public MovementAnimation Forward;

    /// <summary>Анимация хотьбы назд</summary>
    public MovementAnimation Backward;

    /// <summary>Анимация хотьбы вверх</summary>
    public MovementAnimation Top;

    /// <summary>Анимация хотьбы вниз</summary>
    public MovementAnimation Bottom;

    /// <summary>Анимация хотьбы влево-вверх</summary>
    public MovementAnimation TopLeft;

    /// <summary>Анимация хотьбы вправо-вверх</summary>
    public MovementAnimation TopRight;

    /// <summary>Анимация хотьбы влево-вниз</summary>
    public MovementAnimation BottomLeft;

    /// <summary>Анимация хотьбы вправо-вниз</summary>
    public MovementAnimation BottomRight;

    public PlayerAnimation(XmlNode node)
    {
      if (node == null)
        throw new ArgumentNullException("Анимация игрока не найдена");
      AnimationSpeed = int.Parse(node.SelectSingleNode("speed").InnerText);
      DeadAnimation = int.Parse(node.SelectSingleNode("DeadAnimation").InnerText);
      
      XmlNode movement = node.SelectSingleNode("Forward");
      Forward = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
      
      movement = node.SelectSingleNode("BackWard");
      Backward = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
      
      movement = node.SelectSingleNode("Top");
      Top = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
      
      movement = node.SelectSingleNode("Bottom");
      Bottom = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
      
      movement = node.SelectSingleNode("TopLeft");
      TopLeft = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
      
      movement = node.SelectSingleNode("TopRight");
      TopRight = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
     
      movement = node.SelectSingleNode("BottomLeft");
      BottomLeft = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
      
      movement = node.SelectSingleNode("BottomRight");
      BottomRight = new MovementAnimation(int.Parse(movement.SelectSingleNode("Start").InnerText), int.Parse(movement.SelectSingleNode("Stop").InnerText), int.Parse(movement.SelectSingleNode("State").InnerText));
    }
  }
}
