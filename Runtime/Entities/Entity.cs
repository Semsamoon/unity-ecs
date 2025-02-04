using System;

namespace ECS
{
    /// <summary>
    /// Entity in Entity-Component-System architecture.<br/>
    /// <br/>
    /// <i>Entity with <see cref="Id"/> = 0 is a NULL-entity.</i>
    /// </summary>
    public readonly struct Entity : IEquatable<Entity>
    {
        public readonly int Id;
        public readonly int Gen;

        public Entity(int id, int gen)
        {
            id = Math.Max(id, 0);
            gen = Math.Max(gen, 0);
            Id = id;
            Gen = gen;
        }

        /// <summary>
        /// <i>Use only this method to check for NULL-entity.</i>
        /// </summary>
        public bool IsNull()
        {
            return Id == 0;
        }

        public static bool operator ==(Entity lhs, Entity rhs)
        {
            return lhs.Id == rhs.Id && lhs.Gen == rhs.Gen;
        }

        public static bool operator !=(Entity lhs, Entity rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is Entity other && Equals(other);
        }

        public bool Equals(Entity other)
        {
            return Id == other.Id && Gen == other.Gen;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Gen);
        }

        public override string ToString()
        {
            return IsNull() ? "[NULL]" : $"[{Id.ToString()}; {Gen.ToString()}]";
        }
    }
}