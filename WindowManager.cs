using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtelRezervasyon
{
    public static class WindowManager
    {
        public static PasswordWindow PasswordWindow;
        public static MainMenu MainMenu = new MainMenu();
        public static DebugWindow DebugWindow = new DebugWindow();
        public static PersonelMenu PersonelMenu = new PersonelMenu();


        private static bool isSetDone = false;

        /// <summary> must be called one time at beginning </summary>
        public static void SetFormClose()
        {
            if (!isSetDone)
            {
                FormClosedEventHandler act = (sender, e) => { Application.Exit(); };
                MainMenu.FormClosed += act;
                PasswordWindow.FormClosed += act;
                isSetDone = true;
            }
        }

        public static void ShowAboutWindow()
        {
            MessageBox.Show("Muhammet Mustafa Özeski tarafından kodlandı");
        }
    }
}
