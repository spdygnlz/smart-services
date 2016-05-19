using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class SmartService
    {
        [DataMember]
        public string ServiceName { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public List<SmartCommand> Commands { get; set; }

        public SmartService(Guid id, string serviceName)
        {
            ServiceName = serviceName;
            Commands = new List<SmartCommand>();
            Id = id;
        }

        public override string ToString()
        {
            return $"{ServiceName} ({Id}): {string.Join(", ", Commands)}";
        }
    }

    [DataContract]
    public class SmartCommand
    {
        private readonly Func<object, object> _commandAction;

        public SmartCommand()
        {
            Parameters = new List<CommandParameter>();
            Return = null;
        }

        public SmartCommand(Func<object, object> commandAction)
            : this()
        {
            _commandAction = commandAction;
        }

        public Func<object, object> LocalAction => _commandAction;

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public List<CommandParameter> Parameters { get; set; }        

        [DataMember]
        public SmartReturnObject Return { get; set; }

        public override string ToString()
        {
            string returnString = Return?.ToString() ?? string.Empty;
            return $"{returnString} {DisplayName} ({string.Join(", ", Parameters)})".Trim();
        }
    }

    [DataContract]
    public class SmartReturnObject
    {
        [DataMember]
        public SmartType Type { get; set; }

        [DataMember]
        public object Value { get; set; }

        public override string ToString()
        {
            return $"{Type}";
        }
    }

    [DataContract]
    public class CommandParameter
    {
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public SmartType ParamType { get; set; }

        [DataMember]
        public object Value { get; set; }

        public override string ToString()
        {
            return $"{ParamType} {DisplayName}";
        }
    }

    [DataContract]
    public enum SmartType
    {
        [EnumMember]
        UInt,
        [EnumMember]
        Int,
        [EnumMember]
        Float,
        [EnumMember]
        Double,
        [EnumMember]
        String,
        [EnumMember]
        Guid
    }
}
