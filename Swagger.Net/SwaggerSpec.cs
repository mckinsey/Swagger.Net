using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Swagger.Net
{
    public class SwaggerSpec
    {
        private static readonly Dictionary<Type, string> SYSTEM_TYPE_NAMES = new Dictionary<Type, string>
        {
            { typeof(bool), "boolean" },
            { typeof(sbyte), "byte" },
            { typeof(byte), "byte" },
            { typeof(ushort), "int" },
            { typeof(short), "int" },
            { typeof(uint), "int" },
            { typeof(int), "int" },
            { typeof(ulong), "long" },
            { typeof(long), "long" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "double" },
            { typeof(char), "string" },
            { typeof(string), "string" },
            { typeof(DateTime), "Date" },
            { typeof(TimeSpan), "Date" },
            { typeof(object), "object" },
        };

        public static string GetDataTypeName(Type type)
        {
            if (type == typeof(string)) //guard clause because string is also IEnumerable
                return GetNameFromSimpleType(type);

            if (type == typeof(IEnumerable) || type.GetInterfaces().Any(t => t == typeof(IEnumerable)))
            {
                var typeArg = type.IsGenericType ? type.GetGenericArguments().First() : typeof(object);
                return string.Format("Array[{0}]", GetDataTypeName(typeArg));
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                    return string.Format("{0}?", GetDataTypeName(type.GetGenericArguments().First()));
                var genericlessName = type.Name.Remove(type.Name.IndexOf("`"));
                return string.Format("{0}[{1}]", genericlessName, string.Join(", ", type.GetGenericArguments().Select(GetDataTypeName)));
            }
            return GetNameFromSimpleType(type);
        }

        private static string GetNameFromSimpleType(Type type)
        {
            if (SYSTEM_TYPE_NAMES.ContainsKey(type))
                return SYSTEM_TYPE_NAMES[type];
            return type.Name;
        }
    }
}