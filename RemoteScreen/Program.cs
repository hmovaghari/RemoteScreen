using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteScreen
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] isCapture)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (CheckArgs(isCapture) && CheckInstance())
            {
                Application.Run(new Form1(bool.Parse(isCapture[0])));
            }
        }

        private static bool CheckArgs(string[] isCapture)
        {
            if (isCapture.Length != 1 || !(isCapture[0].ToLower() == "true" | isCapture[0].ToLower() == "false"))
            {
                MessageBox.Show("For screen capture run RemoteScreen.exe True\nFor get remote screen run RemoteScreen.exe False",
                    "Invalid parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool CheckInstance()
        {
            bool isInstanceOK = false;
            Mutex mutex = new Mutex(true, "MutexValue1", out isInstanceOK);
            if (!isInstanceOK)
            {
                MessageBox.Show("The program is already running on your pc", "Remote Screen",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}