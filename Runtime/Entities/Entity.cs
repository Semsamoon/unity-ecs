using System;

namespace ECS
{
    /// <summary>
    /// Represents entity in Entity-Component-System architecture.
    /// Consists of identifier <see cref="Id"/> and generation number <see cref="Gen"/> -
    /// this pair of values is unique for each entity.<br/>
    /// <br/>
    /// <i>Entities are kind of game objects with associated data and no logic.</i>
    /// </summary>
    public readonly struct Entity : IEquatable<Entity>
    {
        /// <summary>
        /// Unique identifier of entity.
        /// There can not be more than one existing entity with the same identifier.
        /// To be able to reuse identifier when entity is destroyed, generation number <see cref="Gen"/> is used.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Generation number of entity.
        /// Each time entity is destroyed, its identifier <see cref="Id"/> might be reused.
        /// But to preserve unique definition of entity, its generation number is incremented.
        /// </summary>
        public readonly int Gen;

        /// <param name="id">Identifier</param>
        /// <param name="gen">Generation number</param>
        public Entity(int id, int gen)
        {
            if (id < 0)
            {
                id = 0;
            }

            if (gen < 0)
            {
                gen = 0;
            }

            Id = id;
            Gen = gen;
        }

        /// <summary>
        /// NULL-entity is an entity with identifier <see cref="Id"/> = 0, no matter its generation number <see cref="Gen"/>.<br/>
        /// <br/>
        /// <i>Always use this method to check entity for NULL,
        /// because '==' and 'Equals' check the equality of generation numbers.</i>
        /// </summary>
        /// <returns>True if entity is a NULL-entity, false otherwise</returns>
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