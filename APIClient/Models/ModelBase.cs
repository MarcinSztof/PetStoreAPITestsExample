using System;
using System.Collections.Generic;
using System.Linq;

namespace APIClient.Models
{
    public abstract class ModelBase
    {
        public override bool Equals(object other) // TODO: Probably can be done without dynamic, also some performance testing needed, might be slow on large amounts of data
        {
            if (other == null)
                return false;
            if (this.GetType() != other.GetType())
                return false;
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    dynamic list1 = prop.GetValue(this);
                    dynamic list2 = prop.GetValue(other);
                    if (!DynamicSequenceEqual(list1, list2)) // SequenceEqual has to be done outside of scope where they're declared as dynamic
                        return false;
                }
                else
                {
                    if (!Equals(prop.GetValue(this), prop.GetValue(other)))
                        return false;
                }
            }
            return true;
        }

        private static bool DynamicSequenceEqual<T>(ICollection<T> list1, ICollection<T> list2) => list1.SequenceEqual(list2);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                foreach (var prop in this.GetType().GetProperties())
                {
                    hash = hash * 23 + (prop.GetValue(this)?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }
    }
}
