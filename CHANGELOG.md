# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.10.1] - 2025-02-16

### Changed

- Rename world's EntitiesInternal, PoolsInternal, FiltersInternal and SystemsInternal to
  Entities, Pools, Filters and Systems respectively.
- Combine options into single structure Options.
- Update tests with renamed world's fields and combined options.

### Added

- Read only dense array cast to dense array.
- Tests for read only dense array.

## [0.10.0] - 2025-02-13

### Changed

- Increase default options' values.
- Add index component getter from pool<T> to IPool<T> interface.
- Rename pool<T>'s GetUnchecked method to Get.
- Update tests with renamed methods.

### Fixed

- Add verifier check to index getter from IEntities, IFilter, IPool and IPool<T> interfaces.
- Remove verifier check from Contains method if it is called not from interfaces IEntities,
  IFilter, IPool and IPool<T>.

## [0.9.6] - 2025-02-12

### Changed

- Add span and index getters to entities' filter's, pool's and pool<T>'s interfaces.
- Update package description because this ECS is not fully open for extensions.
- Add constants to options' constructors to allow skipping arguments in constructors.
- Replace options' validations with verifier calls.
- Make verifier conditional for 'ECS_ENABLE_VERIFY'. Disabling increases performance.
- Remove obsolete asserts in tests.
- Options' Default method to property.
- World options' properties to fields.
- Pass options with 'in' keyword to avoid copying.
- Create world only via static method Create to return interface instead of object.
- Update tests for options and static world constructor.

### Added

- Read only dense array structure that protects array.

### Fixed

- Replace returning dense array of entities' components with read only dense array.

## [0.9.5] - 2025-02-12

### Added

- Logger class to debug messages properly.
- Verifier class that makes asserts. It logs errors and does not throw exceptions.

### Changed

- Replace checks with verifier calls.
- Rename dense array's Swap method's arguments from 'i' and 'j' to 'a' and 'b'.
- Reduce overloads calls for small methods to improve their performance.
- Create pool to add only when it does not exist in pools.
- Remove obsolete asserts in tests.

## [0.9.4] - 2025-02-11

### Changed

- Reverse loop in searching component to remove because it is more probable that removing
  component was added recently, so it is placed close to the array's end.
- Extract filters' repeated logic into static methods.
- Make all the classes to use fluent interface pattern.
- Use fluent interfaces in tests and add missing ones.

### Added

- Filters' methods to specify capacity for internal include and exclude arrays.

### Fixed

- Do not extend dense array in Set method. Ignore if index is bigger than its length.

## [0.9.3] - 2025-02-10

### Changed

- NULL-entity is an entity [0; 0] only. Entities [0; gen > 0] are correct.
- Entities starts creating from entity [id; 1]. The first identifier is 0.
- Remove entity test's asserts for obsolete NULL-entity definition. Update numbering
  entities because the new creating rules.
- Remove setter in pool<T> because it does the same as reference-getter.
- Make index getter in pool<T> to return only the entity for uniformity.
- Make filter's sum public to optimize filter building process so there is no need to store
  included and excluded components in builder's arrays.
- Remove options for filter builder.
- Update tests with changed methods and constructors.

### Added

- Entities', pools', pool's and filter's unchecked methods for high performance calls.
- Dense array's Swap method to reduce code.

## [0.9.2] - 2025-02-10

### Added

- Interfaces for filter builder that prevents creating empty filter.
- Options structure for filter builder.

### Changed

- Access to pools from filter builder using world, not direct reference.
- Update filter builder's tests to use options.

## [0.9.1] - 2025-02-09

### Changed

- Use methods' overloads to reduce code.
- Make method ExtendTo of sparse and dense arrays public to allow creating entity with
  specified components' capacity. If internal array already exists, it is extended.
- Mark some unsafe methods in entities, filters and pool with Unchecked in their names.
- Clear entity's components array instead of removing each element.
- Simplify checks in filter's Change method.

