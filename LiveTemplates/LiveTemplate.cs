// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiveTemplate.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="LiveTemplate" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.LiveTemplates
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.ProjectModel.DataContext;
  using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
  using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
  using JetBrains.ReSharper.LiveTemplates.Templates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>
  /// Defines the <see cref="LiveTemplate"/> class.
  /// </summary>
  public class LiveTemplate
  {
    #region Constants and Fields

    /// <summary>
    ///   The description field.
    /// </summary>
    private string description;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="LiveTemplate"/> class.</summary>
    /// <param name="template">The template.</param>
    /// <param name="scope">The scope.</param>
    internal LiveTemplate([NotNull] Template template, [NotNull] ScopeInstance scope)
    {
      if (template == null)
      {
        throw new ArgumentNullException("template");
      }

      if (scope == null)
      {
        throw new ArgumentNullException("scope");
      }

      this.InternalTemplate = template;
      this.Scope = scope;

      this.BuildDescription();
    }

    #endregion

    #region Public Properties

    /// <summary>
    ///   Gets the description.
    /// </summary>
    [NotNull]
    public string Description
    {
      get
      {
        return this.description;
      }
    }

    /// <summary>
    ///   Gets the scope.
    /// </summary>
    /// <value>The scope.</value>
    [NotNull]
    public ScopeInstance Scope { get; private set; }

    /// <summary>
    ///   Gets the text.
    /// </summary>
    [NotNull]
    public string Text
    {
      get
      {
        return this.InternalTemplate.Text;
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <value>The template.</value>
    [NotNull]
    internal Template InternalTemplate { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>Executes the specified context.</summary>
    /// <param name="context">The context.</param>
    public void Execute([NotNull] DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      foreach (var templateField in this.InternalTemplate.Fields)
      {
        foreach (var parameter in this.Scope.Parameters)
        {
          if (templateField.Name == parameter.Key)
          {
            templateField.Expression = new TextHotspotExpression(new List<string>
            {
              parameter.Value
            });
          }
        }
      }

      var liveTemplateManager = Shell.Instance.GetComponent<LiveTemplatesManager>();

      var solution = context.InternalDataContext.GetData(DataConstants.SOLUTION);
      var textControl = context.InternalDataContext.GetData(JetBrains.TextControl.DataContext.DataConstants.TEXT_CONTROL);

      var hotspotSession = liveTemplateManager.CreateHotspotSessionFromTemplate(this.InternalTemplate, solution, textControl);
      if (hotspotSession != null)
      {
        hotspotSession.Execute();
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Builds the description.
    /// </summary>
    private void BuildDescription()
    {
      this.description = this.InternalTemplate.Description;
      foreach (var parameter in this.Scope.Parameters)
      {
        this.description = this.description.Replace("$" + parameter.Key + "$", parameter.Value);
      }
    }

    #endregion
  }
}