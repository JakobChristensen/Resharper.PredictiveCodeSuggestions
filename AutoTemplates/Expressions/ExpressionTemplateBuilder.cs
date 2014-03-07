// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionTemplateBuilder.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ExpressionTemplateBuilder" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="ExpressionTemplateBuilder"/> class.</summary>
  public static class ExpressionTemplateBuilder
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="expression">The expression.</param>
    /// <param name="scopeParameters">The scope parameters.</param>
    /// <returns>Returns the string.</returns>
    [CanBeNull]
    public static ExpressionDescriptor Handle(IExpression expression, Dictionary<string, string> scopeParameters)
    {
      var invocationExpression = expression as IInvocationExpression;
      if (invocationExpression != null)
      {
        return new InvocationExpression().Handle(invocationExpression, scopeParameters);
      }

      var referenceExpression = expression as IReferenceExpression;
      if (referenceExpression != null)
      {
        return new ReferenceExpression().Handle(referenceExpression, scopeParameters);
      }

      var binaryExpression = expression as IBinaryExpression;
      if (binaryExpression != null)
      {
        return new BinaryExpression().Handle(binaryExpression, scopeParameters);
      }

      var isExpression = expression as IIsExpression;
      if (isExpression != null)
      {
        return new IsExpression().Handle(isExpression, scopeParameters);
      }

      var asExpression = expression as IAsExpression;
      if (asExpression != null)
      {
        return new AsExpression().Handle(asExpression, scopeParameters);
      }

      var castExpression = expression as ICastExpression;
      if (castExpression != null)
      {
        return new CastExpression().Handle(castExpression, scopeParameters);
      }

      var unaryOperatorExpression = expression as IUnaryOperatorExpression;
      if (unaryOperatorExpression != null)
      {
        return new UnaryOperatorExpression().Handle(unaryOperatorExpression, scopeParameters);
      }

      var parenthesizedExpression = expression as IParenthesizedExpression;
      if (parenthesizedExpression != null)
      {
        return new ParenthesizedExpression().Handle(parenthesizedExpression, scopeParameters);
      }

      var thisExpression = expression as IThisExpression;
      if (thisExpression != null)
      {
        return new ThisExpression().Handle(thisExpression, scopeParameters);
      }

      var creationExpression = expression as IObjectCreationExpression;
      if (creationExpression != null)
      {
        return new CreationExpression().Handle(creationExpression, scopeParameters);
      }

      var result = new ExpressionDescriptor
      {
        Template = expression.GetText()
      };

      return result;
    }

    #endregion
  }
}