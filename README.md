# netlib-ids

A highly opinionated library for named identifiers in dotnet.

> This library is in early development and is not yet ready for production use.

## Objective

In dotnet, the most common way to represent an identifier is to use an `int` or a `guid`.
A major drawback is that these types are interchangeable, which can lead to bugs and in some cases event security
hazards.

This library aims to provide a type-safe way to represent identifiers in dotnet, so each identifier can be represented
by its own type. This makes unintended mixing of identifiers impossible.

### Caveats

When using type-safe identifiers, it is important to keep in mind that some libraries will not recognize these types as
scalar values and might not work as expected.

Environments to watch out for:

- ASP.NET Core model binding
- Json (de)serialization
- Entity Framework Core
- Hot Chocolate

There are many other libraries that might not work as expected, these are for now out of scope.

## Components

The project is split into multiple components:

- **Code generator**: A code generator that generates the identifier types for AOT support and good performance. There
  is no main shared library as code will be type specific and generated.
- **Extensions**: Extensions for libraries that do not support type-safe identifiers out of the box.
- **Unit tests**: Unit tests for the main library.
- **Samples**: Sample projects that demonstrate (and test) how to use the library.
