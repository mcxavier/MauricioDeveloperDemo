using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.SeedWork
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; set; }
        public int Id { get; set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public static T Cast<T>(string value) where T : Enumeration
        {
            return GetAll<T>().FirstOrDefault(f => f.Name.ToLower().Contains(value.Replace("_", "").ToLower())) ?? throw new InvalidCastException("Cannot casting value");
        }

        public static T Cast<T>(int value) where T : Enumeration
        {
            return GetAll<T>().FirstOrDefault(f => f.Id == value) ?? throw new InvalidCastException("Cannot casting value");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration otherValue))
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        protected bool Equals(Enumeration other)
        {
            return Name == other.Name && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id);
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}