// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnPopulator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ReturnPopulator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="ReturnPopulator"/> class.</summary>
  public class ReturnPopulator : IGeneratorPopulator
  {
    #region Public Methods

    /// <summary>Populates the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns>Returns the I enumerable.</returns>
    [NotNull]
    public IEnumerable<GeneratorBase> Populate(DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      var model = this.GetModel(context);
      if (model == null)
      {
        yield break;
      }

      if (model.TypeName == "bool" || model.TypeName == "System.Boolean")
      {
        yield return new ReturnGenerator(model, "return true", "return true;");
        yield return new ReturnGenerator(model, "return false", "return false;");
      }
      else if (model.TypeName == "void" || model.TypeName == "System.Void")
      {
        yield return new ReturnGenerator(model, "return", "return;");
      }
      else if (model.TypeName == "string" || model.TypeName == "System.String")
      {
        yield return new ReturnGenerator(model, "return string.Empty", "return string.Empty;");
        yield return new ReturnGenerator(model, "return <variable>", "return $result$;", "result", "completeSmart()");
        yield return new ReturnGenerator(model, "return null", "return null;");
      }
      else
      {
        yield return new ReturnGenerator(model, "return null", "return null;");
        yield return new ReturnGenerator(model, "return <variable>", "return $result$;", "result", "completeSmart()");
      }
    }

    #endregion

    #region Methods

    /// <summary>Gets the model.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the model.</returns>
    [CanBeNull]
    private Model GetModel([NotNull] DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      var block = context.GetSelectedElement<IBlock>();
      if (block == null)
      {
        return null;
      }

      var treeNode = context.TreeNode;
      if (treeNode == null)
      {
        return null;
      }

      if (!this.IsAfterLastStatement(block, treeNode))
      {
        return null;
      }

      IType returnType = null;

      var method = block.GetContainingNode<IMethodDeclaration>();
      if (method != null && method.DeclaredElement != null)
      {
        returnType = method.DeclaredElement.ReturnType;
      }

      var accessor = block.GetContainingNode<IAccessorDeclaration>();
      if (accessor != null)
      {
        returnType = accessor.DeclaredElement.ReturnType;
      }

      var constructor = block.GetContainingNode<IConstructorDeclaration>();
      if (constructor != null)
      {
        returnType = constructor.DeclaredElement.ReturnType;
      }

      var indexer = block.GetContainingNode<IIndexerDeclaration>();
      if (indexer != null)
      {
        returnType = indexer.DeclaredElement.ReturnType;
      }

      var destructor = block.GetContainingNode<IDestructorDeclaration>();
      if (destructor != null)
      {
        returnType = destructor.DeclaredElement.ReturnType;
      }

      if (returnType == null)
      {
        return null;
      }

      var typeName = returnType.GetLongPresentableName(CSharpLanguage.Instance);
      /*
      if (typeName != "bool" && typeName != "System.Boolean" && !returnType.IsVoid() && !returnType.IsReferenceType())
      {
        return null;
      }
      */

      return new Model
      {
        TypeName = typeName
      };
    }

    /// <summary>Determines whether [is after last statement] [the specified block].</summary>
    /// <param name="block">The block.</param>
    /// <param name="treeNode">The tree node.</param>
    /// <returns><c>true</c> if [is after last statement] [the specified block]; otherwise, <c>false</c>.</returns>
    private bool IsAfterLastStatement(IBlock block, ITreeNode treeNode)
    {
      var statement = block.Statements.LastOrDefault();
      if (statement == null)
      {
        return true;
      }

      return statement.GetNavigationRange().TextRange.EndOffset <= treeNode.GetNavigationRange().TextRange.EndOffset;
    }

    #endregion

    /// <summary>Defines the <see cref="Model"/> class.</summary>
    public class Model
    {
      #region Public Properties

      /// <summary>
      /// Gets or sets the name of the type.
      /// </summary>
      /// <value>The name of the type.</value>
      public string TypeName { get; set; }

      #endregion
    }
  }
}