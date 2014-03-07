// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssignmentGenerator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AssignmentGenerator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="AssignmentGenerator"/> class.</summary>
  public class AssignmentGenerator : GeneratorBase
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AssignmentGenerator"/> class.</summary>
    /// <param name="model">The model.</param>
    /// <param name="text">The text.</param>
    /// <param name="liveTemplate">The live template.</param>
    public AssignmentGenerator(AssignmentPopulator.Model model, string text, string liveTemplate)
    {
      this.Model = model;
      this.Text = text;
      this.LiveTemplate = liveTemplate;
      this.Variables = new string[0];
    }

    /// <summary>Initializes a new instance of the <see cref="AssignmentGenerator"/> class.</summary>
    /// <param name="model">The model.</param>
    /// <param name="text">The text.</param>
    /// <param name="liveTemplate">The live template.</param>
    /// <param name="variables">The variables.</param>
    public AssignmentGenerator(AssignmentPopulator.Model model, string text, string liveTemplate, params string[] variables)
    {
      this.Model = model;
      this.Text = text;
      this.LiveTemplate = liveTemplate;
      this.Variables = variables;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    /// <value>The model.</value>
    public AssignmentPopulator.Model Model { get; private set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    public string Text { get; private set; }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the live template.
    /// </summary>
    /// <value>The live template.</value>
    protected string LiveTemplate { get; set; }

    /// <summary>
    /// Gets or sets the variables.
    /// </summary>
    /// <value>The variables.</value>
    protected string[] Variables { get; set; }

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
    /// <param name="context">The context.</param>
    public override void Execute(DataContext context)
    {
      LiveTemplateManager.ExecuteLiveTemplate(context, this.LiveTemplate, this.Variables);
    }

    /// <summary>Gets or sets the action group.</summary>
    /// <returns>The action group.</returns>
    public override ActionGroup GetActionGroup()
    {
      return ActionGroup.Generator;
    }

    #endregion

    #region Methods

    /// <summary>Gets the action id.</summary>
    /// <returns>Returns the action id.</returns>
    protected override string GetActionId()
    {
      return "AgentJohnson.ReturnGenerator";
    }

    /// <summary>Gets the title.</summary>
    /// <returns>Returns the title.</returns>
    protected override string GetTitle()
    {
      return this.Text;
    }

    #endregion
  }
}