using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;

using WCFInterfaces;

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

    [CallbackBehavior(IncludeExceptionDetailInFaults = true)]
    public class Client : IMessageServiceCallback
    {
        internal IMyService Proxy { get; set; }

        private Guid _identifier;

        //private Dictionary<Guid, SmartCommand> commands = new Dictionary<Guid, SmartCommand>();

        public Client()
        {
            _identifier = Guid.NewGuid();
            Console.WriteLine($"Started new Client with instance ID: {_identifier}");
            //ConfigureCommands();
        }

        //private void ConfigureCommands()
        //{
        //    var command = new SmartCommand { DisplayName = "OpenBlinds", Id = Guid.NewGuid() };
        //    var parameter = new CommandParameter { DisplayName = "Percentage", ParamType = 1 };

        //    command.Parameters.Add(parameter);

        //    commands.Add(command.Id, command);
        //}

        public List<string> GetCommands()
        {
            return new List<string>() {"test 1", "Test 2"};// .Values.ToList();
        }

        public void AnnounceNewService(string service)
        {
            Console.WriteLine($"New service added: {service}");
        }

        public void Register()
        {
            Proxy.RegisterClient(_identifier, nameof(Client));
        }

        public void Discover()
        {
            var services = Proxy.GetAvailableServices();

            Console.WriteLine("Discovered these other services:");
            foreach (var service in services)
            {
                Console.WriteLine(service);
            }
            //foreach (var smartService in services.Where(s => s.Id != _identifier))
            //{
            //    Console.WriteLine(smartService);
            //}
            //Console.WriteLine();
        }
    }
}
