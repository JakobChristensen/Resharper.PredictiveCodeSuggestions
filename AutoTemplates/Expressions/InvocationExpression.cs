// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvocationExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="InvocationExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="InvocationExpression"/> class.</summary>
  public class InvocationExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="invocationExpression">The invocation expression.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IInvocationExpression invocationExpression, Dictionary<string, string> statementParameters)
    {
      var invokedExpression = invocationExpression.InvokedExpression as IReferenceExpression;
      if (invokedExpression == null)
      {
        return null;
      }

      var result = new ExpressionDescriptor();

      var qualifierExpression = invokedExpression.QualifierExpression;
      if (qualifierExpression != null)
      {
        var descriptor = ExpressionTemplateBuilder.Handle(qualifierExpression, statementParameters);

        if (descriptor != null)
        {
          result.Template = descriptor.Template;

          foreach (var variable in descriptor.TemplateVariables)
          {
            result.TemplateVariables[variable.Key] = variable.Value;
          }
        }
        else
        {
          result.Template += qualifierExpression.GetText();
        }
      }

      string variableName;
      if (!statementParameters.TryGetValue("VariableName", out variableName))
      {
        variableName = string.Empty;
      }

      if (!string.IsNullOrEmpty(result.Template))
      {
        result.Template += ".";
      }

      result.Template += invokedExpression.NameIdentifier.GetText() + "(";

      var index = 0;
      foreach (var argument in invocationExpression.Arguments)
      {
        var parameter = argument.MatchingParameter;
        if (parameter == null)
        {
          continue;
        }

        var parameterName = parameter.Element.ShortName;

        if (index > 0)
        {
          result.Template += ", ";
        }

        index++;

        var argumentText = argument.GetText();

        if (argumentText == variableName)
        {
          result.Template += "$VariableName$";
          continue;
        }

        /*
        if (argument.Value.IsConstantValue())
        {
          var constantValue = argument.Value.ConstantValue;
          result.Template += "$" + parameterName + "$";

          string s;
          if (constantValue.IsPureNull(argument.Language))
          {
            s = "null";
          }
          else
          {
            s = constantValue.Value.ToString().Replace(",", "&#44;").Replace(",", "&quot;");
            if (constantValue.IsString())
            {
              s = "\"" + s + "\"";
            }
          }

          result.TemplateVariables[parameterName] = "c:" + s;
          continue;
        }
        */
        result.Template += "$" + parameterName + "$";
        result.TemplateVariables[parameterName] = string.Empty;
      }

      result.Template += ")";

      return result;
    }

    #endregion
  }
}