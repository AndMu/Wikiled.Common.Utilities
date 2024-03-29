﻿using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace Wikiled.Common.Testing.Utilities.Reflection
{
    public static class ConstructorHelper
    {
        public static void ConstructorMustThrowArgumentNullException<T>(TypeSubstitute? substitute = null)
        {
            ConstructorMustThrowArgumentNullException(typeof(T), substitute);
        }

        public static void ConstructorMustThrowArgumentNullException(Type type, TypeSubstitute? substitute = null)
        {
            foreach (var constructor in type.GetConstructors())
            {
                var parameters = constructor.GetParameters();

                var arguments = parameters.Select(
                                              p =>
                                              {
                                                  var value = substitute?.Construct(p.ParameterType);
                                                  if (value != null)
                                                  {
                                                      return value;
                                                  }

                                                  if (p.ParameterType.IsEnum)
                                                  {
                                                      return p.ParameterType.GetEnumValues().GetValue(0);
                                                  }

                                                  if (p.ParameterType.IsValueType)
                                                  {
                                                      return Activator.CreateInstance(p.ParameterType);
                                                  }

                                                  if (p.ParameterType == typeof(string))
                                                  {
                                                      return "Test";
                                                  }

                                                  var mockType = typeof(Mock<>).MakeGenericType(p.ParameterType);
                                                  return ((Mock)Activator.CreateInstance(mockType)).Object;
                                              })
                                          .ToArray();

                for (var i = 0; i < parameters.Length; i++)
                {
                    var mocksCopy = arguments.ToArray();

                    var argType = mocksCopy[i].GetType();

                    if (substitute?.ChechNotNull(argType, i) == true)
                    {
                        continue;
                    }

                    if (argType.IsValueType)
                    {
                        if (Nullable.GetUnderlyingType(argType) == null)
                        {
                            continue;
                        }
                    }

                    mocksCopy[i] = null;
                    try
                    {
                        constructor.Invoke(mocksCopy);
                        Assert.Fail("ArgumentNullException expected for parameter {0} of constructor, but no exception was thrown", parameters[i].Name);
                    }
                    catch (TargetInvocationException ex)
                    {
                        Assert.AreEqual(
                            typeof(ArgumentNullException),
                            ex.InnerException?.GetType(),
                            $"ArgumentNullException expected for parameter {parameters[i].Name} of  constructor, but exception of type {ex.InnerException.GetType()} was thrown");
                    }
                }
            }
        }
    }
}
