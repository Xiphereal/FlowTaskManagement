The e2e tests follow this architecture:

![Architecture of e2e tests. It shows how a Test uses Action, which in turn uses from 1 up to several Drivers](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/Xiphereal/SGjsdfgklj/refs/heads/trunk/Desktop.Tests/e2e/e2eArchitecture.puml)

While currently the `Tests` just use a single `Action` due to the app simplicity, this allows for using different ones
to automate different applications or decompose one into different modules (e.g. user profile actions, project tree
structure actions, etc.).

To this matter, it's worth noting
the [Page Object pattern](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/).

# How to run these e2e tests

## Prerequisites

These tests use Appium as the automation framework.

For instructions in how to install everything, refer to:

- [Appium install](https://appium.io/docs/en/latest/quickstart/install/).
- [Appium Windows Driver plugin](https://github.com/appium/appium-windows-driver).
    - More specific, note
      the [usage requirements](https://github.com/appium/appium-windows-driver?tab=readme-ov-file#usage).

## Requisites

1. An Appium server is expected to be running on `http://127.0.0.1:4723/`.
2. The Desktop app is expected to be, relative to this test project, at `../Desktop`, in `Debug`, and for
   `net9.0-windows`.
3. The Backend is expected to be running.

# Good to know

Since Appium (WinAppDriver, really) relies on what Windows exposes in its automation layer, it's precisely useful to
know how to inspect what Windows exposes in order to use it to automate these kind of tests.

[Check `Inspect.exe`](https://learn.microsoft.com/en-us/windows/win32/winauto/inspect-objects) for this matter.

# Decisions

- **Usage of _accessibility ids_**.
    - Pros:
        - More robustness, since its purpose is exactly serve as an automation reference and is not subject to change
          as the value, internal name or class may change for other reasons.
    - Cons:
        - The production code is polluted just for testing reasons.
        - It cannot be used in dynamically generated elements (e.g. elements in a list). This forces to use another
          location strategies, thus making the code less homogeneous.
- **Attach the drivers to specific windows**.
    - Pros:
        - Easier to find elements by faster & more precise locator strategies (e.g. by accessibility ids, by class
          name, etc.) rather than relying on XPath.
    - Cons:
        - Tests implementation is coupled to which windows the app opens: if a form is now embedded and previously was a
          window on its own, the test will fail to locate its element and will need adaptative changes.