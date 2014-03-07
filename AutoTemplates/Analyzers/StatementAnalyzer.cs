// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatementAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="StatementAnalyzer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Analyzers
{
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="StatementAnalyzer"/> class.</summary>
  public abstract class StatementAnalyzer
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public abstract bool CanHandle([NotNull] IStatement statement);

    /// <summary>Gets the statement descriptors.</summary>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the statement descriptors.</returns>
    public virtual IEnumerable<StatementDescriptor> GetStatementDescriptors(AutoTemplateScope scope)
    {
      foreach (var smartTemplateDescriptor in this.ProcessTemplate(scope))
      {
        yield return smartTemplateDescriptor;
      }
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the I enumerable.</returns>
    [CanBeNull]
    public abstract AutoTemplateScope Handle([NotNull] IStatement statement);

    #endregion

    #region Methods

    /// <summary>Processes the template.</summary>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the template.</returns>
    protected IEnumerable<StatementDescriptor> ProcessTemplate(AutoTemplateScope scope)
    {
      var nextStatement = scope.NextStatement;
      if (nextStatement == null)
      {
        yield break;
      }

      foreach (var statementTemplate in AutoTemplateManager.StatementTemplates)
      {
        if (!statementTemplate.CanHandle(nextStatement))
        {
          continue;
        }

        var statementDescriptor = statementTemplate.Handle(nextStatement, scope);
        if (statementDescriptor == null)
        {
          continue;
        }

        yield return statementDescriptor;
      }
    }

    #endregion
  }
}