To define an application, one needs to implement a module with a pre-defined specification, [ConsoleApp](ConsoleApp.md). A console application can issue textual messages and receive textual input, which allows it to run under the interactive control in a special _console window_.

The base concept of the API is _job_. A job is an opaque value that causes some action to be performed by the runtime framework and may cause the state of the environment to change. This means that, although Aha! programs have no side effects during the computations, they can affect the environment by issuing jobs. A major difference from the traditional imperative style is that all the interactions with the environment occur outside the program.

The application receives all the dynamic information from the environment exclusively via _events_. This helps make the application's behavior consistent with the actual state of the environment because all the changes in the application's state (i.e. any variables that reflect the state of the environment) occur when the application is informed of the actual changes in the environment rather than based on the actions that the application performs and their presumed effect on the environment.

Jobs run asynchronously and independently, without 'knowing' anything about other jobs, and only communicate with the application/component by sending events. Once a job has been submitted, there is generally no direct way to affect its execution or cancel it, apart of the ability to stop all current jobs.

Jobs can be created using _components_. A component, similarly to an application, is defined by implementing a module with pre-defined specification, [ComponentDef](ComponentDef.md). When a component is added to the system, it is assigned a unique _classname_ by which it can be found.

Both applications and components have _input_ (_commands_) and _output_. Input commands are generated externally (e.g. user input). Output, on the opposite, is generated within the application/component and is passed outside using special jobs. A component/application can handle the output of the components that it instantiated.

A console application itself can be considered a component that receives string commands (user input) and produces string output.

To utilize a component, one needs to use module [Component](Component.md).

Using components is a productive way to implement large software projects and reuse/share code among projects. Each component can be developed as a separate project without relying on any details of projects that will be using it, i.e. applications and components are very loosely coupled.

Aha! applications and components are event-driven. When creating an application/component, the developer needs to choose an arbitrary type to be its event type. Events are triggered by the jobs issued by the application/component. The way events will be handled is completely up to the developer.

Both applications and components are defined by specifying their _behavior_. Behavior is a special user-defined object whose state is an array of jobs and having two actions: `handleInput` and `handleEvent`. `handleEvent` accepts a parameter of the event type chosen by the developer; `handleInput` accepts either an array of characters (for an application) or a command of the type chosen by the developer of the component. The behavior determines what jobs are sent to the environment in response to various events or input strings/commands.

Below is a brief description of the operational semantics of an Aha! application.

First, the behavior object is created using the application definition. Its state is an array of jobs; these jobs are submitted to the runtime framework for execution. The order of their execution is not known, they may be even performed in parallel. Some jobs may produce events; these events are directed to the issuing application's `handleEvent` action and a new application state (i.e. array of jobs) is produced. At any time when input is enabled, the end user may initiate input from the console, and the input is directed to the application's `handleInput` action, which also produces a new state (array of jobs). Whenever jobs are created in either of these ways, each job is submitted for execution, and so on. An application terminates in any of two situations: 1) input is disabled, and there are no unhandled events; 2) either the application's state function or one of the `handle` action fails. However, there is nothing that requires an application to ever terminate (even though an application always consists of terminating computations).

The [diagram](https://drive.google.com/file/d/0B7s-cEATWx1nekdGUHB4UmQwRVU/edit?usp=sharing) shows the flow of jobs, events and data in a console application with a single component. You may notice the following:
  * an application and a component have the same structure
  * there are three types of jobs (by target):
    1. jobs that send input to components
    1. jobs that return output
    1. jobs that control the system
  * there are three types of events (by origin):
    1. receiving an input command by a component or user input by an application
    1. receiving output from a component
    1. feedback from the system
  * the behavior always receives events/data and issues jobs
  * the system receives jobs and issues events

The proposed application model has some significant benefits:
  * it's easy to define new components, as an application can be easily turned into a component
  * components can be added dynamically; the application may run even if some components are missing on the system
  * components have no access to the application's private data and can affect its execution only via events, which provides a high level of security
  * job and application execution are totally asynchronous; the developer doesn't care about the time sequence of operations and events, only about the correct reaction to the events

Currently, the API consists of the following modules:

  * [Environment](Environment.md)
  * [Jobs](Jobs.md)
  * [Component](Component.md)
  * [FileIO](FileIO.md)
  * [FileIOtypes](FileIOtypes.md)
  * [ConsoleApp](ConsoleApp.md)
  * [ComponentDef](ComponentDef.md)

Modules [ConsoleApp](ConsoleApp.md) and [ComponentDef](ComponentDef.md) are not part of the package `API`; they are defined by the user within a user-defined package, but must conform to the provided specifications in order to be accepted by the runtime framework.