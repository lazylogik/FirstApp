using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace App1
{
    public partial class Form1 : Form
    {
        private System.IO.Ports.SerialPort serialPort1;
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

        private void initButton_Click(object sender, EventArgs e)
        {
            String selectedPort = comboBox1.SelectedItem.ToString();
            serialPort1 = new System.IO.Ports.SerialPort(selectedPort);
            serialPort1.Parity = Parity.Even;
            serialPort1.BaudRate = 115200;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Open();
        }

        private void buttonGeneralReset_Click(object sender, EventArgs e)
        {
            //byte[] generalResetFrame = new byte[] {0x01, 0x00, 0x0b, 0x81, 0x00, 0x00, 0x00, 0xff, 0x04, 0x00, 0x04, 0x00, 0x03, 0x8f, 0xf3};
            //serialPort1.Write(generalResetFrame, 0 , generalResetFrame.Length);
            GetDefaultFile frame1 = new GetDefaultFile(200);
            frame1.process(serialPort1);
            GeneralResetFrame frame = new GeneralResetFrame();
            byte[] generalResetFrame = frame.getBytes();
            int length = frame.getLength();
            serialPort1.Write(generalResetFrame, 0, length);
        }

        private void buttonGetDefault_Click(object sender, EventArgs e)
        {
            Parameter[] parameters;// = new Parameter[10];
            //parameters[0] = new Parameter();
            //parameters[0].paramNumber = 4281;
            //parameters[0].subIndex = 0;
            //parameters[0].Value = 2348;

            //parameters[1] = new Parameter();
            //parameters[1].paramNumber = 4280;
            //parameters[1].subIndex = 0;
            //parameters[1].Value = 2111;

            //parameters[2] = new Parameter();
            //parameters[2].paramNumber = 4212;
            //parameters[2].subIndex = 0;
            //parameters[2].Value = 2222;

            //parameters[3] = new Parameter();
            //parameters[3].paramNumber = 4213;
            //parameters[3].subIndex = 0;
            //parameters[3].Value = 2355;

            //updateDataGrid(parameters);
            parameters = getfromDataGrid();
        }

        private void updateDataGrid(Parameter[] paramList)
        {
            string[] row = new String[3];
            for(int counter = 0; counter < paramList.Length; counter++)
            {
                row[0] = paramList[0].paramNumber.ToString();
                row[1] = paramList[0].subIndex.ToString();
                row[2] = paramList[0].Value.ToString();
                dataGridView1.Rows.Add(row);
            }            
        }
        private Parameter[] getfromDataGrid()
        {
            int rowCount = dataGridView1.RowCount;
            Parameter[] paramList = new Parameter[rowCount];                        
            for (int counter = 0; counter < rowCount; counter++)
            {
                paramList[counter] = new Parameter();
                paramList[counter].paramNumber = Convert.ToInt32(dataGridView1.Rows[counter].Cells[0].Value);
                paramList[counter].subIndex = Convert.ToInt32(dataGridView1.Rows[counter].Cells[1].Value);
                paramList[counter].Value = Convert.ToInt32(dataGridView1.Rows[counter].Cells[2].Value);
            }
            return paramList;
        }

        
    }

    public class XTelegraFrame
    {
        private const byte soh = 0x01;
        public List<byte> dataBytes;
        private const byte etx = 0x03;
        private byte vltID;
        private byte frameNo;
        private byte ackNo;
        private int lengthOfDataBytes;
        private int lengthInFrame;
        private int totalLength;
        private UInt16 crc;

        private UInt16[] crc_tab = new UInt16[]{
           0x0000,  0x1021,  0x2042,  0x3063,  0x4084,  0x50a5,  0x60c6,  0x70e7,
           0x8108,  0x9129,  0xa14a,  0xb16b,  0xc18c,  0xd1ad,  0xe1ce,  0xf1ef,
           0x1231,  0x0210,  0x3273,  0x2252,  0x52b5,  0x4294,  0x72f7,  0x62d6,
           0x9339,  0x8318,  0xb37b,  0xa35a,  0xd3bd,  0xc39c,  0xf3ff,  0xe3de,
           0x2462,  0x3443,  0x0420,  0x1401,  0x64e6,  0x74c7,  0x44a4,  0x5485,
           0xa56a,  0xb54b,  0x8528,  0x9509,  0xe5ee,  0xf5cf,  0xc5ac,  0xd58d,
           0x3653,  0x2672,  0x1611,  0x0630,  0x76d7,  0x66f6,  0x5695,  0x46b4,
           0xb75b,  0xa77a,  0x9719,  0x8738,  0xf7df,  0xe7fe,  0xd79d,  0xc7bc,
           0x48c4,  0x58e5,  0x6886,  0x78a7,  0x0840,  0x1861,  0x2802,  0x3823,
           0xc9cc,  0xd9ed,  0xe98e,  0xf9af,  0x8948,  0x9969,  0xa90a,  0xb92b,
           0x5af5,  0x4ad4,  0x7ab7,  0x6a96,  0x1a71,  0x0a50,  0x3a33,  0x2a12,
           0xdbfd,  0xcbdc,  0xfbbf,  0xeb9e,  0x9b79,  0x8b58,  0xbb3b,  0xab1a,
           0x6ca6,  0x7c87,  0x4ce4,  0x5cc5,  0x2c22,  0x3c03,  0x0c60,  0x1c41,
           0xedae,  0xfd8f,  0xcdec,  0xddcd,  0xad2a,  0xbd0b,  0x8d68,  0x9d49,
           0x7e97,  0x6eb6,  0x5ed5,  0x4ef4,  0x3e13,  0x2e32,  0x1e51,  0x0e70,
           0xff9f,  0xefbe,  0xdfdd,  0xcffc,  0xbf1b,  0xaf3a,  0x9f59,  0x8f78,
           0x9188,  0x81a9,  0xb1ca,  0xa1eb,  0xd10c,  0xc12d,  0xf14e,  0xe16f,
           0x1080,  0x00a1,  0x30c2,  0x20e3,  0x5004,  0x4025,  0x7046,  0x6067,
           0x83b9,  0x9398,  0xa3fb,  0xb3da,  0xc33d,  0xd31c,  0xe37f,  0xf35e,
           0x02b1,  0x1290,  0x22f3,  0x32d2,  0x4235,  0x5214,  0x6277,  0x7256,
           0xb5ea,  0xa5cb,  0x95a8,  0x8589,  0xf56e,  0xe54f,  0xd52c,  0xc50d,
           0x34e2,  0x24c3,  0x14a0,  0x0481,  0x7466,  0x6447,  0x5424,  0x4405,
           0xa7db,  0xb7fa,  0x8799,  0x97b8,  0xe75f,  0xf77e,  0xc71d,  0xd73c,
           0x26d3,  0x36f2,  0x0691,  0x16b0,  0x6657,  0x7676,  0x4615,  0x5634,
           0xd94c,  0xc96d,  0xf90e,  0xe92f,  0x99c8,  0x89e9,  0xb98a,  0xa9ab,
           0x5844,  0x4865,  0x7806,  0x6827,  0x18c0,  0x08e1,  0x3882,  0x28a3,
           0xcb7d,  0xdb5c,  0xeb3f,  0xfb1e,  0x8bf9,  0x9bd8,  0xabbb,  0xbb9a,
           0x4a75,  0x5a54,  0x6a37,  0x7a16,  0x0af1,  0x1ad0,  0x2ab3,  0x3a92,
           0xfd2e,  0xed0f,  0xdd6c,  0xcd4d,  0xbdaa,  0xad8b,  0x9de8,  0x8dc9,
           0x7c26,  0x6c07,  0x5c64,  0x4c45,  0x3ca2,  0x2c83,  0x1ce0,  0x0cc1,
           0xef1f,  0xff3e,  0xcf5d,  0xdf7c,  0xaf9b,  0xbfba,  0x8fd9,  0x9ff8,
           0x6e17,  0x7e36,  0x4e55,  0x5e74,  0x2e93,  0x3eb2,  0x0ed1,  0x1ef0};



        public byte[] getBytes()
        {
            List<byte> frame = new List<byte>();
            frame.Add(soh);
            frame.Add((byte)(lengthInFrame >> 8));
            frame.Add((byte)lengthInFrame);
            frame.Add(vltID);
            frame.Add(frameNo);
            frame.Add(ackNo);
            frame.AddRange(dataBytes);
            frame.Add((byte)etx);
            frame.Add((byte)crc);
            frame.Add((byte)(crc >> 8));
            return frame.ToArray();
        }
        public XTelegraFrame()
        {
            lengthOfDataBytes = 0;
            totalLength = lengthOfDataBytes + 9;
            lengthInFrame = lengthOfDataBytes + 5;
            dataBytes = new List<byte>();
            vltID = 0x81;
            frameNo = 0;
            ackNo = 0;
            crc = 0;
        }
        public int getLength()
        {
            return totalLength;
        }
        public UInt16 calculateCRC(byte d, UInt16 local_crc)
        {
            UInt16 returnValue = 0;
            returnValue = (UInt16)(local_crc << 8);
            returnValue = (UInt16)(returnValue ^ (UInt16)crc_tab[(((UInt16)local_crc >> (UInt16)8) ^ (UInt16)d)]);
            //return ((UInt16)((UInt16)local_crc << (UInt16)8) ^ (UInt16)crc_tab[(byte)(((UInt16)local_crc >> (UInt16)8) ^ (UInt16)d)]);
            return returnValue;

        }
        public void process()
        {
            lengthOfDataBytes = dataBytes.Count;
            totalLength = dataBytes.Count + 9;
            lengthInFrame = dataBytes.Count + 5;
            byte[] wholeFrame = getBytes();
            crc = 0;
            for (int counter = 1; counter < getLength() - 3; counter++)
            {
                crc = calculateCRC(wholeFrame[counter], crc);
            }
        }


    }

    public class SOPFrame : XTelegraFrame
    {
        public enum confRequest_E
        {
            customizationRequest = 0x00,
            eventLogSizeRequest = 0x01,
            eventLogRequest = 0x02,
            level1PasswordChange = 0x03,
            generalReset = 0x04,
            defaultConfigurationFileRequest = 0x08
        };
        //private XTelegraFrame xTel;
        private byte[] constData;
        private const byte sourceOfTelegram = 0x00;
        public confRequest_E reqCode;
        public byte frameNumber;
        public new List<byte> dataBytes;
        private int lengthOfDataBytes;
        public SOPFrame()
            : base() //((_lengthOfDataBytes + 3 + 3))
        {
            lengthOfDataBytes = 0;
            constData = new byte[] { 0x00, 0xff, 0x04 };
            //xTel = new XTelegraFrame((lengthOfDataBytes + 3 + 3));
            dataBytes = new List<byte>();
        }
        public new int getLength()
        {
            return base.getLength();
        }
        public void updateReqCode(confRequest_E _reqCode)
        {
            reqCode = _reqCode;
        }
        public new byte[] getBytes()
        {
            List<byte> frame = new List<byte>();
            frame.AddRange(base.getBytes());
            //frame.AddRange(constData);
            //frame.Add((byte)sourceOfTelegram);
            //frame.Add((byte)reqCode);
            //frame.Add((byte)frameNumber);         
            return frame.ToArray();
        }
        public new void process()
        {
            base.dataBytes.AddRange(constData);
            base.dataBytes.Add((byte)sourceOfTelegram);
            base.dataBytes.Add((byte)reqCode);
            base.dataBytes.Add((byte)frameNumber);
            base.dataBytes.AddRange(dataBytes);
            lengthOfDataBytes = dataBytes.Count;
            base.process();
        }

    }

    public class GeneralResetFrame
    {
        private SOPFrame sopFrame;
        public byte[] getBytes()
        {
            List<byte> frame = new List<byte>();
            frame.AddRange(sopFrame.getBytes());
            return frame.ToArray();
        }
        public GeneralResetFrame()
        {
            sopFrame = new SOPFrame();
            sopFrame.reqCode = SOPFrame.confRequest_E.generalReset;
            sopFrame.frameNumber = 0;
            sopFrame.process();
        }
        public int getLength()
        {
            return sopFrame.getLength();
        }
    }
    


    public class GetDefaultFile
    {
        private SOPFrame sopFrame;
        private UInt16 fileVersion;
        public byte[] getBytes()
        {
            List<byte> frame = new List<byte>();
            frame.AddRange(sopFrame.getBytes());
            return frame.ToArray();
        }
        public GetDefaultFile(UInt16 _fileVersion)
        {
            fileVersion = _fileVersion;
        }
        public int getLength()
        {
            return sopFrame.getLength();
        }
        public void process(SerialPort port)
        {
            sopFrame = new SOPFrame();
            sopFrame.reqCode = SOPFrame.confRequest_E.defaultConfigurationFileRequest;
            sopFrame.frameNumber = 0;
            sopFrame.dataBytes.Add((byte)fileVersion);
            sopFrame.dataBytes.Add((byte)(fileVersion >> 8));
            sopFrame.process();
            port.Write(sopFrame.getBytes(), 0, sopFrame.getLength());

        }
    }

    class Parameter
    { 
        public int paramNumber;
        public int subIndex;
        public int Value;
    }


    


}
