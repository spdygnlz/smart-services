using System;
using System.Collections.Generic;
using System.ServiceModel;

using SmartSystem.Interfaces;

namespace SmartSystem.IndependentService
{
    [CallbackBehavior(IncludeExceptionDetailInFaults = true)]
    public class Client : IMessageServiceCallback
    {
        public IMyService Proxy { get; set; }

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
            return new List<string>() { "test 1", "Test 2" };// .Values.ToList();
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
