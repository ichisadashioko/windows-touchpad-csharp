
using System;
using System.Runtime.InteropServices;

namespace TouchpadHandwriting
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUTHEADER
    {
        public uint dwType;
        public uint dwSize;
        public IntPtr hDevice;
        public IntPtr wParam;
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawmouse
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct RAWMOUSE
    {
        [FieldOffset(0)]
        public ushort usFlags;
        [FieldOffset(4)]
        public uint ulButtons;
        [FieldOffset(4)]
        public ushort usButtonFlags;
        [FieldOffset(6)]
        public ushort usButtonData;
        [FieldOffset(8)]
        public uint ulRawButtons;
        [FieldOffset(12)]
        public int lLastX;
        [FieldOffset(16)]
        public int lLastY;
        [FieldOffset(20)]
        public uint ulExtraInformation;
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawkeyboard
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWKEYBOARD
    {
        public ushort MakeCode;
        public ushort Flags;
        public ushort Reserved;
        public ushort VKey;
        public uint Message;
        public uint ExtraInformation;
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawhid
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RAWHID
    {
        public int dwSizeHid;
        public int dwCount;
        public byte bRawData;

        public override string ToString()
        {
            return $"dwSizeHid: {dwSizeHid} - dwCount: {dwCount} - bRawData: {bRawData}";
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RAWINPUTDATA
    {
        [FieldOffset(0)]
        public RAWMOUSE mouse;
        [FieldOffset(0)]
        public RAWKEYBOARD keyboard;
        [FieldOffset(0)]
        public RAWHID hid;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RAWINPUT
    {
        public RAWINPUTHEADER header;
        public RAWINPUTDATA data;
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdata
    /// </summary>
    public enum RID : uint
    {
        HEADER = 0x10000005, // Get the header information from the `RAWINPUT` structure.
        INPUT = 0x10000003, // Get the raw data from the `RAWINPUT` structure.
    }

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