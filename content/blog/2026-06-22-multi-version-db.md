+++
title = "Multi Version Database"
date = 2026-06-22
description = "Multi Version Database"
slug = "2026-06-22-multi-version-db"
taxonomies = { tags = ["DB", "Rust"] }
+++

**Problem Statement:** Introducing any form of statefulness to an application introduces a new layer of complexity during deployment and operations. A cache such as Redis is stateful and has a relatively low cost in maintenance; the cache can simply be thrown away and restarted upon deployment of the application. A database, however, can lead to many problems at deployment as the database schema and the data may require a migration upon release of a new version of the application. The database needs to be versioned and needs to support rollbacks in the application without rolling back the database. In a cloud native environment, it may seem trivial to rollback the database, however, the rollbacks cannot easily capture data-lossy transitions. Also, testing rollback scenarios is extremely difficult as it requires a combinatorial explosion of test cases. Developers, therefore, rarely test rollback behavior.

**Solution:** Build a database repository (dbrepo) in Rust to expose different versions of gRPC endpoints to a single target database. This allows migrating the database more broadly against the application versions.

**Architecture:**
1. *SQLx:* Use the SQLx (https://github.com/launchbadge/sqlx) crate to compile SQL queries for PostgreSQL in source. Write migrations as standard SQL DDL commands (as SQLx recommends). Avoid ORMs because they do not discourage developers from issuing queries from within loops. Also, ORMs provide a whole new DSL to learn and maintain (https://github.com/launchbadge/sqlx?tab=readme-ov-file#sqlx-is-not-an-orm). 
2. *Tonic:* Use the tonic crate (https://github.com/hyperium/tonic) to provide the gRPC endpoints for other services in the namespaces. gRPC (https://grpc.io/) is a highly performant protocol and provides well-typed interfaces via protobuf files. Tonic provides the code generation of the interface traits and types via prost (https://github.com/tokio-rs/prost).

When a new version of a consuming client is under development, introduce a newly versioned protobuf file. The dbrepo then migrates the database to accommodate both versions at the same time and maps each version into a single target db module. The gRPC client developers are then free to build their application with their language of choice. You now have the freedom to rollback the client without risk to the database release and transitively to other services that depend on the dbrepo.

The code in the dbrepo is organized as follows. [Note the **/mod.rs have been omitted for brevity.] The terms “app” and “manager” relate to example schemas in PostgreSQL for organizing tables.

```bash
$ tree
.
├── Cargo.toml
├── Dockerfile
├── Migrations.Dockerfile
├── build.rs
├── crates
│   └── migrations
│       ├── Cargo.toml
│       └── src
│           └── main.rs
├── migrations
│   ├── 0001_init.down.sql
│   └── 0001_init.up.sql
├── proto
│   ├── app
│   │   └── v1.proto
│   └── manager
│       ├── v1.proto
│       └── v2.proto
├── src
│   ├── api
│   │   ├── app
│   │   │   └── v1.rs
│   │   └── manager
│   │       ├── v1.rs
│   │       └── v2.rs
│   ├── core
│   │   ├── error.rs
│   │   └── sql
│   │       ├── app.rs
│   │       └── manager.rs
│   ├── io
│   │   └── db
│   │       ├── appdb.rs
│   │       └── managerdb.rs
│   ├── lib.rs
│   └── main.rs
├── target
└── tests
    ├── integration.rs
    └── real
        ├── appdb.rs
        └── managerdb.rs
```

*Notes*
- The migrations crate is a secondary crate with full DDL permissions against the PostgreSQL database when deployed in its own docker container with the supplied `migrations/*`.sql files.
- The `migrations/*` are written in standard SQL for PostgreSQL.
- The dbrepo crate is built as a docker image and only given DML permissions at runtime in the PostgreSQL db.
- The `build.rs` script provides the pre-built gRPC endpoints via prost for tonic.
- The `proto/**.proto` (protobuf) files provide the versioned interfaces for the consuming clients.
- The `src/api/**.rs` modules provide the mapping of the gRPC endpoints from tonic to the `core/sql/**.rs` in-memory types.
- The `core/sql/**.rs` modules provide Rust structs that bind to the db queries at compile time via sqlx and traits of the IO for dependency injection.
- The `io/db/*.rs` modules provide the runtime IO implementations (impls) of the traits defined in the `core/sql/*.rs` to the database via sqlx. The `io/db/*.rs` modules provide the queries and the command execution against the live database.
- The `lib.rs` rolls up the modules to provide a public interface for both `main.rs` and the integration tests.
- The `main.rs` module provides the entrypoint and construction of the runtime implementations (impl) with the IO.
- The `tests/integration.rs` provides tests with a fake, in-memory implementation (impl) of the `core/sql/*.rs` module traits.
- The `tests/real/*.rs` provide real tests against a running db. The database instances are managed by the Testcontainers for Rust (https://rust.testcontainers.org/) crate. This crate spins up PostgreSQL containers for each test.
