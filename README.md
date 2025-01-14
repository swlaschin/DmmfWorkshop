# Workshop: Domain Modeling Made Functional

This is the git repository for the 16 hour (2 day) Domain Modeling Made Functional course.
All documentation and code will be available via this repo. Please refresh and re-pull if the contents have changed.

Before the course starts:

* Ensure your development environment is set up (see <a href=#Setup>Setup section</a> below)


## About this course

Functional programming and domain-driven design might not seem to be a good match,
but in fact functional programming can be an excellent approach to designing decoupled,
reusable systems with a rich domain model. This workshop will show you why.

This will be a hands-on workshop designed for beginners in functional programming.
We'll do lots of exercises that take us all the way from high-level design to low-level implementation.



## Who is this for?

This will be especially useful for people learning functional programming -- all concepts used in the workshop will be explained.

Previous development experience is recommended but no functional programming experience is needed.

## What you'll learn

* The concepts of functional programming
* How to represent the nouns and verbs of a domain using types and functions
* How to decouple the pure domain logic from the outside world
* How to ensure that constraints and business rules are captured in the design
* How to represent state transitions in the domain model
* How to build a business workflow from smaller components
* How to handle errors as part of the domain

By the end of the workshop you'll know how to build working solutions
with rich domain models, using only functional programming techniques.

## Program

Day 1

* Overview of DDD principles
  * Event storming
  * Persistence ignorance
  * Domain modelling with AND/OR
* Introduction to functional programming
  * Functions and types
  * Composition as the fundamental principle
* Domain Modeling with algebraic types
  * Records, choices, simple types, and functions
  * Modeling constraints, options
  * Making illegal states unrepresentable
  * Modeling states

Day 2:

* Error handling
  * Handling domain errors
  * Validating the input
* Keeping IO at the edges
* Functional architecture
  * The importance of bounded contexts
  * Deployment: Modular monolith vs microservices vs. functions


----
<h2 id=Setup>Setup</h2>

We will be using F# as our development language. No experience with F# needed. I will explain everything as we need it.

**1. Please install the F# compiler and an F#-friendly editor**

Please install the F# compiler and an F#-friendly editor such as:

* [JetBrains Rider](https://www.jetbrains.com/rider/) (free for non-commercial use on all platforms)
* Visual Studio Code (using the instructions at fsharp.org or [Ionide.io](https://ionide.io/Editors/Code/getting_started.html))


Platform-specific Installation instructions for F#:

* For Mac: https://fsharp.org/use/mac/
* For Windows: https://fsharp.org/use/windows/
* For Linux: https://fsharp.org/use/linux/

NOTE: I prefer to change the defaults in VSCode and Visual Studio to be less distracting. A list of the settings is available in the file [vscode_setup.md](vscode_setup.md)

I personally have not used Vim and Emacs for interactive coding in F#, but here are some links on how to use them:

* Emacs: [fsharp-mode](https://github.com/fsharp/emacs-fsharp-mode)
* Vim: [Ionide support](https://ionide.io/Editors/Vim/getting_started.html) and [a blog post explaining how to use it](https://www.codesuji.com/2021/04/10/F-Vim/).

If you would prefer not install on your local system, there are a number of ways to work on a remote VM or to develop in docker container, depending on your environment and editor. For example, VS Code has excellent support for [remote development using SSH](https://code.visualstudio.com/docs/remote/ssh) and for [editing in a local docker container](https://www.howtogeek.com/devops/how-to-edit-code-in-docker-containers-with-visual-studio-code/).

**2. Clone or download this repo locally**

If you already have git installed, you can use:

```
mkdir DmmfWorkshop
git clone https://github.com/swlaschin/DmmfWorkshop
```

If you don't have git installed, you can install it using [these instructions](https://github.com/git-guides/install-git). NOTE: we will not be using the features of git in the course, so installing it is not required

If you don't want to install git, you can download a zip file from the "code" button on the github website.


**3. Check that you can run F#**

* Open the code folder:
  * In VS Code, open the `/src` folder
  * In VisualStudio or JetBrains Rider, open the `DmmfWorkshop.sln` solution file, then "Solution Items"
* Open `00-HelloWorld.fsx` and follow instructions to check that F# is working


---

## Directory Structure

The folders in this repository are laid out as follows:

* /src/ contains the code
* /doc/ contains pdf documents
* /packages/ contains the external binary packages used by the code