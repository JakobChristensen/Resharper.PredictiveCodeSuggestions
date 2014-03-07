// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvocationAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="InvocationAnalyzer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Analyzers
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CodeAnnotations;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="InvocationAnalyzer"/> class.</summary>
  public class InvocationAnalyzer : StatementAnalyzer
  {
    #region Public Methods

    /// <summary>Determines whether this instance [can generate invocation] the specified parameters.</summary>
    /// <param name="expressionStatement">The expression statement.</param>
    /// <param name="statementParameters">The parameters.</param>
    /// <returns><c>true</c> if this instance [can generate invocation] the specified parameters; otherwise, <c>false</c>.</returns>
    public static bool HandleInvocation([NotNull] IExpressionStatement expressionStatement, [NotNull] Dictionary<string, string> statementParameters)
    {
      if (statementParameters == null)
      {
        throw new ArgumentNullException("statementParameters");
      }

      if (expressionStatement == null)
      {
        throw new ArgumentNullException("expressionStatement");
      }

      var invocationExpression = expressionStatement.Expression as IInvocationExpression;
      if (invocationExpression == null)
      {
        return false;
      }

      var invokedExpression = invocationExpression.InvokedExpression as IReferenceExpression;
      if (invokedExpression == null)
      {
        return false;
      }

      var qualifierExpression = invokedExpression.QualifierExpression;
      if (qualifierExpression == null)
      {
        return false;
      }

      var resolveResult = invokedExpression.Reference.Resolve();

      var m = resolveResult.DeclaredElement as IMethod;
      if (m == null)
      {
        return false;
      }

      statementParameters["VariableName"] = qualifierExpression.GetText();
      statementParameters["MethodName"] = m.ShortName;
      statementParameters["QualifiedMethodName"] = m.ShortName;

      var fullName = m.ShortName;

      var classDeclaration = m.GetContainingType();
      if (classDeclaration != null)
      {
        statementParameters["ContainingType"] = classDeclaration.ShortName;
        fullName = classDeclaration.ShortName + "." + m.ShortName;
        statementParameters["QualifiedMethodName"] = fullName;

        var ns = classDeclaration.GetContainingNamespace();
        fullName = ns.QualifiedName + "." + fullName;
      }

      statementParameters["FullName"] = fullName;

      var cache = m.GetPsiServices().GetCodeAnnotationsCache();
      if (cache != null)
      {
        if (cache.IsAssertionMethod(m))
        {
          statementParameters["IsAssertion"] = "true";
        }
      }

      return true;
    }

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IExpressionStatement;
    }

    /// <summary>Gets the templates.</summary>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the templates.</returns>
    public override IEnumerable<StatementDescriptor> GetStatementDescriptors(AutoTemplateScope scope)
    {
      foreach (var smartTemplateDescriptor in this.ProcessTemplate(scope))
      {
        yield return smartTemplateDescriptor;
      }
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the I enumerable.</returns>
    public override AutoTemplateScope Handle(IStatement statement)
    {
      var expression = statement as IExpressionStatement;
      if (expression == null)
      {
        return null;
      }

      var scopeParameters = new Dictionary<string, string>();
      if (!HandleInvocation(expression, scopeParameters))
      {
        return null;
      }

      string isAssertion;
      if (scopeParameters.TryGetValue("IsAssertion", out isAssertion))
      {
        if (isAssertion == "true")
        {
          return null;
        }
      }

      string methodName;
      if (!scopeParameters.TryGetValue("FullName", out methodName))
      {
        methodName = "(unknown method)";
      }

      var key = string.Format("After call to \"{0}\"", methodName);

      return new AutoTemplateScope(statement, key, scopeParameters);
    }

    #endregion
  }
}