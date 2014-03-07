// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatementTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="StatementTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="StatementTemplate"/> class.</summary>
  public abstract class StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public abstract bool CanHandle([NotNull] IStatement statement);

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public abstract StatementDescriptor Handle([NotNull] IStatement statement, [NotNull] AutoTemplateScope scope);

    #endregion
  }
}