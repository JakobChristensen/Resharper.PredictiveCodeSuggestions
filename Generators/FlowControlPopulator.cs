// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowControlPopulator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="FlowControlPopulator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="FlowControlPopulator"/> class.</summary>
  public class FlowControlPopulator : IGeneratorPopulator
  {
    #region Public Methods

    /// <summary>Populates the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns>Returns the I enumerable.</returns>
    [NotNull]
    public IEnumerable<GeneratorBase> Populate(DataContext context)
    {
      var model = this.GetModel(context);
      if (model == null)
      {
        yield break;
      }

      yield return new FlowControlGenerator("continue", "continue;");
      yield return new FlowControlGenerator("break", "break;");
    }

    #endregion

    #region Methods

    /// <summary>Gets the model.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the model.</returns>
    [CanBeNull]
    private Model GetModel([NotNull] DataContext context)
    {
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

      var hasLoop = context.GetSelectedElement<IForeachStatement>() != null;

      if (!hasLoop)
      {
        hasLoop = context.GetSelectedElement<IForStatement>() != null;
      }

      if (!hasLoop)
      {
        hasLoop = context.GetSelectedElement<IDoStatement>() != null;
      }

      if (!hasLoop)
      {
        hasLoop = context.GetSelectedElement<IWhileStatement>() != null;
      }

      if (!hasLoop)
      {
        return null;
      }

      return new Model();
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
    }
  }
}