// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplatesService.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplatesService" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Options
{
  using JetBrains.Application;
  using JetBrains.Application.Communication;
  using JetBrains.Application.Settings;

  /// <summary>
  /// Defines the <see cref="AutoTemplatesService"/> class.
  /// </summary>
  [ShellComponent]
  public class AutoTemplatesService
  {
    #region Constants and Fields

    /// <summary>
    /// The my proxy settings reader field.
    /// </summary>
    private readonly WebProxySettingsReader myProxySettingsReader;

    /// <summary>
    /// The my settings store field.
    /// </summary>
    private readonly ISettingsStore mySettingsStore;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="AutoTemplatesService"/> class.</summary>
    /// <param name="proxySettingsReader">The proxy settings reader.</param>
    /// <param name="settingsStore">The settings store.</param>
    public AutoTemplatesService(WebProxySettingsReader proxySettingsReader, ISettingsStore settingsStore)
    {
      this.myProxySettingsReader = proxySettingsReader;
      this.mySettingsStore = settingsStore;
    }

    #endregion

    #region Public Methods

    /// <summary>Gets the client.</summary>
    /// <returns>Returns the client.</returns>
    public AutoTemplateSettings GetSettings()
    {
      var boundSettings = this.mySettingsStore.BindToContextTransient(ContextRange.Smart((lt, contexts) => contexts.Empty));

      var settings = boundSettings.GetKey<AutoTemplateSettings>(SettingsOptimization.OptimizeDefault);

      return settings;
    }

    #endregion
  }
}