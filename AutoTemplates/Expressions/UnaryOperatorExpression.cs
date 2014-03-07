// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnaryOperatorExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="UnaryOperatorExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="UnaryOperatorExpression"/> class.</summary>
  public class UnaryOperatorExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="unaryOperatorExpression">The invocation expression.</param>
    /// <param name="scopeParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IUnaryOperatorExpression unaryOperatorExpression, Dictionary<string, string> scopeParameters)
    {
      var left = unaryOperatorExpression.Operand;
      if (left == null)
      {
        return null;
      }

      var statement = ExpressionTemplateBuilder.Handle(left, scopeParameters);
      if (statement == null)
      {
        return null;
      }

      var result = new ExpressionDescriptor
      {
        Template = string.Format("{0}{1}", unaryOperatorExpression.OperatorSign.GetTokenType().TokenRepresentation, statement.Template)
      };

      foreach (var variable in statement.TemplateVariables)
      {
        result.TemplateVariables[variable.Key] = variable.Value;
      }

      return result;
    }

    #endregion
  }
}