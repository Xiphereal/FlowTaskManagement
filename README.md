A simple task manager focused on minimal management and some flow metrics.

# Docs
The UML usage style in this project is _sketching_, as described by Marting Fowler e.g. _UML Distilled, Martin Fowler (2009)_. This basically means that it is used as a quick sketch to clarify ideas in broad strokes, and is neither intended to be kept up to date nor describe the project fully. 

In a real-life scenario, I'd discuss with the team if they are conformable with this approach or if another is needed.

# Decisions

- Many of the design choices, naming, not making extensive documentation on APIs (or public members, of any sort), have been taken with the idea that this is "pet project", and that my default and preferred way of working is collaboratively with practices like pair and ensemble programing. In this scenario, unless stated otherwise to avoid interruption of flow, discussions occur on the go and decisions are taken withing seconds (as well as code reviews). Also, knowledge silos are mostly mitigated in this way.
  - As the only person in the team is me, obviously neither pair nor ensemble programming has taken place.  
  - As with many things, I would discuss with the team if they prefer another way of working.

# Backend

In ASP.NET Core, using EntityFramework ORM for handling the persistence.

Conventions:
- **REST**. Since currently the application is a simple CRUD application, REST is a perfect match.

## Decisions

- **To code directly on the ASP.NET Controllers rather than decouple from them.** Due to simplicity of the use cases, it is much simpler to just orchestrate the [repository](https://martinfowler.com/eaaCatalog/repository.html) directly. In a real-life scenario, I would have considered making a POCO controller class (in terms of [MVC](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller)) as an early abstraction. This would enable testing without having to deal with any HTTP or ASP.NET concerns in tests that want to test business logic rather than these technological aspects; also, a better separation of concerns to reduce cognitive complexity as the codebase inevitably grows. 
- **To have the Backend and Frontend in the same solution, and referenced by project rather than NuGet packages.** Mainly, to keep things simple: all the projects are in the same place, changes are reflected instantly without needed to republish packages and avoid the extra complexity in the CI/CD for packaging and publishing NuGets. Again, in a real-life scenario, things would be different: versioning the backend would be imperative, and to maintain the Developer Experience a simple mechanism in the `.csprojs` for replacing package references with project references in `Debug`/development will be in place.

# Testing 

## Decisions

- **Strong focus on integration tests.** This has been taken due to the reason that project is a simple CRUD application, with almost no domain logic to be tested. The sparse control logic is tested alongside the main use cases. What remains is the backend->persistence and frontend->backend integration.

## Conventions

- Arrange Act Assert (AAA pattern).
  - Implicitly: by grouping statements and leaving empty lines between each part. 
  - Explicitly: when needed, by using comments to specify each part.

### Naming

- `Test API`: any test code that serves the purpose of making tests easier, more readable or maintainable in general by encapsulating knowledge or boilerplate code.
  - It may well be synonym of `test helpers`, `test utils`, etc.
