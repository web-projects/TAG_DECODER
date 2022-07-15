using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VIPA_PARSER.Config.Binder
{
    public static class PropertyBinderTypeResolver
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Bind(string environmentVariableName, PropertyInfo property, object parentObject)
        {
            Type propertyType = property.PropertyType;
            string environmentVariableValue = Environment.GetEnvironmentVariable(environmentVariableName);

            if (propertyType == typeof(bool))
            {
                if (!bool.TryParse(environmentVariableValue, out bool result))
                {
                    throw new InvalidCastException(GetConversionErrorMessage(environmentVariableName, nameof(Boolean)));
                }

                property.SetValue(parentObject, result);
            }
            else if (propertyType == typeof(int))
            {
                if (!int.TryParse(environmentVariableValue, out int result))
                {
                    throw new InvalidCastException(GetConversionErrorMessage(environmentVariableName, nameof(Int32)));
                }

                property.SetValue(parentObject, result);
            }
            else if (propertyType == typeof(string))
            {
                property.SetValue(parentObject, environmentVariableValue);
            }
            else
            {
                throw new Exception($"Unable to determine cast conversion for environment variable '{environmentVariableName}' on property '{property.Name}'.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetConversionErrorMessage(string environtVariableName, string expectedType)
            => $"Unable to convert '{environtVariableName}' to expected type '{expectedType}'";
    }
}
