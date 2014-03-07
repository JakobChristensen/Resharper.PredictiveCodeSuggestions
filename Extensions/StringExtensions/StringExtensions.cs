// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="StringExtensions" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Extensions.StringExtensions
{
  using System;
  using System.Text;
  using System.Text.RegularExpressions;
  using JetBrains.Annotations;

  /// <summary>Defines the <see cref="StringExtensions"/> class.</summary>
  public static class StringExtensions
  {
    #region Constants and Fields

    /// <summary>The remove tags regex field.</summary>
    private static readonly Regex removeTagsRegex = new Regex("<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    #endregion

    #region Public Methods

    /// <summary>Clips the specified text.</summary>
    /// <param name="text">The text to clip.</param>
    /// <param name="length">The length.</param>
    /// <returns>The clipped text.</returns>
    public static string Clip([NotNull] this string text, int length)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text");
      }

      if (text.Length <= length)
      {
        return text;
      }

      var n = length;
      while (n >= 0 && char.IsLower(text[n]))
      {
        n--;
      }

      if (n < 0)
      {
        return text;
      }

      while (n >= 0 && !char.IsLower(text[n]))
      {
        n--;
      }

      if (n < 0)
      {
        return text;
      }

      return text.Substring(0, n);
    }

    /// <summary>Converts to pascal case.</summary>
    /// <param name="text">The identifier.</param>
    /// <returns>The to pascal case.</returns>
    [NotNull]
    public static string RemoveControlChars([NotNull] this string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text");
      }

      var s = new StringBuilder(text);

      for (var i = s.Length - 2; i >= 0; i--)
      {
        if (s[i] == '\\')
        {
          s.Remove(i, 2);
        }
      }

      return s.ToString();
    }

    /// <summary>Remove tags from a string.</summary>
    /// <param name="text">The text to remove tags from.</param>
    /// <returns>The text without tags.</returns>
    [NotNull]
    public static string RemoveTags([NotNull] this string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text");
      }

      return removeTagsRegex.Replace(text, string.Empty);
    }

    #endregion
  }
}