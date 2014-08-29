using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetTextinCursor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.MainWindowTitle.Contains("Notepad"))
                {
                    SetForegroundWindow(proc.MainWindowHandle);
                    Thread.Sleep(1000);
                    SendKeys.SendWait("^C");
                    //IDataObject idata = Clipboard.GetDataObject();
                    //Console.WriteLine(idata.ToString());
                    if (Clipboard.ContainsText())
                    {
                        string text = Clipboard.GetText(TextDataFormat.UnicodeText);
                        Console.WriteLine(text);
                    }
                    else
                    {
                        Console.WriteLine("No Text");
                    }
                }
            }
        }
    }
}
