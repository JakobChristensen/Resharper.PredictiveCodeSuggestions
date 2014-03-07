// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForEachTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ForEachTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Templates
{
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.AutoTemplates.Expressions;

  /// <summary>Defines the <see cref="ForEachTemplate"/> class.</summary>
  public class ForEachTemplate : StatementTemplate
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      var parent = statement.Parent;
      if (parent == null)
      {
        return false;
      }

      return parent is IForeachStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the string.</returns>
    public override StatementDescriptor Handle(IStatement statement, AutoTemplateScope scope)
    {
      var parent = statement.Parent;
      if (parent == null)
      {
        return null;
      }

      var foreachStatement = parent as IForeachStatement;
      if (foreachStatement == null)
      {
        return null;
      }

      var collection = foreachStatement.Collection;
      if (collection == null)
      {
        return null;
      }

      var iteratorDeclaration = foreachStatement.IteratorDeclaration;
      if (iteratorDeclaration == null)
      {
        return null;
      }

      var iterator = string.Empty;
      if (iteratorDeclaration.IsVar)
      {
        iterator += "var";
      }
      else
      {
        var type = iteratorDeclaration.Type;
        if (type == null || type.IsUnknown || !type.IsResolved)
        {
          return null;
        }

        iterator += type.GetLongPresentableName(statement.Language);
      }

      iterator += " $iterator$";

      var expressionDescriptor = ExpressionTemplateBuilder.Handle(collection, scope.ScopeParameters);
      if (expressionDescriptor != null)
      {
        var r = new StatementDescriptor(scope, string.Format("foreach ({0} in {1}) {{ $END$ }}", iterator, expressionDescriptor.Template), expressionDescriptor.TemplateVariables);
        r.TemplateVariables["iterator"] = "suggestVariableName()";
        return r;
      }

      var result = new StatementDescriptor(scope)
      {
        Template = string.Format("foreach ({0} in {1}) {{ $END$ }}", iterator, collection.GetText())
      };

      string variableName;
      if (scope.ScopeParameters.TryGetValue("VariableName", out variableName))
      {
        result.Template = result.Template.Replace(variableName, "$VariableName$");
      }

      result.TemplateVariables["iterator"] = "suggestVariableName()";

      return result;
    }

    #endregion
  }
}