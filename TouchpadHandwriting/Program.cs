using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RawInputWithCS
{
    /// <summary>
    /// Raw input device info.
    /// </summary>
    enum RIDI : uint
    {
        DEVICENAME = 0x20000007,
        DEVICEINFO = 0x2000000b,
        PREPARSEDDATA = 0x20000005,
    }

    static class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint numberDevices, uint size);

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
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetRawInputDeviceInfo(IntPtr hDevice, RIDI uiCommand, IntPtr pData, ref uint pcbSize);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICELIST
        {
            public IntPtr hDevice;
            public uint dwType;
        }

        public static void GetListOfHIDDevices()
        {
            var dwSize = (Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            uint deviceCount = 0;

            if (GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
            {
                Console.WriteLine($"Successfully retreive number of devices: {deviceCount} devices");
                var pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
                GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

                for (var i = 0; i < deviceCount; i++)
                {
                    uint pcbSize = 0;

                    var rid = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(new IntPtr((pRawInputDeviceList.ToInt64() + (dwSize * i))), typeof(RAWINPUTDEVICELIST));

                    GetRawInputDeviceInfo(rid.hDevice, RIDI.DEVICENAME, IntPtr.Zero, ref pcbSize);

                    if (pcbSize <= 0)
                    {
                        Console.WriteLine($"Skipping device #{i} because device name is empty!");
                        continue;
                    }

                    var pData = Marshal.AllocHGlobal((int)pcbSize);

                    GetRawInputDeviceInfo(rid.hDevice, RIDI.DEVICENAME, pData, ref pcbSize);
                    var deviceName = Marshal.PtrToStringAnsi(pData);

                    Console.WriteLine($"Device #{i}'s name is {deviceName}");


                    Marshal.FreeHGlobal(pData);
                }

                Marshal.FreeHGlobal(pRawInputDeviceList);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Hello!");
            GetListOfHIDDevices();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
