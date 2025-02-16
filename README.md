<h1 align="center">Semsamoon Unity ECS</h1>

Entity-Component-System framework for Unity Engine. It is designed to be simple and high-performance.
The framework has a clear structure, minimizes memory allocations, and provides convenient interfaces.

## Installation

The framework can be installed via Unity's Package Manager using a Git reference:

```
https://github.com/Semsamoon/unity-ecs.git
```

Another way is to add this line to the `Packages/manifest.json` file manually:

```
"com.semsamoon.unity-ecs": "https://github.com/Semsamoon/unity-ecs.git",
```

## Entity-Component-System

The Entity-Component-System architecture is based on the principle of complete separation of data and
logic. This means that data structures never contain logic, and logic never contains data.

The main elements of the ECS architecture are:

- **Components** - data structures with no logic.
- **Entities** - game objects with associated components.
- **Systems** - logic blocks that operate entities containing specified components.

However, there are many other elements as well.

There are two approaches to store components in memory:

- **Pools** - separate containers for each component type.
- **Archetypes** - combined containers for each unique set of component types.

The framework implements the pool-based approach. The pool consists of two arrays:

- **Sparse Array** - for each entity, stores an index to the component in a dense array.
- **Dense Array** - stores entities' components contiguously in memory.

Systems need to know which entities contain the specific set of components. **Filter** collects an
array of suitable entities and provides it to the system.

Finally, all the elements are stored together in **World**. It serves as a convenient container for
all parts of the architecture.

## Framework - Elementary

### Entities

`Entity` structure represents an entity in the framework. It consists of two integers:

- `Id` - an identifier of the entity. No two entities can have the same `Id` simultaneously. After
  the entity is destroyed, its `Id` may be reused.
- `Gen` - a generation number of the entity. To determine whether entities with the same `Id` are
  really the same or the identifier was reused, the generation number is incremented every time it is
  reused. As a result, the (`Id`, `Gen`) pair uniquely identifies each entity.

> [!WARNING]
> Both integers must be non-negative for the framework to function correctly. Creating entities with
> negative values is not prohibited, but using them in the other methods surely might cause errors.

The `Entities` class manages entities. It has several methods:

```csharp
// 'entities' is a variable of Entities

// Creates a new entity
var entity = entities.Create();

// True, because the entity is created
if (entities.Contains(entity))
{
    // Removes the entity
    entities.Remove(entity);
}

// False, because the entity is removed
if (entities.Contains(entity))
{
}
```

Use a `foreach` loop or an index getter along with the `Length` property to iterate through all
existing entities. They are represented as tuples containing the entity itself and an array of its
component types:

```csharp
// 'entities' is a variable of Entities

foreach (var (entity, components) in entities)
{
    // Iterates through all existing entities
}

for (var i = 0; i < entities.Length; i++)
{
    var (entity, components) = entities[i];
    // Iterates through all existing entities
    
    foreach (var componentType in components)
    {
        // Iterates through all component types added to the entity
    }
}
```

> [!CAUTION]
> Remember that modifying array during the `foreach` loop is restricted and throws exception. Use the
> reversed `for` loop or decrement `i` if `entities[i]` is removed. It concerns all the loops below.

### Components

To define a component, create a structure with `public` fields. To define a tag (a component without
data), make the structure inherit from the `ITag` interface, like this:

```csharp
public struct Component
{
    public float Data;
    // Other fields here
}

public struct Tag : ITag
{
}
```

Components are stored in `Pool<T>`, where `T` is the type of component. Tags are stored in `Pool`
since generics are not needed for them. These two classes offer slightly different methods:

```csharp
// 'poolComponents' is a variable of Pool<Component>
// 'poolTags' is a variable of Pool for Tag
// 'entity' is a variable of Entity

// Gets (and creates if needed) a component from the entity
var component = poolComponents.Get(entity);

// True, because the component exists for the entity
if (poolComponents.Contains(entity))
{
    // Removes the component from the entity
    poolComponents.Remove(entity);
}

// False, because the component was removed from the entity
if (poolComponents.Contains(entity))
{
}

var newComponent = new Component();
// Sets (no matter if it was created or not) the component to the entity
poolComponents.Set(entity, newComponent);

// Adds a tag for the entity
poolTags.Add(entity);

// True, because the tag exists for the entity
if (poolTags.Contains(entity))
{
    // Removes the tag from the entity
    poolTags.Remove(entity);
}

// False, because the tag was removed from the entity
if (poolTags.Contains(entity))
{
}
```

Use a `foreach` loop or index getter along with the `Length` property to iterate through all entities
and `Get` method in `Pool<T>` to get a reference to the component of the entity:

```csharp
// 'poolComponents' is a variable of Pool<Component>
// 'poolTags' is a variable of Pool for Tag

foreach (var entity in poolComponents)
{
    ref var component = ref poolComponents.Get(entity);
    // Iterates through all entities with the component
}

for (var i = 0; i < poolComponents.Length; i++)
{
    var entity = entities[i];
    ref var component = ref poolComponents.Get(i);
    // Iterates through all entities with the component
}

foreach (var entity in poolTags)
{
    // Iterates through all entities with the tag
}

for (var i = 0; i < poolTags.Length; i++)
{
    var entity = entities[i];
    // Iterates through all entities with the tag
}
```

