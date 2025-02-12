using System;

namespace ECS
{
    public static class Verifier
    {
        public static void ArgumentWarning(string name, bool succeed, string message)
        {
            if (!succeed)
            {
                Logger.Warning($"Argument '{name}' {message}");
            }
        }

        public static void ArgumentError(string name, bool succeed, string message)
        {
            if (!succeed)
            {
                Logger.Error($"Argument '{name}' {message}");
            }
        }

        public static void EntityNotNull(Entity entity)
        {
            if (entity == Entity.Null)
            {
                Logger.Error($"Using entity {Entity.Null} is forbidden.");
            }
        }

        public static void EntityExists(Entity entity, Entities entities)
        {
            if (!entities.Contains(entity))
            {
                Logger.Error($"Entity {entity} does not exists.");
            }
        }

        public static void EntityInPool(Entity entity, IPoolInternal pool, Type type)
        {
            if (!pool.Contains(entity))
            {
                Logger.Error($"Entity {entity} is not in the pool of {type.Name}.");
            }
        }

        public static void EntityNotInPool(Entity entity, IPoolInternal pool, Type type)
        {
            if (pool.Contains(entity))
            {
                Logger.Error($"Entity {entity} is already in the pool of {type.Name}.");
            }
        }
    }
}