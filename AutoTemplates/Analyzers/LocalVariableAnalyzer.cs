// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalVariableAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="LocalVariableAnalyzer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Analyzers
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="LocalVariableAnalyzer"/> class.</summary>
  public class LocalVariableAnalyzer : StatementAnalyzer
  {
    #region Public Methods

    /// <summary>Determines whether this instance [can generate invocation] the specified parameters.</summary>
    /// <param name="declarationStatement">The expression statement.</param>
    /// <param name="scopeParameters">The parameters.</param>
    /// <returns><c>true</c> if this instance [can generate invocation] the specified parameters; otherwise, <c>false</c>.</returns>
    public static bool HandleLocalVariable([NotNull] IDeclarationStatement declarationStatement, [NotNull] Dictionary<string, string> scopeParameters)
    {
      if (declarationStatement == null)
      {
        throw new ArgumentNullException("declarationStatement");
      }

      if (scopeParameters == null)
      {
        throw new ArgumentNullException("scopeParameters");
      }

      var localVariableDeclarations = declarationStatement.VariableDeclarations;
      if (localVariableDeclarations.Count != 1)
      {
        return false;
      }

      var localVariableDeclaration = localVariableDeclarations.FirstOrDefault();
      if (localVariableDeclaration == null)
      {
        return false;
      }

      var type = localVariableDeclaration.Type;
      if (!type.IsResolved || type.IsUnknown)
      {
        return false;
      }

      var initial = localVariableDeclaration.Initial;
      if (initial == null)
      {
        return false;
      }

      var value = initial.GetText();

      var localVariable = localVariableDeclaration as ILocalVariable;
      if (localVariable == null)
      {
        return false;
      }

      scopeParameters["VariableName"] = localVariable.ShortName;
      scopeParameters["VariableType"] = type.GetPresentableName(localVariable.PresentationLanguage);
      scopeParameters["FullName"] = type.GetLongPresentableName(localVariable.PresentationLanguage);
      scopeParameters["Value"] = value;

      return true;
    }

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IDeclarationStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the I enumerable.</returns>
    public override AutoTemplateScope Handle(IStatement statement)
    {
      var expression = statement as IDeclarationStatement;
      if (expression == null)
      {
        return null;
      }

      var scopeParameters = new Dictionary<string, string>();
      if (!HandleLocalVariable(expression, scopeParameters))
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