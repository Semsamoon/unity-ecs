using System;

namespace ECS
{
    public readonly struct Entity : IEquatable<Entity>
    {
        public readonly int Id;
        public readonly int Gen;

        public Entity(int id, int gen)
        {
            Id = id;
            Gen = gen;
        }

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
            return IsNull() ? "NULL-entity" : $"Entity({Id.ToString()}; {Gen.ToString()})";
        }
    }
}