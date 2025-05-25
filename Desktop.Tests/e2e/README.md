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