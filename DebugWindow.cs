using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtelRezervasyon
{
    public partial class DebugWindow : Form
    {
        // You can close this window by making this bool false
        static bool debugMode = false;

        private static ulong __printCount__ = 0;
        public static bool DebugMode
        {
            get { return debugMode; }
            set
            {
                if (value)
                    WindowManager.DebugWindow.Show();
                else
                    WindowManager.DebugWindow.Close();
                debugMode = value;
            }
        }
        public DebugWindow()
        {
            InitializeComponent();
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {
            #region flexRight
            // Ekranın genişliğini al
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;

            // Pencerenin genişliğini al
            int windowWidth = this.Width;

            // Yeni X konumunu hesapla (ekranın genişliği - pencerenin genişliği)
            int newX = screenWidth - windowWidth;

            // Y koordinatını koru
            int newY = this.Location.Y;

            // Pencerenin konumunu ayarla
            this.Location = new Point(newX, newY);
            #endregion
        }

        public static void Print(object obj)
        {
            var box = WindowManager.DebugWindow.DebugTextBox;
            box.Text = " > " + "(" + ++__printCount__ + ")" + " " + obj.ToString() + "\n\n" + box.Text;
            box.ScrollToCaret();
        }
    }
}
