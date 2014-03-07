// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="IsExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="IsExpression"/> class.</summary>
  public class IsExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="isExpression">The invocation expression.</param>
    /// <param name="scopeParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IIsExpression isExpression, Dictionary<string, string> scopeParameters)
    {
      var operand = isExpression.Operand;
      if (operand == null)
      {
        return null;
      }

      var type = isExpression.TypeOperand;
      if (type == null)
      {
        return null;
      }

      var expressionDescriptor = ExpressionTemplateBuilder.Handle(operand, scopeParameters);
      if (expressionDescriptor == null)
      {
        return null;
      }

      var result = new ExpressionDescriptor
      {
        Template = string.Format("{0} {1} {2}", expressionDescriptor.Template, isExpression.OperatorSign.GetTokenType().TokenRepresentation, type.GetText())
      };

      foreach (var variable in expressionDescriptor.TemplateVariables)
      {
        result.TemplateVariables[variable.Key] = variable.Value;
      }

      return result;
    }

    #endregion
  }
}