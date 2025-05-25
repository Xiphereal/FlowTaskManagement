# Prerequisites to run these e2e tests

These tests use Appium as the automation framework.

For instructions in how to install everything, refer to:

- [Appium install](https://appium.io/docs/en/latest/quickstart/install/).
- [Appium Windows Driver plugin](https://github.com/appium/appium-windows-driver).
    - More specific, note
      the [usage requirements](https://github.com/appium/appium-windows-driver?tab=readme-ov-file#usage).

# Requisites to run these e2e tests

1. An Appium server is expected to be running on `http://127.0.0.1:4723/`.
2. The Desktop app is expected to be, relative to this test project, at `../Desktop`, in `Debug`, and for
   `net9.0-windows`.
3. The Backend is expected to be running.

# Good to know

Since Appium (WinAppDriver, really) relies on what Windows exposes in its automation layer, it's precisely useful to
know how to inspect what Windows exposes in order to use it to automate these kind of tests.

[Check `Inspect.exe`](https://learn.microsoft.com/en-us/windows/win32/winauto/inspect-objects) for this matter. 