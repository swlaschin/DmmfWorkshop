## Tips for VS Code setup


For working with F# in VSCode, the plugin I prefer is [Ionide.io](https://ionide.io/Editors/Code/getting_started.html).

However there are some settings that I prefer to turn changes. Try turning them on and off yourself to see what you prefer. If you use Visual Studio, many of the options are available there too.

They are:

* **Editor > Minimap**. Turn this off if you want more space for coding. This applies to all of VS Code, not just F#.

* **FSharp > Line Lens**: This shows the function signature at the end of the line. You might find this useful when learning as alternative to the Code Lens signature below.  If you want to use the Code Lens features below, you will need to changed the default of `replaceCodeLens`

* **FSharp > Pipeline Hints**: This shows detail at each stage of a pipeline. You might find this useful when learning.

* **FSharp > Code Lenses > Signature**: This shows the signature of the function *above* the function.
   I prefer to have this turned off and if I want to see the signature, I hover over the function definition.
   However, you might find this useful when learning.

* **FSharp > Code Lenses > References**: This shows the number of references to the function above the function.
   I prefer to have this always turned off.

* **Editor > Inlay Hints: Enabled**: You might find these hints useful when learning. This will need to be turned on to show the inlay hints below.

* **FSharp > Inlay Hints > Type Annotations**: This shows the inferred types.
  You can read more about them [in this blog post](https://devblogs.microsoft.com/dotnet/fsharp-inline-hints-visual-studio/).
  You might find this useful when learning, but I find them distracting.

* **FSharp > Inlay Hints > Parameter Names**: As above, but for parameter names.


If you want to change the `settings.json` directly, my one looks like this:

```
{
    "editor.minimap.enabled": false,
    "FSharp.codeLenses.references.enabled": false,
    "FSharp.codeLenses.signature.enabled": false
    "editor.inlayHints.enabled": "off",
    "FSharp.inlayHints.typeAnnotations": false,
    "FSharp.inlayHints.parameterNames": false,
    "FSharp.pipelineHints.enabled": false,
 }
```