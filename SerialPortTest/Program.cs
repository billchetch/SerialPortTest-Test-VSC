using System.Diagnostics;
using Chetch.Utilities;

namespace SerialPortTest;

class Program
{

    static void Main(string[] args)
    {

        //String portName = "/dev/cu.usbmodem1401"; //apple
        //String portName = "/dev/ttyACM0"; //raspberry pi
        //String devicePath = "/dev/rfcomm0";
        String devicePath = "/dev/cu.Bluetooth-Incoming-Port";

        BluetoothSerialConnection? btsc = null;
        try
        {
            btsc = new BluetoothSerialConnection(devicePath);

            btsc.Connected += (sender, connected) =>
            {
                Console.WriteLine("Serial port connected {0}", connected);
            };
            btsc.DataReceived += (sender, data) =>
            {
                Console.WriteLine("Data {0} bytes received", data.Length);
            };
            ConsoleHelper.PK("Press a key to try and connect ");
            btsc.Connect();
            ConsoleHelper.PK("Press a key to send");
            byte[] data = new byte[10];
            data[0] = 1;
            data[1] = 2;
            for (int i = 0; i < 2; i++)
            {
                btsc.SendData(data);
                ConsoleHelper.PK("Press to send again");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            ConsoleHelper.PK("Press a key to end");
            if (btsc.IsConnected)
            {
                btsc.Disconnect();
            }
        }
    }   
}