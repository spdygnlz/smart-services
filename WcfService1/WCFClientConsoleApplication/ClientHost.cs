using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;

using SmartSystem.IndependentService;
using SmartSystem.Interfaces;

namespace WCFClientConsoleApplication
{
    
    class ClientHost 
    {
        private DiscoveryClient discoveryClient;

        private Client client = null;

        static void Main(string[] args)
        {
            var program = new ClientHost();

            Console.WriteLine("Press any key to start method 1...");
            Console.ReadKey(true);

            program.Method2();

            //Console.WriteLine("Press any key to start method 2...");
           // Console.ReadKey(true);

           // program.Method2();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);

            program.Close();
        }

        public void Method2()
        {
            // foreach (var binding in new Binding[] { new NetTcpBinding(), new WSDualHttpBinding() })
            {
                var endpoint = new DynamicEndpoint(ContractDescription.GetContract(typeof(IMyService)), new NetTcpBinding());

                client = new Client();
                var ic = new InstanceContext(client);

                var factory = new DuplexChannelFactory<IMyService>(ic, endpoint);
                
                var proxy = factory.CreateChannel();

                client.Proxy = proxy;

                client.Register();

                client.Discover(); 
            }
        }

        public void Close()
        {
            ((ICommunicationObject)client.Proxy).Close();
        }
    }

}
