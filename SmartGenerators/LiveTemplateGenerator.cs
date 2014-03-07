// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiveTemplateGenerator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="LiveTemplateGenerator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators
{
  using System;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="LiveTemplateGenerator"/> class.</summary>
  public class LiveTemplateGenerator : GeneratorBase
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="LiveTemplateGenerator"/> class.</summary>
    /// <param name="liveTemplate">The LiveTemplate.</param>
    public LiveTemplateGenerator([NotNull] LiveTemplate liveTemplate)
    {
      if (liveTemplate == null)
      {
        throw new ArgumentNullException("liveTemplate");
      }

      this.LiveTemplate = liveTemplate;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the LiveTemplate.
    /// </summary>
    /// <value>The LiveTemplate.</value>
    [NotNull]
    public LiveTemplate LiveTemplate { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Determines whether this instance can execute the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns><c>true</c> if this instance can execute the specified data context; otherwise, <c>false</c>.</returns>
    public override bool CanExecute(DataContext context)
    {
      return true;
    }

    /// <summary>Executes the specified data context.</summary>
    /// <param name="context">The data context.</param>
    public override void Execute(DataContext context)
    {
      this.LiveTemplate.Execute(context);
    }

    /// <summary>
    /// Gets or sets the action group.
    /// </summary>
    /// <returns>The action group.</returns>
    public override ActionGroup GetActionGroup()
    {
      return ActionGroup.LiveTemplate;
    }

    #endregion

    #region Methods

    /// <summary>Gets the action id.</summary>
    /// <returns>Returns the action id.</returns>
    protected override string GetActionId()
    {
      return "SmartGenerate.CreateLiveTemplate";
    }

    /// <summary>Gets the title.</summary>
    /// <returns>Returns the title.</returns>
    protected override string GetTitle()
    {
      return this.LiveTemplate.Description;
    }

    #endregion
  }
}