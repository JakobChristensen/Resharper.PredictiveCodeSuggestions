// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssignmentAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AssignmentAnalyzer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Analyzers
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="AssignmentAnalyzer"/> class.</summary>
  public class AssignmentAnalyzer : StatementAnalyzer
  {
    #region Public Methods

    /// <summary>Determines whether this instance [can generate invocation] the specified parameters.</summary>
    /// <param name="assignmentExpression">The expression statement.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance [can generate invocation] the specified parameters; otherwise, <c>false</c>.</returns>
    public static bool HandleAssignment([NotNull] IAssignmentExpression assignmentExpression, [NotNull] Dictionary<string, string> parameters)
    {
      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      if (assignmentExpression == null)
      {
        throw new ArgumentNullException("assignmentExpression");
      }

      var reference = assignmentExpression.Dest as IReferenceExpression;
      if (reference == null)
      {
        return false;
      }

      var source = assignmentExpression.Source;
      if (source == null)
      {
        return false;
      }

      if (!source.Type().IsResolved || source.Type().IsUnknown)
      {
        return false;
      }

      parameters["VariableName"] = reference.GetText();
      parameters["VariableType"] = reference.Type().GetPresentableName(reference.Language);
      parameters["FullName"] = reference.Type().GetLongPresentableName(reference.Language);
      parameters["Value"] = source.GetText();

      return true;
    }

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      var expressionStatement = statement as IExpressionStatement;
      if (expressionStatement == null)
      {
        return false;
      }

      return expressionStatement.Expression is IAssignmentExpression;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the I enumerable.</returns>
    public override AutoTemplateScope Handle(IStatement statement)
    {
      var expressionStatement = statement as IExpressionStatement;
      if (expressionStatement == null)
      {
        return null;
      }

      var assignmentExpression = expressionStatement.Expression as IAssignmentExpression;
      if (assignmentExpression == null)
      {
        return null;
      }

      var scopeParameters = new Dictionary<string, string>();
      if (!HandleAssignment(assignmentExpression, scopeParameters))
      {
        return null;
      }

      string variableType;
      if (!scopeParameters.TryGetValue("FullName", out variableType))
      {
        variableType = "(unknown variable)";
      }

      var key = string.Format("After variable of type \"{0}\"", variableType);

      return new AutoTemplateScope(statement, key, scopeParameters);
    }

    #endregion
  }
}