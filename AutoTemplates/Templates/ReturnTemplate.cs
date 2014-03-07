// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ReturnTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.AutoTemplates.Expressions;

  /// <summary>Defines the <see cref="ReturnTemplate"/> class.</summary>
  public class ReturnTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IReturnStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var returnStatement = statement as IReturnStatement;
      if (returnStatement == null)
      {
        return null;
      }

      var value = returnStatement.Value;
      if (value == null)
      {
        return new StatementDescriptor(scope)
        {
          Template = "return;"
        };
      }

      var expressionDescriptor = ExpressionTemplateBuilder.Handle(value, scope.ScopeParameters);
      if (expressionDescriptor == null)
      {
        return null;
      }

      return new StatementDescriptor(scope, string.Format("return {0}", expressionDescriptor.Template), expressionDescriptor.TemplateVariables);
    }

    #endregion
  }
}