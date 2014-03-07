// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfNullAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="IfNullAnalyzer" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates.Analyzers
{
  using System.Collections.Generic;
  using JetBrains.DocumentModel;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="IfNullAnalyzer"/> class.</summary>
  public class IfNullAnalyzer : BlockStatementAnalyzer
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

      var equalityExpression = ifStatement.Condition as IEqualityExpression;
      if (equalityExpression == null)
      {
        return false;
      }

      var operand = equalityExpression.RightOperand;
      if (operand == null)
      {
        return false;
      }

      if (operand.GetText() == "null")
      {
        return true;
      }

      operand = equalityExpression.LeftOperand;
      if (operand == null)
      {
        return false;
      }

      return operand.GetText() == "null";
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

      var equalityExpression = ifStatement.Condition as IEqualityExpression;
      if (equalityExpression == null)
      {
        return null;
      }

      var operand = equalityExpression.RightOperand;
      if (operand == null)
      {
        return null;
      }

      if (operand.GetText() == "null")
      {
        operand = equalityExpression.LeftOperand;
      }

      if (operand == null)
      {
        return null;
      }

      var type = operand.Type();
      if (type.IsUnknown || !type.IsResolved)
      {
        return null;
      }

      var equalityType = equalityExpression.EqualityType == EqualityExpressionType.EQEQ ? "is" : "is not";

      var typeName = type.GetLongPresentableName(statement.Language);

      var scopeParameters = new Dictionary<string, string>();
      scopeParameters["VariableType"] = type.GetPresentableName(statement.Language);
      scopeParameters["FullName"] = type.GetLongPresentableName(statement.Language);

      var key = string.Format("If expression of type \"{0}\" {1} null", typeName, equalityType);

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