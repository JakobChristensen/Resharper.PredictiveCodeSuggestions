// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalVariableTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="LocalVariableTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.AutoTemplates.Expressions;

  /// <summary>Defines the <see cref="LocalVariableTemplate"/> class.</summary>
  public class LocalVariableTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IDeclarationStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var declarationStatement = statement as IDeclarationStatement;
      if (declarationStatement == null)
      {
        return null;
      }

      var localVariableDeclarations = declarationStatement.VariableDeclarations;
      if (localVariableDeclarations.Count != 1)
      {
        return null;
      }

      var localVariableDeclaration = localVariableDeclarations.FirstOrDefault();
      if (localVariableDeclaration == null)
      {
        return null;
      }

      var expressionInitializer = localVariableDeclaration.Initializer as IExpressionInitializer;
      if (expressionInitializer == null)
      {
        return null;
      }

      var value = expressionInitializer.Value;
      if (value != null)
      {
        var expressionDescriptor = ExpressionTemplateBuilder.Handle(value, scope.ScopeParameters);
        if (expressionDescriptor != null)
        {
          var typeName = string.Empty;
          if (localVariableDeclaration.IsVar)
          {
            typeName += "var";
          }
          else
          {
            var type = localVariableDeclaration.Type;
            if (type != null && !type.IsUnknown && type.IsResolved)
            {
              typeName += type.GetPresentableName(localVariableDeclaration.Language);
            }
            else
            {
              var scalarTypeName = localVariableDeclaration.ScalarTypeName;
              if (scalarTypeName != null)
              {
                typeName += scalarTypeName.QualifiedName;
              }
              else
              {
                return null;
              }
            }
          }

          expressionDescriptor.Template = string.Format("{0} $name$ {1} {2};", typeName, localVariableDeclaration.AssignmentSign.GetTokenType().TokenRepresentation, expressionDescriptor.Template);
          expressionDescriptor.TemplateVariables["name"] = "suggestVariableName()";

          return new StatementDescriptor(scope, expressionDescriptor.Template, expressionDescriptor.TemplateVariables);
        }
      }

      var result = new StatementDescriptor(scope)
      {
        Template = declarationStatement.GetText()
      };

      string variableName;
      if (scope.ScopeParameters.TryGetValue("VariableName", out variableName))
      {
        result.Template = result.Template.Replace(variableName, "$VariableName$");
      }

      return result;
    }

    #endregion
  }
}