// ================================
// Check that F# is working
//
// Highlight/select the "printfn "Hello World" line below
// and run it interactively

printfn "Hello World"

// Using VisualStudio:
//   1. right click and do "Execute in Interactive"
//      shortcut: Alt+Enter
//   2. then check that "F# interactive" window appears and "Hello World" is output
//
// Using VS Code:
//   1. Ctrl+Shift+P and do "FSI: Send Selection"
//      shortcut: Alt+Enter
//   2. then check that "F# interactive" terminal appears and "Hello World" is output
//
// ================================

// expected output...
(*

Hello World
val it : unit = ()

*)

// ================================
// Killing the interactive session
//
// Sometimes the interactive session gets confused when you
// have redefined the same code many times.
// In that case, best to kill it and start again!
// ================================

// Using VisualStudio:
//   1. right click in F# interactive window and do "Reset Interactive Session"
//   2. then re-select and re-execute all your code
//
// Using VS Code:
//   1. Kill the F# terminal
//   2. then re-select and re-execute all your code




// ================================
// Troubleshooting
// ================================

(*
If you don't understand a compiler error, see doc/troubleshooting.pdf
*)

(*
If you see red squigglies when you don't expect to (due to missing references)

In all systems:
* close the IDE, delete the "obj" and "bin" directory, and restart

In Visual Studio and JetBrains Rider:
* clear any caches in the IDE

In VS Code:
1. go to VSCode settings
2. "Use Sdk Scripts": Use 'dotnet fsi' instead of 'fsi.exe'/'fsharpi' checkbox.
3. (optionally) "Dot Net Root": change to /usr/share/dotnet/ or whatever the path is

4. try uninstalling Ionide and reinstalling it


*)
