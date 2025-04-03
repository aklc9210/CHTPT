using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database2ServerApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int serverId = 2;
            int port = 5002;
            /* if (args.Length >= 1 && int.TryParse(args[0], out int id))
                 serverId = id;
             if (args.Length >= 2 && int.TryParse(args[1], out int p))
                 port = p;*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Database2ServerForm(serverId, port));
        }
    }
}
