using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using DataContracts;

using SmartSystem.Interfaces;

namespace SmartSystem.CentralService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public class MyService : IMyService
    {
        private IMessageServiceCallback _callback;
        private Dictionary<Guid, IMessageServiceCallback> _clients = new Dictionary<Guid, IMessageServiceCallback>();

        private List<string> _services = new List<string>();

        public bool RegisterClient(Guid id, string clientName)
        {
            var smartService = new SmartService(id, clientName);
            try
            {
                // Get the client that is requesting to be registered
                _callback = OperationContext.Current.GetCallbackChannel<IMessageServiceCallback>();

                // Get the commands this client supports
                var commands = _callback.GetCommands();
                smartService.Commands.AddRange(commands);

                _services.Add(clientName);

                // tell all of the other clients that the new client/service is here
                foreach (var client in _clients.Values)
                {
                    client.AnnounceNewService(clientName);
                }

                // Add the new client to the list of known clients
                _clients.Add(id, _callback);

                Console.WriteLine("Server added {0} to the list.", smartService);

                SmartCommand command = commands.FirstOrDefault();
                command.Parameters.First().Value = 42;
                var result = _callback.CallCommand(command);
                Console.WriteLine(result.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public List<string> GetAvailableServices()
        {
            return _services;
        }
    }
}
