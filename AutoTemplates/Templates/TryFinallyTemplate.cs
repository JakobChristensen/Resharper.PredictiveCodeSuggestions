// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryFinallyTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="TryFinallyTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="TryFinallyTemplate"/> class.</summary>
  public class TryFinallyTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      var tryStatement = statement as ITryStatement;
      if (tryStatement == null)
      {
        return false;
      }

      return tryStatement.FinallyBlock != null && tryStatement.Catches.Count == 0;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var result = new StatementDescriptor(scope)
      {
        Template = string.Format("try {{ $END$ }} finally {{ }}")
      };

      return result;
    }

    #endregion
  }
}