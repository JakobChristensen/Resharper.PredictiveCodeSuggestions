// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ReferenceExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="ReferenceExpression"/> class.</summary>
  public class ReferenceExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="referenceExpression">The invoked expression.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IReferenceExpression referenceExpression, Dictionary<string, string> statementParameters)
    {
      var result = new ExpressionDescriptor();

      var qualifierExpression = referenceExpression.QualifierExpression;
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

      if (!string.IsNullOrEmpty(result.Template))
      {
        result.Template += ".";
      }

      var resolveResult = referenceExpression.Reference.Resolve();
      var declaredElement = resolveResult.DeclaredElement;

      string qualifiedName = null;
      if (qualifierExpression == null && declaredElement != null)
      {
        var typeElement = declaredElement as ITypeElement;
        if (typeElement != null)
        {
          qualifiedName = typeElement.ShortName;

          var ns = typeElement.GetContainingNamespace();

          if (!string.IsNullOrEmpty(ns.QualifiedName))
          {
            qualifiedName = ns.QualifiedName + "." + qualifiedName;
          }
        }
      }

      if (string.IsNullOrEmpty(qualifiedName))
      {
        qualifiedName = referenceExpression.NameIdentifier.GetText();
      }

      string variableName;
      if (!statementParameters.TryGetValue("VariableName", out variableName))
      {
        variableName = string.Empty;
      }

      if (qualifiedName == variableName)
      {
        qualifiedName = "$VariableName$";
      }
      else
      {
        if (declaredElement != null)
        {
          var localVariable = declaredElement as ILocalVariable;
          if (localVariable != null)
          {
            if (localVariable.Type.IsUnknown)
            {
              var name = declaredElement.ShortName;
              qualifiedName = "$" + name + "$";
              result.TemplateVariables[name] = "suggestVariableName()";
            }
            else
            {
              var name = localVariable.Type.GetPresentableName(declaredElement.PresentationLanguage).Replace("<", string.Empty).Replace(">", string.Empty).Replace(".", string.Empty);
              qualifiedName = "$" + name + "$";
              result.TemplateVariables[name] = string.Format("variableOfType(\"{0}\")", localVariable.Type.GetLongPresentableName(declaredElement.PresentationLanguage));
            }
          }

          var parameter = declaredElement as IParameter;
          if (parameter != null)
          {
            if (parameter.Type.IsUnknown)
            {
              var name = declaredElement.ShortName;
              qualifiedName = "$" + name + "$";
              result.TemplateVariables[name] = "suggestVariableName()";
            }
            else
            {
              var name = parameter.Type.GetPresentableName(declaredElement.PresentationLanguage).Replace("<", string.Empty).Replace(">", string.Empty).Replace(".", string.Empty);
              qualifiedName = "$" + name + "$";
              result.TemplateVariables[name] = string.Format("variableOfType(\"{0}\")", parameter.Type.GetLongPresentableName(declaredElement.PresentationLanguage));
            }
          }
        }
      }

      result.Template += qualifiedName;

      return result;
    }

    #endregion
  }
}