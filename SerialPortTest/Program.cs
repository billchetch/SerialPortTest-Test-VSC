using System.IO.Ports;
using Chetch.Messaging;

namespace SerialPortTest;

class Program
{
    static void Main(string[] args)
    {

        var portNames = SerialPort.GetPortNames();
        
        int baudRate = 9600;
        Parity parity = Parity.None;
        int dataBits = 8;
        StopBits stopBits = StopBits.One;

        String portName = "/dev/cu.usbmodem1401";
        
        try
        {
            using(var serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)){
                if(serialPort.IsOpen){
                    Console.WriteLine("Serial port is open so closing...");
                    serialPort.Close();
                }
                serialPort.DataReceived += (sender, eargs) => {
                        Console.WriteLine("Data received");
                        int dataLength = serialPort.BytesToRead;
                        byte[] data = new byte[dataLength];
                        int nbrDataRead = serialPort.Read(data, 0, dataLength);
                        if (nbrDataRead == 0)
                            return;

                        string str = System.Text.Encoding.ASCII.GetString(data);
                        String[] sentences = str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        foreach (String sentence in sentences)
                        {
                            Console.WriteLine("Sentence {0}: ", sentence);
                        }

                    };
                serialPort.Open();
                Console.WriteLine("Opened {0}", portName);
                ConsoleHelper.PK("Press a key to close");
                serialPort.Close();
                
                Console.WriteLine("Closed {0}", portName);
            }
        } 
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
