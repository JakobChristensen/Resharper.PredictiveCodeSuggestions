// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateGenerator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplateGenerator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators
{
  using System;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.AutoTemplates;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="AutoTemplateGenerator"/> class.</summary>
  public class AutoTemplateGenerator : GeneratorBase
  {
    #region Constants and Fields

    /// <summary>The title field.</summary>
    [NotNull]
    private readonly string title;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplateGenerator"/> class.</summary>
    /// <param name="autoTemplate">The smart template.</param>
    /// <param name="scope">The scope.</param>
    public AutoTemplateGenerator([NotNull] AutoTemplate autoTemplate, ScopeInstance scope)
    {
      if (autoTemplate == null)
      {
        throw new ArgumentNullException("autoTemplate");
      }

      this.AutoTemplate = autoTemplate;
      this.Scope = scope;

      this.title = this.AutoTemplate.Template;

      foreach (var parameter in this.Scope.Parameters)
      {
        this.title = this.title.Replace("$" + parameter.Key + "$", parameter.Value);
      }

      foreach (var parameter in this.AutoTemplate.TemplateVariables)
      {
        this.title = this.title.Replace("$" + parameter.Key + "$", parameter.Key);
      }

      this.title = this.title.Replace("$END$", string.Empty);

      this.title = this.title.Replace("{", string.Empty).Replace("}", string.Empty);

      while (this.title.IndexOf("  ", StringComparison.Ordinal) >= 0)
      {
        this.title = this.title.Replace("  ", " ");
      }

      this.title = this.title.Trim();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the LiveTemplate.
    /// </summary>
    /// <value>The LiveTemplate.</value>
    [NotNull]
    public AutoTemplate AutoTemplate { get; private set; }

    /// <summary>
    /// Gets the scope.
    /// </summary>
    [NotNull]
    public ScopeInstance Scope { get; private set; }

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
      this.AutoTemplate.Execute(context, this.Scope);
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
      return "SmartGenerate.CreateAutoTemplate";
    }

    /// <summary>Gets the title.</summary>
    /// <returns>Returns the title.</returns>
    protected override string GetTitle()
    {
      return this.title;
    }

    #endregion
  }
}