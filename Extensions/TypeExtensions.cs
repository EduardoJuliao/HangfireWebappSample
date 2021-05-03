using System;
using System.Collections.Generic;
using System.Linq;

namespace HangfireWebAppSample.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetClassesAssignableFrom(this Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && p.IsClass);
        }
    }
}
