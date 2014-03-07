// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsingAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="UsingAnalyzer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Analyzers
{
  using System.Collections.Generic;
  using JetBrains.DocumentModel;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="UsingAnalyzer"/> class.</summary>
  public class UsingAnalyzer : BlockStatementAnalyzer
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      return statement is IUsingStatement;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the I enumerable.</returns>
    public override AutoTemplateScope Handle(IStatement statement)
    {
      var usingStatement = statement as IUsingStatement;
      if (usingStatement == null)
      {
        return null;
      }

      var declarations = usingStatement.VariableDeclarations;
      if (declarations.Count != 1)
      {
        return null;
      }

      var localVariableDeclaration = declarations.FirstOrDefault();
      if (localVariableDeclaration == null)
      {
        return null;
      }

      var type = localVariableDeclaration.Type;
      if (!type.IsResolved || type.IsUnknown)
      {
        return null;
      }

      var localVariable = localVariableDeclaration as ILocalVariable;
      if (localVariable == null)
      {
        return null;
      }

      var scopeParameters = new Dictionary<string, string>();

      scopeParameters["VariableName"] = localVariable.ShortName;
      scopeParameters["VariableType"] = type.GetPresentableName(statement.Language);
      scopeParameters["FullName"] = type.GetLongPresentableName(statement.Language);

      var key = string.Format("Inside using of type \"{0}\"", type.GetLongPresentableName(statement.Language));

      var result = new AutoTemplateScope(statement, key, scopeParameters)
      {
        GetNextStatement = this.GetInnerStatement, 
        GetInsertPosition = this.GetInsertPosition
      };

      return result;
    }

    #endregion

    #region Methods

    /// <summary>Gets the inner statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the inner statement.</returns>
    protected IStatement GetInnerStatement(IStatement statement)
    {
      var foreachStatement = statement as IUsingStatement;
      if (foreachStatement == null)
      {
        return null;
      }

      var innerStatement = foreachStatement.Body;
      if (innerStatement == null)
      {
        return null;
      }

      var block = innerStatement as IBlock;
      if (block != null)
      {
        innerStatement = block.Statements.FirstOrDefault();
      }

      return innerStatement;
    }

    /// <summary>Gets the insert position.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the insert position.</returns>
    protected DocumentRange GetInsertPosition(IStatement statement)
    {
      var foreachStatement = statement as IUsingStatement;
      if (foreachStatement == null)
      {
        return DocumentRange.InvalidRange;
      }

      var factory = CSharpElementFactory.GetInstance(statement.GetPsiModule());

      var body = foreachStatement.Body;
      if (body == null)
      {
        foreachStatement.SetBody(factory.CreateEmptyBlock());
      }

      var block = foreachStatement.Body as IBlock;
      if (block == null)
      {
        block = factory.CreateEmptyBlock();
        block.AddStatementAfter(foreachStatement.Body, null);
        foreachStatement.SetBody(factory.CreateEmptyBlock());
      }

      return block.LBrace.GetDocumentRange().EndOffsetRange();
    }

    #endregion
  }
}