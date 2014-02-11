using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace WindowsFormsApplication1
{    
    public partial class Form1 : Form
    {
        public static int keepRuning = 0;
        private String[] listOfPorts;
        public Form1()
        {
            InitializeComponent();            
            listOfPorts = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String s in listOfPorts)
            {
                comboBox1.Items.Add(s);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // To start a thread using a shared thread procedure, use 
            // the class name and method name when you create the  
            // ParameterizedThreadStart delegate. C# infers the  
            // appropriate delegate creation syntax: 
            //    new ParameterizedThreadStart(Work.DoWork) 
            //        
            Thread newThread = new Thread(this.DoWork);

            keepRuning = 1;

            // Use the overload of the Start method that has a 
            // parameter of type Object. You can create an object that 
            // contains several pieces of data, or you can pass any  
            // reference type or value type. The following code passes 
            // the integer value 42. 
            //
            newThread.Start(42);
        
        }
        
        public void DoWork(object data)
        {
            FCFrame getParam, getParamRes, setParam;
            getParam = new FCFrame();
            getParamRes = new FCFrame();
            setParam = new FCFrame();
            getParam.stx = 0x02;
            getParam.lge = 0x0e;
            getParam.addr = 0x01;
            getParam.data.Add(0x16);
            getParam.data.Add(0x7c);
            getParam.data.Add(0x00);
            getParam.data.Add(0x00);
            getParam.data.Add(0x00);
            getParam.data.Add(0x00);
            getParam.data.Add(0x00);
            getParam.data.Add(0x00);
            getParam.ctw = 0x474;
            getParam.reference = 0x0;
            getParam.cs = 0x17;
            {
                setParam.stx = 0x02;
                setParam.lge = 0x0e;
                setParam.addr = 0x01;
                setParam.data.Add(0x23);
                setParam.data.Add(0xcc);
                setParam.data.Add(0x00);
                setParam.data.Add(0x00);
                setParam.data.Add(0x00);
                setParam.data.Add(0x00);
                setParam.data.Add(0x00);
                setParam.data.Add(0x01);
                setParam.ctw = 0x474;
                setParam.reference = 0x0;
                setParam.cs = 0x93;
            
            }


            System.IO.Ports.SerialPort serialPort1;
            String selectedPort = "COM2";
            serialPort1 = new System.IO.Ports.SerialPort(selectedPort);
            serialPort1.Parity = Parity.Even;
            serialPort1.BaudRate = 9600;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Open();
            while (keepRuning == 1)
            {
                int length = getParam.getLength();
                byte[] receivedData = new byte[100];
                byte[] receivedData1 = new byte[100];
                bool keepReading = true;
                do
                {                    
                    while (serialPort1.BytesToRead < 16)
                    {
                        serialPort1.Write(getParam.getBytes(), 0, length);
                        Thread.Sleep(2000);
                    }
                    serialPort1.Read(receivedData, 0, serialPort1.BytesToRead);
                    int indexof2 = Array.IndexOf(receivedData, (byte)0x02);
                    if (indexof2 != -1)
                    {
                        Array.Copy(receivedData, indexof2, receivedData1, 0, 16);
                        getParamRes.data.Clear();
                        getParamRes.setBytes(receivedData1);
                        if (getParamRes.data[7] == 0x10)
                            keepReading = false;
                    }
                } while (keepReading);
                length = setParam.getLength();
                serialPort1.Write(setParam.getBytes(), 0, length);
                while (serialPort1.BytesToRead < 16)
                {
                    Thread.Sleep(1000);
                }
                serialPort1.Read(receivedData, 0, serialPort1.BytesToRead);
                Thread.Sleep(5000);
                Console.WriteLine("DI is set");
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            keepRuning = 0;
        }

        
    }

    public class FCFrame
    {
        public byte stx;
        public byte lge;
        public byte addr;
        public List<byte> data;
        public short ctw;
        public short reference;
        public byte cs;
        public FCFrame()
        { 
            data = new List<byte>();
        }
        public byte[] getBytes()
        {
            List<byte> frame = new List<byte>();
            frame.Add(stx);
            frame.Add(lge);
            frame.Add(addr);
            frame.AddRange(data);
            frame.Add((byte)(ctw>> 8));
            frame.Add((byte)ctw);
            frame.Add((byte)(reference >> 8));
            frame.Add((byte)reference);            
            frame.Add((byte)cs);
            return frame.ToArray();
        }
        public int getLength()
        {
            return 16;
        }
        public void setBytes(byte[] rcd)
        {
            stx = rcd[0];
            lge = rcd[1];
            addr = rcd[2];
            data.Add(rcd[3]);
            data.Add(rcd[4]);
            data.Add(rcd[5]);
            data.Add(rcd[6]);
            data.Add(rcd[7]);
            data.Add(rcd[8]);
            data.Add(rcd[9]);
            data.Add(rcd[10]);
            ctw = (short)((short)rcd[11] << 8);
            ctw |= rcd[12];
            reference = (short)((short)rcd[13] << 8);
            reference |= rcd[14];
            cs = rcd[15];
            
        }
    }
}
