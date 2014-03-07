// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvocationTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="InvocationTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.AutoTemplates.Expressions;

  /// <summary>Defines the <see cref="InvocationTemplate"/> class.</summary>
  public class InvocationTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IExpressionStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var expressionStatement = statement as IExpressionStatement;
      if (expressionStatement == null)
      {
        return null;
      }

      var invocationExpression = expressionStatement.Expression as IInvocationExpression;
      if (invocationExpression == null)
      {
        return null;
      }

      var expressionDescriptor = ExpressionTemplateBuilder.Handle(invocationExpression, scope.ScopeParameters);
      if (expressionDescriptor != null)
      {
        return new StatementDescriptor(scope, expressionDescriptor.Template + ";", expressionDescriptor.TemplateVariables);
      }

      return null;
    }

    #endregion
  }
}