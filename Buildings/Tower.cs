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

  /// <summary>Башня игрока</summary>
  public class Tower:Building
  {
    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="Tower"/>.</summary>
    /// <param name="game">Ссылка на игру.</param>
    /// <param name="node">Запись в файле.</param>
    public Tower(XmlNode node)
    {

      Type = BuildingType.Tower;
      //Сам замок
      Sprite = new AnimateSprite(Helper.Game.Content.Load<Texture2D>("game/" + node.SelectSingleNode("Texture").InnerText), 1, 4);
      Sprite.CurrentFrame = Convert.ToInt32(node.SelectSingleNode("position").InnerText);
      
      //Подпись
      Signature = new AnimateSprite(Helper.Game.ColorStripe, 1, 5);
      Signature.CurrentFrame = Convert.ToInt32(node.SelectSingleNode("signaturePosition").InnerText);
      
      //Хоязин
      OwnerName = node.SelectSingleNode("ownerName").InnerText;
      
      //Положение
      X = Convert.ToInt32(node.SelectSingleNode("x").InnerText);
      Y = Convert.ToInt32(node.SelectSingleNode("y").InnerText);

      //Ресурс здания
      MaxResource = int.Parse(node.SelectSingleNode("maxResource").InnerText);
      CurrentResource = MaxResource;
      Reduction = MaxResource / 10;
      Font = Helper.Game.SmallFont;
      
      //Размеры здания
      var param = node.SelectSingleNode("width");
      int width = 0;
      int height = 0;
      if (param != null)
        width = Convert.ToInt32(param.InnerText);
      param = node.SelectSingleNode("height");
      if (param != null)
        height = Convert.ToInt32(param.InnerText);

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
        Rect = new Rectangle(X, Y, Width, Height);
      }
      EnemySelector = Helper.Game.Selectors[0];
      OurSelector = Helper.Game.Selectors[1];
    }
    #endregion

    #region Методы

    public override void Draw(SpriteBatch spriteBatch)
    {
      //Здание
      Sprite.Draw(spriteBatch, Rect);

      //Подпись
      var health = CurrentResource.ToString() + "/" + MaxResource.ToString();
      Vector2 FontOrigin = Font.MeasureString(health) / 2;
      var signatureRect = new Rectangle(Rect.X + Rect.Width / 2 - Signature.Width / 2, Rect.Y + Rect.Height - Signature.Height, Signature.Width, Signature.Height);
      Signature.Draw(spriteBatch, signatureRect);
      spriteBatch.DrawString(Font, health, new Vector2(signatureRect.Right - signatureRect.Width / 2, signatureRect.Bottom - signatureRect.Height / 2), Color.Black, 0, FontOrigin, 1, SpriteEffects.None, 1);

      //Селекторы
      if (DrawEnemySelector)
        spriteBatch.Draw(EnemySelector, Rect, Color.White);
      else if (DrawOurSelector)
        spriteBatch.Draw(OurSelector,Rect, Color.White);
    }

    public override void Update(MouseState state)
    {
      //
    }
    #endregion
  }
}
