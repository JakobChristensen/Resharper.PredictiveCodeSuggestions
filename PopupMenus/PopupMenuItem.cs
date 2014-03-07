// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopupMenuItem.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="PopupMenuItem" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.PopupMenus
{
  using System;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="PopupMenuItem"/> class.</summary>
  public class PopupMenuItem
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="PopupMenuItem"/> class.</summary>
    /// <param name="text">The text.</param>
    public PopupMenuItem([NotNull] string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text");
      }

      this.Text = text;
    }

    #endregion

    #region Public Events

    /// <summary>
    /// Occurs when clicked.
    /// </summary>
    public event EventHandler Clicked;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    /// <value>The tag.</value>
    public object Tag { get; set; }

    /// <summary>
    /// Gets the text.
    /// </summary>
    [NotNull]
    public string Text { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Raises the clicked.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public void RaiseClicked([NotNull] object sender, [NotNull] EventArgs e)
    {
      if (sender == null)
      {
        throw new ArgumentNullException("sender");
      }

      if (e == null)
      {
        throw new ArgumentNullException("e");
      }

      var c = this.Clicked;
      if (c != null)
      {
        c(this, e);
      }
    }

    #endregion
  }
}