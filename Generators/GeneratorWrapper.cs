// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorWrapper.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   The action group.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System;
  using JetBrains.Annotations;
  using JetBrains.Application.DataContext;
  using JetBrains.ReSharper.Feature.Services.ActionsMenu;
  using JetBrains.ReSharper.Feature.Services.Generate.Actions;
  using JetBrains.UI.Icons;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>
  /// The action group.
  /// </summary>
  public enum ActionGroup
  {
    /// <summary>The live template field.</summary>
    LiveTemplate, 

    /// <summary>The create live template field.</summary>
    CreateLiveTemplate, 

    /// <summary>The generator field.</summary>
    Generator
  }

  /// <summary>Defines the <see cref="GeneratorWrapper"/> class.</summary>
  internal class GeneratorWrapper : IGenerateActionWorkflow
  {
    #region Constants and Fields

    /// <summary>The create generator action group field.</summary>
    private static readonly GenerateActionGroup createGeneratorActionGroup = new GenerateActionGroup("SmartTemplates.Generators", 0);

    /// <summary>The create live template action group field.</summary>
    private static readonly GenerateActionGroup createLiveTemplateActionGroup = new GenerateActionGroup("SmartGenerate.CreateLiveTemplate", 9000);

    /// <summary>The live template action group field.</summary>
    private static readonly GenerateActionGroup liveTemplateActionGroup = new GenerateActionGroup("SmartGenerate.LiveTemplate", 0);

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes static members of the <see cref="GeneratorWrapper"/> class.
    /// </summary>
    static GeneratorWrapper()
    {
      var propertyInfo = typeof(JetBrains.ReSharper.Feature.Services.ActionsMenu.ActionGroup).GetProperty("Order");
      propertyInfo.SetValue(liveTemplateActionGroup, (float)-2, null);
      propertyInfo.SetValue(createGeneratorActionGroup, (float)-1, null);
    }

    /// <summary>Initializes a new instance of the <see cref="GeneratorWrapper"/> class.</summary>
    /// <param name="generator">The generator.</param>
    /// <param name="order">The order.</param>
    public GeneratorWrapper([NotNull] GeneratorBase generator, double order)
    {
      if (generator == null)
      {
        throw new ArgumentNullException("generator");
      }

      this.Generator = generator;
      this.Order = order;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the action group.
    /// </summary>
    public GenerateActionGroup ActionGroup
    {
      get
      {
        switch (this.Generator.GetActionGroup())
        {
          case Generators.ActionGroup.LiveTemplate:
            return liveTemplateActionGroup;

          case Generators.ActionGroup.CreateLiveTemplate:
            return createLiveTemplateActionGroup;

          case Generators.ActionGroup.Generator:
            return createGeneratorActionGroup;
        }

        return GenerateActionGroup.CLR_LANGUAGE;
      }
    }

    /// <summary>
    /// Gets the action id.
    /// </summary>
    [NotNull]
    public string ActionId
    {
      get
      {
        return this.Generator.ActionId;
      }
    }

    /// <summary>
    /// Gets the order.
    /// </summary>
    public double Order { get; private set; }

    /// <summary>
    /// Id of short action that overrides VS command. Used for presentation purposes only
    /// </summary>
    public string ShortActionId
    {
      get
      {
        return this.Generator.ActionId;
      }
    }

    /// <summary>
    /// Gets the title.
    /// </summary>
    [NotNull]
    public string Title
    {
      get
      {
        return this.Generator.Title;
      }
    }

    #endregion

    #region Explicit Interface Properties

    /// <summary>
    /// Gets the icon.
    /// </summary>
    IconId IWorkflow<GenerateActionGroup>.Icon
    {
      get
      {
        return null;
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the generator.
    /// </summary>
    /// <value>The generator.</value>
    private GeneratorBase Generator { get; set; }

    #endregion

    #region Public Methods

    /// <summary>Executes the specified data context.</summary>
    /// <param name="dataContext">The data context.</param>
    public void Execute([NotNull] IDataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      this.Generator.Execute(new DataContext(dataContext));
    }

    /// <summary>Determines whether the specified data context is available.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if the specified data context is available; otherwise, <c>false</c>.</returns>
    public bool IsAvailable([NotNull] IDataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var context = new DataContext(dataContext);

      return this.Generator.CanExecute(context);
    }

    /// <summary>Determines whether the specified context is enabled.</summary>
    /// <param name="context">The context.</param>
    /// <returns><c>true</c> if the specified context is enabled; otherwise, <c>false</c>.</returns>
    public bool IsEnabled([NotNull] IDataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      return true;
    }

    #endregion
  }
}