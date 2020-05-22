# Reading HID raw input

__Goal__

Retrieve absolute X and Y touch position.

# Windows API

Windows Precision Touchpad will expose 3 HID top-level collections.

- Windows Precision Touchpad
- Mouse
- Configuration

These collections will show up as 3 separated devices when calling `GetRawInputDeviceList`.

I think we need to explore the Windows Precision Touchpad collection.

Each HID collections will have the `Usage Page ID` and the `Usage ID` to identify the purpose of HID device. These numbers are set by the HID device. We can use these numbers to filter and identify the touchpad device.

[Windows Precision Touchpad collection](https://docs.microsoft.com/en-us/windows-hardware/design/component-guidelines/windows-precision-touchpad-required-hid-top-level-collections#windows-precision-touchpad-collection) will have `Usage Page ID = 0x0D` and `Usage ID = 0x05`.

I retrieved all the HID devices and it seems that my touchpad is from Synaptic (`HID#SYNA7DB5`) and it exposes 4 HID top-level collections.

There is an optional HID top-level collection and it might be used for firmware update.

```
Successfully retreive number of devices: 9 devices
Device #4's name is \\?\HID#SYNA7DB5&Col04#5&22d87139&0&0003#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #5's name is \\?\HID#SYNA7DB5&Col03#5&22d87139&0&0002#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #6's name is \\?\HID#SYNA7DB5&Col02#5&22d87139&0&0001#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #8's name is \\?\HID#SYNA7DB5&Col01#5&22d87139&0&0000#{378de44c-56ef-11d1-bc8c-00a0c91405dd}
```

# References

- https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputdevice
- https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerrawinputdevices

# Notes

- `ULONG` is the same as `UINT`. That's why I use `uint` for `ULONG` in C# `struct`.