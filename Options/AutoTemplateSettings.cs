// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoTemplateSettings.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="AutoTemplateSettings" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Options
{
  using JetBrains.Application.Settings;

  /// <summary>
  /// Defines the <see cref="AutoTemplateSettings"/> class.
  /// </summary>
  [SettingsKey(typeof(EnvironmentSettings), "Auto Templates Settings")]
  public class AutoTemplateSettings : ResharperSettings
  {
    #region Public Methods

    /// <summary>Gets the max templates.</summary>
    /// <returns>Returns the max templates.</returns>
    public int GetMaxTemplates()
    {
      int result;
      return int.TryParse(this.MaxTemplates, out result) ? result : 7;
    }

    /// <summary>Mins the percentage.</summary>
    /// <returns>Returns the percentage.</returns>
    public int GetMinPercentage()
    {
      int result;
      return int.TryParse(this.MinPercentage, out result) ? result : 20;
    }

    /// <summary>Gets the occurances.</summary>
    /// <returns>Returns the occurances.</returns>
    public int GetOccurances()
    {
      int result;
      return int.TryParse(this.Occurances, out result) ? result : 3;
    }

    /// <summary>
    /// Gets the file saves.
    /// </summary>
    /// <returns>Returns the int32.</returns>
    public int GetFileSaves()
    {
      int result;
      return int.TryParse(this.FileSaves, out result) ? result : 40;
    }

    /// <summary>
    /// Gets the use complete statement.
    /// </summary>
    /// <returns><c>true</c> if use complete statement, <c>false</c> otherwise</returns>
    public bool GetUseCompleteStatement()
    {
      return this.UseCompleteStatement;
    }

    #endregion
  }
}