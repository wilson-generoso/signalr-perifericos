using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public class ParameterValue
    {
        public ParameterValue(string connectionId)
        {
            this.Context = new SignalRHubContext(connectionId);
        }

        public SignalRHubContext Context { get; }
    }

    public class ParameterValue<TValue> : ParameterValue
    {
        public ParameterValue(TValue value, string connectionId)
            : base(connectionId)
        {
            this.Value = value;
        }

        public virtual TValue Value { get; }
    }

    public class ParameterValue<TValue, TContextValue>
    {
        public ParameterValue(TValue value, TContextValue contextValue, string connectionId)
        {
            this.Value = value;
            this.Context = new SignalRHubContext<TContextValue>(connectionId, contextValue);
        }

        public SignalRHubContext<TContextValue> Context { get; }

        public virtual TValue Value { get; }
    }

    public static class ParameterValueExtensions
    {
        public static ParameterValue<TValue> ToParameterValue<TValue>(this TValue value, string connectionId)
        {
            return new ParameterValue<TValue>(value, connectionId);
        }

        public static ParameterValue<TValue, TContextValue> ToParameterValue<TValue, TContextValue>(this TValue value, string connectionId, TContextValue contextValue)
        {
            return new ParameterValue<TValue, TContextValue>(value, contextValue, connectionId);
        }
    }
}
