using System;

namespace PriorMoney.Utils.ExceptionHandling
{
    public static class Throw
    {
        public static class If
        {
            public static class String
            {
                public static void IsNullOrWhiteSpace(string str, string className, string methodName, string parameterName)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        throw new ArgumentException($"The value for parameter '{parameterName}' of '{className}.{methodName}' method cannot be empty or null or whitespace");
                    }
                }
            }

            public static void IsNull<T>(T obj, string className, string methodName, string parameterName) where T: class
            {
                if(obj == null)
                {
                    throw new ArgumentException($"The value for parameter '{parameterName}' of '{className}.{methodName}' method cannot be null");
                };
            }
        }
    }
}