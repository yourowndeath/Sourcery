using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sourcery
{

  /// <summary>Зона с магиями игроков</summary>
  class FloatingZone
  {
    #region Поля
    /// <summary>Левое обрамление</summary>
    private Texture2D _Left;

    /// <summary>Правое обрамление</summary>
    private Texture2D _Right;

    /// <summary>Разделитель</summary>
    private Texture2D _Divider;

    /// <summary>Зоны</summary>
    private List<ColorZone> _Zones;

    /// <summary>Список игроков</summary>
    private List<Player> _Players;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="FloatingZone"/>.</summary>
    /// <param name="game">Ссылка на игру.</param>
    /// <param name="players">Список игроков.</param>
    public FloatingZone(SourceryGame game, List<Player> players)
    {
      _Left = game.Content.Load<Texture2D>("Controls/LeftFloat");
      _Right = game.Content.Load<Texture2D>("Controls/RigthFloat");
      _Divider = game.Content.Load<Texture2D>("Controls/divider");
      _Zones = new List<ColorZone>();
      _Players = players;
      int i = 0;
      foreach (Player player in _Players)
      {
        _Zones.Add(new ColorZone(game, player.MaxMagic, player.CurrentMagic, i));
        i++;
      }
      _Players = players;
    }
    #endregion

    #region Методы

    /// <summary>Рисуем</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
      var width = spriteBatch.GraphicsDevice.Viewport.Width;
      var height = spriteBatch.GraphicsDevice.Viewport.Height;

      var rect = new Rectangle(10, height - _Left.Height, _Left.Width, _Left.Height);
      var zoneWidth = (width - 20 - _Left.Width - _Right.Width) / _Zones.Count;
      var zoneHeight = _Left.Height - 5;

      spriteBatch.Draw(_Left, rect, Color.White);
      int i = 0;
      foreach (ColorZone zone in _Zones)
      {
        zone.Draw(spriteBatch, new Rectangle((10 + _Left.Width) + zoneWidth * i, height - _Left.Height + 5, zoneWidth, zoneHeight));
        i++;
      }
      spriteBatch.Draw(_Right, new Vector2(10 + _Left.Width + zoneWidth * _Zones.Count, height - _Left.Height), Color.White);
    }

    /// <summary>Обновляемся</summary>
    public void Update()
    {
      int i = 0;
      foreach (ColorZone zone in _Zones)
      {
        zone.Update(_Players[i].CurrentMagic, _Players[i].MaxMagic);
        i++;
      }
    }
    #endregion
  }
}
