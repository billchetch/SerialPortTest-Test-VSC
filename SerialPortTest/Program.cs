using System.IO.Ports;
using Chetch.Utilities;


namespace SerialPortTest;

class Program
{
    class BTSerialConnection : SerialPortConnection
    {

        public BTSerialConnection(int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(baudRate, parity, dataBits, stopBits) { }

        protected override string GetPortName()
        {
            //return "COM1"; //windows
            //return "/dev/tty.Bluetooth-Incoming-Port";
            return "/dev/cu.Bluetooth-Incoming-Port";
        }
    }

    static void Main(string[] args)
    {

        //String portName = "/dev/cu.usbmodem1401"; //apple
        //String portName = "/dev/ttyACM0"; //raspberry pi
        try
        {
            BTSerialConnection btsp = new BTSerialConnection(115200, System.IO.Ports.Parity.None, 8, StopBits.One);
            btsp.Connected += (sender, connected) =>
            {
                Console.WriteLine("Serial port connected {0}", connected);
            };
            btsp.DataReceived += (sender, data) =>
            {
                Console.WriteLine("Data {0} bytes received", data.Length);
            };
            ConsoleHelper.PK("Press a key to connect");
            Console.WriteLine("Connecting bt serial connection");
            btsp.Connect();
            ConsoleHelper.PK("Press a key to send");
            byte[] data = new byte[10];
            data[0] = 1;
            data[1] = 2;
            for (int i = 0; i < 5; i++)
            {
                btsp.SendData(data);
                ConsoleHelper.PK("Press to send again");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        ConsoleHelper.PK("Press a key to end");
    }
}
