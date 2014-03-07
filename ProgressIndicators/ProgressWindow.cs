// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressWindow.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ProgressWindow" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.ProgressIndicators
{
  using System;
  using System.Windows.Forms;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="ProgressWindow"/> class.</summary>
  public partial class ProgressWindow : Form
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ProgressWindow"/> class.</summary>
    public ProgressWindow()
    {
      this.InitializeComponent();

      this.ProgressIndicator = new ProgressIndicator();

      this.ProgressIndicator.Updated += this.UpdateProgress;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the progress bar.
    /// </summary>
    [NotNull]
    public ProgressIndicator ProgressIndicator { get; private set; }

    #endregion

    #region Methods

    /// <summary>Updates the progress.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void UpdateProgress(object sender, EventArgs e)
    {
      this.ProjectName.Text = this.ProgressIndicator.Task;
      this.FileName.Text = this.ProgressIndicator.Text;

      this.Update();
    }

    #endregion
  }
}