using System;

namespace PriorMoney.Utils
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            var service = provider.GetService(typeof(T));

            if (service == null)
            {
                throw new Exception($"Could not resolve a service of type {typeof(T).Name}");
            }

            return (T)service;
        }
    }
}
