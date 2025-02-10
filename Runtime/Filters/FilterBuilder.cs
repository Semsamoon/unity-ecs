namespace ECS
{
    public readonly struct FilterBuilder : IFilterBuilder
    {
        private readonly World _world;
        private readonly Filter _filter;

        public FilterBuilder(World world) : this(world, OptionsFilter.Default())
        {
        }

        public FilterBuilder(World world, OptionsFilter optionsFilter)
        {
            _world = world;
            _filter = new Filter(0, optionsFilter);
        }

        public IFilterBuilder Include<T>()
        {
            var pool = _world.PoolsInternal.GetPool<T>();
            _filter.Sum++;
            _world.FiltersInternal.Include(_filter, typeof(T));

            for (var i = 0; i < pool.Length; i++)
            {
                _filter.ChangeUnchecked(pool[i], 1);
            }

            return this;
        }

        public IFilterBuilder Include<T1, T2>()
        {
            Include<T1>();
            Include<T2>();
            return this;
        }

        public IFilterBuilder Include<T1, T2, T3>()
        {
            Include<T1>();
            Include<T2>();
            Include<T3>();
            return this;
        }

        public IFilterBuilder Exclude<T>()
        {
            var pool = _world.PoolsInternal.GetPool<T>();
            _world.FiltersInternal.Exclude(_filter, typeof(T));
            _filter.Sum++;

            foreach (var entity in _world.EntitiesInternal)
            {
                if (!pool.Contains(entity.Entity))
                {
                    _filter.ChangeUnchecked(entity.Entity, 1);
                }
            }

            return this;
        }

        public IFilterBuilder Exclude<T1, T2>()
        {
            Exclude<T1>();
            Exclude<T2>();
            return this;
        }

        public IFilterBuilder Exclude<T1, T2, T3>()
        {
            Exclude<T1>();
            Exclude<T2>();
            Exclude<T3>();
            return this;
        }

        public IFilter Build()
        {
            for (var i = _filter.Length - 1; i >= 0; i--)
            {
                _filter.ChangeUnchecked(_filter[i], 0);
            }

            return _filter;
        }
    }
}