// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XElementExtensions.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   The x element extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Extensions.XElementExtensions
{
  using System;
  using System.ComponentModel;
  using System.Xml.Linq;
  using JetBrains.Annotations;

  /// <summary>The x element extensions.</summary>
  public static class XElementExtensions
  {
    #region Public Methods

    /// <summary>Elements the specified element.</summary>
    /// <param name="element">The element.</param>
    /// <param name="index">The index.</param>
    /// <returns>Returns the X element.</returns>
    [CanBeNull]
    public static XElement Element([NotNull] this XElement element, int index)
    {
      var elements = element.Elements();

      var count = 0;
      foreach (var e in elements)
      {
        if (count == index)
        {
          return e;
        }

        count++;
      }

      return null;
    }

    /// <summary>Gets the attribute date time.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Returns the attribute date time.</returns>
    public static DateTime GetAttributeDateTime([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName, DateTime defaultValue)
    {
      var value = GetAttributeValue(element, attributeName);
      if (string.IsNullOrEmpty(value))
      {
        return defaultValue;
      }

      DateTime result;
      if (DateTime.TryParse(value, out result))
      {
        return result;
      }

      return defaultValue;
    }

    /// <summary>Gets the attribute double.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Returns the attribute double.</returns>
    public static double GetAttributeDouble([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName, double defaultValue)
    {
      var value = GetAttributeValue(element, attributeName);
      if (string.IsNullOrEmpty(value))
      {
        return defaultValue;
      }

      double result;
      if (double.TryParse(value, out result))
      {
        return result;
      }

      return defaultValue;
    }

    /// <summary>Gets the attribute int.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Returns the attribute int.</returns>
    public static int GetAttributeInt([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName, int defaultValue)
    {
      var value = GetAttributeValue(element, attributeName);
      if (string.IsNullOrEmpty(value))
      {
        return defaultValue;
      }

      int result;
      if (int.TryParse(value, out result))
      {
        return result;
      }

      return defaultValue;
    }

    /// <summary>Gets the attribute int.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Returns the attribute int.</returns>
    public static long GetAttributeLong([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName, long defaultValue)
    {
      var value = GetAttributeValue(element, attributeName);
      if (string.IsNullOrEmpty(value))
      {
        return defaultValue;
      }

      long result;
      if (long.TryParse(value, out result))
      {
        return result;
      }

      return defaultValue;
    }

    /// <summary>Gets the attribute value.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns>Returns the attribute value.</returns>
    [NotNull]
    public static string GetAttributeValue([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName)
    {
      if (!element.HasAttributes)
      {
        return string.Empty;
      }

      var attribute = element.Attribute(attributeName);

      return attribute == null ? string.Empty : attribute.Value;
    }

    /// <summary>Gets the attribute value.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Returns the attribute value.</returns>
    [CanBeNull]
    public static string GetAttributeValue([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName, [CanBeNull] string defaultValue)
    {
      if (!element.HasAttributes)
      {
        return defaultValue;
      }

      var attribute = element.Attribute(attributeName);

      return attribute == null ? defaultValue : attribute.Value;
    }

    /// <summary>Gets the element value.</summary>
    /// <param name="element">The element.</param>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>Returns the element value.</returns>
    [NotNull]
    public static string GetElementValue([NotNull] this XElement element, [NotNull] [Localizable(false)] string elementName)
    {
      var e = element.Element(elementName);

      return e == null ? string.Empty : e.Value;
    }

    /// <summary>Determines whether the specified element has attribute.</summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns><c>true</c> if the specified element has attribute; otherwise, <c>false</c>.</returns>
    public static bool HasAttribute([NotNull] this XElement element, [NotNull] [Localizable(false)] string attributeName)
    {
      if (!element.HasAttributes)
      {
        return false;
      }

      return element.Attribute(attributeName) != null;
    }

    #endregion
  }
}