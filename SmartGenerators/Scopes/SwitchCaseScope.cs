// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchCaseScope.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="SwitchCaseScope" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators.Scopes
{
  using System.Collections.Generic;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="SwitchCaseScope"/> class.</summary>
  public class SwitchCaseScope : ScopeBase
  {
    #region Public Methods

    /// <summary>Determines whether this instance can execute the specified context.</summary>
    /// <param name="context">The context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance can execute the specified context; otherwise, <c>false</c>.</returns>
    public override bool CanExecute(DataContext context, Dictionary<string, string> parameters)
    {
      return context.CanGenerateSwitchCase();
    }

    /// <summary>Gets the shortcut.</summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Returns the shortcut.</returns>
    public override string GetShortcut(Dictionary<string, string> parameters)
    {
      return "Switch case";
    }

    #endregion
  }
}