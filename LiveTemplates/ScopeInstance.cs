// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScopeInstance.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ScopeInstance" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.LiveTemplates
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="ScopeInstance"/> class.</summary>
  public class ScopeInstance
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ScopeInstance"/> class.</summary>
    /// <param name="scope">The scope.</param>
    /// <param name="parameters">The parameters.</param>
    public ScopeInstance([NotNull] ScopeBase scope, [NotNull] Dictionary<string, string> parameters)
    {
      if (scope == null)
      {
        throw new ArgumentNullException("scope");
      }

      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      this.Scope = scope;
      this.Parameters = parameters;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <value>The parameters.</value>
    [NotNull]
    public Dictionary<string, string> Parameters { get; private set; }

    /// <summary>
    /// Gets the scope.
    /// </summary>
    /// <value>The scope.</value>
    [NotNull]
    public ScopeBase Scope { get; private set; }

    /// <summary>
    /// Gets the shortcut.
    /// </summary>
    [NotNull]
    public string Shortcut
    {
      get
      {
        return this.Scope.GetShortcut(this.Parameters);
      }
    }

    #endregion
  }
}