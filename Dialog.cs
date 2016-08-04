using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sourcery
{

  /// <summary>Отображает диалог с пользователем</summary>
  class Dialog
  {
    #region Поля

    /// <summary>Панель диалога</summary>
    private Texture2D _Panel;

    /// <summary>Шрифт сообщения</summary>
    private SpriteFont _Font;

    /// <summary>Текст сообщения</summary>
    private string _Text;

    /// <summary>Кнопка ОК</summary>
    private Button _Ok;

    /// <summary>Кнопка отмена</summary>
    private Button _Cancel;

    /// <summary>Ширина окна</summary>
    private int _ClientWidth;

    /// <summary>Высота окна</summary>
    private int _ClientHeight;

    /// <summary>Область отрисовки диалога</summary>
    private Rectangle _DialogRect;
    #endregion

    #region События

    /// <summary>Делегат события</summary>
    public delegate void DialogClose(ModalResult result);

    /// <summary>Возникает при закрытии диалога</summary>
    public event DialogClose OnDialogClose;
    #endregion

    #region Конструкторы

    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="Dialog"/>.
    /// </summary>
    /// <param name="game">Ссылка на игру.</param>
    /// <param name="text">Текст сообщения.</param>
    public Dialog(SourceryGame game, string text)
    {
      _Panel = game.Panel;
      _Font = game.Font;
      _Ok = new Button(game, "Oк");
      _Ok.OnValueChanged += OnOkClick;
      _Cancel = new Button(game, "Отмена");
      _Cancel.OnValueChanged += OnCancelClick;
      _Text = text;
      _ClientWidth = game.graphics.GraphicsDevice.Viewport.Width;
      _ClientHeight = game.graphics.GraphicsDevice.Viewport.Height;
      _DialogRect = new Rectangle(_ClientWidth / 2 - 200, _ClientHeight / 2 - 100, 400, 200);
    }
    #endregion

    #region Методы

    /// <summary>Нажали Ок</summary>
    public void OnOkClick()
    {
      if (OnDialogClose != null)
        OnDialogClose(ModalResult.Ok);
    }

    /// <summary>Нажали отмена</summary>
    public void OnCancelClick()
    {
      if (OnDialogClose != null)
        OnDialogClose(ModalResult.Cancel);
    }

    /// <summary>Рисуем диалог</summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_Panel, _DialogRect, Color.White);
      Vector2 FontOrigin = _Font.MeasureString(_Text) / 2;
      spriteBatch.DrawString(_Font, _Text, new Vector2(_DialogRect.Right - _DialogRect.Width / 2, _DialogRect.Bottom - _DialogRect.Height / 2), Color.Black, 0, FontOrigin, 1, SpriteEffects.None, 1);
      _Ok.Draw(spriteBatch, new Rectangle(_DialogRect.Left + 50, _DialogRect.Bottom - 50, 100, 30));
      _Cancel.Draw(spriteBatch, new Rectangle(_DialogRect.Right - 150, _DialogRect.Bottom - 50, 100, 30));
    }

    /// <summary>Обновляем состояние кнопок</summary>
    /// <param name="state">Состояние мыши</param>
    public void Update(MouseState state)
    {
      _Ok.ChangeState(state);
      _Cancel.ChangeState(state);
    }
    #endregion  
  }
}