### Added

- Dense array's Clear method.

### Fixed

- Add check of entity existence in the world for pool's Add and pool<T>'s Set methods.

## [0.9.0] - 2025-02-09

### Added

- Options structures for entities, pool, pools, filter, filters, systems and world.

### Changed

- Update tests to use options.

## [0.8.4] - 2025-02-08

### Changed

- Entities store entity's components together with entity itself and removes existing
  components from pools and filters when entity is removed.
- Entities depend on world to call the other services.
- Update entities' and pool's tests because of new dependencies and methods.

### Added

- Pool's unchecked remove to delete component without calling checks and filters' update.
- Pools' unchecked getter to return pool without calling checks.
- Entities' unchecked record and erase to allow pool's notices about changed components.

## [0.8.3] - 2025-02-07

### Added

- ISystem and ISystems interfaces for system and systems.
- Systems class with XML documentation.
- Tests for systems.
- World's method Destroy to react on destroying.

### Changed

- Put systems into the world.

## [0.8.2] - 2025-02-07

### Changed

- Pools, pool and pool<T> depend on world to call the other services.
- Pool stores type of tags it contains.
- Rename IPool to IPoolInternal interface.
- Replace pool in filter to sparse and dense arrays for full control.
- Min capacity for sparse array and length for dense array is 1.
- Update filters', pools' and pool's tests because of new dependencies and interfaces.

### Fixed

- Check entity existence in pool before getting the component.

### Added

- New IPool and IPool<T> interfaces for external usage.

## [0.8.1] - 2025-02-07

### Added

- World class with XML documentation.
- Filters' and world's interfaces for external usage.
- Filter builder.
- Tests for filter builder.

### Changed

- Rename filters' Create and Remove methods to Record and Erase, because Create is used
  for creating filter, but not adding component to the entity.
- Filter depends on world to call the other services.
- Update filters' tests because of renaming and new dependencies.

## [0.8.0] - 2025-02-06

### Changed

- Remove pools' method Remove because it is difficult to maintain and hardly needed.
- Merge IContains into IPool interface.

### Added

- Entities', pools' and filter's interfaces for external usage.
- Add GetPool<T> method to return a pool from pools by IPool interface.

## [0.7.2] - 2025-02-06

### Added

- Filters class with XML documentation.
- Tests for filters.

### Changed

- Remove filter's method Recheck because Change can be used for the same purpose.
- Remove filter's test of method Recheck.

## [0.7.1] - 2025-02-05

### Added

- Filter class with XML documentation.
- Tests for filter.

## [0.7.0] - 2025-02-05

### Added

- Interface ITag for tags.
- Pools class with XML documentation.
- Tests for pools.
- Interfaces IPool and IContains for pool and pool<T>.
- Pools' GetEnumerator to allow using foreach loop.
- Tests for enumeration in pools.

### Changed

- Store pools and pools<T> in containers by IPool interface instead of object.

## [0.6.4] - 2025-02-04

### Added

- Capacity and index getter for entities.

### Changed

- Simplify arguments' checks in entities' constructor.
- String format for entity is [NULL] and [id; gen].
- Reduce documentation of entity.
- Improve entity, sparse array, dense array, pool, pool<T> and entities tests.

### Fixed

- Add a single gap between existing and removed entities in internal array of entities.
  This prevents incorrect result of Contains method when the only entity was removed but
  still exists at the same index for recycle (since 0 is not invalid index).

## [0.6.3] - 2025-02-03

### Added

- Entities' GetEnumerator to allow using foreach loop.
- Tests for enumeration in entities.

### Changed

- Use sparse and dense arrays directly in entities instead of pool.
- Use Length property instead of internal array's length in pool.
- Reduce documentation of entities.

## [0.6.2] - 2025-02-03

### Added

- Capacity for pool and pool<T>.
- Pool's and pool<T>'s GetEnumerator to allow using foreach loop.
- Tests for enumeration in pool and pool<T>.

