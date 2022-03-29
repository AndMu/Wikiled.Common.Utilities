using System;
using System.Collections.Generic;

namespace Wikiled.Common.Testing.Utilities.Reflection
{
    public class TypeSubstitute
    {
        private readonly Dictionary<Type, object> typeTable = new ();

        private readonly Dictionary<(Type, int), bool> typeNullTable = new();

        private TypeSubstitute()
        {
        }

        public static TypeSubstitute Create()
        {
            return new TypeSubstitute();
        }

        public TypeSubstitute Add<T>(T instance)
        {
            typeTable[typeof(T)] = instance;
            return this;
        }

        public TypeSubstitute NeverNull<T>(int index)
        {
            typeNullTable[(typeof(T), 1)] = true;
            return this;
        }

        public bool ChechNotNull(Type type, int index)
        {
            return typeNullTable.TryGetValue((type, index), out var value) && value;
        }

        public object? Construct(Type type)
        {
            if (typeTable.TryGetValue(type, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
