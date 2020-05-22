
using System;
using System.Runtime.InteropServices;

namespace TouchpadHandwriting
{
    /// <summary>
    /// Raw input device info.
    /// </summary>
    public enum RIDI : uint
    {
        DEVICENAME = 0x20000007,
        DEVICEINFO = 0x2000000b,
        PREPARSEDDATA = 0x20000005,
    }

    public enum RIM : uint
    {
        TYPEMOUSE = 0,
        TYPEKEYBOARD = 1,
        TYPEHID = 2,
    }

    /// <summary>
    /// Contains information about a raw input device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICELIST
    {
        /// <summary>
        /// A handle to the raw input device.
        /// </summary>
        public IntPtr hDevice;

        /// <summary>
        /// The type of device. See `RIM`.
        /// </summary>
        public uint dwType;
    }

    public enum RIDEV : uint
    {
        APPKEYS = 0x00000400,
        CAPTUREMOUSE = 0x00000200,
        DEVNOTIFY = 0x00002000,
        EXCLUDE = 0x00000010,
        EXINPUTSINK = 0x00001000,
        INPUTSINK = 0x00000100, // If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that `hwndTarget` must be specified.
        NOHOTKEYS = 0x00000200,
        NOLEGACY = 0x00000030,
        PAGEONLY = 0x00000020,
        REMOVE = 0x00000001,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTDEVICE
    {
        public ushort usUsagePage;
        public ushort usUsage;
        public uint dwFlags;
        public IntPtr hwndTarget;
    }
}