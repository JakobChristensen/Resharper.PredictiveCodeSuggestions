// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThisExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ThisExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="ThisExpression"/> class.</summary>
  public class ThisExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="thisExpression">The invoked expression.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IThisExpression thisExpression, Dictionary<string, string> statementParameters)
    {
      var name = thisExpression.Type().GetPresentableName(thisExpression.Language).Replace("<", string.Empty).Replace(">", string.Empty).Replace(".", string.Empty);

      var result = new ExpressionDescriptor
      {
        Template = "$" + name + "$"
      };

      result.TemplateVariables[name] = string.Format("variableOfType(\"{0}\")", thisExpression.Type().GetLongPresentableName(thisExpression.Language));

      return result;
    }

    #endregion
  }
}