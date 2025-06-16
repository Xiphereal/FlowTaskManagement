<video src="../../Docs/E2eTestsExecution.mkv" width="720"></video>

The e2e tests follow this architecture:

![Architecture of e2e tests. It shows how a Test uses Actions, which in turn uses from 1 up to several Drivers](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/Xiphereal/SGjsdfgklj/refs/heads/trunk/Desktop.Tests/e2e/e2eArchitecture.puml)

- `Test`: a test case itself, which has the executable specifications.
- `Actions`: a set of actions that the user (thus the automated test) can perform on the application. By extension, they
  have a business relevance. Each action may orchestrate different steps by using a `Driver`. Examples of actions may be
  _login_ or _add item to the shopping cart_.
- `Driver`: the low level technological automation that actually knows how to perform the different actions. It has the
  knowledge of how to find a certain element. Examples of steps perform by a `Driver`: _find the
  InputBox for the username_, _write the given text_, _find the InputBox for the password_ or _click on the login
  button_.

While currently the `Tests` just use a single `Action` due to the app simplicity, this allows for using different ones
to automate different applications or decompose one into different modules (e.g. user profile actions, project tree
structure actions, etc.). Also, by having the `Actions` and `Driver` separated, changes in the
technological spectrum such as a Button changing its id or location or a Label its text does not cascade into the
business `Actions` (unless the workflow has actually changed).

I don't remember if this 3-layer architecture style for e2e tests has been already defined and name by any other person
before, but it has been inspired for sure by both
the [Page Object pattern](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/) and
Dave Farley in [How to Write Acceptance Tests -
Modern Software Engineering
](https://youtu.be/JDD5EEJgpHU?t=376&si=xwFJav4upgoNv6qE).

# Decisions

- **Usage of _[accessibility ids](https://www.waldo.com/blog/appium-accessibility-id)_**.
    - Pros:
        - More robustness, since its purpose is exactly serve as an automation reference and is not subject to change
          as the value, internal name or class may change for other reasons.
        - Also, pretty much the reasons mentioned [here](https://www.browserstack.com/guide/locators-in-appium).
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
- **Splitting the execution of e2e tests from the rest**. In the CI, the e2e tests are executed separately from the rest
  due to their specific requirements. To do so, any `namespace` that contains `e2e` are excluded from the common test
  runs. This is a quick approach, but relies on following the convention of naming the e2e namespaces with `e2e`.
  Another approach would be to use different `csproj` or _test annotations_; relying on the `namespace` has been chosen
  for convenience giving up flexibility and a better separation of concerns.

# How to run these e2e tests

## Prerequisites

These tests use [Appium](https://appium.io/docs/en/latest/) as the automation framework.

For instructions in how to install everything, refer to:

- [Appium install](https://appium.io/docs/en/latest/quickstart/install/).
- [Appium Windows Driver plugin](https://github.com/appium/appium-windows-driver).
    - More specific, note
      the [usage requirements](https://github.com/appium/appium-windows-driver?tab=readme-ov-file#usage).

# Good to know

Since Appium ([WinAppDriver](https://github.com/microsoft/WinAppDriver), really) relies on what Windows exposes in its
automation layer, it's precisely useful to
know how to inspect what Windows exposes in order to use it to automate these kind of tests.

[Check `Inspect.exe`](https://learn.microsoft.com/en-us/windows/win32/winauto/inspect-objects) for this matter.
