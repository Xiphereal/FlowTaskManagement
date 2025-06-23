# What is this?

A simple task manager focused on minimal management and some flow metrics.

Domain model:

![Domain model diagram of a task management application. It portraits a Project, Task, TaskProposal, a FlowMetric package and their relations](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/Xiphereal/SGjsdfgklj/refs/heads/trunk/Docs/DomainModel.puml)

Use cases:

![Uses cases of a task management application](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/Xiphereal/SGjsdfgklj/refs/heads/trunk/Docs/UseCases.puml)

Actors definition:

- Stakeholder: anyone interested in the project.
- Product owner: the role responsible for having the final word on product related decisions.
- User: anyone using the project manager application.

> [!NOTE]
> These are a beforehand sketch of the ideal use cases. Not all are, nor will be, implemented. 

# Architecture

![Architecture diagram. It portraits as Desktop, Backend, Database and their relations](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/Xiphereal/SGjsdfgklj/refs/heads/trunk/Docs/Architecture.puml)

## Frontend

There is a single one, for Windows desktop. It's called `Desktop`, and is implemented in [Windows Presentation Foundation (WPF)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview/). 

### Folder structure

Rather than using a technological layering, I prefer a folder structure that reflects the domain. This is to both to create a better mental model and for having more cohesion inside folders and less coupling between them. 

## Backend

In ASP.NET Core, using EntityFramework ORM for handling the persistence.

Conventions:
- **REST**. Since currently the application is a simple CRUD application, REST is a perfect match.

### Decisions

