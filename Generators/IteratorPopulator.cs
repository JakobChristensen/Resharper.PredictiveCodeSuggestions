// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IteratorPopulator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace PredictiveCodeSuggestions.Generators
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Util;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>
  /// Defines the <see cref="IteratorPopulator"/> class.
  /// </summary>
  public class IteratorPopulator : IGeneratorPopulator
  {
    #region Public Methods and Operators

    /// <summary>Populates the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns>Returns the I enumerable.</returns>
    public IEnumerable<GeneratorBase> Populate(DataContext context)
    {
      var model = this.GetModel(context);
      if (model == null)
      {
        yield break;
      }

      yield return new IteratorGenerator(model, string.Format("foreach (var item in {0})", model.VariableName), "foreach (var $item$ in " + model.VariableName + ")\r\n{$END$\r\n}\r\n", "item", "suggestVariableName()");
    }

    #endregion

    #region Methods

    /// <summary>Gets the model.</summary>
    /// <param name="context">The context.</param>
    /// <returns>Returns the model.</returns>
    private Model GetModel(DataContext context)
    {
      var treeNode = context.TreeNode;
      if (treeNode == null)
      {
        return null;
      }

      var statement = context.GetPreviousStatement() as IDeclarationStatement;
      if (statement == null)
      {
        return null;
      }

      foreach (var variableDeclaration in statement.VariableDeclarations)
      {
        if (!this.IsEnumerable(variableDeclaration.Type))
        {
          continue;
        }

        var typeName = variableDeclaration.Type.GetLongPresentableName(CSharpLanguage.Instance);
        if (typeName == "string" || typeName == "System.String")
        {
          continue;
        }

        return new Model
        {
          VariableName = variableDeclaration.DeclaredName, 
          TypeName = typeName
        };
      }

      return null;
    }

    /// <summary>Gets a value indicating whether this instance is enumerable.</summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if the specified type is enumerable; otherwise, <c>false</c>.</returns>
    private bool IsEnumerable(IType type)
    {
      var enumerable = TypeFactory.CreateTypeByCLRName("System.Collections.IEnumerable", type.Module, type.GetResolveContext());
      return type.IsSubtypeOf(enumerable);
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="Model"/> class.
    /// </summary>
    public class Model
    {
      #region Public Properties

      /// <summary>
      ///   Gets or sets the name of the type.
      /// </summary>
      /// <value>The name of the type.</value>
      public string TypeName { get; set; }

      /// <summary>
      ///   Gets or sets the name of the variable.
      /// </summary>
      /// <value>The name of the variable.</value>
      public string VariableName { get; set; }

      #endregion
    }
  }
}