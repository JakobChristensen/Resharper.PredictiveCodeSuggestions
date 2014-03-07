// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScopeManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ScopeManager" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="ScopeManager"/> class.</summary>
  public static class ScopeManager
  {
    #region Constants and Fields

    /// <summary>The scopes field.</summary>
    private static readonly List<ScopeBase> scopes = new List<ScopeBase>();

    #endregion

    #region Public Methods

    /// <summary>Gets the scopes.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the scopes.</returns>
    public static IEnumerable<ScopeInstance> GetScopes([NotNull] DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      foreach (var scope in scopes)
      {
        var parameters = new Dictionary<string, string>();

        if (scope.CanExecute(context, parameters))
        {
          yield return new ScopeInstance(scope, parameters);
        }
      }
    }

    /// <summary>Registers the specified scope.</summary>
    /// <param name="scope">The scope.</param>
    public static void Register([NotNull] ScopeBase scope)
    {
      if (scope == null)
      {
        throw new ArgumentNullException("scope");
      }

      scopes.Add(scope);
    }

    #endregion
  }
}