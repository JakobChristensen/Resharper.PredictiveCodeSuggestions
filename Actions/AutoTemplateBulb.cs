// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateBulb.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace PredictiveCodeSuggestions.Actions
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application.Progress;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Intentions.Extensibility;
  using JetBrains.TextControl;
  using JetBrains.Util;
  using PredictiveCodeSuggestions.AutoTemplates;

  /// <summary>Defines the <see cref="AutoTemplateBulb"/> class.</summary>
  public class AutoTemplateBulb : BulbActionBase
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplateBulb"/> class.</summary>
    /// <param name="scopeParameters">The scope.</param>
    /// <param name="autoTemplate">The smart template.</param>
    /// <param name="textRange">The document range.</param>
    public AutoTemplateBulb(Dictionary<string, string> scopeParameters, AutoTemplate autoTemplate, TextRange textRange)
    {
      this.ScopeParameters = scopeParameters;
      this.AutoTemplate = autoTemplate;
      this.TextRange = textRange;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the template.
    /// </summary>
    [NotNull]
    public AutoTemplate AutoTemplate { get; private set; }

    /// <summary>
    /// Gets the descriptor.
    /// </summary>
    [NotNull]
    public Dictionary<string, string> ScopeParameters { get; private set; }

    /// <summary>
    /// Popup menu item text
    /// </summary>
    public override string Text
    {
      get
      {
        var result = this.AutoTemplate.Template;

        foreach (var parameter in this.ScopeParameters)
        {
          result = result.Replace("$" + parameter.Key + "$", parameter.Value);
        }

        foreach (var parameter in this.AutoTemplate.TemplateVariables)
        {
          result = result.Replace("$" + parameter.Key + "$", parameter.Key);
        }

        result = result.Replace("$END$", string.Empty);

        result = result.Replace("{", string.Empty).Replace("}", string.Empty);

        while (result.IndexOf("  ", StringComparison.Ordinal) >= 0)
        {
          result = result.Replace("  ", " ");
        }

        result = result.Trim();

        return "Insert: " + result;
      }
    }

    /// <summary>
    /// Gets the document range.
    /// </summary>
    public TextRange TextRange { get; private set; }

    #endregion

    #region Methods

    /// <summary>Executes QuickFix or ContextAction. Returns post-execute method.</summary>
    /// <param name="solution">The solution.</param>
    /// <param name="progress">The progress.</param>
    /// <returns>Action to execute after document and PSI transaction finish. Use to open TextControls, navigate caret, etc.</returns>
    protected override Action<ITextControl> ExecutePsiTransaction([NotNull] ISolution solution, [NotNull] IProgressIndicator progress)
    {
      return textControl =>
      {
        var textControlPos = textControl.Coords.FromDocOffset(this.TextRange.EndOffset);

        textControl.Caret.MoveTo(textControlPos, CaretVisualPlacement.Generic);
        this.AutoTemplate.Execute(solution, textControl, this.ScopeParameters, true);
      };
    }

    #endregion
  }
}