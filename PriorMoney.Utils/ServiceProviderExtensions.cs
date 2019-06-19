using System;

namespace PriorMoney.Utils
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            var service = provider.GetService(typeof(T));

            return (T)service;
        }
    }
}
