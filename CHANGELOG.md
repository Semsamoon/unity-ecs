# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

- Sparse array structure with XML documentation.

## [0.1.0] - 2025-01-19

### Added

- Entity structure with XML documentation.
- README, LICENSE and Package files.
- Assembly definition for Runtime folder.
