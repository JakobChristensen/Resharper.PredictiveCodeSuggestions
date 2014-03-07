// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellManager.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   Defines the <see cref="ShellManager" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PredictiveCodeSuggestions.Shell
{
  using System;
  using JetBrains.ActionManagement;
  using JetBrains.Annotations;
  using JetBrains.Application;
  using JetBrains.DataFlow;
  using JetBrains.Threading;

  /// <summary>Defines the <see cref="ShellManager"/> class.</summary>
  [ShellComponent]
  public class ShellManager
  {
    #region Static Fields

    /// <summary>The instance.</summary>
    private static ShellManager instance;

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate DocumentSavedEventHandler
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    public delegate void DocumentSavedEventHandler([NotNull] string fileName);

    #endregion

    #region Public Events

    /// <summary>
    /// Occurs when [document saved].
    /// </summary>
    public event DocumentSavedEventHandler DocumentSaved;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    [NotNull]
    public static ShellManager Instance
    {
      get
      {
        return instance ?? (instance = new ShellManager());
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Executes the reentrancy guard.</summary>
    /// <param name="key">The key.</param>
    /// <param name="action">The action.</param>
    public static void ExecuteReentrancyGuard([NotNull] string key, [NotNull] Action action)
    {
      if (key == null)
      {
        throw new ArgumentNullException("key");
      }

      if (action == null)
      {
        throw new ArgumentNullException("action");
      }

      Shell.Instance.GetComponent<IThreading>().ReentrancyGuard.ExecuteOrQueue(key, action);
    }

    /// <summary>Gets the component.</summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>Returns the component.</returns>
    [NotNull]
    public static T GetComponent<T>() where T : class
    {
      return Shell.Instance.GetComponent<T>();
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public void Init()
    {
      var actionManager = Shell.Instance.GetComponent<ActionManager>();

      var action = actionManager.TryGetAction("CompleteStatement") as IUpdatableAction;
      if (action != null)
      {
        action.AddHandler(EternalLifetime.Instance, new CompleteStatementContextAction());
      }
    }

    /// <summary>Raises the document changed.</summary>
    /// <param name="fileName">Name of the file.</param>
    public void RaiseDocumentChanged(string fileName)
    {
      var saved = this.DocumentSaved;
      if (saved != null)
      {
        saved(fileName);
      }
    }

    #endregion
  }
}