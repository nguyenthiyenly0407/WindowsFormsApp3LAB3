using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create and show both forms
            SERVER1 form2 = new SERVER1();
            CLIENT1 form1 = new CLIENT1();
            form1.Show();
            form2.Show();

            Application.Run();
        }
    }
}
