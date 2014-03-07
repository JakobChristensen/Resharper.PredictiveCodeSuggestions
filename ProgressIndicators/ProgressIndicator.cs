// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressIndicator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ProgressIndicator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.ProgressIndicators
{
  using System;

  /// <summary>Defines the <see cref="ProgressIndicator"/> class.</summary>
  public class ProgressIndicator
  {
    #region Constants and Fields

    /// <summary>The task field.</summary>
    private string task;

    /// <summary>The subtask field.</summary>
    private string text;

    #endregion

    #region Public Events

    /// <summary>
    /// Occurs when [updated].
    /// </summary>
    public event EventHandler Updated;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the task.
    /// </summary>
    /// <value>The task.</value>
    public string Task
    {
      get
      {
        return this.task;
      }

      set
      {
        this.task = value;
        this.RaiseChanged();
      }
    }

    /// <summary>
    /// Gets or sets the subtask.
    /// </summary>
    /// <value>The subtask.</value>
    public string Text
    {
      get
      {
        return this.text;
      }

      set
      {
        this.text = value;
        this.RaiseChanged();
      }
    }

    #endregion

    #region Methods

    /// <summary>Raises the changed.</summary>
    private void RaiseChanged()
    {
      var updated = this.Updated;
      if (updated != null)
      {
        updated(this, EventArgs.Empty);
      }
    }

    #endregion
  }
}