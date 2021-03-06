﻿using System;
using System.Collections.Generic;
using System.Net.Security;
using System.ServiceModel;

using DataContracts;

namespace SmartSystem.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IMessageServiceCallback), Namespace = "http://david.leatham.com/MyService/2016/1", Name = "IMyService")]
    public interface IMyService
    {
        [OperationContract]
        bool RegisterClient(Guid id, string clientName);

        [OperationContract]
        List<string> GetAvailableServices();
    }

    [ServiceContract(Namespace = "http://david.leatham.com/MessageServiceCallback/2016/1", Name = "IMessageServiceCallback")]
    public interface IMessageServiceCallback
    {
        [OperationContract]
        List<SmartCommand> GetCommands();

        [OperationContract]
        void AnnounceNewService(string service);

        [OperationContract]
        SmartReturnObject CallCommand(SmartCommand command);
    }


}
