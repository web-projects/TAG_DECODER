using System;

namespace VIPA_PARSER.Config.Binder
{ 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class EnvConfigBinderAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the environment variable key name to read from.
        /// </summary>
        public string EnvironmentConfigKey { get; }

        /// <summary>
        /// Initialize a new environment variable configuration binder.
        /// </summary>
        /// <param name="environmentConfigKey">The environment variable key name to bind to.</param>
        public EnvConfigBinderAttribute(string environmentConfigKey)
            => EnvironmentConfigKey = environmentConfigKey;
    }
}
