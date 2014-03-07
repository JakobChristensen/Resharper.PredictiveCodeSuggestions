// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmartGeneratorPopulator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="SmartGeneratorPopulator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.SmartGenerators
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.AutoTemplates;
  using PredictiveCodeSuggestions.Generators;
  using PredictiveCodeSuggestions.LiveTemplates;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="SmartGeneratorPopulator"/> class.</summary>
  public class SmartGeneratorPopulator : IGeneratorPopulator
  {
    #region Public Methods

    /// <summary>Populates the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns>Returns the I enumerable.</returns>
    [NotNull]
    public IEnumerable<GeneratorBase> Populate(DataContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      var scopes = ScopeManager.GetScopes(context).ToList();
      if (!scopes.Any())
      {
        yield break;
      }

      yield return new CreateLiveTemplateGenerator();

      var templates = scopes.SelectMany(scope => LiveTemplateManager.GetLiveTemplates(context, scope));
      foreach (var template in templates.OrderBy(t => t.Description))
      {
        yield return new LiveTemplateGenerator(template);
      }

      foreach (var s in scopes)
      {
        var scope = s;
        var smartTemplates = AutoTemplateManager.AutoTemplates.Where(t => t.Key == scope.Shortcut);

        foreach (var smartTemplate in smartTemplates)
        {
          yield return new AutoTemplateGenerator(smartTemplate, scope);
        }
      }
    }

    #endregion
  }
}