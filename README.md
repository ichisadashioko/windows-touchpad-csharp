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
Device #0's name is \\?\HID#VID_18F8&PID_0F97&MI_01&Col01#7&23146815&0&0000#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}
Device #1's name is \\?\HID#VID_18F8&PID_0F97&MI_00#7&c2d5ad&0&0000#{378de44c-56ef-11d1-bc8c-00a0c91405dd}
Device #2's name is \\?\HID#VID_18F8&PID_0F97&MI_01&Col03#7&23146815&0&0002#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #3's name is \\?\HID#VID_18F8&PID_0F97&MI_01&Col02#7&23146815&0&0001#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #4's name is \\?\HID#SYNA7DB5&Col04#5&22d87139&0&0003#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #5's name is \\?\HID#SYNA7DB5&Col03#5&22d87139&0&0002#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #6's name is \\?\HID#SYNA7DB5&Col02#5&22d87139&0&0001#{4d1e55b2-f16f-11cf-88cb-001111000030}
Device #7's name is \\?\ACPI#MSFT0001#4&3a5de18a&0#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}
Device #8's name is \\?\HID#SYNA7DB5&Col01#5&22d87139&0&0000#{378de44c-56ef-11d1-bc8c-00a0c91405dd}
```