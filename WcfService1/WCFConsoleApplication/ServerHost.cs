using System;
using System.Collections.Generic;
using System.ServiceModel;

using SmartSystem.CentralService;

namespace WCFConsoleApplication
{
    public class ServerHost
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(MyService)))
            {
                host.Open();
                Console.WriteLine("The host is listening at:");
                foreach (var item in host.Description.Endpoints)
                {
                    Console.WriteLine("{0}\n\t{1}", item.Address, item.Binding);
                }

                Console.WriteLine();
                Console.ReadKey(true);
            }
        }
    }
}
