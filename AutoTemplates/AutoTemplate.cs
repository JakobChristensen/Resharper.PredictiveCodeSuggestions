// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.AutoTemplates
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.ProjectModel;
  using JetBrains.ProjectModel.DataContext;
  using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
  using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
  using JetBrains.ReSharper.LiveTemplates;
  using JetBrains.ReSharper.LiveTemplates.Templates;
  using JetBrains.TextControl;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="AutoTemplate"/> class.</summary>
  public class AutoTemplate
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplate"/> class.</summary>
    public AutoTemplate()
    {
      this.TemplateVariables = new Dictionary<string, string>();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets the shortcut.
    /// </summary>
    /// <value>The shortcut.</value>
    [NotNull]
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the percentage.
    /// </summary>
    /// <value>The percentage.</value>
    public int Percentage { get; set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    [NotNull]
    public string Template { get; set; }

    /// <summary>
    /// Gets the variables.
    /// </summary>
    [NotNull]
    public Dictionary<string, string> TemplateVariables { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Executes the specified context.</summary>
    /// <param name="context">The context.</param>
    /// <param name="scope">The scope.</param>
    public void Execute([NotNull] DataContext context, [NotNull] ScopeInstance scope)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      if (scope == null)
      {
        throw new ArgumentNullException("scope");
      }

      var solution = context.InternalDataContext.GetData(DataConstants.SOLUTION);
      var textControl = context.InternalDataContext.GetData(JetBrains.TextControl.DataContext.DataConstants.TEXT_CONTROL);

      this.Execute(solution, textControl, scope.Parameters);
    }

    /// <summary>Executes the specified solution.</summary>
    /// <param name="solution">The solution.</param>
    /// <param name="textControl">The text control.</param>
    /// <param name="parameters">The parameters.</param>
    /// <param name="newLineBefore">if set to <c>true</c> [new line].</param>
    public void Execute(ISolution solution, ITextControl textControl, Dictionary<string, string> parameters, bool newLineBefore = false)
    {
      var text = this.Template;

      if (newLineBefore)
      {
        text = "\r\n" + text;
      }

      foreach (var parameter in parameters)
      {
        text = text.Replace("$" + parameter.Key + "$", parameter.Value);
      }

      var template = new Template(string.Empty, string.Empty, text, true, true, true, TemplateApplicability.Live);

      foreach (var variable in this.TemplateVariables)
      {
        template.Fields.Add(new TemplateField(variable.Key, variable.Value, 0));
      }

      template.Sections.Add(new TemplateSection());

      var liveTemplateManager = Shell.Instance.GetComponent<LiveTemplatesManager>();

      var hotspotSession = liveTemplateManager.CreateHotspotSessionFromTemplate(template, solution, textControl);
      if (hotspotSession != null)
      {
        hotspotSession.Execute();
      }
    }

    #endregion
  }
}