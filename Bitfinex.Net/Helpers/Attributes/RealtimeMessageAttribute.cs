using System;
using System.Linq;
using System.Reflection;

namespace Bitfinex.Net.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RealtimeMessageAttribute : Attribute
    {
        public RealtimeMessageAttribute(string eventName)
        {
            EventName = eventName;
        }

        public string EventName { get; protected set; }

        public static string GetValue(Type type)
        {
            return
                type.GetCustomAttributes(typeof(RealtimeMessageAttribute))
                    .Cast<RealtimeMessageAttribute>()
                    .Select(attribute => attribute.EventName)
                    .FirstOrDefault();
        }
    }
}