A simple task manager focused on minimal management and some flow metrics.

# Docs
The UML usage style in this project is _sketching_, as described by Marting Fowler e.g. _UML Distilled, Martin Fowler (2009)_. This basically means that it is used as a quick sketch to clarify ideas in broad strokes, and is neither intended to be kept up to date nor describe the project fully. 

# Testing 

## Conventions

- Arrange Act Assert (AAA pattern).
  - Implicitly: by grouping statements and leaving empty lines between each part. 
  - Explicitly: when needed, by using comments to specify each part.

### Naming

- `Test API`: any test code that serves the purpose of making tests easier, more readable or maintainable in general by encapsulating knowledge or boilerplate code.
  - It may well be synonym of `test helpers`, `test utils`, etc.
