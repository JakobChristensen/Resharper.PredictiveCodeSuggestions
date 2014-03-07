// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneratorBase.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="GeneratorBase" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Generators
{
  using JetBrains.Annotations;
  using PredictiveCodeSuggestions.Shell;

  /// <summary>Defines the <see cref="GeneratorBase"/> class.</summary>
  public abstract class GeneratorBase
  {
    #region Public Properties

    /// <summary>
    /// Gets the action id.
    /// </summary>
    [NotNull]
    public string ActionId
    {
      get
      {
        return this.GetActionId();
      }
    }

    /// <summary>
    /// Gets the title.
    /// </summary>
    [NotNull]
    public string Title
    {
      get
      {
        return this.GetTitle();
      }
    }

    #endregion

    #region Public Methods

    /// <summary>Determines whether this instance can execute the specified data context.</summary>
    /// <param name="context">The data context.</param>
    /// <returns><c>true</c> if this instance can execute the specified data context; otherwise, <c>false</c>.</returns>
    public abstract bool CanExecute([NotNull] DataContext context);

    /// <summary>Executes the specified data context.</summary>
    /// <param name="context">The context.</param>
    public abstract void Execute([NotNull] DataContext context);

    /// <summary>
    /// Gets or sets the action group.
    /// </summary>
    /// <returns>The action group.</returns>
    public abstract ActionGroup GetActionGroup();

    #endregion

    #region Methods

    /// <summary>Gets the action id.</summary>
    /// <returns>Returns the action id.</returns>
    [NotNull]
    protected abstract string GetActionId();

    /// <summary>Gets the title.</summary>
    /// <returns>Returns the title.</returns>
    [NotNull]
    protected abstract string GetTitle();

    #endregion
  }
}