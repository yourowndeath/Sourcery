using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Класс для отображения анимированного спрайта</summary>
  public class AnimateSprite
  {
    #region Поля

    /// <summary>Текстура</summary>
    private Texture2D _Texture;

    /// <summary>Количество строк в текстуре</summary>
    private int _Rows;

    /// <summary>Количество колонок в текстуре</summary>
    private int _Columns;

    /// <summary>Текущее положение</summary>
    private int _CurrentFrame;

    /// <summary>Всего элементов в спрайте</summary>
    private int _TotalFrames;

    /// <summary>Область отрисовки</summary>
    private Rectangle _Rect;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="AnimateSprite" />.</summary>
    /// <param name="texture">Спрайт</param>
    /// <param name="rows">Количество строк</param>
    /// <param name="columns">Количество колонок</param>
    public AnimateSprite(Texture2D texture, int rows, int columns)
    {
      _Texture = texture;
      _Rows = rows;
      _Columns = columns;
      _CurrentFrame = 0;
      _TotalFrames = _Rows * _Columns;
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает единичную ширину</summary>
    public int Width
    {
      get { return Texture.Width / _Columns; }
    }

    /// <summary>Возвращает единичную высоту</summary>
    public int Height
    {
      get { return Texture.Height / _Rows; }
    }

    /// <summary>Возвращает или задает спрайт</summary>
    public Texture2D Texture
    {
      get { return _Texture; }
      set { _Texture = value; }
    }

    /// <summary>Возвращает или задает количество строк</summary>
    public int Rows 
    {
      get { return _Rows; }
      set { _Rows = value; }
    }

    /// <summary>Возвращает или задает количество колонок</summary>
    public int Columns
    {
      get { return _Columns; }
      set { _Columns = value; }
    }

    /// <summary>Возвращает или задает текущий кадр</summary>
    public int CurrentFrame
    {
      get { return _CurrentFrame; }
      set { _CurrentFrame = value; }
    }

    /// <summary>Возвращает или задает общее количество фреймов</summary>
    public int TotalFrames
    {
      get { return _TotalFrames; }
      set { _TotalFrames = value; }
    }
    #endregion

    #region Методы

    /// <summary>Содержит ли область отрисовки координаты</summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <returns>true если содержит и false, если не содержит</returns>
    public bool Contains(int x, int y)
    {
      if (_Rect == null)
        return false;
      return _Rect.Contains(x, y);
    }
    /// <summary>Обновляем текущий кадр</summary>
    public void Update()
    {
      if (_CurrentFrame != _TotalFrames-1)
        _CurrentFrame++;
    }


    /// <summary>Обновляем спрайт в промежутке.</summary>
    /// <param name="begin">Начальный кадр.</param>
    /// <param name="end">Конечный кадр.</param>
    public void UpdateInRange(int begin, int end)
    {
      if (_CurrentFrame >= end || _CurrentFrame <= begin)
        _CurrentFrame = begin;
      if (_CurrentFrame <= end)
        _CurrentFrame++;
      else
        _CurrentFrame = begin;
    }


    /// <summary>Обновляем последовательно кадр за кадром.</summary>
    public void UpdateConstantly()
    {
      if (_CurrentFrame != _TotalFrames-1)
        _CurrentFrame++;
      else
        _CurrentFrame = 0;
    }

    /// <summary>Отрисовываем кадр в векторе</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">Вектор отрисовки.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 location)
    {
      int width = Texture.Width / _Columns;
      int height = Texture.Height / _Rows;
      int row = (int)((float)_CurrentFrame / (float)_Columns);
      int column = _CurrentFrame % _Columns;
      Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
      Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
      spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
    }

    /// <summary>Отрисовываем кадр в прямоугольнике</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    /// <param name="location">Прямоугольник отрисовки.</param>
    public  void Draw(SpriteBatch spriteBatch, Rectangle location)
    {
      _Rect = location;
      int width = Texture.Width / _Columns;
      int height = Texture.Height / _Rows;
      int row = (int)((float)_CurrentFrame / (float)_Columns);
      int column = _CurrentFrame % _Columns;

      Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
      Rectangle destinationRectangle = location;

      spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

    }

    #endregion
  }
}
