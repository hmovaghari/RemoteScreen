using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;

namespace RemoteScreen
{
    public partial class Form1 : Form
    {
        enum CaptureMode
        {
            Ready,
            Wait
        }

        enum ReaderMode
        {
            Ready,
            Wait
        }

        enum ReaderMouseMode
        {
            Ready,
            Wait
        }

        enum MouseMode
        {
            MouseMove,
            MouseClick,
            DoubleClick,
            MouseDown,
            MouseUp
        }

        struct ScreenSize
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public bool isCapture { get; set; }
        PictureBox pictureBox1;
        ScreenSize screenSize;
        int interval;
        private static string screenShotFile = "Screenshot.png";

        public Form1(bool isCapture)
        {
            InitializeComponent();
            this.isCapture = isCapture;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RenameStopStartCaptureButton(btnStop, isTickStarted: true);
            ReadInterval();
            DeleteMode();
            DeleteMouse();
            if (isCapture)
            {
                GetScreenSize();
                HideForm();
                CreateMode(CaptureMode.Ready);
                RunScreenCapture();
                RunMouseEvent();
            }
            else if (!isCapture)
            {
                Maximize();
                CreateMode(ReaderMode.Wait);
                CreateMouse();
                AddPictureBox();
                GetScreen();
            }
        }

        private void RunScreenCapture()
        {
            tickCapture.Enabled = true;
            tickCapture.Tick += TickCapture_Tick;
            tickCapture.Start();
        }

        private void GetScreenSize()
        {
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            screenSize.Width = screen.Width;
            screenSize.Height = screen.Height;
            screenSize.X = screen.X;
            screenSize.Y = screen.Y;
        }

        private void TickCapture_Tick(object sender, EventArgs e)
        {
            bool isCaptureReady = IsMode(CaptureMode.Ready);
            bool isReaderWait = IsMode(ReaderMode.Wait);
            if (isCaptureReady && isReaderWait)
            {
                tickCapture.Interval = GetInterval();
                CaptureScreen();
            }
        }

        private void CaptureScreen()
        {
            //ScreenCapture.WindowScreenshot(this.Handle, screenShotFile, ImageFormat.Png, @"Everyone",
            //    FileSystemRights.FullControl, AccessControlType.Allow);
            ScreenCapture.FullScreenshot(this.Handle, screenShotFile, ImageFormat.Png, @"Everyone",
                FileSystemRights.FullControl, AccessControlType.Allow);
            //GetCursorPosition();
            ChangeMode(CaptureMode.Ready, CaptureMode.Wait);
            ChangeMode(ReaderMode.Wait, ReaderMode.Ready);
        }

        private void RunMouseEvent()
        {
            tickCaptureMouse.Enabled = true;
            tickCaptureMouse.Tick += tickCaptureMouse_Tick;
            tickCaptureMouse.Start();
        }

        private void GetScreen()
        {
            tickReader.Enabled = true;
            tickReader.Tick += tickReader_Tick;
            tickReader.Start();
        }

        private void tickReader_Tick(object sender, EventArgs e)
        {
            bool isCaptureWait = IsMode(CaptureMode.Wait);
            bool isReaderReady = IsMode(ReaderMode.Ready);
            if (isCaptureWait && isReaderReady)
            {
                tickReader.Interval = GetInterval();
                ReadScreen();
            }
        }

        private void ReadScreen()
        {
            //Point cursor = GetCursorPosition();
            Image img;
            using (var bmpTemp = new Bitmap("Screenshot.png"))
            {
                img = new Bitmap(bmpTemp);
            }
            //Image mouseCursor = Properties.Resources.cursor16;
            //Graphics g = Graphics.FromImage(img);
            //g.DrawImage(mouseCursor, cursor.X, cursor.Y);
            pictureBox1.Image = img;
            ChangeMode(ReaderMode.Ready, ReaderMode.Wait);
            ChangeMode(CaptureMode.Wait, CaptureMode.Ready);
        }

        private static string GetFilePath(string fileName = null)
        {
            /*
            Directory.GetCurrentDirectory()
            Environment.CurrentDirectory;
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            System.AppDomain.CurrentDomain.BaseDirectory;
            */
            if (fileName != null)
                return Directory.GetCurrentDirectory() + "\\" + fileName;
            else
                return Directory.GetCurrentDirectory();
        }

        private bool IsMode(CaptureMode cMode)
        {
            string modPath = GetFilePath("Capture." + cMode.ToString().ToLower());
            return File.Exists(modPath);
        }

