using System;
using Bridge;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Ezkrocer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InjectionResult result = Injector.Inject();
            if (result.Result == InjectionError.Success)
            {
                Sentinel.Start();
                MessageBox.Show("Injetado");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sentinel.ExecuteScript(richTextBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sentinel.ExecuteScript(richTextBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Authenticator.LoginViaHWIDAsync();

            IPC.Instance.Connected += Instance_Connected;
            IPC.Instance.Disconnected += Instance_Disconnected;
            var path = Directory.GetCurrentDirectory();
            var parent = Directory.GetParent(path).FullName;
            var newPath = Path.Combine(parent, "scripts");
            DirectoryInfo dinfo = new DirectoryInfo(newPath);

            FileInfo[] files = dinfo.GetFiles("*.txt");
            foreach (FileInfo file in files)
            {
                listBox1.Items.Add(file.Name);
            }

        }

        private void Instance_Connected()
        {
            label1.ForeColor = Color.DarkOliveGreen;
        }

        private void Instance_Disconnected()
        {
            label1.ForeColor = Color.Crimson;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "LUA Files|*.lua|TXT Files|*.txt|All Files|*.*";
            opn.RestoreDirectory = true;

            if (opn.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = System.IO.File.ReadAllText(opn.FileName);
            }
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var parent = Directory.GetParent(path).FullName;
            var newPath = Path.Combine(parent, "scripts");
            richTextBox1.Text = File.ReadAllText(newPath + $"\\{listBox1.SelectedItem}");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var path = Directory.GetCurrentDirectory();
            var parent = Directory.GetParent(path).FullName;
            var newPath = Path.Combine(parent, "scripts");

            Sentinel.ExecuteScript(File.ReadAllText(newPath +  $"\\{listBox1.SelectedItem}"));
        }
    }
}
