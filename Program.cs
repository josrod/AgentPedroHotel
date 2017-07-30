using System;
using BotASR;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using WebApp;

namespace AIEBOT
{

    class Program
    {

        #region Para menejar El Pos y size de la Consola
        const int SWP_NOSIZE = 0x0001;

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();
        #endregion

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SacarinoForm formSacarino = new SacarinoForm();
            formSacarino.WebApp("", "");
            
            // Funcion para posicionar el FomrSacarino en la ezquina derecha superior
            PositionReporterEdge(formSacarino);

            // Modifico la posición de la ventana de la consola
            AllocConsole();
            IntPtr MyConsole = GetConsoleWindow();
            int xpos = 0;
            int ypos = 650;
            SetWindowPos(MyConsole, 0, xpos, ypos, 0, 0, SWP_NOSIZE);


            Principal myPrincipal;
            myPrincipal = new Principal();
           
            Thread ct = new Thread(
                new ThreadStart(
                 delegate()
                 {
                      while (true)
                      {
                          Console.Write("You: ");
                          string input = Console.ReadLine();
                          //if (input.ToLower() == "quit")
                          //{
                          //    break;
                          //}
                          //else
                          //{
                            myPrincipal.IniciarPrograma(formSacarino,input);
                          //}
                      }
                  }));
            ct.Start();
            formSacarino.ShowDialog();
           
            //Application.Run(new SacarinoForm()); //  esto no lo estoy utilizando
            Environment.Exit(0);


        }

        /// <summary>
        /// Position the "SacarinoForm"
        /// </summary>
        private static void PositionReporterEdge(Form formSacarino)
        {
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;

            Point parentPoint = formSacarino.Location;

            int parentHeight = formSacarino.Height;
            int parentWidth = formSacarino.Width;

            //formSacarino.Size = new Size(parentWidth, parentHeight);

            int resultX;
            int resultY;

            // Position on the edge.
            resultY = parentPoint.Y;
            resultX = parentPoint.X + screenWidth - parentWidth - 2;

            // set our child form to the new position
            formSacarino.Location = new Point(resultX, resultY);
        }

    }
}
