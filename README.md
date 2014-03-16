Predictive Code Suggestions
===========================
Predictive Code Suggestions is a plugin for JetBrains ReSharper, that helps you produce code by suggesting the next line of code.

<a href="https://www.youtube.com/watch?v=gGElb01GAsI" target="_blank">Watch the demo on YouTube</a>

<h3>Idea</h3>
When writing code there are often certain patterns to how the code is constructed. Certain statements are often followed by certain other statements, or function calls are followed by certain checks.

For instance, if a function can return `null`, you often want to do a null check.

<pre>
var text = Path.GetDirectoryName(fileName);
if (text == null) 
{
  ...
}
</pre>

When you write a call to `Path.GetDirectoryName(...)` wouldn't it be nice, if ReSharper suggested that the next line should be a null check.

Predictive Code Suggestions attempts to do this:

<img src="http://vsplugins.sitecore.net/downloads/github/pcs1.gif" alt="" />

<h3>Benefits</h3>
There are several benefits to suggesting the next line of code.
* Produce code faster
* Reduce typos
* Increase correctness

The two first benefits are rather obvious, but increasing correctness may need some explaining. 

In the example above, a new developer may not know, that the return value from `Path.GetDirectoryName` may be `null` and therefore may forget to do a null check. If the developer is using Predictive Code Suggestions, he will be made aware that calls to `Path.GetDirectoryName` is usually followed by a check for null. 

In this way the developer can learn the coding patterns in the solution.

<h3>Features</h3>
Predictive Code Suggestions offers 3 types of suggestions; predefined, manual and automatic.

<h4>Predefined suggestions</h4>
Predictive Code Suggestions comes with a number of predefined suggestions.

* Suggests a `return` statement, if the caret is at the end of a block.
* Suggests a `continue` or `break` statement, if the caret is at the end of a block inside a `for` or `foreach` loop.
* Suggests a `foreach` statement, if the caret is after a variable declaration of type Enumerable.
* Suggests a check for null, if the caret is a after a variable declaration where the variable might be `null`, typically if a variable is initialized by a function call. This works well with Code Annotation attributes.

<h4>Manual suggestions</h4>
Predictive Code Suggestions allows you to create your own suggestions by providing highly context-aware Live Templates.

When you open the the ReSharper Generate popup inside a text editor, you will see a new option at the bottom: "Create live template". Selecting this option opens a new menu, where you select the context you want to use.

Predictive Code Suggestions offers these contexts:
* After variable of type &lt;type name>
* After enumerable of type &lt;type name>
* After call to &lt;method name>
* Before variable of type &lt;type name>
* Before call to &lt;method name>
* Beginning of function
* Class member
* Empty file
* End of statement block
* Inside method
* Interface member
* Struct member
* Switch case
* Class, struct, interface or enum

After selecting the context, the ReSharper Live Template editor opens. You will notice that the `Shortcut` field starts with "Do not change". This is so that Predictive Code Suggestions may differentiate your context-aware Live Templates from standard Live Templates. As the text suggests, you should not change the `Shortcut`.

You may use some of the predefined macros offered by Predictive Code Suggestions, but remember to delete the comments before saving the Live Template.

When you open Generate popup menu, you will see your context-aware Live Template at the very top.

<h4>Automatic suggestions</h4>

<b>Please notice that automatic suggestions may impact performance when working with huge solutions or projects. And huge projects mean projects with millions and millions of lines of code.</b>

Predictive Code Suggestions analyzes your code in the background to determine common patterns. When a pattern is detected, Predictive Code Suggestions offers the suggestions as a Live Template in the Generate popup menu.

Over time Predictive Code Suggestions "learn" how you write code and is able to suggest Live Templates, that are automatically generated.

The image below shows a part of Options page for Predictive Code Suggestions. The highlighted line shows that in the code, there are 74 call to `System.Collections.Generics.List.Add` and of these 74 calls, 26% are followed by a `return` statement. If another call to the `Add` is made, there is a good chance, it should be followed by a `return` statement.

<img src="http://vsplugins.sitecore.net/downloads/github/pcs2.png" alt="" />

As mentioned, the analysis takes place in the background, but you can trigger an analysis of the entire solution either from the Options page or from the ReSharper main menu. 

When a file is saved, it is put into a queue of files, that will be analyzed at regular intervals.

The Options page allows you to tweak how the suggestions are generated.
* Max number of statements per statements limits the number of suggestions in the Generate popup menu, picking the suggestions with highest probability.
* Number of occurances determines the minimum number of occurances needed before a suggestion is generated.
* Minimum percentage determines how big a percentage the suggestion must have before being generated.

<img src="http://vsplugins.sitecore.net/downloads/github/pcs3.png" alt="" />

<h4>Text Editor Integration</h4>
Predictive Code Suggestions are integrated in the ReSharper Generate popup menu, but you may also choose to integrate it into the ReSharper Complete Statement action.

This provides a very slick development experience, as you can generate a lot of code by just pressing `Ctrl+Shift+Enter` and selecting code from the popup menu.

<h3>Patent</h3>
Sitecore A/S holds a patent for Predictive Code Suggestions.
