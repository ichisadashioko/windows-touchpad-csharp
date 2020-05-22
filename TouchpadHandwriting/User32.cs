
using System;
using System.Runtime.InteropServices;

namespace TouchpadHandwriting
{
    public static class User32
    {
        public const string DLL_NAME = "user32.dll";

        public const int WM_INPUT = 0x00FF;

        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint numberDevices, uint size);

        /// <summary>
        /// Retrieves information about the raw input device.
        /// </summary>
        /// <param name="hDevice">A handle to the raw input device. This comes from the `hDevice` member of `RAWINPUTHEADER` or from `GetRawInputDeviceList`.</param>
        /// <param name="uiCommand">Specifies what data will be returned in `pData`.</param>
        /// <param name="pData">A pointer to a buffer that contains the information specified by `uiCommand`. If `uiCommand` is `RIDI_DEVICEINFO`, set the `cbSize` member of `RID_DEVICE_INFO` to `sizeof(RID_DEVICE_INFO)` before calling `GetRawInputDeviceInfo`.</param>
        /// <param name="pcbSize">The size, in bytes, of the data in `pData`.</param>
        /// <returns>If successful, this function returns a non-negative number indicating the number of bytes copied to `pData`.
        /// If `pData` is not large enough for the data, the function returns -1. If `pData` is `NULL`, the function returns a value of zero. In both of these cases, `pcbSize` is set to the minimum size required for the `pData` buffer.
        /// 
        /// Call `GetLastError` to identify any other errors.</returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, RIDI uiCommand, IntPtr pData, ref uint pcbSize);

        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdata
        /// </summary>
        /// <returns></returns>
        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern uint GetRawInputData(IntPtr hRawInput, RID uiCommand, [Out] IntPtr pData, [In, Out] ref uint pcbSize, int cbSizeHeader);

        [DllImport(DLL_NAME, SetLastError = true)]
        public static extern uint GetRawInputData(IntPtr hRawInput, RID uiCommand, [Out] out RAWINPUT pData, [In, Out] ref uint pcbSize, int cbSizeHeader);
    }
}