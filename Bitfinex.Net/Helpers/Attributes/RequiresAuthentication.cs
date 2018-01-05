using System;
using System.Linq;
using System.Reflection;

namespace Bitfinex.Net.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresAuthentication : Attribute
    {
        public static bool DoesRequireAuthentication(Type type)
        {
            return type.GetCustomAttributes(typeof(EnumStringValueAttribute)).Any();
        }
    }
}