- **To code directly on the ASP.NET Controllers rather than decouple from them.** Due to simplicity of the use cases, it is much simpler to just orchestrate the [repository](https://martinfowler.com/eaaCatalog/repository.html) directly. In a real-life scenario, I would have considered making a POCO controller class (in terms of [MVC](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller)) as an early abstraction. This would enable testing without having to deal with any HTTP or ASP.NET concerns in tests that want to test business logic rather than these technological aspects; also, a better separation of concerns to reduce cognitive complexity as the codebase inevitably grows. 
- **To have the Backend and Frontend in the same solution, and referenced by project rather than NuGet packages.** Mainly, to keep things simple: all the projects are in the same place, changes are reflected instantly without needed to republish packages and avoid the extra complexity in the CI/CD for packaging and publishing NuGets. Again, in a real-life scenario, things would be different: versioning the backend would be imperative, and to maintain the Developer Experience a simple mechanism in the `.csprojs` for replacing package references with project references in `Debug`/development will be in place.

## A note on system design

The architecture is as such given this is a learning project. In a real world, [back-of-envelope calculations](https://en.wikipedia.org/wiki/Back-of-the-envelope_calculation) for expected concurrent users, requests per second and other consideration would have been taken into account to ensure scalability and act on the [consistency/availability tradeoff](https://en.wikipedia.org/wiki/CAP_theorem) (given partition tolerance is always desired) for the different use cases. This will probably require different architecture among use cases (e.g. [CQRS](https://martinfowler.com/bliki/CQRS.html): separating the writes and reads).

For instance, an API gateway will be in place between the front and the back, to decouple the front from the backend location. Also, it will probably be responsible for handling authentication and authorization. Likewise, it would be a great point to introduce a load balancer instead, in case we decide to go with horizontal scaling (though I prefer to squeeze vertical scaling as much as possible, due to costs and complexity concerns).

Again, simplicity has been the main drive point for this learning project.

## Packages version management

This project make use of the NuGet's [Central Package Management (CPM)](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management) in order to remove duplication of versions of certain packages that are used in different projects (some `ASP.NET` and testing ones).

For trickier scenarios (or when working with older versions than .NET 6) where CPM is not a good fit, another solution would be:
1. Central `.props` files in which versions for packages are defined once by means of [variables](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-properties?view=vs-2022).
2. Define a `Directory.Build.props` to import the previously created `.props`.
3. On each `.csproj`, reference the given variables for the target packages.

> [!NOTE]
> If desired, this also allows having different versions of the same package in different projects e.g. to allow gradually migrating to a new version of a third party library that introduces breaking changes. Mind issues with inconsistencies on transitives dependencies scenarios, though.

# Testing 

- [e2e tests](./Desktop.Tests/e2e/README.md)

Conventions:
- **Arrange Act Assert (AAA pattern).**
  - Implicitly: by grouping statements and leaving empty lines between each part. 
  - Explicitly: when needed, by using comments to specify each part.

## Decisions

- **Strong focus on integration tests.** This has been taken due to the reason that project is a simple CRUD application, with almost no domain logic to be tested. The sparse control logic is tested alongside the main use cases. What remains is the `backend->persistence` and `frontend->backend` integration.

### Naming

- **Test API**: any test code that serves the purpose of making tests easier, more readable or maintainable in general by encapsulating knowledge or boilerplate code.
  - It may well be synonym of `test helpers`, `test utils`, etc.

# Continuous Integration & Continuous Deployment (CI/CD) pipelines

## Tools
Since this project is hosted in GitHub, that the CI/CD pipelines requirements are quite simple and due to my familiarity with the tool, I've decided to use GitHub Actions.

## Workflow

The idea, is that after each `git push`, the CI pipelines are triggered in order to verify that all the projects in the solution successfully build and passes their tests. If, on the contrary, any pipeline fails, I [stop&fix](https://martinfowler.com/articles/continuousIntegration.html) to return to a healthy state as soon as possible (see _Continuous Delivery, Jez Humble and Dave Farley (2010)_).

## Pipelines

They are under `/.github/workflows/`.

# General decisions

Many of the design choices, naming, not making extensive documentation on APIs (or public members, of any sort), have been taken with the idea that this is "pet project", and that my default and preferred way of working is collaboratively with practices like pair and ensemble programing. In this scenario, unless stated otherwise to avoid interruption of flow, discussions occur on the go and decisions are taken withing seconds (as well as code reviews). Also, knowledge silos are mostly mitigated in this way.
- As the only person in the team is me, obviously neither pair nor ensemble programming has taken place.
- As with many things, I would discuss with the team if they prefer another way of working.

## Docs
The UML usage style in this project is _sketching_, as described by Marting Fowler e.g. _UML Distilled, Martin Fowler (2009)_. This basically means that it is used as a quick sketch to clarify ideas in broad strokes, and is neither intended to be kept up to date nor describe the project fully.

> [!IMPORTANT]
> In a real-life scenario, I'd discuss with the team if they are conformable with this approach or if another is needed.

### Documenting architectural, design or almost any decision made

Regarding keeping track of pretty much any kind of decision that the team takes, I like to use the [Architectural Decision Records (ADRs), by Michael Nygard](https://www.cognitect.com/blog/2011/11/15/documenting-architecture-decisions). For example, why one technology is used in favor of others, the reason why the API does things in a certain way, or even a convention that the team has decided following.

Given the simplicity of this project and that I am the only contributor, I've decided not to use them.

### Philosophy on comments & summaries

Since I value documentation, I consider having written documentation to its minimum, so that it conveys the relevant bits and important info about the matter at hand.

This means that comments in code are used only when necessary: if the information can be conveyed through member (class, method, variables, etc.) naming or by a test (executable specifications), it will be better documented as such. The same goes for any member documentation: `summaries`, `remarks`, etc.

Basically, this allows better [signal-to-noise ratio](https://en.wikipedia.org/wiki/Signal-to-noise_ratio): rather than having all members with redundant summaries, only the essential is there. In other words, if you see a "green stain" in the code (or in whatever colour you have configured in your IDE) you know it's something important and that you'd better read it. On the contrary, having all with redundant comments or summaries (set aside the out-of-date docs issue) trains your brain to just ignore it, so it's easier for that relevant info to go unnoticed.   

### Tools

#### PlantUML

Textual DSL for diagrams. [Refer to the official docs](https://plantuml.com/). It's my tool of choice given it is free software (under GPL-3.0) and supports changing the arrows orientation (something `Mermaid` does not). It's also worth mentioning [PlantText](https://www.planttext.com/) as an online PlantUML editor.

#### GitHub Alerts

Usage of [Alerts](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#alerts) in order to remark some info and lighten the burden of having to read extensive paragraphs.
