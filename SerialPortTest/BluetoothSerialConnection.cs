
using System.IO.Ports;
using Chetch.Utilities;

namespace SerialPortTest;

public class BluetoothSerialConnection : SerialPortConnection
{
    #region Fields
    String devicePath; //dev path or description (Windows)
    #endregion

    #region Constructors
    public BluetoothSerialConnection(String devicePath, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        : base(baudRate, parity, dataBits, stopBits)
    {
        this.devicePath = devicePath;
    }
    #endregion

    #region Methods
    protected override string GetPortName()
    {
        if (OperatingSystem.IsWindows())
        {
            var devices = SerialPortConnection.GetUSBDevices(devicePath);

            foreach (var f in devices)
            {
                SerialPortConnection.USBDeviceInfo di = SerialPortConnection.GetUSBDeviceInfo(f);
            }
            throw new Exception(String.Format("Cannot find device based on search term {0}", devicePath));
        }
        else if (OperatingSystem.IsLinux())
        {
            return devicePath;
        }
        else if (OperatingSystem.IsMacOS())
        {
            return devicePath;
        }
        else
        {
            throw new NotSupportedException(String.Format("Operating system is not supported"));
        }
    }

    protected override bool PortExists()
    {
        if (OperatingSystem.IsLinux())
        {
            if (!File.Exists(PortName))
            {
                return false;
            }
            else
            {
                String result = CMD.Exec("rfcomm", "-a");
                if (String.IsNullOrEmpty(result))
                {
                    return false;
                }
                else
                {
                    String connected = "channel 1 connected";
                    return result.Contains(connected);
                }
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            return false;
        }
        else
        {
            return base.PortExists();
        }
    }
    #endregion
}

