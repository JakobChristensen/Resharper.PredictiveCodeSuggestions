// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiveTemplateManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="LiveTemplateManager" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.LiveTemplates
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.Application.Settings;
  using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
  using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
  using JetBrains.ReSharper.LiveTemplates;
  using JetBrains.ReSharper.LiveTemplates.Templates;
  using JetBrains.ReSharper.LiveTemplates.UI.TemplateEditor;
  using JetBrains.TextControl.DataContext;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="LiveTemplateManager"/> class.</summary>
  public static class LiveTemplateManager
  {
    #region Constants and Fields

    /// <summary>The do not change field.</summary>
    private const string DoNotChange = "`Do not change: ";

    #endregion

    #region Public Methods

    /// <summary>Creates the live template.</summary>
    /// <param name="key">The key.</param>
    /// <param name="description">The description.</param>
    /// <param name="text">The text.</param>
    public static void CreateLiveTemplate(string key, string description, string text)
    {
      var template = new Template(DoNotChange + key, description, text ?? string.Empty, true, true, false, TemplateApplicability.Live);

      Action action = () =>
      {
        using (ReadLockCookie.Create())
        {
          var component = Shell.Instance.GetComponent<ISettingsStore>();
          var transient = component.BindToContextTransient(ContextRange.ManuallyRestrictWritesToOneContext((lifetime, contexts) => component.DataContexts.Empty));

          TemplateEditorManager.Instance.CreateTemplate(template, transient);
        }
      };

      ShellManager.ExecuteReentrancyGuard("Smart Generate Create Template", action);
    }

    /// <summary>Gets the live templates.</summary>
    /// <param name="context">The context.</param>
    /// <param name="liveTemplate">The live template.</param>
    /// <param name="variables">The variables.</param>
    public static void ExecuteLiveTemplate([NotNull] DataContext context, [NotNull] string liveTemplate, [NotNull] string[] variables)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      if (liveTemplate == null)
      {
        throw new ArgumentNullException("liveTemplate");
      }

      if (variables == null)
      {
        throw new ArgumentNullException("variables");
      }

      var solution = context.InternalDataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
      if (solution == null)
      {
        return;
      }

      var textControl = context.InternalDataContext.GetData(DataConstants.TEXT_CONTROL);
      if (textControl == null)
      {
        return;
      }

      var template = new Template(string.Empty, string.Empty, liveTemplate, true, true, false, TemplateApplicability.Live);

      for (var index = 0; index < variables.Length - 1; index += 2)
      {
        var name = variables[index];
        var value = variables[index + 1];

        template.Fields.Add(new TemplateField(name, value, 0));
      }

      template.Sections.Add(new TemplateSection());

      var templateManager = Shell.Instance.GetComponent<LiveTemplatesManager>();

      var hotSpot = templateManager.CreateHotspotSessionFromTemplate(template, solution, textControl);
      if (hotSpot != null)
      {
        hotSpot.Execute();
      }
    }

    /// <summary>
    /// Gets the live templates.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="scope">The scope.</param>
    /// <returns>Returns the live templates.</returns>
    [NotNull]
    public static IEnumerable<LiveTemplate> GetLiveTemplates([NotNull] DataContext context, [NotNull] ScopeInstance scope)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      if (scope == null)
      {
        throw new ArgumentNullException("scope");
      }

      var s = DoNotChange + scope.Shortcut;

      var provider = context.Provider;
      if (provider == null)
      {
        yield break;
      }

      var solution = provider.Solution;

      var textControl = context.InternalDataContext.GetData(DataConstants.TEXT_CONTROL);
      if (textControl == null)
      {
        yield break;
      }

      var templateManager = Shell.Instance.GetComponent<LiveTemplatesManager>();

      foreach (var template in templateManager.GetAvailableTemplates(solution, textControl, false))
      {
        if (template.Shortcut == s)
        {
          yield return new LiveTemplate(template, scope);
        }
      }
    }

    #endregion
  }
}