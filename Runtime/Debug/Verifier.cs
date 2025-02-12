using System;
using System.Diagnostics;

namespace ECS
{
    public static class Verifier
    {
        [Conditional("ECS_ENABLE_VERIFY")]
        public static void ArgumentWarning(string name, bool succeed, string message)
        {
            if (!succeed)
            {
                Logger.Warning($"Argument '{name}' {message}");
            }
        }

        [Conditional("ECS_ENABLE_VERIFY")]
        public static void ArgumentError(string name, bool succeed, string message)
        {
            if (!succeed)
            {
                Logger.Error($"Argument '{name}' {message}");
            }
        }

        [Conditional("ECS_ENABLE_VERIFY")]
        public static void EntityNotNull(Entity entity)
        {
            if (entity == Entity.Null)
            {
                Logger.Error($"Using entity {Entity.Null} is forbidden.");
            }
        }

        [Conditional("ECS_ENABLE_VERIFY")]
        public static void EntityExists(Entity entity, Entities entities)
        {
            if (!entities.Contains(entity))
            {
                Logger.Error($"Entity {entity} does not exists.");
            }
        }

        [Conditional("ECS_ENABLE_VERIFY")]
        public static void EntityInPool(Entity entity, IPoolInternal pool, Type type)
        {
            if (!pool.Contains(entity))
            {
                Logger.Error($"Entity {entity} is not in the pool of {type.Name}.");
            }
        }

        [Conditional("ECS_ENABLE_VERIFY")]
        public static void EntityNotInPool(Entity entity, IPoolInternal pool, Type type)
        {
            if (pool.Contains(entity))
            {
                Logger.Error($"Entity {entity} is already in the pool of {type.Name}.");
            }
        }
    }
}