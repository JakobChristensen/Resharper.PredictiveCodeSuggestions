// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CastExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="CastExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="CastExpression"/> class.</summary>
  public class CastExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="castExpression">As expression.</param>
    /// <param name="scopeParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(ICastExpression castExpression, Dictionary<string, string> scopeParameters)
    {
      var operand = castExpression.Op;
      if (operand == null)
      {
        return null;
      }

      var type = castExpression.TargetType;
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
        Template = string.Format("({0}){1}", type.GetText(), expressionDescriptor.Template)
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