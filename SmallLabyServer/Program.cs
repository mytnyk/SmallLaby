using System;
using System.ServiceModel;

namespace SmallLabyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost service_host = new ServiceHost(typeof(SmallLaby));
            service_host.Open();

            Console.WriteLine("Press return to finish server process.\n");
            Console.ReadLine();

            service_host.Close();
            Game.Instance.GameOver = true;
        }
    }
}
