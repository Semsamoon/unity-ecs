using System;

namespace ECS
{
    public sealed class Entities
    {
        private const int DefaultSparseCapacity = 64;
        private const int DefaultDenseCapacity = 64;

        private readonly Pool _pool;
        private readonly Pool _removed;
        private int _id;

        public int Length => _pool.Length;

        public Entities(int sparseCapacity = DefaultSparseCapacity, int denseCapacity = DefaultDenseCapacity)
        {
            _pool = new Pool(sparseCapacity, denseCapacity);
            _removed = new Pool(sparseCapacity, denseCapacity);
            _id = 1;
        }

        public Entity Create()
        {
            return _removed.Length <= 1 ? CreateNew() : Recycle();
        }

        public bool Contains(Entity entity)
        {
            return _pool.Contains(entity);
        }

        public void Remove(Entity entity)
        {
            _removed.Add(entity);
            _pool.Remove(entity);
        }

        public ReadOnlySpan<Entity> AsReadOnlySpan()
        {
            return _pool.AsReadOnlySpan();
        }

        private Entity CreateNew()
        {
            var entity = new Entity(_id++, 0);
            _pool.Add(entity);
            return entity;
        }

        private Entity Recycle()
        {
            while (_removed.Length > 1)
            {
                var recycle = _removed[1];
                _removed.Remove(recycle);

                if (recycle.Gen >= int.MaxValue)
                {
                    continue;
                }

                recycle = new Entity(recycle.Id, recycle.Gen + 1);
                _pool.Add(recycle);
                return recycle;
            }

            return CreateNew();
        }
    }
}