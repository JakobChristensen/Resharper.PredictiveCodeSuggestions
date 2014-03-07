// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PredictiveCodeSuggestions")]
[assembly: AssemblyDescription("Suggests the next line of code by finding patterns in existing code. In many cases a line of code is often followed by a specific other line of code forming a common pattern of lines. Patent Pending.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Jakob Christensen")]
[assembly: AssemblyProduct("AutoTemplates")]
[assembly: AssemblyCopyright("Copyright © Jakob Christensen")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.2.0")]
[assembly: ActionsXml("AutoTemplates.Actions.xml")] 

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("Predictive Code Suggestions")]
[assembly: PluginDescription("Suggests the next line of code by finding patterns in existing code. In many cases a line of code is often followed by a specific other line of code forming a common pattern of lines. Patent Pending.")]
[assembly: PluginVendor("Jakob Christensen")]