// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="GeneratorManager" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application.DataContext;
  using JetBrains.ReSharper.Feature.Services.Generate.Actions;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="GeneratorManager"/> class.</summary>
  public static class GeneratorManager
  {
    #region Constants and Fields

    /// <summary>The populators field.</summary>
    private static readonly List<GeneratorBase> generators = new List<GeneratorBase>();

    /// <summary>The populators field.</summary>
    private static readonly List<IGeneratorPopulator> populators = new List<IGeneratorPopulator>();

    #endregion

    #region Public Methods

    /// <summary>Populates the specified context.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns>Returns the I enumerable.</returns>
    [NotNull]
    public static IEnumerable<IGenerateActionWorkflow> Populate([NotNull] IDataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      var context = new DataContext(dataContext);

      var order = 0;

      foreach (var populator in populators)
      {
        foreach (var generatorBase in populator.Populate(context))
        {
          yield return new GeneratorWrapper(generatorBase, order);
          order++;
        }

        foreach (var generatorBase in generators)
        {
          yield return new GeneratorWrapper(generatorBase, order);
          order++;
        }
      }
    }

    /// <summary>Registers the specified populator.</summary>
    /// <param name="populator">The populator.</param>
    public static void Register([NotNull] IGeneratorPopulator populator)
    {
      if (populator == null)
      {
        throw new ArgumentNullException("populator");
      }

      populators.Add(populator);
    }

    /// <summary>Registers the specified generator.</summary>
    /// <param name="generator">The generator.</param>
    public static void Register([NotNull] GeneratorBase generator)
    {
      if (generator == null)
      {
        throw new ArgumentNullException("generator");
      }

      generators.Add(generator);
    }

    #endregion
  }
}