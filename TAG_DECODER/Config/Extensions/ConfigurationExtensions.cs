using Microsoft.Extensions.Configuration;
using System.Reflection;
using VIPA_PARSER.Config.Binder;

namespace VIPA_PARSER.Config.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetByEnv<T>(this IConfigurationSection configurationSection)
            where T : new()
        {
            T configurationObject = configurationSection.Get<T>();
            configurationObject ??= new T();
            return RecursivePropertyBinding<T>(configurationObject);
        }

        /**
         * Surprisingly enough you cannot get away with tail recursion to further
         * optimize this chunk of code to run 3x - 5x faster. Therefore, since
         * the probability of deeply nested reference objects is low for this
         * method use case, it's safe to assume we'll never run into a stack overflow
         * and we'll have acceptable performance.
         */
        private static T RecursivePropertyBinding<T>(object configurationObject)
        {
            if (configurationObject is null)
            {
                return default;
            }

            PropertyInfo[] properties = configurationObject.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<EnvConfigBinderAttribute>() is { } binderAttribute)
                {
                    PropertyBinderTypeResolver.Bind(binderAttribute.EnvironmentConfigKey, property, configurationObject);
                }
                else if (property.PropertyType.IsClass && 
                    !property.PropertyType.Namespace.StartsWith("System"))
                {
                    RecursivePropertyBinding<object>(property.GetValue(configurationObject));
                }
            }

            return (T)configurationObject;
        }
    }
}
