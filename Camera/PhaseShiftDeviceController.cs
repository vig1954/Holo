using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    public class PhaseShiftDeviceController
    {
        public event Action<byte[], string> SerialPortDataRecieved;

        private SerialPort serialPort;

        public PhaseShiftDeviceController(string portName)
        {
            this.serialPort = new SerialPort(portName);
        }

        public void Initialize()
        {
            this.serialPort.BaudRate = 115200;
            this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialPort_DataReceived);

            this.serialPort.WriteTimeout = 500;
            this.serialPort.Handshake = Handshake.None;

            this.serialPort.RtsEnable = true;
            this.serialPort.DtrEnable = true;


            this.serialPort.Open();

        }

        private void SetBuffersSizes()
        {
            this.serialPort.WriteBufferSize = 2;
            this.serialPort.ReadBufferSize = 2;
        }

        private void DiscardBuffers()
        {
            this.serialPort.DiscardOutBuffer();
            this.serialPort.DiscardInBuffer();
        }

        public short SetShift(byte[] bytesValues)
        {
            if (bytesValues.Length != 2)
            {
                throw new Exception("Phase shift must contain 2 bytes");
            }

            this.serialPort.Write(bytesValues, 0, bytesValues.Length);
            //this.serialPort.Write( new byte[] { 1 }, 0, 1 );

            return 0;
        }

        public void WriteByte(byte value)
        {
            this.serialPort.Write(new byte[] { value }, 0, 1);
        }

        public void WriteBytes(byte[] values)
        {
            this.serialPort.Write(values, 0, values.Length);
        }

        public byte ReadByte()
        {
            return (byte)this.serialPort.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];
            this.serialPort.Read(buffer, 0, count);
            return buffer;
        }

        // 0x20, 0x20 - 0x2f, 0x2f

        public short SetShift(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            byte byteOne = bytes[1];
            byte byteTwo = bytes[0];

            bytes = new byte[] { byteOne, byteTwo };

            short response = this.SetShift(bytes);
            return response;
        }

        public string PortName
        {
            get
            {
                return this.serialPort.PortName;
            }
            set
            {
                this.serialPort.PortName = value;
            }
        }

        ~PhaseShiftDeviceController()
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Close();
            }
        }

        public void Dispose()
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Close();
            }
        }

        //-----------------------------------------------------------------------------------------
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;

            if (serialPort.BytesToRead == 1)
            {
                int value = serialPort.ReadByte();
                byte byteValue = Convert.ToByte(value);

                SerialPortDataRecieved?.Invoke(new byte[]{byteValue}, $"Response: {byteValue:X}");
            }
            else if (serialPort.BytesToRead == 2)
            {

                int value1 = serialPort.ReadByte();
                byte byteValue1 = Convert.ToByte(value1);

                int value2 = serialPort.ReadByte();
                byte byteValue2 = Convert.ToByte(value2);

                SerialPortDataRecieved?.Invoke(new byte[] { byteValue1, byteValue2 }, $"Response: {byteValue1:X} {byteValue2:X}");
            }
            else
            {
                string data = serialPort.ReadExisting();

                SerialPortDataRecieved?.Invoke(new byte[] {  }, $"Response: {data}");
            }
        }
    }
}
