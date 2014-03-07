// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScopeBase.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ScopeBase" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.LiveTemplates
{
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="ScopeBase"/> class.</summary>
  public abstract class ScopeBase
  {
    #region Public Methods

    /// <summary>Determines whether this instance can execute the specified context.</summary>
    /// <param name="context">The context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance can execute the specified context; otherwise, <c>false</c>.</returns>
    public abstract bool CanExecute(DataContext context, [NotNull] Dictionary<string, string> parameters);

    /// <summary>Gets the shortcut.</summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Returns the shortcut.</returns>
    /// <value>The shortcut.</value>
    [NotNull]
    public abstract string GetShortcut([NotNull] Dictionary<string, string> parameters);

    #endregion
  }
}