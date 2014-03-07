// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContext.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="DataContext" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Shell
{
  using System;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.Application.DataContext;
  using JetBrains.ReSharper.Feature.Services.Bulbs;
  using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>Defines the <see cref="DataContext"/> class.</summary>
  public class DataContext
  {
    #region Constants and Fields

    /// <summary>The provider field.</summary>
    private IContextActionDataProvider provider;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="DataContext"/> class.</summary>
    /// <param name="dataContext">The data context.</param>
    internal DataContext([NotNull] IDataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      this.InternalDataContext = dataContext;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the tree node.
    /// </summary>
    [CanBeNull]
    public ITreeNode TreeNode
    {
      get
      {
        return this.GetSelectedElement<ITreeNode>();
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the internal data context.
    /// </summary>
    /// <value>The internal data context.</value>
    [NotNull]
    internal IDataContext InternalDataContext { get; private set; }

    /// <summary>
    /// Gets the provider.
    /// </summary>
    [CanBeNull]
    internal IContextActionDataProvider Provider
    {
      get
      {
        return this.provider ?? (this.provider = this.GetProvider());
      }
    }

    #endregion

    #region Methods

    /// <summary>Gets the selected element.</summary>
    /// <typeparam name="T">The type of element to get.</typeparam>
    /// <returns>Returns the selected element.</returns>
    [CanBeNull]
    internal T GetSelectedElement<T>() where T : class, ITreeNode
    {
      var p = this.Provider;
      return p != null ? p.GetSelectedElement<T>(true, true) : null;
    }

    /// <summary>Gets the provider.</summary>
    /// <returns>Returns the provider.</returns>
    [CanBeNull]
    private IContextActionDataProvider GetProvider()
    {
      var textControl = this.InternalDataContext.GetData(JetBrains.TextControl.DataContext.DataConstants.TEXT_CONTROL);
      if (textControl == null)
      {
        return null;
      }

      var solution = this.InternalDataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
      {
        return null;
      }

      var builder = Shell.Instance.GetComponent<IContextActionDataBuilders>();

      var contextActionDataBuilder = builder.TryGetBuilder(typeof(ICSharpContextActionDataProvider));

      return contextActionDataBuilder.Build(solution, textControl);
    }

    #endregion
  }
}