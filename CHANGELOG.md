# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
- Tests for Entity, Sparse Array, Dense Array and Pool.

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
