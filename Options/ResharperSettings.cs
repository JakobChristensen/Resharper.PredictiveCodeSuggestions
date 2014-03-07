// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResharperSettings.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ResharperSettings" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Options
{
  using JetBrains.Application.Settings;

  /// <summary>
  /// Defines the <see cref="ResharperSettings"/> class.
  /// </summary>
  public class ResharperSettings
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether [file saves].
    /// </summary>
    /// <value><c>true</c> if [file saves]; otherwise, <c>false</c>.</value>
    [SettingsEntry(40, "FileSaves")]
    public string FileSaves { get; set; }

    /// <summary>
    /// Gets or sets the max templates.
    /// </summary>
    /// <value>The max templates.</value>
    [SettingsEntry(7, "MaxTemplates")]
    public string MaxTemplates { get; set; }

    /// <summary>
    /// Gets or sets the min percentage.
    /// </summary>
    /// <value>The min percentage.</value>
    [SettingsEntry(0, "MinPercentage")]
    public string MinPercentage { get; set; }

    /// <summary>
    /// Gets or sets the non public assertion.
    /// </summary>
    /// <value>
    /// The non public assertion.
    /// </value>
    [SettingsEntry("", "Non-Public assertions")]
    public string NonPublicAssertion { get; set; }

    /// <summary>
    /// Gets or sets the occurances.
    /// </summary>
    /// <value>The occurances.</value>
    [SettingsEntry(0, "Occurances")]
    public string Occurances { get; set; }

    /// <summary>
    /// Gets or sets the public assertion.
    /// </summary>
    /// <value>
    /// The public assertion.
    /// </value>
    [SettingsEntry("", "Public assertions")]
    public string PublicAssertion { get; set; }

    /// <summary>
    /// Gets or sets the use complete statement.
    /// </summary>
    /// <value>The use complete statement.</value>
    [SettingsEntry(true, "UseCompleteStatement")]
    public bool UseCompleteStatement { get; set; }

    #endregion
  }
}