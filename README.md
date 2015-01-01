_**IMPORTANT NOTE:**_ This project is currently in beta and the documentation is currently incomplete. Please bear with us while the documentation is being written.

####SuperScript offers a means of declaring assets in one part of a .NET web solution and have them emitted somewhere else.


When developing web solutions, assets such as JavaScript declarations or HTML templates are frequently written in a location that differs from their desired output location.

For example, all JavaScript declarations should ideally be emitted together just before the HTML document is closed. And if caching is preferred then these declarations should be in an external file with caching headers set.

This is the functionality offered by SuperScript.



##A modular framework with options

SuperScript is modular: `SuperScript.Common` is the core which facilitates all other SuperScript modules but which won't produce any meaningful output on its own.

You'll likely want to download `SuperScript.Common` along with, for example, `SuperScript.Container` or `SuperScript.JavaScript`.

###Build your own modules

Furthermore, SuperScript has been designed so that other developers can pick up the baton and add their own modules.

The wiki has an explanation of how the framework can be built upon and how declarations can be manipulated in any way.


##What's in this project?

Below is a list of some of the more important classes in the `SuperScript.Common` project.

* `SuperScript.Declarables.DeclarationBase`

  This is an abstract class which every type of declaration should inherit from.

* `SuperScript.Declarations`

  The central store of instances of `DeclarationBase`. In other words, every time an implementation of `DeclarationBase` is instantiated, it should be added to the application-wide collection using one of the methods in this static class.
  
  Furthermore, this class also contains the methods for emitting declarations.

* `SuperScript.Emitters.IEmitter`

  An emitter is a pipeline through which instances of `DeclarationBase` are passed through and whose declared components determine how the declaration should be processed and rendered. 
  
  Emitters are added to a central, application-wide collection of `IEmitter` instances, which can be found on the `SuperScript.Declarations` class.

* Configuration classes

  SuperScript can be configured inside a `.config` file. This allows emitters and declarations to be modified without rebuilding the assembly. `SuperScript.Common` contains the core classes needed for this.


`SuperScript.Common` has been made available under the [MIT License](https://github.com/Supertext/SuperScript.Common/blob/master/LICENSE).
