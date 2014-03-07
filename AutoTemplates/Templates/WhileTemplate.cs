// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhileTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="WhileTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.AutoTemplates.Expressions;

  /// <summary>Defines the <see cref="WhileTemplate"/> class.</summary>
  public class WhileTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IWhileStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var whileStatement = statement as IWhileStatement;
      if (whileStatement == null)
      {
        return null;
      }

      var condition = whileStatement.Condition;
      if (condition == null)
      {
        return null;
      }

      var expressionDescriptor = ExpressionTemplateBuilder.Handle(condition, scope.ScopeParameters);
      if (expressionDescriptor != null)
      {
        return new StatementDescriptor(scope, string.Format("while ({0}) {{ $END$ }}", expressionDescriptor.Template), expressionDescriptor.TemplateVariables);
      }

      var result = new StatementDescriptor(scope)
      {
        Template = string.Format("while ({0}) {{ $END$ }}", condition.GetText())
      };

      string variableName;
      if (scope.ScopeParameters.TryGetValue("VariableName", out variableName))
      {
        result.Template = result.Template.Replace(variableName, "$VariableName$");
      }

      return result;
    }

    #endregion
  }
}