The `Pools` class manages pools. It has a few methods:

```csharp
// 'pools' is a variable of Pools

// Adds a new Pool<Component> to the container if it does not exist.
pools.Add<Component>();

// Adds a new Pool of Tags to the container if it does not exist.
// The type of pool to create is detected automatically by the ITag interface.
pools.Add<Tag>();

// Gets (and creates if needed) Pool<Component>
var poolComponent = pools.Get<Component>();

// Gets (and creates if needed) Pool of the Tag
var poolTag = pools.GetTag<Tag>();
```

> [!CAUTION]
> Notice that the methods to get `Pool<T>` and `Pool` are different, because they have different
> types. Calling the wrong method throws invalid cast exception.

### Filters

After building, `Filter` provides a single method:

```csharp
// 'filter' is a variable of Filter
// 'entity' is a variable of Entity

// True if the entity matches to the filter's conditions, false elsewhere
if (filter.Contains(entity))
{
}
```

Use a `foreach` loop or index getter along with the `Length` property to iterate through all matching
entities:

```csharp
// 'filter' is a variable of Filter

foreach (var entity in filter)
{
    // Iterates through all entities that match filter's conditions
}

for (var i = 0; i < filter.Length; i++)
{
    var entity = filter[i];
    // Iterates through all entities that match filter's conditions
}
```

The `Filters` class manages filters. It allows to build a new `Filter`:

```csharp
// 'filters' is a variable of Filters

// Method Create begins building process
// Included components are required to be on entity to pass the filter
// Excluded components are required NOT to be on entity to pass the filter
// Add up to 3 components' and tags' types with single method call and chain them
// Build the filter to get it
var filter = filters.Create()
    .Include<ComponentA>()
    .Exclude<TagA, ComponentB>()
    .Build();
```

### Systems

To define a system, create a class inherited from `ISystem` interface. It requires implementing a few
methods, like described below:

```csharp
public class System : ISystem
{
    public override void Initialize(IWorld world)
    {
        // Initialization of the system
        // Called when it is created
    }
    
    public override void Update()
    {
        // Business-logic of the system
        // Called every frame or with fixed timestep
    }
    
    public override void Destroy()
    {
        // Clearing the system's data
        // Called when it is goind to be destroyed
    }
}
```

The `Systems` class manages systems. It has several methods:

```csharp
// 'systems' is a variable of Systems

// Adds systems to the manager
systems
    .Add<SystemA>()
    .Add(new SystemB);

// Calls Update method of all added systems in the same order as they were added
systems.Update();
```

> [!IMPORTANT]
> The order of systems significantly affects game logic. Wrong order may cause incorrect results of
> the game behavior.

### World

All the containers (`Entities`, `Pools`, `Filters` and `Systems`) are created automatically when the
`World` class is created:

```csharp
// Use a static method Create to construct the World
var world = World.Create();

var entities = world.Entities;
var pools = world.Pools;
var filters = world.Filters;
var systems = world.Systems;

// When the world is no longer needed, it has to be destroyed
world.Destroy();
```

> [!NOTE]
> To access the required containers in the `ISystem`, the `World` instance is passed as an argument
> in the `Initialize` method.

