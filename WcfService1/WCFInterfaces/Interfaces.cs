using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFInterfaces
{
    [ServiceContract(CallbackContract = typeof(IMessageServiceCallback))]
    public interface IMyService
    {
        [OperationContract]
        bool RegisterClient(Guid id, string clientName);

        [OperationContract]
        List<string> GetAvailableServices();
    }

    [ServiceContract]
    public interface IMessageServiceCallback
    {
        [OperationContract]
        List<string> GetCommands();

        [OperationContract]
        void AnnounceNewService(string service);
    }

    //[DataContract]
    //public class SmartService
    //{
    //    [DataMember]
    //    public string ServiceName { get; set; }

    //    [DataMember]
    //    public Guid Id { get; set; }

    //    [DataMember]
    //    public List<string> Commands { get; set; }

    //    public SmartService(Guid id, string serviceName)
    //    {
    //        ServiceName = serviceName;
    //        Commands = new List<string>();
    //        Id = id;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{ServiceName} ({Id}): {string.Join(", ", Commands)}";
    //    }
    //}

    //[DataContract]
    //public class SmartCommand
    //{
    //    public SmartCommand()
    //    {
    //        Parameters = new List<CommandParameter>();
    //    }

    //    [DataMember]
    //    public Guid Id { get; set; }

    //    [DataMember]
    //    public string DisplayName { get; set; }

    //    [DataMember]
    //    public List<CommandParameter> Parameters { get; set; }
    //}

    //[DataContract]
    //public class CommandParameter
    //{
    //    [DataMember]
    //    public string DisplayName { get; set; }

    //    [DataMember]
    //    public int ParamType { get; set; }

    //    [DataMember]
    //    public object Value { get; set; }
    //}

    //[DataContract]
    //public enum SmartType
    //{
    //    UInt,

    //    Int,

    //    Float,

    //    Double,

    //    String,

    //    Guid
    //}

}
