// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlNodeExtensions.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="XmlNodeExtensions" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Extensions.XmlNodeExtensions
{
  using System;
  using System.Xml;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="XmlNodeExtensions"/> class.</summary>
  public static class XmlNodeExtensions
  {
    #region Public Methods

    /// <summary>Gets the attribute string.</summary>
    /// <param name="node">The node.</param>
    /// <param name="name">The name.</param>
    /// <returns>Returns the attribute string.</returns>
    [NotNull]
    public static string GetAttributeString([NotNull] this XmlNode node, string name)
    {
      if (node == null)
      {
        throw new ArgumentNullException("node");
      }

      var attributes = node.Attributes;
      if (attributes == null)
      {
        return string.Empty;
      }

      var attribute = attributes[name];
      if (attribute == null)
      {
        return string.Empty;
      }

      return attribute.Value ?? string.Empty;
    }

    #endregion
  }
}