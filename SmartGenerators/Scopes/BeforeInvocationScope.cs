// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforeInvocationScope.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="BeforeInvocationScope" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators.Scopes
{
  using System.Collections.Generic;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="BeforeInvocationScope"/> class.</summary>
  public class BeforeInvocationScope : ScopeBase
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

      return context.CanGenerateInvocation(parameters, statement);
    }

    /// <summary>Gets the shortcut.</summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>Returns the shortcut.</returns>
    public override string GetShortcut(Dictionary<string, string> parameters)
    {
      string methodName;
      if (!parameters.TryGetValue("FullName", out methodName))
      {
        methodName = "(unknown method)";
      }

      return string.Format("Before call to \"{0}\"", methodName);
    }

    #endregion
  }
}