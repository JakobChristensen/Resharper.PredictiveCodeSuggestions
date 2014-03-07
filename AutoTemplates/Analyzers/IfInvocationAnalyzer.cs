// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfInvocationAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="IfInvocationAnalyzer" /> class.
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

  /// <summary>Defines the <see cref="IfInvocationAnalyzer"/> class.</summary>
  public class IfInvocationAnalyzer : BlockStatementAnalyzer
  {
    #region Public Methods

    /// <summary>Determines whether this instance can handle the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance can handle the specified statement; otherwise, <c>false</c>.</returns>
    public override bool CanHandle(IStatement statement)
    {
      var ifStatement = statement as IIfStatement;
      if (ifStatement == null)
      {
        return false;
      }

      var referenceExpression = ifStatement.Condition as IReferenceExpression;
      if (referenceExpression == null)
      {
        return false;
      }

      var resolveResult = referenceExpression.Reference.Resolve();
      if (!resolveResult.IsValid())
      {
        return false;
      }

      var declaredElement = resolveResult.DeclaredElement as ITypeMember;
      if (declaredElement == null)
      {
        return false;
      }

      return true;
    }

    /// <summary>Handles the specified statement.</summary>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns the I enumerable.</returns>
    public override AutoTemplateScope Handle(IStatement statement)
    {
      var ifStatement = statement as IIfStatement;
      if (ifStatement == null)
      {
        return null;
      }

      var referenceExpression = ifStatement.Condition as IReferenceExpression;
      if (referenceExpression == null)
      {
        return null;
      }

      var resolveResult = referenceExpression.Reference.Resolve();
      if (!resolveResult.IsValid())
      {
        return null;
      }

      var declaredElement = resolveResult.DeclaredElement as ITypeMember;
      if (declaredElement == null)
      {
        return null;
      }

      var name = declaredElement.ShortName;

      var type = declaredElement.GetContainingType();
      if (type != null)
      {
        var ns = type.GetContainingNamespace();
        if (ns.QualifiedName != null)
        {
          name = type.ShortName + "." + name;
        }
        else
        {
          name = ns.QualifiedName + "." + type.ShortName + "." + name;
        }
      }

      var scopeParameters = new Dictionary<string, string>();

      var key = string.Format("If expression \"{0}\"", name);

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
      var foreachStatement = statement as IIfStatement;
      if (foreachStatement == null)
      {
        return null;
      }

      var innerStatement = foreachStatement.Then;
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
      var foreachStatement = statement as IIfStatement;
      if (foreachStatement == null)
      {
        return DocumentRange.InvalidRange;
      }

      var factory = CSharpElementFactory.GetInstance(statement.GetPsiModule());

      var body = foreachStatement.Then;
      if (body == null)
      {
        foreachStatement.SetThen(factory.CreateEmptyBlock());
      }

      var block = foreachStatement.Then as IBlock;
      if (block == null)
      {
        block = factory.CreateEmptyBlock();
        block.AddStatementAfter(foreachStatement.Then, null);
        foreachStatement.SetThen(factory.CreateEmptyBlock());
      }

      return block.LBrace.GetDocumentRange().EndOffsetRange();
    }

    #endregion
  }
}