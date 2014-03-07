// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreationExpression.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="CreationExpression" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Expressions
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp.Tree;

  /// <summary>Defines the <see cref="CreationExpression"/> class.</summary>
  public class CreationExpression
  {
    #region Public Methods

    /// <summary>Handles the specified expression.</summary>
    /// <param name="creationExpression">The invocation expression.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns>Returns the string.</returns>
    public ExpressionDescriptor Handle(IObjectCreationExpression creationExpression, Dictionary<string, string> statementParameters)
    {
      string variableName;
      if (!statementParameters.TryGetValue("VariableName", out variableName))
      {
        variableName = string.Empty;
      }

      var typeName = creationExpression.TypeName;
      if (typeName == null)
      {
        return null;
      }

      var result = new ExpressionDescriptor
      {
        Template = creationExpression.NewKeyword.GetTokenType().TokenRepresentation + " "
      };

      var qualifiedName = string.Empty;

      var resolveResult = typeName.Reference.Resolve();
      if (resolveResult.IsValid())
      {
        var declaredElement = resolveResult.DeclaredElement;
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
        qualifiedName = typeName.QualifiedName;
      }

      result.Template += qualifiedName + "(";

      var index = 0;
      foreach (var argument in creationExpression.Arguments)
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