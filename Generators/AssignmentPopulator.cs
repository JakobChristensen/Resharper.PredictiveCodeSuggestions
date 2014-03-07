// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssignmentPopulator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AssignmentPopulator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Annotations;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Util;
  using PredictiveCodeSuggestions.Diagnostics;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="AssignmentPopulator"/> class.</summary>
  public class AssignmentPopulator : IGeneratorPopulator
  {
    #region Public Methods

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

      foreach (var descriptor in model.Descriptors)
      {
        var typeName = descriptor.Type.GetLongPresentableName(CSharpLanguage.Instance);
        if (typeName == "string" || typeName == "System.String")
        {
          yield return new AssignmentGenerator(model, string.Format("Check if \"{0}\" is null or empty", descriptor.Name), "if (string.IsNullOrEmpty(" + descriptor.Name + "))\n{\n$END$\n}");
        }

        yield return new AssignmentGenerator(model, string.Format("Check if \"{0}\" is null", descriptor.Name), "if (" + descriptor.Name + " == null)\n{\n$END$\n}");
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

      var typeMember = statement.GetContainingTypeMemberDeclaration();
      if (typeMember == null)
      {
        return null;
      }

      var names = new List<Descriptor>();

      var codeAnnotation = new CodeAnnotation(typeMember);

      foreach (var variableDeclaration in statement.VariableDeclarations)
      {
        var type = variableDeclaration.Type;
        if (!type.IsReferenceType())
        {
          continue;
        }

        var name = variableDeclaration.DeclaredName;

        var state = codeAnnotation.GetExpressionNullReferenceState(treeNode, variableDeclaration.DeclaredElement, statement);

        if (state == CodeAnnotationAttribute.CanBeNull || state == CodeAnnotationAttribute.Undefined)
        {
          names.Add(new Descriptor(name, variableDeclaration.Type));
        }
      }

      if (!names.Any())
      {
        return null;
      }

      return new Model(names);
    }

    #endregion

    /// <summary>Defines the <see cref="Descriptor"/> class.</summary>
    public class Descriptor
    {
      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="Descriptor"/> class.</summary>
      /// <param name="name">The name.</param>
      /// <param name="type">The type.</param>
      public Descriptor(string name, IType type)
      {
        this.Name = name;
        this.Type = type;
      }

      #endregion

      #region Public Properties

      /// <summary>
      /// Gets or sets the name.
      /// </summary>
      /// <value>The name.</value>
      public string Name { get; set; }

      /// <summary>
      /// Gets or sets the type.
      /// </summary>
      /// <value>The type.</value>
      public IType Type { get; set; }

      #endregion
    }

    /// <summary>Defines the <see cref="Model"/> class.</summary>
    public class Model
    {
      #region Constants and Fields

      /// <summary>The descriptors.</summary>
      private readonly List<Descriptor> descriptors = new List<Descriptor>();

      #endregion

      #region Constructors and Destructors

      /// <summary>Initializes a new instance of the <see cref="Model"/> class.</summary>
      /// <param name="list">The list.</param>
      public Model([NotNull] List<Descriptor> list)
      {
        if (list == null)
        {
          throw new ArgumentNullException("list");
        }

        this.descriptors.AddRange(list);
      }

      #endregion

      #region Public Properties

      /// <summary>
      /// Gets the names.
      /// </summary>
      [NotNull]
      public IEnumerable<Descriptor> Descriptors
      {
        get
        {
          return this.descriptors;
        }
      }

      #endregion
    }
  }
}