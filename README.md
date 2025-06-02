A simple task manager focused on minimal management and some flow metrics.

Use cases:

![Uses cases of a task management application](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/Xiphereal/SGjsdfgklj/refs/heads/trunk/Docs/UseCases.puml)

> [!NOTE]
> These are a beforehand sketch of the ideal use cases. Not all are, nor will be, implemented. 

# General decisions

- Many of the design choices, naming, not making extensive documentation on APIs (or public members, of any sort), have been taken with the idea that this is "pet project", and that my default and preferred way of working is collaboratively with practices like pair and ensemble programing. In this scenario, unless stated otherwise to avoid interruption of flow, discussions occur on the go and decisions are taken withing seconds (as well as code reviews). Also, knowledge silos are mostly mitigated in this way.
  - As the only person in the team is me, obviously neither pair nor ensemble programming has taken place.  
  - As with many things, I would discuss with the team if they prefer another way of working.
  
## Docs
The UML usage style in this project is _sketching_, as described by Marting Fowler e.g. _UML Distilled, Martin Fowler (2009)_. This basically means that it is used as a quick sketch to clarify ideas in broad strokes, and is neither intended to be kept up to date nor describe the project fully. 

> [!IMPORTANT]
> In a real-life scenario, I'd discuss with the team if they are conformable with this approach or if another is needed.

### Documenting architectural, design or almost any decision made

Regarding keeping track of pretty much any kind of decision that the team takes, I like to use the [Architectural Decision Records (ADRs), by Michael Nygard](https://www.cognitect.com/blog/2011/11/15/documenting-architecture-decisions). For example, why one technology is used in favor of others, the reason why the API does things in a certain way, or even a convention that the team has decided following.

Given the simplicity of this project and that I am the only contributor, I've decided not to use them.

### Tools

#### PlantUML

As a textual DSL for diagrams. [Refer to the official docs](https://plantuml.com/).

It's my tool of choice given it is free software (under GPL-3.0) and supports changing the arrows orientation (something `Mermaid` does not).

It's also worth mentioning [PlantText](https://www.planttext.com/) as an online PlantUML editor.

#### GitHub Alerts

Usage of [Alerts](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#alerts) in order to remark some info and lighten the burden of having to read extensive paragraphs.

# Backend

In ASP.NET Core, using EntityFramework ORM for handling the persistence.

Conventions:
- **REST**. Since currently the application is a simple CRUD application, REST is a perfect match.

## Decisions

- **To code directly on the ASP.NET Controllers rather than decouple from them.** Due to simplicity of the use cases, it is much simpler to just orchestrate the [repository](https://martinfowler.com/eaaCatalog/repository.html) directly. In a real-life scenario, I would have considered making a POCO controller class (in terms of [MVC](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller)) as an early abstraction. This would enable testing without having to deal with any HTTP or ASP.NET concerns in tests that want to test business logic rather than these technological aspects; also, a better separation of concerns to reduce cognitive complexity as the codebase inevitably grows. 
- **To have the Backend and Frontend in the same solution, and referenced by project rather than NuGet packages.** Mainly, to keep things simple: all the projects are in the same place, changes are reflected instantly without needed to republish packages and avoid the extra complexity in the CI/CD for packaging and publishing NuGets. Again, in a real-life scenario, things would be different: versioning the backend would be imperative, and to maintain the Developer Experience a simple mechanism in the `.csprojs` for replacing package references with project references in `Debug`/development will be in place.

# Testing 

- [e2e tests](./Desktop.Tests/e2e/README.md)

Conventions:
- **Arrange Act Assert (AAA pattern).**
  - Implicitly: by grouping statements and leaving empty lines between each part. 
  - Explicitly: when needed, by using comments to specify each part.

## Decisions

- **Strong focus on integration tests.** This has been taken due to the reason that project is a simple CRUD application, with almost no domain logic to be tested. The sparse control logic is tested alongside the main use cases. What remains is the backend->persistence and frontend->backend integration.

### Naming

- **Test API**: any test code that serves the purpose of making tests easier, more readable or maintainable in general by encapsulating knowledge or boilerplate code.
  - It may well be synonym of `test helpers`, `test utils`, etc.
