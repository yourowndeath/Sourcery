using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Sourcery
{

  /// <summary>Настройки экранов</summary>
  public class ScreenSettings
  {
    #region Поля

    /// <summary>Порядковый номер</summary>
    public int Number;

    /// <summary>Время отрисовки</summary>
    public int Time;

    /// <summary>Имя текстуры задника</summary>
    public string Texture;

    /// <summary>Тип экрана</summary>
    public ScreenType Type;

    /// <summary>Прозрачность появления</summary>
    public int FadeIn;

    /// <summary>Цвет изчезания</summary>
    public int FadeOut;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="ScreenSettings"/>.</summary>
    /// <param name="node">Запись xml.</param>
    /// <exception cref="System.ArgumentNullException">Настройки экрана отсутствуют</exception>
    public ScreenSettings(XmlNode node)
    {
      if (node == null)
        throw new ArgumentNullException("Настройки экрана отсутствуют");

      Number = int.Parse(node.SelectSingleNode("Number").InnerText);
      Time = int.Parse(node.SelectSingleNode("Time").InnerText);
      Texture = node.SelectSingleNode("Texture").InnerText;
      Type = (ScreenType)Enum.Parse(typeof(ScreenType), node.SelectSingleNode("Type").InnerText);
      FadeIn = int.Parse(node.SelectSingleNode("FadeIn").InnerText);
      FadeOut = int.Parse(node.SelectSingleNode("FadeOut").InnerText);
    }


    #endregion

  }
}
