// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGeneratorPopulator.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   The <see cref="IGeneratorPopulator" /> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>
  /// The <see cref="IGeneratorPopulator"/> interface.
  /// </summary>
  public interface IGeneratorPopulator
  {
    #region Public Methods

    /// <summary>Populates the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns>Returns the I enumerable.</returns>
    IEnumerable<GeneratorBase> Populate([NotNull] DataContext context);

    #endregion
  }
}