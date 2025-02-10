﻿using System;

namespace ECS
{
    /// <summary>
    /// Entity in Entity-Component-System architecture.
    /// </summary>
    public readonly struct Entity : IEquatable<Entity>
    {
        public readonly int Id;
        public readonly int Gen;

        public static Entity Null => new(0, 0);

        public Entity(int id, int gen)
        {
            id = Math.Max(id, 0);
            gen = Math.Max(gen, 0);
            Id = id;
            Gen = gen;
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
            return this == Null ? "[NULL]" : $"[{Id.ToString()}; {Gen.ToString()}]";
        }
    }
}