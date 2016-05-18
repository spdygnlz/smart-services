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
            foreach (var binding in new Binding[] { new NetTcpBinding(), new WSDualHttpBinding() })
            {
                var endpoint = new DynamicEndpoint(ContractDescription.GetContract(typeof(IMyService)), binding);

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


        public void Method1()
        {
            discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            
            discoveryClient.FindProgressChanged += Client_FindProgressChanged;
            discoveryClient.FindCompleted += ClientOnFindCompleted;
            var criteria = new FindCriteria(typeof(IMyService)) { Duration = new TimeSpan(0, 0, 5) };

            discoveryClient.FindAsync(criteria);
        }

        private void Client_FindProgressChanged(object sender, FindProgressChangedEventArgs e)
        {
            Console.WriteLine($"Found Endpoint at {e.EndpointDiscoveryMetadata.Address}");
        }

        private void ClientOnFindCompleted(object sender, FindCompletedEventArgs findCompletedEventArgs)
        {
            if (findCompletedEventArgs.Cancelled)
            {
                Console.WriteLine("Discovery Cancelled");
            }
            else if (findCompletedEventArgs.Error != null)
            {
                Console.WriteLine(findCompletedEventArgs.Error.Message);                
            }
            else
            {
                foreach (var item in findCompletedEventArgs.Result.Endpoints)
                {
                    Console.WriteLine(item.Address);
                }

                Console.WriteLine();

                var address = findCompletedEventArgs.Result.Endpoints.First(ep => ep.Address.Uri.Scheme == "net.tcp").Address;

                Console.WriteLine(address);

                var client = new Client();
                var ic = new InstanceContext(client);

                var factory = new DuplexChannelFactory<IMyService>(ic, new NetTcpBinding(), address);

                var proxy = factory.CreateChannel();

                client.Proxy = proxy;

                client.Register();

                client.Discover();

                ((ICommunicationObject)proxy).Close();
            }

            this.discoveryClient = null;
        }
    }

}
