// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="BinaryExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="BinaryExpression"/> class.</summary>
  public class BinaryExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="binaryExpression">The invocation expression.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IBinaryExpression binaryExpression, Dictionary<string, string> statementParameters)
    {
      var left = binaryExpression.LeftOperand;
      if (left == null)
      {
        return null;
      }

      var right = binaryExpression.RightOperand;
      if (right == null)
      {
        return null;
      }

      var leftStatement = ExpressionTemplateBuilder.Handle(left, statementParameters);
      if (leftStatement == null)
      {
        return null;
      }

      var rightStatement = ExpressionTemplateBuilder.Handle(right, statementParameters);
      if (rightStatement == null)
      {
        return null;
      }

      var result = new ExpressionDescriptor
      {
        Template = string.Format("{0} {1} {2}", leftStatement.Template, binaryExpression.OperatorSign.GetTokenType().TokenRepresentation, rightStatement.Template)
      };

      foreach (var variable in rightStatement.TemplateVariables)
      {
        result.TemplateVariables[variable.Key] = variable.Value;
      }

      foreach (var variable in leftStatement.TemplateVariables)
      {
        result.TemplateVariables[variable.Key] = variable.Value;
      }

      return result;
    }

    #endregion
  }
}