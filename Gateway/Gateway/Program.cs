using System.Diagnostics;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            //File.WriteAllBytes("libraries", Properties.Resources.gateway);
            //ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + "\\libraries", Directory.GetCurrentDirectory());

         

            Communication.Listen(130);
            Process.Start("LoginService.exe");
        }
    }
}