# Workshop: Domain Modeling Made Functional

Functional programming and domain-driven design might not seem to be a good match,
but in fact functional programming can be an excellent approach to designing decoupled,
reusable systems with a rich domain model. This workshop will show you why.

This will be a hands-on workshop designed for beginners in functional programming.
We'll do lots of exercises and build some small projects that take us all the way
from high-level design to low-level implementation.


## Who is this for?

This will be especially useful for people learning functional programming -- all concepts used in the workshop will be explained. Previous development experience is recommended.

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
  * The importance of bounded contexts
  * Persistence ignorance
  * Domain modelling with AND/OR
  * Exercises: Model some common domains 
* Introduction to functional programming
  * Functions and types
  * Composition as the fundamental principle
  * Exercises: Functional programming
* Functional architecture
  * Working with IO
* Domain Modeling with algebraic types
  * Records, choices, simple types (SCU), and functions
  * Modeling constraints, options
  * Making illegal states unrepresentable
  * Modeling states
  * Exercises: Model the common domains using types

Day 2:

* Programming a workflow
  * Exercises: building workflows
* Error handling
  * Handling domain errors
  * Composing complex functions
  * Exercises: working with errors
* Keeping IO at the edges
  * Validating the input
  * Exercises: adding IO
* Evolving the domain:
  * Exercises: Dealing with changes in requirements

## Prerequisites

We will be using F# as our development language. No experience with F# needed.
Please install the F# compiler and an F#-friendly editor such as Visual Studio Code using the instructions at fsharp.org or http://ionide.io

### F# Setup with VS Code
* Install VS Code
* Install VS Code extensions: “Ionide-fsharp”, “Ionide-paket”
* Follow instructions on [Ionide-fsharp page](http://ionide.io/#20150804gettingstarted)

### F# Setup with Visual Studio
* Install workload adding F# support.


## Directory Structure

These folders contain training material for a 4 day course in F#

* /src/ contains the code
* /doc/ contains pdf documents
* /packages/ contains the external binary packages used by the code