using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using DataContracts;

using SmartSystem.Interfaces;

namespace SmartSystem.IndependentService
{
    [CallbackBehavior(IncludeExceptionDetailInFaults = true)]
    public class Client : IMessageServiceCallback
    {
        public IMyService Proxy { get; set; }

        private Guid _identifier;

        private Dictionary<Guid, SmartCommand> commands = new Dictionary<Guid, SmartCommand>();

        public Client()
        {
            _identifier = Guid.NewGuid();
            Console.WriteLine($"Started new Client with instance ID: {_identifier}");
            ConfigureCommands();
        }

        private void ConfigureCommands()
        {
            var returnObj = new SmartReturnObject { Type = SmartType.Int };
            var command = new SmartCommand((percent) => OpenBlinds((int)percent)) { DisplayName = "OpenBlinds", Id = Guid.NewGuid(), Return = returnObj};
            var parameter = new CommandParameter { DisplayName = "Percentage", ParamType = SmartType.Int };
            command.Parameters.Add(parameter);
            commands.Add(command.Id, command);
        }

        private int OpenBlinds(int percentage)
        {
            Console.WriteLine($"Opening Blinds to {percentage} percent.");
            return percentage;
        }

        public SmartReturnObject CallCommand(SmartCommand command)
        {
            var temp = commands[command.Id].LocalAction;
            
            var result = temp(command.Parameters.First().Value);
            var retVal = new SmartReturnObject() { Type = command.Return.Type, Value = result };
            return retVal;
        }

        public List<SmartCommand> GetCommands()
        {
            return commands.Values.ToList();
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
