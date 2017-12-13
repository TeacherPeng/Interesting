using System;
using System.Windows.Forms;

namespace VisualsHitTesting
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}