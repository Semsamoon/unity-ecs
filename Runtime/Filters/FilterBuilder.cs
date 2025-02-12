namespace ECS
{
    public readonly struct FilterBuilder : IFilterBuilder
    {
        private readonly World _world;
        private readonly Filter _filter;

        public FilterBuilder(World world) : this(world, OptionsFilter.Default)
        {
        }

        public FilterBuilder(World world, OptionsFilter optionsFilter)
        {
            _world = world;
            _filter = new Filter(0, optionsFilter);
        }

        IFilterBuilder IFilterBuilderEmpty.Include<T>()
        {
            return Include<T>();
        }

        public FilterBuilder Include<T>()
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

        IFilterBuilder IFilterBuilderEmpty.Include<T1, T2>()
        {
            return Include<T1>().Include<T2>();
        }

        public FilterBuilder Include<T1, T2>()
        {
            return Include<T1>().Include<T2>();
        }

        IFilterBuilder IFilterBuilderEmpty.Include<T1, T2, T3>()
        {
            return Include<T1>().Include<T2>().Include<T3>();
        }

        public FilterBuilder Include<T1, T2, T3>()
        {
            return Include<T1>().Include<T2>().Include<T3>();
        }

        IFilterBuilder IFilterBuilderEmpty.Exclude<T>()
        {
            return Exclude<T>();
        }

        public FilterBuilder Exclude<T>()
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

        IFilterBuilder IFilterBuilderEmpty.Exclude<T1, T2>()
        {
            return Exclude<T1>().Exclude<T2>();
        }

        public FilterBuilder Exclude<T1, T2>()
        {
            return Exclude<T1>().Exclude<T2>();
        }

        IFilterBuilder IFilterBuilderEmpty.Exclude<T1, T2, T3>()
        {
            return Exclude<T1>().Exclude<T2>().Exclude<T3>();
        }

        public FilterBuilder Exclude<T1, T2, T3>()
        {
            return Exclude<T1>().Exclude<T2>().Exclude<T3>();
        }

        IFilter IFilterBuilder.Build()
        {
            return Build();
        }

        public Filter Build()
        {
            for (var i = _filter.Length - 1; i >= 0; i--)
            {
                _filter.ChangeUnchecked(_filter[i], 0);
            }

            return _filter;
        }
    }
}