### Changed

- Add names (entity, value) to the tuple elements returned from pool<T>.
  Return the tuple in pool's index getter.
- Rename pool<T>'s method AddOrSet to Set because it is simpler.
- Add default constructor for pool<T>.
- Reduce documentation of pool and pool<T>.
- The first element in pool is not invalid.
- Update pool's and pool<T>'s tests.

## [0.6.1] - 2025-02-03

### Added

- Dense array's GetEnumerator to allow using foreach loop.
- Tests for enumeration in dense array.

### Changed

- Use internal array's length as capacity instead of property in dense array.
- Extend dense array to index instead of doubling.
- Rename and simplify methods in dense array.
- Default length of dense array is 0.
- Reduce documentation of dense array.
- Update dense array's tests.

## [0.6.0] - 2025-02-02

### Added

- Sparse array's GetEnumerator to allow using foreach loop.
- Tests for enumeration in sparse array.

### Changed

- Use internal array's length instead of property in sparse array.
- Rename and simplify methods in sparse array.
- Reduce documentation of sparse array.

## [0.5.2] - 2025-01-31

### Changed

- Add default constructors for sparse array, dense array, pool and entities.
- Rename entities' internal pool with existing entities Pool to Existing
  because it explains its purpose.
- Separate existing and deleted pools' capacities in entities' constructor.

## [0.5.1] - 2025-01-31

### Fixed

- Add check whether entity to remove from entities exists.
  If it is not, there is no need to remove the entity.
- Reorder assignment of sparse array's elements when use back swap removing in pool.
  This prevents invalid value in sparse array when remove the last added entity.

### Changed

- Add exception check for pool tests when remove the last added entity.

### Added

- Tests for entities.

## [0.5.0] - 2025-01-31

### Added

- Entities class with XML documentation.

## [0.4.3] - 2025-01-21

### Removed

- Dense array with two types of items (because it can be replaced with tuples).

### Changed

- Use one-typed dense array with tuple of two items in pool instead of two-typed dense array.
- Make index operator in pool work to get only entities. Separate getters for items in pool
  (by index or entity).

## [0.4.2] - 2025-01-21

### Added

- Assembly definition for Tests folder.
- Tests for entity, sparse array, dense array and pool.

## [0.4.1] - 2025-01-21

### Added

- Dense array with two types of items to store together.
- Index operator to access item in pool by entity.

### Changed

- Use one dense array with two items instead of two dense arrays in pool.

## [0.4.0] - 2025-01-20

### Added

- Pool class with XML documentation.

## [0.3.3] - 2025-01-19

### Changed

- Index operator of sparse array returns reference to element (no get/set value).
- Use 'element' instead of 'item' in terms of arrays.
- Add documentation references between different classes.

## [0.3.2] - 2025-01-19

### Changed

- Make the first element in dense array counted as invalid.
- Simplify back swap removing description in dense array.

## [0.3.1] - 2025-01-19

### Fixed

- Try to recover instead of throwing exception. Use default values for invalid arguments.
- Add missed index check in dense array's method Remove.
- Make comparison operations look more natural.

## [0.3.0] - 2025-01-19

### Added

- Dense array class with XML documentation.

## [0.2.2] - 2025-01-19

### Fixed

- Add check whether index in sparse array's getter is bigger than array's current length.
  If true, then returns default value because it is certainly not assigned yet.
- Add arguments check to entity's and sparse array's constructors.
  In case of incorrect values throws exceptions.

## [0.2.1] - 2025-01-19

### Changed

- Rename sparse array's property Capacity to Length because arrays have Length property.
- Rename sparse array's method ResizeForIndex to ExtendToIndex because it always extends array.

## [0.2.0] - 2025-01-19

### Added

- Sparse array class with XML documentation.

## [0.1.0] - 2025-01-19

### Added

- Entity structure with XML documentation.
- README, LICENSE and Package files.
- Assembly definition for Runtime folder.