        private bool IsMode(ReaderMode rMode)
        {
            string modPath = GetFilePath("Reader." + rMode.ToString().ToLower());
            return File.Exists(modPath);
        }

        private void ChangeMode(CaptureMode oldCMode, CaptureMode newCMode)
        {
            string oldPath = GetFilePath("Capture." + oldCMode.ToString().ToLower());
            string newPath = GetFilePath("Capture." + newCMode.ToString().ToLower());
            File.Move(oldPath, newPath);
        }

        private void ChangeMode(ReaderMode oldRMode, ReaderMode newRMode)
        {
            string oldPath = "Reader." + oldRMode.ToString().ToLower();
            string newPath = "Reader." + newRMode.ToString().ToLower();
            File.Move(oldPath, newPath);
        }

        private void CreateMode(CaptureMode cMode)
        {
            string modPath = GetFilePath("Capture." + cMode.ToString().ToLower());
            File.Create(modPath).Close();
            Secuirty.AddFileSecurity(modPath, @"Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
        }

        private void CreateMode(ReaderMode rMode)
        {
            string modPath = GetFilePath("Reader." + rMode.ToString().ToLower());
            File.Create(modPath).Close();
            Secuirty.AddFileSecurity(modPath, @"Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
        }

        private void DeleteMode()
        {
            string fileName = (isCapture) ? "Capture" : "Reader";

            string modPath = GetFilePath(fileName + "." + CaptureMode.Ready.ToString().ToLower());
            if (File.Exists(modPath))
                File.Delete(modPath);

            modPath = GetFilePath(fileName + "." + CaptureMode.Wait.ToString().ToLower());
            if (File.Exists(modPath))
                File.Delete(modPath);
        }

        private void CreateMouse()
        {
            string readerMouse = GetFilePath("ReaderMouse." + ReaderMouseMode.Ready.ToString().ToLower());
            File.Create(readerMouse).Close();
            Secuirty.AddFileSecurity(readerMouse, @"Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
        }

        private void DeleteMouse()
        {
            if (isCapture)
                return;

            string fileName = "ReaderMouse";
            string mousePath = GetFilePath(fileName + "." + ReaderMouseMode.Ready.ToString().ToLower());
            if (File.Exists(mousePath))
                File.Delete(mousePath);

            mousePath = GetFilePath(fileName + "." + ReaderMouseMode.Wait.ToString().ToLower());
            if (File.Exists(mousePath))
                File.Delete(mousePath);
        }

        private void ReadInterval()
        {
            string filePath = GetFilePath();
            string[] files = Directory.GetFiles(filePath, "Interval.*");
            if (files.Length == 1)
            {
                string[] array = files[0].Split('.');
                string extention = array[array.Length - 1];
                interval = int.Parse(extention);
                return;
            }
            else if (files.Length >= 0)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            File.Create("Interval.100").Close();
            interval = 100;
        }

        private int GetInterval()
        {
            if (!File.Exists("Interval." + interval))
            {
                ReadInterval();
            }
            return interval;
        }

        private void WriteInterval(int interval)
        {
            if (isCapture)
                tickCapture.Stop();
            else
                tickReader.Stop();

            Form2 form2 = new Form2(interval);
            form2.ShowDialog();
            if (form2.DialogResult == DialogResult.OK)
            {
                string oldInterval = GetFilePath("Interval." + interval);
                string newInterval = GetFilePath("Interval." + form2.interval);
                File.Move(oldInterval, newInterval);
                interval = 1;
            }

            if (isCapture)
                tickCapture.Start();
            else
                tickReader.Start();
        }

        private void HideForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = BackColor;
            this.ShowInTaskbar = false;
            this.Size = new Size(0, 0);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Maximize()
        {
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void AddPictureBox()
        {
            pictureBox1 = new PictureBox();
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            this.Controls.Add(pictureBox1);
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            string readerMouse = GetFilePath("ReaderMouse." + ReaderMouseMode.Ready.ToString().ToLower());
            if (File.Exists(readerMouse))
            {
                GetMousePosition(readerMouse, MouseMode.MouseMove, e, pictureBox1.Size);
                ChangeMouseMode(ReaderMouseMode.Ready, ReaderMouseMode.Wait);
            }
        }

        private void tickCaptureMouse_Tick(object sender, EventArgs e)
        {
            string readerMouse = GetFilePath("ReaderMouse." + ReaderMouseMode.Wait.ToString().ToLower());
            if (File.Exists(readerMouse))
            {
                tickCaptureMouse.Interval = GetInterval();
                string reader = File.ReadAllText(readerMouse);
                string[] mouse = reader.ToString().Split(',');

                if (mouse[0] == MouseMode.MouseMove.ToString())
                {
                    MoveMouse(new Point(int.Parse(mouse[1]), int.Parse(mouse[2])),
                                new Point(int.Parse(mouse[3]), int.Parse(mouse[4])));
                }
                else if (mouse[0] == MouseMode.MouseClick.ToString())
                {

                }
                else if (mouse[0] == MouseMode.DoubleClick.ToString())
                {

                }
                else if (mouse[0] == MouseMode.MouseDown.ToString())
                {

                }
                else if (mouse[0] == MouseMode.MouseUp.ToString())
                {

                }

                ChangeMouseMode(ReaderMouseMode.Wait, ReaderMouseMode.Ready);
            }
        }

        private void MoveMouse(Point readerClickedPosition, Point spaceSize)
        {
            Cursor.Position = new Point((readerClickedPosition.X * screenSize.Width) / spaceSize.X,
                (readerClickedPosition.Y * screenSize.Height) / spaceSize.Y);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
        }

        private void ChangeMouseMode(ReaderMouseMode oldRMMode, ReaderMouseMode newRMMode)
        {
            string oldPath = GetFilePath("ReaderMouse." + oldRMMode.ToString().ToLower());
            string newPath = GetFilePath("ReaderMouse." + newRMMode.ToString().ToLower());
            File.Move(oldPath, newPath);
        }

        private void GetMousePosition(string filePath, MouseMode mouseMode, MouseEventArgs mouseClickPosition,
            Size spaceSize)
        {
            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                string output = mouseMode + "," + mouseClickPosition.X + "," + mouseClickPosition.Y + "," +
                    spaceSize.Width + "," + spaceSize.Height;
                outputFile.Write(output);
            }

            //int x;
            //int y;
            //string fileName = "Cursor.pos";
            //if (isCapture)
            //{
            //    x = Cursor.Position.X;
            //    y = Cursor.Position.Y;
            //    using (StreamWriter file = new StreamWriter(fileName))
            //    {
            //        file.Write($"{x},{y},{screenSize.Width},{screenSize.Height}");
            //    }
            //    Secuirty.AddFileSecurity(fileName, @"Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
            //    return new Point(x, y);
            //}
            //using (StreamReader file = new StreamReader(fileName))
            //{
            //    MessageBox.Show(file.Read().ToString());
            //    string[] cursorPosition = file.Read().ToString().Split(',');
            //    x = (int.Parse(cursorPosition[0]) / screenSize.Width) * int.Parse(cursorPosition[2]);
            //    MessageBox.Show(x.ToString());
            //    y = (int.Parse(cursorPosition[1]) / screenSize.Height) * int.Parse(cursorPosition[3]);
            //    MessageBox.Show(y.ToString());
            //}
            //return new Point(x, y);
        }

        private Point GetMousePosition()
        {
            return Cursor.Position;
        }

        private void RenameStopStartCaptureButton(ToolStripItem btn, bool isTickStarted)
        {
            btn.Text = (isTickStarted) ? "Stop" : "Start";
            if (isCapture)
                btnStop.Text += " Capture";
            else
                btnStop.Text += " Reader";
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;

            switch (item.Name)
            {
                case "btnStop":
                    StopTick(item);
                    break;

                case "btnStart":
                    StartTick(item);
                    break;

                case "btnSettings":
                    WriteInterval(interval);
                    break;

                case "btnExit":
                    this.Dispose();
                    break;
            }
        }

        private void StartTick(ToolStripItem item)
        {
            if (isCapture)
                tickCapture.Start();
            else
                tickReader.Start();
            RenameStopStartCaptureButton(item, isTickStarted: true);
            item.Name = "btnStop";
        }

        private void StopTick(ToolStripItem item = null)
        {
            if (isCapture)
                tickCapture.Stop();
            else
                tickReader.Stop();
            if (item != null)
            {
                RenameStopStartCaptureButton(item, isTickStarted: false);
                item.Name = "btnStart";
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (!isCapture)
                if (WindowState == FormWindowState.Normal)
                    WindowState = FormWindowState.Maximized;
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            Application_Exit();
        }

        private void Form1_Disposed(object sender, EventArgs e)
        {
            Application_Exit();
        }

        private void Application_Exit()
        {
            StopTick();
            DeleteMode();
            DeleteMouse();
            notifyIcon1.Dispose();
            Application.Exit();
        }
    }
}
