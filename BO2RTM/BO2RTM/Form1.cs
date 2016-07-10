using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS3Lib;
using MetroFramework.Forms;

namespace BO2RTM
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
            MaximizeBox = false;
        }
        public static PS3API PS3 = new PS3API();
        public static PS3API PS32 = new PS3API();
        public static CCAPI PS3C = new CCAPI();


        public static string ClientNames(uint client)
        {
            string getnames = PS3.Extension.ReadString(0x1780F28 + 0x5544 + client * 0X5808);
            return getnames;
        }

        public string GetName(int ClientNum)
        {
            string str = "";
            try
            {
                str = PS3.Extension.ReadString(0x1780F28 + 0x5544 + (0X5808 * (uint)ClientNum));
            }
            catch (Exception) { }

            return str;
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void Connection_Click(object sender, EventArgs e)
        {

        }

        public static byte[] ReverseBytes(byte[] inArray)
        {
            Array.Reverse(inArray);
            return inArray;
        }
        public static void WriteSingle(uint address, float[] input)
        {
            int length = input.Length;
            byte[] array = new byte[length * 4];
            for (int i = 0; i < length; i++)
            {
                ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (int)(i * 4));
            }
            Form1.PS32.SetMemory(address, array);
        }
        public static void WriteSingle(uint address, float input)
        {
            byte[] array = new byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            Form1.PS32.SetMemory(address, array);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PS3.AttachProcess())//what this does is runs a error code if theres nothing there, so instead of it saying "Connected"                                             like some tools do it will say ERROR.
                {
                    label2.Text = "Failed To Attach";
                    label2.ForeColor = Color.Red;
                    return;
                }
                if (PS3.GetCurrentAPI() == SelectAPI.ControlConsole)
                {
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY4, "Successfully Attached To Zoltan Fuzesi RTM Tool!");
                    PS3.CCAPI.RingBuzzer(CCAPI.BuzzerMode.Double);
                }
                label2.Text = "Status: Attached";
                label2.ForeColor = Color.Green;
                MessageBox.Show("Successfully Attached To Zoltan Fuzesi RTM Tool", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button3.Enabled = true;
            }
            catch
            {
                label2.Text = "Failed To Attach";
                label2.ForeColor = Color.Red;
                PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY4, "Failed To Attach :/");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                PS3.DisconnectTarget();
                MessageBox.Show("PS3 Disconnected");
            }
            catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!PS3.ConnectTarget(0))
                {
                    label1.Text = "Failed To Connect";
                    label1.ForeColor = Color.Red;
                    return;
                }

                if (PS3.GetCurrentAPI() == SelectAPI.ControlConsole)
                {
                    MessageBox.Show("Successfully Connected To PS3", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    label1.Text = " Status: Connected";
                    label1.ForeColor = Color.Green;
                    PS3.CCAPI.RingBuzzer(CCAPI.BuzzerMode.Single);
                    PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY4, "Successfully Connected To PS3");
                    button2.Enabled = true;
                    button3.Enabled = false;
                   
                }
                else if (PS3.GetCurrentAPI() == SelectAPI.TargetManager)
                {
                    MessageBox.Show("Connected", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    label1.Text = " Status: Connected";
                    label1.ForeColor = Color.Green;
                    button2.Enabled = true;
                    button3.Enabled = false;

                }

            }
            catch
            {

                MessageBox.Show("Something is wrong please try again :/", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "Failed To Connect";
                label1.ForeColor = Color.Red;

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            PS3.ChangeAPI(SelectAPI.ControlConsole);
            button1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            PS3.ChangeAPI(SelectAPI.TargetManager);
            button1.Enabled = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                try
                {
                    PS3.SetMemory(0x1CC52D8, new byte[] { 0x42, 0xDC });
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    PS3.SetMemory(0x1CC52D8, new byte[] { 0x41, 0x20 });
                }
                catch (Exception) { }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (PS3.Extension.ReadByte(0x1CBF9F8) != 0)
                {
                    timer1.Start();
                    PS3.SetMemory(0x1CBF9F8, new byte[] { 0x00 }); //r_dof_enabled by jewels by winter
                    PS3.SetMemory(0x1CBF9F8, new byte[] { 0x00 });
                    PS3.SetMemory(0x000834D0, new byte[] { 0x38, 0xC0, 0xFF, 0xFF }); //wallhack by jewels by winter
                    PS3.SetMemory(0x000834D0, new byte[] { 0x38, 0xC0, 0xFF, 0xFF }); //wallhack by jewels by winter
                }
                else
                {
                    timer1.Stop();
                    PS3.SetMemory(0x1CBF9F8, new byte[] { 0x01 }); //r_dof_enabled by jewels
                    PS3.SetMemory(0x1CBF9F8, new byte[] { 0x01 });
                    PS3.SetMemory(0x000834D0, new byte[] { 0x63, 0x26, 0x00, 0x00 }); //wallhack by jewels by winter 
                    PS3.SetMemory(0x000834D0, new byte[] { 0x63, 0x26, 0x00, 0x00 }); //wallhack by jewels by winter
                }
            }
            catch (Exception) { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PS3.Extension.ReadByte(0x1CBF9F8) != 0)
            {
                PS3.SetMemory(0x1CBF9F8, new byte[] { 0x00 }); //r_dof_enabled by jewels by winter
                PS3.SetMemory(0x1CBF9F8, new byte[] { 0x00 });
                PS3.SetMemory(0x000834D0, new byte[] { 0x38, 0xC0, 0xFF, 0xFF }); //wallhack by jewels by winter
                PS3.SetMemory(0x000834D0, new byte[] { 0x38, 0xC0, 0xFF, 0xFF }); //wallhack by jewels by winter
            }
            else
            {
                //NO need to have a off o.O
            }
        }

        private void godModToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //God mode OFF
        private void oFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS3.SetMemory(0x1780F28 + 0x1B + 0x5808 * (uint)dataGridView1.CurrentRow.Index, new byte[] { 0x08 });
            RPC.iPrintln(dataGridView1.CurrentRow.Index, "God Mod : ^1Taken");
        }

        //Get Names client mode
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Enabled = true; dataGridView1.RowCount = 12;
                for (int i = 0; i < 12; i++)
                {
                    dataGridView1.Update();
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = GetName(i);
                }

            }

            catch
            {
                MessageBox.Show("Are you in a game???\n\nAre you host or not?? ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true; dataGridView1.RowCount = 12;
            for (int i = 0; i < 12; i++)
            {
                dataGridView1.Update();
                dataGridView1.Rows[i].Cells[0].Value = i;
                dataGridView1.Rows[i].Cells[1].Value = GetName(i);
            }
        }

        //Auto refres
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox3.Checked)
            {
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }
        }

        //God mode ON
        private void oNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS3.SetMemory(0x1780F28 + 0x1B + 0x5808 * (uint)dataGridView1.CurrentRow.Index, new byte[] { 0x05 });
            RPC.iPrintln(dataGridView1.CurrentRow.Index, "God Mod : ^2Given");
        }

        //Unlimited ammo ON
        private void oNToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            uint[] AmmoTypes = { 0x0178135d, 0x01781361, 0x1781355, 0x1781319, 0x1781359, 0x1781365 };

            for (int i = 0; i <= AmmoTypes.Length - 1; i++)
            {
                PS32.Extension.WriteBytes(AmmoTypes[i] + ((uint)dataGridView1.CurrentRow.Index * 0x5808), new byte[] { 0xFF, 0xFF });
            }
            RPC.iPrintln(dataGridView1.CurrentRow.Index, "^7Unlimited Ammo: ^2Given");
        }


        //Unlimited ammo OFF
        private void oFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uint[] AmmoTypes = { 0x0178135d, 0x01781361, 0x1781355, 0x1781319, 0x1781359, 0x1781365 };

            for (int i = 0; i <= AmmoTypes.Length - 1; i++)
            {
                PS32.Extension.WriteBytes(AmmoTypes[i] + ((uint)dataGridView1.CurrentRow.Index * 0x5808), new byte[6]);
            }
            RPC.iPrintln(dataGridView1.CurrentRow.Index, "^7Unlimited Ammo: ^1Taken");
        }

        //Shut down PS3
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Shut down PS3", "Shut Down PS3", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Form1.PS3.CCAPI.ShutDown(CCAPI.RebootFlags.ShutDown);
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
          
        }


        //Hide MIC
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
                Form1.PS3.SetMemory(14164312U, new byte[1]);
            else
                Form1.PS3.SetMemory(14164312U, new byte[1]
                {
          (byte) 1
                });
        }

        //Red Boxes
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                Form1.PS3.Extension.WriteBytes(492512U, new byte[4]
                {
          (byte) 56,
          (byte) 96,
          (byte) 0,
          (byte) 1
                });
            }
            else
            {
                byte[] input = new byte[4]
                {
          (byte) 56,
          (byte) 96,
          (byte) 0,
          (byte) 0
                };
                Form1.PS3.Extension.WriteBytes(492512U, input);
            }
        }

        //Recoil
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox5.Checked)
            {
                PS3.Extension.WriteBytes(0xE9E54, new byte[] { 0x60, 0x00, 0x00, 0x00 });
            }
            else
            {
                PS3.Extension.WriteBytes(0xE9E54, new byte[] { 0x48, 0x50, 0x6E, 0xF5 });
            }
        }

        //Vsat
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox7.Checked)
            {
                Extension extension = Form1.PS3.Extension;
                int num = 212064;
                byte[] numArray = new byte[4];
                numArray[0] = (byte)96;
                byte[] input = numArray;
                extension.WriteBytes((uint)num, input);
            }
            else
                Form1.PS3.Extension.WriteBytes(212064U, new byte[4]
                {
          (byte) 64,
          (byte) 129,
          (byte) 0,
          (byte) 68
                });
        }
        //Laser
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox8.Checked)
                Form1.PS3.Extension.WriteBytes(980620U, new byte[4]
                {
          (byte) 44,
          (byte) 3,
          (byte) 0,
          (byte) 1
                });
            else
                Form1.PS3.Extension.WriteBytes(980620U, new byte[4]
                {
          (byte) 44,
          (byte) 3,
          (byte) 0,
          (byte) 0
                });
        }

        //Super steady aim
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox9.Checked)
                Form1.PS3.Extension.WriteBytes(6228512U, new byte[4]
                {
          (byte) 44,
          (byte) 4,
          (byte) 0,
          (byte) 0
                });
            else
                Form1.PS3.Extension.WriteBytes(6228512U, new byte[4]
                {
          (byte) 44,
          (byte) 4,
          (byte) 0,
          (byte) 2
                });
        }
        //Anti-freeze
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox10.Checked)
            {
                Extension extension = Form1.PS3.Extension;
                int num = 6797348;
                byte[] numArray = new byte[4];
                numArray[0] = (byte)96;
                byte[] input = numArray;
                extension.WriteBytes((uint)num, input);
            }
            else
                Form1.PS3.Extension.WriteBytes(6797348U, new byte[4]
                {
          (byte) 144,
          (byte) 154,
          (byte) 0,
          (byte) 0
                });
        }
        //Target finder
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox11.Checked)
            {
                Extension extension = Form1.PS3.Extension;
                int num = 30166008;
                byte[] numArray = new byte[4];
                numArray[0] = (byte)96;
                byte[] input = numArray;
                extension.WriteBytes((uint)num, input);
            }
            else
                Form1.PS3.Extension.WriteBytes(30166008U, new byte[4]
                {
          (byte) 72,
          (byte) 80,
          (byte) 110,
          (byte) 245
                });
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //Message to PS3 screen
        private void button6_Click(object sender, EventArgs e)
        {
            Form1.PS3.CCAPI.Notify(CCAPI.NotifyIcon.TROPHY4, this.textBox1.Text);
        }


        //System info
        private void button7_Click(object sender, EventArgs e)
        {
            //GET SYSINFOs


            string GetattProc = PS3.CCAPI.GetAttachedProcess().ToString();
            string FMWV = PS3.CCAPI.GetFirmwareVersion();
            string CPU = PS3.CCAPI.GetTemperatureCELL();
            string RSX = PS3.CCAPI.GetTemperatureRSX();


        /*    //GET CID

            byte[] buffer = new byte[0x10];
            PS3.CCAPI.GetLv2Memory(9223372036858981040L, buffer);
            string str = BitConverter.ToString(buffer).Replace("-", " ");

            //GET PSID

            byte[] buffer2 = new byte[0x10];
            PS3.CCAPI.GetLv2Memory(9223372036859580196L, buffer2);
            string str2 = BitConverter.ToString(buffer2).Replace("-", " ");*/
        

            //Show Infos

          //  MessageBox.Show("Console IP: " + ipaddresstextbox.Text + Environment.NewLine + "ProcessID: " + GetattProc + Environment.NewLine + "ConsoleI//GET PSID: " + str + Environment.NewLine + "PSID: " + str2 + Environment.NewLine + "CPU TEMP: " + CPU + Environment.NewLine + "RSX TEMP: " + RSX + Environment.NewLine + "Firmware Version: " + FMWV);
         //  [/COLOR]
    }

    }
    }
    
    

