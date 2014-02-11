using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app1
{
    public partial class CANIDInfo : Form
    {
        public CANIDInfo()
        {
            InitializeComponent();
            var source = new AutoCompleteStringCollection();
            source.AddRange(canID.getNodeList());
            textBox2.AutoCompleteCustomSource = source;
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox3.AutoCompleteCustomSource = source;
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var source1 = new AutoCompleteStringCollection();
            source1.AddRange(canID.getMtType());
            textBox4.AutoCompleteCustomSource = source1;
            textBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox4.AutoCompleteSource = AutoCompleteSource.CustomSource;
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            canID v_canID = new canID(((TextBox)sender).Text.ToString());
            mtTypeLabel.Text = v_canID.getMsg();
            fromLabel.Text = v_canID.getFrom();
            toLabel.Text = v_canID.getTo();
            canversionLabel.Text = v_canID.getCANV().ToString();
            if (v_canID.getTx() == true)
            {
                TxLabel.Text = "Yes";
            }
            else 
            {
                TxLabel.Text = "No";
            }
                    
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            updateCANIDs();
        }

        private void updateCANIDs()
        {
            label1.Text = "0x" + Convert.ToString(canID.getCANIDV2(textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), false), 16);
            label2.Text = "0x" + Convert.ToString(canID.getCANIDV2(textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), true), 16);
            label3.Text = "0x" + Convert.ToString(canID.getCANIDV3(textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), false), 16);
            label4.Text = "0x" + Convert.ToString(canID.getCANIDV3(textBox2.Text.ToString(), textBox3.Text.ToString(), textBox4.Text.ToString(), true), 16);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            updateCANIDs();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            updateCANIDs();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
    }

    public class canID
    {
        private Int32 canID_int, m_canID_V;
        private bool m_canID_extended;
        private static String[] nodeList = new String[] { "olPowercard",
        "olOptionC2", 
        "olAoc",
        "olOptionA",  
        "olOptionB",  
        "olOptionC0", 
        "olOptionC1", 
        "olProductionDll",
        "olPowercard1slave",
        "olPowercard2slave",
        "olPowercard3slave",
        "olPowercard4slave",
        "olPowercard5slave",
        "olPowercard6slave",
        "olPowercard7slave",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olInvalid",
        "olOptionAndAocGroup",
        "olPowerCardGroup",
        "olOptionAndPowerCardGroup",
        "olOptionD",
        "olExternalDebug",
        "olInternalDebug",
        "olNoGroup",
        "olAFHiddenParams",
        "noOfOptionLocations",
        "noOfOptionLocations"};


        private static String[] folderDir = new String[] {
             "mtKingsDoc",
             "mtMayorsDocV3_8_15",
             "mtMayorsDoc",
             "mtMayorsDocV3_16_23",
             "mtOldBlockTransfer",
             "mtMayorsDocV3_24_31",
             "mtUNKNOWN",
             "mtUNKNOWN",
             "mtSafe",
             "mtUNKNOWN",
             "mtProfibusInterface",
             "mtUNKNOWN",
             "mtUNKNOWN",
             "mtUNKNOWN",
             "mtIO",
             "mtUNKNOWN",
             "mtGSV",
             "mtUNKNOWN",
             "mtVLTDataIf",
             "mtSubscriber",
             "mtUNKNOWN",
             "mtUNKNOWN",
             "mtUNKNOWN",
             "mtBulkData",
             "mtRTCDoc",
             "mtUNKNOWN",
             "mtPowerStatusDoc",
             "mtEcbDoc",
             "mtAfcDoc",
             "mtUNKNOWN",
             "mtBuildInDebugging",
             "mtUNKNOWN",
             "mtUNKNOWN"
    };
        public static String [] getNodeList()
        {
            return nodeList;
        }

        public static String[] getMtType()
        {
            return folderDir;
        }

        public canID(String _canID)
        {
            try 
            {
                canID_int = Convert.ToInt32(_canID,16);
            }
            catch 
            {
                try
                {
                    canID_int = Convert.ToInt32(_canID, 10);
                }
                catch 
                {
                    canID_int = 0;
                }
            }
            analyze();

        }
        public canID(Int32 _canID)
        {
            canID_int = _canID;
            analyze();
        }
        public Int32 get()
        {
            return canID_int;
        }       
        public Int32 getCANV()
        {
            return m_canID_V;
        }
        public bool getTx()
        {
            return m_canID_extended;
        }
        public void analyze()
        {
            //dddddd-ttttt-1b111-FFFFF-xxxxxxxx
            if ((canID_int & 0x2e000) == 0x2e000)
            {
                m_canID_V = 3;
                if (((canID_int) & 0xf) == 0xf)
                {
                    m_canID_extended = false;
                }
                else
                {
                    m_canID_extended = true;
                }
            }
            else
            {
                m_canID_V = 2;
                if (((canID_int >> 23) & 3) == 0x3)
                {
                    m_canID_extended = true;
                }
                else
                {
                    m_canID_extended = false;
                }
            }
        }
        public String getTo()
        {
            if (m_canID_V == 2)
                return getToV2();
            else
                return getToV3();
        }
        public String getFrom()
        {
            if (m_canID_V == 2)
                return getFromV2();
            else
                return getFromV3();
        }
        public String getMsg()
        {
            if (m_canID_V == 2)
                return getMsgV2();
            else
                return getMsgV3();
        }
        public String getFromV3()
        {
            String retrunValue;
            try
            {
                retrunValue = nodeList[((canID_int >>8) & 0x1F)];
            }
            catch
            {
                retrunValue = "olNowhere";
            }
            return retrunValue;
        }
        public String getToV3()
        {
            String retrunValue;
            Int32 to = (canID_int >> 18) & 0x1F;
            try
            {
                 if(to == 0)
                        retrunValue = "olBroadcast";
                    else
                        retrunValue = nodeList[to];
            }
            catch
            {
                retrunValue = "olNowhere";
            }
            return retrunValue;

        }
        public String getMsgV3()
        {
            String retrunValue;            
            try
            {
                retrunValue = folderDir[(canID_int >> 23) & 0x3f];                
            }
            catch
            {
                retrunValue = "mtUnknown";
            }

            return retrunValue;            

        }

        public String getFromV2()
        { 
            String retrunValue;
            try
            {
                if (m_canID_extended == true)
                {
                    retrunValue = nodeList[canID_int & 0xf];
                }
                else
                {
                    retrunValue = nodeList[canID_int & 0x7];
                }
            }
            catch
            {
                retrunValue = "olNowhere";
            }
            return retrunValue;
        }
        public String getToV2()
        {
            String retrunValue;
            Int32 to, count = -1;
            try
            {
                if (m_canID_extended == true)
                {
                    to = ~((canID_int >> 4) & 0x3fff);
                    
                    while (to != 0)
                    {
                        to = (to << 1) & 0x3fff;
                        count++;
                    }
                    retrunValue = nodeList[count];
                }
                else 
                {
                    to =(canID_int & 0x38) >> 3;
                    if(to == 0)
                        retrunValue = "olBroadcast";
                    else
                        retrunValue = nodeList[to];
                }
            }
            catch
            {
                retrunValue = "olNowhere";
            }
            
            return retrunValue;
        }
        public String getMsgV2()
        {
            String retrunValue;            
            try
            {
                if (m_canID_extended == true)
                {
                    retrunValue = folderDir[(canID_int & 0x7c0000) >> 18];
                    
                }
                else
                {
                    retrunValue = folderDir[(canID_int >> 6) & 0x1f];
                }
            }
            catch
            {
                retrunValue = "mtUnknown";
            }

            return retrunValue;
        }

        public static Int32 getCANIDV2(String from, String to, String mtType, bool extended)
        { 
            Int32 count, fromint = 0, toint = 0, mtTypeint = 0;
            for (count = 0; count < nodeList.Length; count++)
            {
                if (from.ToLower() == nodeList[count].ToLower())
                {
                    fromint = count;
                    break;
                }
            }
            for (count = 0; count < nodeList.Length; count++)
            {
                if (to.ToLower() == nodeList[count].ToLower())
                {
                    toint = count;
                    break;
                }
            }
            for (count = 0; count < folderDir.Length; count++)
            {
                if (mtType.ToLower() == folderDir[count].ToLower())
                {
                    mtTypeint = count;
                    break;
                }
            }
            
            if (extended == true)
                return 0x01800000 + (mtTypeint << 18) + (((1 << 17) >> toint) ^ 0x3FFF0) + (fromint);
            else
                return (fromint + (toint << 3) + (mtTypeint << 6));
        
        }

        public static Int32 getCANIDV3(String from, String to, String mtType, bool extended)
        {
            Int32 count, fromint = 0, toint = 0, mtTypeint = 0;
            for (count = 0; count < nodeList.Length; count++)
            {
                if (from.ToLower() == nodeList[count].ToLower())
                {
                    fromint = count;
                    break;
                }
            }
            for (count = 0; count < nodeList.Length; count++)
            {
                if (to.ToLower() == nodeList[count].ToLower())
                {
                    toint = count;
                    break;
                }
            }
            for (count = 0; count < folderDir.Length; count++)
            {
                if (mtType.ToLower() == folderDir[count].ToLower())
                {
                    mtTypeint = count;
                    break;
                }
            }
            
            if (extended == true)
                return 0x0003E001 | (mtTypeint << 23) | (toint << 18) | (fromint << 8);
            else
                return 0x0002E0FF | (mtTypeint << 23) | (toint << 18) | (fromint << 8);                
        }
    }
}
