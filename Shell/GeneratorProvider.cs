// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorProvider.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="GeneratorProvider" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Shell
{
  using System;
  using System.Collections.Generic;
  using JetBrains.Annotations;
  using JetBrains.Application.DataContext;
  using JetBrains.ReSharper.Feature.Services.Generate.Actions;
  using PredictiveCodeSuggestions.Generators;

  /// <summary>Defines the <see cref="GeneratorProvider"/> class.</summary>
  [GenerateProvider]
  internal class GeneratorProvider : IGenerateActionProvider
  {
    #region Public Methods

    /// <summary>Creates the workflow.</summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns>Returns the workflow.</returns>
    public IEnumerable<IGenerateActionWorkflow> CreateWorkflow([NotNull] IDataContext dataContext)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      return GeneratorManager.Populate(dataContext);
    }

    #endregion
  }
}