using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TouchpadHandwriting
{
    public class TouchpadInput : NativeWindow
    {
        public TouchpadInput(IntPtr hWnd)
        {
            AssignHandle(hWnd);
            Program.RegisterWindowsPrecisionTouchpad(hWnd);
        }

        public void ProcessRawInputMessage(Message m)
        {
            Console.WriteLine($"WParam: {m.WParam}");
            Console.WriteLine($"LParam: {m.LParam}");

            uint dwSize = 0;
            User32.GetRawInputData(m.LParam, RID.INPUT, IntPtr.Zero, ref dwSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));

            RAWINPUT raw;

            if (User32.GetRawInputData(m.LParam, RID.INPUT, out raw, ref dwSize, Marshal.SizeOf(typeof(RAWINPUTHEADER))) != dwSize)
            {
                Console.WriteLine($"GetRawInputData does not return the correct size!");
                return;
            }

            switch (raw.header.dwType)
            {
                case (uint)RIM.TYPEMOUSE:
                    Console.WriteLine("Received expected mouse input!");
                    return;
                case (uint)RIM.TYPEKEYBOARD:
                    Console.WriteLine("Received expected keyboard input!");
                    return;
                case (uint)RIM.TYPEHID:
                    break;
                default:
                    Console.WriteLine($"Unknown RAWINPUT header {raw.header.dwType}!");
                    Console.WriteLine($"Last error code: {Marshal.GetLastWin32Error()}");
                    return;
            }

            Console.WriteLine(raw.data.hid);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case User32.WM_INPUT:
                    ProcessRawInputMessage(m);
                    break;
            }

            base.WndProc(ref m);
        }
    }

    public static class Program
    {
        public static RegistryKey GetRegistryKey(string name)
        {
            // sample name
            // \\?\HID#VID_18F8&PID_0F97&MI_01&Col01#7&23146815&0&0000#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}
            var split = name.Substring(4).Split('#');

            var classCode = split[0];
            var subClassCode = split[1];
            var protocolCode = split[2];

            return Registry.LocalMachine.OpenSubKey($"System\\CurrentControlSet\\Enum\\{classCode}\\{subClassCode}\\{protocolCode}");
        }

        public static void GetListOfHIDDevices()
        {
            var dwSize = (Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            uint deviceCount = 0;

            if (User32.GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
            {
                Console.WriteLine($"Successfully retreive number of devices: {deviceCount} devices");
                var pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
                User32.GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

                for (var i = 0; i < deviceCount; i++)
                {
                    uint pcbSize = 0;

                    var rid = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(new IntPtr((pRawInputDeviceList.ToInt64() + (dwSize * i))), typeof(RAWINPUTDEVICELIST));

                    User32.GetRawInputDeviceInfo(rid.hDevice, RIDI.DEVICENAME, IntPtr.Zero, ref pcbSize);

                    if (pcbSize <= 0)
                    {
                        Console.WriteLine($"Skipping device #{i} because device name is empty!");
                        continue;
                    }

                    var pData = Marshal.AllocHGlobal((int)pcbSize);

                    User32.GetRawInputDeviceInfo(rid.hDevice, RIDI.DEVICENAME, pData, ref pcbSize);
                    var deviceName = Marshal.PtrToStringAnsi(pData);

                    Console.WriteLine($"Device #{i}'s name is {deviceName}");

                    switch (rid.dwType)
                    {
                        case (uint)RIM.TYPEMOUSE:
                            Console.WriteLine($"Device #{i} is a mouse!");
                            break;
                        case (uint)RIM.TYPEKEYBOARD:
                            Console.WriteLine($"Device #{i} is a keyboard!");
                            break;
                        case (uint)RIM.TYPEHID:
                            Console.WriteLine($"Device #{i} is a HID device!");
                            break;
                        default:
                            Console.WriteLine($"Device #{i} has unknown device type {rid.dwType}!");
                            break;
                    }

                    var registryKey = GetRegistryKey(deviceName);
                    var valueNames = registryKey.GetValueNames();
                    foreach (var valueName in valueNames)
                    {
                        Console.WriteLine($"- {valueName}: {registryKey.GetValue(valueName)}");
                    }

                    Marshal.FreeHGlobal(pData);
                }

                Marshal.FreeHGlobal(pRawInputDeviceList);
            }
        }

        /// <summary>
        /// Register for Windows Precision Touchpad HID collection.
        /// </summary>
        /// <param name="hWnd">A instance of System.Windows.Forms.Control.Handle</param>
        public static void RegisterWindowsPrecisionTouchpad(IntPtr hWnd)
        {
            var rawInputDevices = new RAWINPUTDEVICE[1];
            rawInputDevices[0].usUsagePage = 0x0D;
            rawInputDevices[0].usUsage = 0x05;
            rawInputDevices[0].dwFlags = (uint)RIDEV.INPUTSINK;
            rawInputDevices[0].hwndTarget = hWnd;

            if (User32.RegisterRawInputDevices(rawInputDevices, (uint)rawInputDevices.Length, (uint)Marshal.SizeOf(rawInputDevices[0])))
            {
                Console.WriteLine($"Successfully register for Windows Precision Touchpad collection!");
            }
            else
            {
                Console.WriteLine($"Failed to register for Windows Precision Touchpad collection!");
                int lastErrorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"Error code: {lastErrorCode}");
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("Hello!");
            GetListOfHIDDevices();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
