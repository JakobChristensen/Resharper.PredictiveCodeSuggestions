// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforeAssignmentExpressionScope.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="BeforeAssignmentExpressionScope" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators.Scopes
{
  using System.Collections.Generic;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="BeforeAssignmentExpressionScope"/> class.</summary>
  public class BeforeAssignmentExpressionScope : ScopeBase
  {
    #region Public Methods

    /// <summary>Determines whether this instance can execute the specified context.</summary>
    /// <param name="context">The context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance can execute the specified context; otherwise, <c>false</c>.</returns>
    public override bool CanExecute(DataContext context, Dictionary<string, string> parameters)
    {
      var statement = context.GetNextStatement();
      if (statement == null)
      {
        return false;
      }

      return context.GetAssignmentExpression(parameters, statement);
    }

    /// <summary>Gets the shortcut.</summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Returns the shortcut.</returns>
    /// <value>The shortcut.</value>
    public override string GetShortcut(Dictionary<string, string> parameters)
    {
      string variableType;
      if (!parameters.TryGetValue("FullName", out variableType))
      {
        variableType = "(unknown variable)";
      }

      return string.Format("After variable of type \"{0}\"", variableType);
    }

    #endregion
  }
}