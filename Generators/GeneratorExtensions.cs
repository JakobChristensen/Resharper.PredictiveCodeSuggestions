// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorExtensions.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="GeneratorExtensions" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Metadata.Reader.API;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.AutoTemplates.Analyzers;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="GeneratorExtensions"/> class.</summary>
  public static class GeneratorExtensions
  {
    #region Public Methods

    /// <summary>Determines whether this instance [can generate assert] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if this instance [can generate assert] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateAssert(this DataContext dataContext)
    {
      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var functionDeclaration = treeNode.GetContainingNode<ICSharpFunctionDeclaration>(true);
      if (functionDeclaration == null)
      {
        return false;
      }

      var block = functionDeclaration.Body;
      if (block == null)
      {
        return false;
      }

      var statements = block.Statements;
      if (statements.Count <= 0)
      {
        return true;
      }

      var statement = statements[0];
      var range = statement.GetDocumentRange();

      return treeNode.GetTreeTextRange().StartOffset.Offset <= range.TextRange.StartOffset;
    }

    /// <summary>Determines whether this instance [can generate class member] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance [can generate class member] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateClassMember([NotNull] this DataContext dataContext, [NotNull] Dictionary<string, string> parameters)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var classDeclaration = treeNode.GetContainingNode<IClassDeclaration>(true);
      if (classDeclaration == null)
      {
        return false;
      }

      var memberDeclaration = treeNode.GetContainingNode<IClassMemberDeclaration>(true);
      if (memberDeclaration != null && !(memberDeclaration is IClassDeclaration))
      {
        return false;
      }

      parameters["Modifier"] = GetModifier(treeNode, classDeclaration);

      return true;
    }

    /// <summary>Determines whether this instance [can generate empty file] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance [can generate empty file] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateEmptyFile([NotNull] this DataContext dataContext, [NotNull] Dictionary<string, string> parameters)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var namespaceDeclaration = treeNode.GetContainingNode<INamespaceDeclaration>(true);
      if (namespaceDeclaration != null)
      {
        return false;
      }

      var file = treeNode.GetContainingFile();
      if (file == null)
      {
        return false;
      }

      var sourceFile = file.GetSourceFile();
      if (sourceFile == null)
      {
        return false;
      }

      parameters["FileName"] = sourceFile.Name;

      return true;
    }

    /// <summary>Determines whether [is class member] [the specified data context].</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if [is class member] [the specified data context]; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateInsideMethod([NotNull] this DataContext dataContext, [NotNull] Dictionary<string, string> parameters)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var methodDeclaration = treeNode.GetContainingNode<IMethodDeclaration>(true);

      return methodDeclaration != null;
    }

    /// <summary>Determines whether this instance [can generate interface member] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if this instance [can generate interface member] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateInterfaceMember([NotNull] this DataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var interfaceDeclaration = treeNode.GetContainingNode<IInterfaceDeclaration>(true);
      if (interfaceDeclaration == null)
      {
        return false;
      }

      var memberDeclaration = treeNode.GetContainingNode<ITypeMemberDeclaration>(true);

      return memberDeclaration == null || (memberDeclaration is IInterfaceDeclaration);
    }

    /// <summary>Determines whether this instance [can generate after invocation] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance [can generate after invocation] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateInvocation([NotNull] this DataContext dataContext, [NotNull] Dictionary<string, string> parameters, [NotNull] IStatement statement)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      if (statement == null)
      {
        throw new ArgumentNullException("statement");
      }

      var stmt = statement;

      var expressionStatement = stmt as IExpressionStatement;
      if (expressionStatement == null)
      {
        return false;
      }

      return InvocationAnalyzer.HandleInvocation(expressionStatement, parameters);
    }

    /// <summary>Determines whether [is after last statement] [the specified data context].</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if [is after last statement] [the specified data context]; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateReturn(this DataContext dataContext)
    {
      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var block = treeNode.GetContainingNode<IBlock>(true);
      if (block == null)
      {
        return false;
      }

      if (block.Statements.Count <= 0)
      {
        return true;
      }

      var statement = block.Statements[block.Statements.Count - 1];
      var range = statement.GetDocumentRange();

      var end = range.TextRange.StartOffset + range.TextRange.Length;

      return end <= treeNode.GetTreeTextRange().StartOffset.Offset;
    }

    /// <summary>Determines whether this instance [can generate struct member] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns><c>true</c> if this instance [can generate struct member] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateStructMember([NotNull] this DataContext dataContext, [NotNull] Dictionary<string, string> parameters)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      if (parameters == null)
      {
        throw new ArgumentNullException("parameters");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var classDeclaration = treeNode.GetContainingNode<IStructDeclaration>(true);
      if (classDeclaration == null)
      {
        return false;
      }

      var memberDeclaration = treeNode.GetContainingNode<IClassMemberDeclaration>(true);
      if (memberDeclaration != null && !(memberDeclaration is IStructDeclaration))
      {
        return false;
      }

      parameters["Modifier"] = GetModifier(treeNode, classDeclaration);

      return true;
    }

    /// <summary>Determines whether this instance [can generate switch case] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if this instance [can generate switch case] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateSwitchCase([NotNull] this DataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var switchStatement = treeNode.GetContainingNode<ISwitchStatement>();
      if (switchStatement == null)
      {
        return false;
      }

      var block = switchStatement.Block;
      if (block == null)
      {
        return false;
      }

      return treeNode.Parent == block;
    }

    /// <summary>Determines whether this instance [can generate type] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if this instance [can generate type] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool CanGenerateType([NotNull] this DataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var treeNode = dataContext.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return false;
      }

      var classLikeDeclaration = treeNode.GetContainingNode<IClassLikeDeclaration>(true);
      if (classLikeDeclaration != null)
      {
        return false;
      }

      var enumDecl = treeNode.GetContainingNode<IEnumDeclaration>(true);
      if (enumDecl != null)
      {
        return false;
      }

      var namespaceDeclaration = treeNode.GetContainingNode<INamespaceDeclaration>(true);

      return namespaceDeclaration != null;
    }

    /// <summary>Gets the assignment expression.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="statement">The statement.</param>
    /// <returns>Returns <c>true</c>, if successful, otherwise <c>false</c>.</returns>
    public static bool GetAssignmentExpression([NotNull] this DataContext dataContext, Dictionary<string, string> parameters, [NotNull] IStatement statement)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var expressionStatement = statement as IExpressionStatement;
      if (expressionStatement == null)
      {
        return false;
      }

      var assignmentExpression = expressionStatement.Expression as IAssignmentExpression;
      if (assignmentExpression == null)
      {
        return false;
      }

      return AssignmentAnalyzer.HandleAssignment(assignmentExpression, parameters);
    }

    /// <summary>Determines whether this instance [can generate iterator] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance [can generate iterator] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool GetIterator([NotNull] this DataContext dataContext, Dictionary<string, string> parameters, [NotNull] IStatement statement)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var declarationStatement = statement as IDeclarationStatement;
      if (declarationStatement == null)
      {
        return false;
      }

      var localVariableDeclarations = declarationStatement.VariableDeclarations;
      if (localVariableDeclarations.Count != 1)
      {
        return false;
      }

      var localVariableDeclaration = localVariableDeclarations.First();
      var value = localVariableDeclaration.Initial.GetText();

      var localVariable = localVariableDeclaration as ILocalVariable;
      if (localVariable == null)
      {
        return false;
      }

      var declaredType = localVariable.Type as IDeclaredType;
      if (declaredType == null)
      {
        return false;
      }

      var enumerable = TypeFactory.CreateTypeByCLRName("System.Collections.IEnumerable", declaredType.Module, UniversalModuleReferenceContext.Instance);
      if (!declaredType.IsSubtypeOf(enumerable))
      {
        return false;
      }

      parameters["VariableName"] = localVariable.ShortName;
      parameters["VariableType"] = localVariable.Type.GetPresentableName(localVariable.PresentationLanguage);
      parameters["Value"] = value;

      return true;
    }

    /// <summary>Determines whether this instance [can generate after variable] the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="statement">The statement.</param>
    /// <returns><c>true</c> if this instance [can generate after variable] the specified data context; otherwise, <c>false</c>.</returns>
    public static bool GetLocalVariableDeclaration([NotNull] this DataContext dataContext, Dictionary<string, string> parameters, [NotNull] IStatement statement)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var declarationStatement = statement as IDeclarationStatement;
      if (declarationStatement == null)
      {
        return false;
      }

      return LocalVariableAnalyzer.HandleLocalVariable(declarationStatement, parameters);
    }

    /// <summary>Gets the next statement.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the next statement.</returns>
    [CanBeNull]
    public static IStatement GetNextStatement([NotNull] this DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      var treeNode = context.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return null;
      }

      var block = treeNode.GetContainingNode<IBlock>(true);
      if (block == null)
      {
        return null;
      }

      var statement = treeNode.GetContainingNode<IStatement>(true);
      if (statement != null && !block.Contains(statement))
      {
        return null;
      }

      return GetNextStatement(block, treeNode);
    }

    /// <summary>Gets the previous statement.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the previous statement.</returns>
    [CanBeNull]
    public static IStatement GetPreviousStatement([NotNull] this DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      var treeNode = context.GetSelectedElement<ITreeNode>();
      if (treeNode == null)
      {
        return null;
      }

      var block = treeNode.GetContainingNode<IBlock>(true);
      if (block == null)
      {
        return null;
      }

      var statement = treeNode.GetContainingNode<IStatement>(true);
      if (statement != null && !block.Contains(statement))
      {
        return null;
      }

      return GetPreviousStatement(block, treeNode);
    }

    #endregion

    #region Methods

    /// <summary>Gets the modifier.</summary>
    /// <param name="treeNode">The element.</param>
    /// <param name="classDeclaration">The class declaration.</param>
    /// <returns>Returns the modifier.</returns>
    private static string GetModifier(ITreeNode treeNode, IClassLikeDeclaration classDeclaration)
    {
      ITypeMemberDeclaration classMember = null;

      var caret = treeNode.GetTreeStartOffset();

      foreach (var typeMemberDeclaration in classDeclaration.MemberDeclarations)
      {
        if (typeMemberDeclaration.GetTreeStartOffset() > caret)
        {
          break;
        }

        classMember = typeMemberDeclaration;
      }

      var modifier = "public";

      var accessRightsOwner = classMember as IAccessRightsOwner;
      if (accessRightsOwner != null)
      {
        var rights = accessRightsOwner.GetAccessRights();
        switch (rights)
        {
          case AccessRights.PUBLIC:
            modifier = "public";
            break;
          case AccessRights.INTERNAL:
            modifier = "internal";
            break;
          case AccessRights.PROTECTED:
            modifier = "protected";
            break;
          case AccessRights.PROTECTED_OR_INTERNAL:
            modifier = "protected";
            break;
          case AccessRights.PROTECTED_AND_INTERNAL:
            modifier = "protected internal";
            break;
          case AccessRights.PRIVATE:
            modifier = "private";
            break;
          case AccessRights.NONE:
            modifier = string.Empty;
            break;
        }
      }

      var modifiersOwner = classMember as IModifiersOwner;
      if (modifiersOwner != null)
      {
        if (modifiersOwner.IsStatic)
        {
          if (!string.IsNullOrEmpty(modifier))
          {
            modifier += ' ';
          }

          modifier += "static";
        }
      }

      if (!string.IsNullOrEmpty(modifier))
      {
        modifier += ' ';
      }

      return modifier;
    }

    /// <summary>Gets the next statement.</summary>
    /// <param name="block">The block.</param>
    /// <param name="treeNode">The element.</param>
    /// <returns>Returns the next statement.</returns>
    [CanBeNull]
    private static IStatement GetNextStatement([NotNull] IBlock block, [NotNull] ITreeNode treeNode)
    {
      if (block == null)
      {
        throw new ArgumentNullException("block");
      }

      if (treeNode == null)
      {
        throw new ArgumentNullException("treeNode");
      }

      var caret = treeNode.GetTreeStartOffset();

      foreach (var statement in block.Statements)
      {
        if (statement.GetTreeStartOffset() > caret)
        {
          return statement;
        }
      }

      return null;
    }

    /// <summary>Gets the previous statement.</summary>
    /// <param name="block">The block.</param>
    /// <param name="treeNode">The element.</param>
    /// <returns>The <see cref="IStatement"/>.</returns>
    [CanBeNull]
    private static IStatement GetPreviousStatement([NotNull] IBlock block, [NotNull] ITreeNode treeNode)
    {
      if (block == null)
      {
        throw new ArgumentNullException("block");
      }

      if (treeNode == null)
      {
        throw new ArgumentNullException("treeNode");
      }

      IStatement result = null;
      var caret = treeNode.GetTreeStartOffset();

      foreach (var statement in block.Statements)
      {
        if (statement.GetTreeStartOffset() > caret)
        {
          break;
        }

        result = statement;
      }

      return result;
    }

    #endregion
  }
}