> [!IMPORTANT]
> In fact, `Create` returns an `IWorld` interface, not a `World` instance itself. All the classes in
> the framework implement relevant interfaces (`IEntities`, `IPool<T>`, `IPool`, `IPools`, `IFilter`,
> `IFilters` and `ISystems`). Using any of them is safe (if the [rules](#rules) are followed), and
> references to other objects inside them are interfaces too. Escaping interfaces with casting is a
> key step to the [advanced level](#framework---advanced)

## Start

Create a `MonoBehavior` class that represents application's entry point. Create a `World` object in
its `Awake` or `Start` method to begin working with ECS. Then define necessary systems, components
and create new entities. This is a quick example to start with:

```csharp
public class EntryPoint : MonoBehavior
{
    private IWorld _world;
    
    private void Awake()
    {
        _world = World.Create();
        
        _world.Systems
            .Add<SystemMovement>();
        
        var player = _world.Entities.Create();
        
        _world.Pools.Get<ComponentVelocity>.Set(player, new ComponentVelocity());
        _world.Pools.Get<ComponentPosition>.Set(player, new ComponentPosition());
    }
    
    private void Update()
    {
        _world.Systems.Update();
    }
    
    private void OnDestroy()
    {
        _world.Destroy();
    }
}

public struct ComponentVelocity
{
    public Vector3 Value;
}

public struct ComponentPosition
{
    public Vector3 Value;
}

public class SystemMovement : ISystem
{
    private IFilter _filter;
    private IPool _poolVelocity;
    private IPool _poolPosition;
    
    public void Initialize(IWorld world)
    {
        _filter = world.Filters.Create()
            .Include<ComponentVelocity, ComponentPosition>()
            .Build();
        _poolVelocity = world.Pools.Get<ComponentVelocity>();
        _poolPosition = world.Pools.Get<ComponentPosition>();
    }
    
    public void Update()
    {
        var deltaTime = Time.deltaTime;
        
        foreach (var entity in _filter)
        {
            ref var position = ref _poolPosition.Get(entity);
            position.Value += _poolVelocity.Get(entity).Value * Time.deltaTime;
        }
    }
    
    public void Destroy()
    {
    }
}
```

## Rules

To maintain a balance between safety and high performance, the framework omits certain checks and
requires following these rules:

- Never use an entity that is not created yet in `Entities` or has already been removed.
- Never pass incorrect values (e.g., negative capacity) as an argument to any method.
- Never access value in a container with an index that is bigger than the `Length` of the container.

> [!CAUTION]
> In most cases, violation of these rules does not throw any exception, but internal state of data
> may become completely broken and cause unpredictable results. Enable the `Verifier` to detect such
> errors: add define symbol `ECS_ENABLE_VERIFY` to the `Project Settings/Player/Other Settings/Script
> Compilation/Scripting Define Symbols` in Unity. The `Verifier` logs errors to the console, but does
> not throw exceptions. Therefore, it is useful only during the development process (do not forget to
> remove the define symbol).

## Framework - Advanced

### Span

All interfaces that provide the `GetEnumerator` method, also offer `AsReadOnlySpan`, which returns a
`ReadOnlySpan`. See the [MSDN](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1)
page to learn how to use it.

### Options

To specify the capacities of internal arrays, pass an `Options` structure to `World.Create`. This
structure is immutable and can be passed with the `in` keyword to avoid copying. Pass a custom value
to other creation methods to override the capacities defined in `Options`:

```csharp
// Define necessary options values (others will use default values)
var options = new Options(
        entitiesCapacity: 64,
        entityComponentsCapacity: 4,
        poolsCapacity: 32,
        poolComponentsCapacity: 16,
        filtersCapacity: 32,
        filtersWithSameComponentCapacity: 4,
        filterEntitiesCapacity: 16,
        systemsCapacity: 32);

// Get an options instance with default values
var defaultOptions = Options.Default;

// Create a world with specified options
var world = World.Create(in options);

// Pass a custom capacity when creating a new filter to override the default value (16)
world.Filters
    .Create(filterEntitiesCapacity: 8)
    .Include<Component>()
    .Build();
```

> [!WARNING]
> Creating `Options` with incorrect values violates the [rules](#rules). While this can be detected
> by the `Verifier`, it may also result in exceptions being thrown.

### Advanced Features

To open all the features available in the framework, cast the given interface to the underlying class:

```csharp
var world = (World)interfaceWorld;
var pool = (Pool)interfacePool;
// etc
```

The `IWorld.Entities` provides `IEntities` interface, but `World.Entities` provides `Entities`
itself. It is true for all the fields and methods in underlying classes. Cast a class to the
according interface to turn it back.

> [!WARNING]
> The `Verifier` does not work in methods called not through the interface. Be absolutely sure that
> all the [rules](#rules) are followed before switching to the underlying classes.

Besides described earlier methods, underlying classes provide `Unchecked` ones - these methods do
not perform checks and are used internally for better performance. They also do not notice other
elements about changes. There are many `Unchecked` methods, and it is hard to describe all of them
here, so read the source code to figure out the differences for all of them.

> [!NOTE]
> For example, `Pool.Remove` not only removes the component, but also notice filters about this
> change. Thus, `Pool.Remove` calls `Filters.EraseUnchecked` internally, but `Pool.RemoveUnchecked`
> does not.

## FAQ

#### How to update some systems in `Update` and others in `FixedUpdate`?

```csharp
public class EntryPoint : MonoBehavior
{
    private IWorld _world;
    private Systems _systemsFixedUpdate;
    
    private void Awake()
    {
        _world = World.Create();
        _systemsFixedUpdate = new Systems(_world);
        
        // Adding systems to _world.Systems and _systemsFixedUpdate
    }
    
    private void Update()
    {
        _world.Systems.Update();
    }
    
    private void FixedUpdate()
    {
        _systemsFixedUpdate.Update();
    }
    
    private void OnDispose()
    {
        _systemsFixedUpdate.Dispose();
        _world.Dispose();
    }
}
```

#### How to implement a reactive behavior in ECS?

ECS does not inherently support reactive programming patterns. The common approach is to use 'event
entities' with specific components or tags:

```csharp
// Defined somewhere
public struct TagEntityCreated : ITag
{
}

// Some system adds the tag when creates a new entity
var entity = _world.Entities.Create();
_world.Pools.GetTag<TagEntityCreated>().Add(entity);

// Another system creates a filter for entities that has been created and handles entities
_filterCreatedEntities = world.Filters.Create().Include<TagEntityCreated>().Build();

// The last system removes the TagEntityCreated from all the entities so they are handled once
while (_poolTagEntityCreated.Length > 0)
{
    _poolTagEntityCreated.Remove(_poolTagEntityCreated[0]);
}
```
