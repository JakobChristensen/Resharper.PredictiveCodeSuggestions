// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupMenu.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="PopupMenu" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.PopupMenus
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.CommonControls;
  using JetBrains.DataFlow;
  using JetBrains.UI.PopupMenu;

  /// <summary>Defines the <see cref="PopupMenu"/> class.</summary>
  public class PopupMenu
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="PopupMenu"/> class.</summary>
    /// <param name="caption">The caption.</param>
    public PopupMenu([NotNull] string caption)
    {
      if (caption == null)
      {
        throw new ArgumentNullException("caption");
      }

      this.Caption = caption;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the caption.
    /// </summary>
    [NotNull]
    public string Caption { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Shows this instance.</summary>
    /// <param name="menuItems">The menu Items.</param>
    public void Show([NotNull] IEnumerable<PopupMenuItem> menuItems)
    {
      if (menuItems == null)
      {
        throw new ArgumentNullException("menuItems");
      }

      Action<Lifetime, JetPopupMenu> action = (lifetime, popupMenu) =>
      {
        if (popupMenu == null)
        {
          return;
        }

        var items = new List<SimpleMenuItem>();

        foreach (var menuItem in menuItems)
        {
          if (menuItem == null)
          {
            continue;
          }

          var item = new SimpleMenuItem
          {
            Text = menuItem.Text,
            Style = MenuItemStyle.Enabled
          };

          var executed = item.Executed;
          if (executed == null)
          {
            continue;
          }

          var m = menuItem;
          executed.Advise(EternalLifetime.Instance, () => m.RaiseClicked(this, EventArgs.Empty));

          items.Add(item);
        }

        popupMenu.SetItems(items.ToArray());

        popupMenu.Caption.Value = WindowlessControl.Create(this.Caption);
        popupMenu.KeyboardAcceleration.SetValue(KeyboardAccelerationFlags.Mnemonics);
      };

      Shell.Instance.GetComponent<JetPopupMenus>().Show(EternalLifetime.Instance, JetPopupMenu.ShowWhen.NoItemsBannerIfNoItems, action);
    }

    #endregion
  }
}