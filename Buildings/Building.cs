using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Абстрактный класс строения</summary>
  abstract public class Building
  {
    #region Поля
    /// <summary>Спрайт строения</summary>
    public AnimateSprite Sprite;

    /// <summary>Имя хозяина строения</summary>
    public string OwnerName;

    /// <summary>Тип строения</summary>
    public BuildingType Type;

    /// <summary>Спрайт подписи</summary>
    public AnimateSprite Signature;

    /// <summary>Шрифт для отрисовки</summary>
    public SpriteFont Font;

    /// <summary>Выделение</summary>
    public Texture2D Selector;

    /// <summary>Область отрисовки</summary>
    public Rectangle Rect;

    /// <summary>Ширина</summary>
    public int Width;

    /// <summary>Высота</summary>
    public int Height;

    /// <summary>Верхний левый угол X</summary>
    public int X;

    /// <summary>Верхний левый угол Y</summary>
    public int Y;

    /// <summary>Хозяин здания</summary>
    private Player _Owner;

    /// <summary>Возвращает или задает Reduction</summary>
    public int Reduction { get; set; }

    /// <summary>Возвращает или задает максимальное количество маны</summary>
    public int MaxResource { get; set; }

    /// <summary>Возвращает или задает текущее количество маны</summary>
    public int CurrentResource { get; set; }

    /// <summary>Возвращает или задает отрисовку вражеского селектора</summary>
    public bool DrawEnemySelector { get; set; }

    /// <summary>Возвращает или задает отрисовку дружеского селектора</summary>
    public bool DrawOurSelector { get; set; }

    /// <summary>Выделение атаки</summary>
    public Texture2D EnemySelector;

    /// <summary>Мирный выделитель</summary>
    public Texture2D OurSelector;
    #endregion

    #region Свойства
    /// <summary>Возвращает или задает хозяина строения</summary>
    public Player Owner
    {
      get { return _Owner; }

      set
      {
        if (value != null)
        {
          if (Type == BuildingType.Castle)
          {
            Sprite.CurrentFrame -= 3;
            Signature.CurrentFrame = 1;
            _Owner = value;
          }
          else
          {
            if (_Owner != null)
            {
              Sprite.CurrentFrame = 1;
              Signature.CurrentFrame = 1;
              _Owner.DeleteTower(this as Tower);
            }
            _Owner = value;
          }
        }
      }
    }
    #endregion

    #region Методы
    /// <summary>Определяет принадлежность точки области здания</summary>
    public bool Contains(int x, int y)
    {
      return Rect.Contains(x, y);
    }

    /// <summary>Рисует строение</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Update(MouseState state);
    #endregion
  }
}
