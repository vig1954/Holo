using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera;
using HolographicInterferometryVNext;
using Infrastructure;

namespace SimpleApplication
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            Singleton.Register(new CameraConnector());
            Singleton.Register(new OpenClSourcesProvider());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
