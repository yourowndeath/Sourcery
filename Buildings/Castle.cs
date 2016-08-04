using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Класс нейтрального строения</summary>
  public class Castle : Building
  {
    #region Поля
    /// <summary>Текстура для значка улучшения</summary>
    private readonly Texture2D _Upgrade;

    /// <summary>Уровень города</summary>
    private int _Level;

    /// <summary>Начало улучшения</summary>
    private bool _StartUpdate;

    /// <summary>Время на улучшение</summary>
    private int _UpdateDelay;
    private int _Position;

    /// <summary>Предыдущее состояние мыши</summary>
    private MouseState _LastMouseState;

    /// <summary>Максимально возможный уровень</summary>
    private int _MaxLevel;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="Castle"/>.</summary>
    /// <param name="node">Запись в xml файле.</param>
    public Castle(XmlNode node)
    {
      Type = BuildingType.Castle;
      Font = Helper.Game.SmallFont;
      _Upgrade = Helper.Game.Upgrade;
      LoadFromDocument(node, Helper.Game);
      EnemySelector = Helper.Game.Selectors[0];
      OurSelector = Helper.Game.Selectors[1];

    }

    #endregion

    #region Свойства

    /// <summary>Возвращает или задает текущий уровень замка</summary>
    public int Level
    {
      get { return _Level; }
      set
      {
        _Level = value;
        Sprite.CurrentFrame += 5;
        MaxResource = 50 + 25 * (_Level - 1);
        CurrentResource = MaxResource;
        Reduction = MaxResource / 10;
      }
    }
    /// <summary>Меняем цвет</summary>
    public int Position
    {
      get { return _Position; }
      set { _Position = value; Signature.CurrentFrame = value; Sprite.CurrentFrame = value; }
    }

    /// <summary>Возвращает стоимость прокачки</summary>
    public int Cost
    {
      get { return MaxResource / 2; }
    }

    #endregion

    #region Методы

    /// <summary>Загружаем параметры из документа</summary>
    /// <param name="node">Запись.</param>
    /// <param name="game">Ссылка на игру.</param>
    private void LoadFromDocument(XmlNode node, SourceryGame game)
    {
      //Читаем настройки из файла
      _UpdateDelay = int.Parse(node.SelectSingleNode("UpdateDelay").InnerText);
      _MaxLevel = int.Parse(node.SelectSingleNode("MaxLevel").InnerText);
      Sprite = new AnimateSprite(game.Content.Load<Texture2D>("Game/" + node.SelectSingleNode("Texture").InnerText), 5, 5);
      Sprite.CurrentFrame = Convert.ToInt32(node.SelectSingleNode("position").InnerText);
      Signature = new AnimateSprite(game.ColorStripe, 1, 5);
      Signature.CurrentFrame = int.Parse(node.SelectSingleNode("signaturePosition").InnerText);
      X = Convert.ToInt32(node.SelectSingleNode("x").InnerText);
      Y = Convert.ToInt32(node.SelectSingleNode("y").InnerText);
      _Level= Convert.ToInt32(node.SelectSingleNode("level").InnerText);
      OwnerName = node.SelectSingleNode("ownerName").InnerText;
      MaxResource = 50 + 25 * (_Level - 1);
      CurrentResource = MaxResource;
      Reduction = MaxResource / 10;

      var param = node.SelectSingleNode("width");
      int width = 0;
      int height = 0;
      if (param != null)
        width = Convert.ToInt32(param.InnerText);
      param = node.SelectSingleNode("height");
      if (param != null)
        height = Convert.ToInt32(param.InnerText);

      //Задаем начальные параметры
      if (width != 0 && height != 0)
      {
        Rect = new Rectangle(X, Y, width, height);
        Width = width;
        Height = height;
      }
      else
      {
        Width = Sprite.Width;
        Height = Sprite.Height;
        Rect = new Rectangle(X,Y,Width,Height);
      }
    }
    
    /// <summary>Рисуем</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
      //Здание
      Sprite.Draw(spriteBatch, Rect);

      //Подпись
      var health = CurrentResource.ToString() + "/" + MaxResource.ToString();
      Vector2 FontOrigin = Font.MeasureString(health) / 2;
      var signatureRect = new Rectangle(Rect.X+Rect.Width/2-Signature.Width/2,Rect.Y+Rect.Height-Signature.Height,Signature.Width,Signature.Height);
      Signature.Draw(spriteBatch,signatureRect);
      spriteBatch.DrawString(Font, health, new Vector2(signatureRect.Right - signatureRect.Width / 2, signatureRect.Bottom - signatureRect.Height / 2), Color.Black, 0, FontOrigin, 1, SpriteEffects.None, 1);

      //Кнопка апдейта
      if (Owner != null && Owner.Type == PlayerType.Human && _Level!=_MaxLevel && !_StartUpdate)
        spriteBatch.Draw(_Upgrade, new Vector2(Rect.X+10,Rect.Y+10));

      //Селекторы
      if (DrawEnemySelector)
        spriteBatch.Draw(EnemySelector, Rect, Color.White);
      else if (DrawOurSelector)
        spriteBatch.Draw(OurSelector, Rect, Color.White);
    }

    /// <summary>Обновляем</summary>
    /// <param name="state">Состояние мыши.</param>
    public override void Update(MouseState state)
    {
      if (_StartUpdate)
      {
        _UpdateDelay--;
        if (_UpdateDelay == 0)
        {
          Level += 1;
          _StartUpdate = false;
          _UpdateDelay = 500;
        }
      }

      if (state.LeftButton == ButtonState.Pressed && _LastMouseState.LeftButton == ButtonState.Released && Sprite.CurrentFrame != 21)
      {

        var rect = new Rectangle(Rect.X+10,Rect.Y+10, _Upgrade.Width, _Upgrade.Height);
        if (rect.Contains(state.X, state.Y))
          if (Cost <= Owner.CurrentMagic)
            _StartUpdate = true;
      }
      _LastMouseState = state;
    }

    #endregion
  }
}
