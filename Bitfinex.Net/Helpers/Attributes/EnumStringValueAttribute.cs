using System;
using System.Linq;
using System.Reflection;

namespace Bitfinex.Net.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumStringValueAttribute : Attribute
    {
        public EnumStringValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; protected set; }

        public static string GetValue(Enum value)
        {
            return
                value.GetType()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(EnumStringValueAttribute))
                    .Cast<EnumStringValueAttribute>()
                    .Select(attribute => attribute.Value)
                    .FirstOrDefault();
        }
    }
}