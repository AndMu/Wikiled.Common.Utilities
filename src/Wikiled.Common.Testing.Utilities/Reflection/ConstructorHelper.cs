using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy.Internal;

namespace Wikiled.Common.Testing.Utilities.Reflection
{
    public static class ConstructorHelper
    {
        public static void ConstructorMustThrowArgumentNullException<T>(params object[] args)
        {
            ConstructorMustThrowArgumentNullException(typeof(T), args);
        }

        public static void ConstructorMustThrowArgumentNullException(Type type, params object[] args)
        {
            var lookup = args.ToLookup(item => item.GetType(), item => item);

            foreach (var constructor in type.GetConstructors())
            {
                var parameters = constructor.GetParameters();

                var arguments = parameters.Select(
                                              p =>
                                              {
                                                  if (lookup.Contains(p.ParameterType))
                                                  {
                                                      return lookup[p.ParameterType].First();
                                                  }

                                                  if (p.ParameterType.IsEnum)
                                                  {
                                                      return p.ParameterType.GetEnumValues().GetValue(0);
                                                  }

                                                  Type mockType = typeof(Mock<>).MakeGenericType(p.ParameterType);
                                                  return ((Mock) Activator.CreateInstance(mockType)).Object;
                                              })
                                          .ToArray();

                for (int i = 0; i < parameters.Length; i++)
                {
                    var mocksCopy = arguments.ToArray();

                    if (!mocksCopy[i].GetType().IsNullableType())
                    {
                        continue;
                    }

                    mocksCopy[i] = null;

                    try
                    {
                        constructor.Invoke(mocksCopy);
                        Assert.Fail("ArgumentNullException expected for parameter {0} of constructor, but no exception was thrown",
                                    parameters[i].Name);
                    }
                    catch (TargetInvocationException ex)
                    {
                        Assert.AreEqual(typeof(ArgumentNullException),
                                        ex.InnerException?.GetType(),
                                        $"ArgumentNullException expected for parameter {parameters[i].Name} of  constructor, but exception of type {ex.InnerException.GetType()} was thrown");
                    }
                }
            }
        }
    }
}
