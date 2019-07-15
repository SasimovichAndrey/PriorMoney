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
        }
    }
}