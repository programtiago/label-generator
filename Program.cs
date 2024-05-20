using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace pocketlabeldata;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());

    }   
}

