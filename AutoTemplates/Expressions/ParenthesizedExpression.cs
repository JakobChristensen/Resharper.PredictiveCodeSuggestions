// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParenthesizedExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ParenthesizedExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="ParenthesizedExpression"/> class.</summary>
  public class ParenthesizedExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="parenthesizedExpression">The invocation expression.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IParenthesizedExpression parenthesizedExpression, Dictionary<string, string> statementParameters)
    {
      var expression = parenthesizedExpression.Expression;
      if (expression == null)
      {
        return null;
      }

      var statement = ExpressionTemplateBuilder.Handle(expression, statementParameters);
      if (statement == null)
      {
        return null;
      }

      var result = new ExpressionDescriptor
      {
        Template = string.Format("({0})", statement.Template)
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