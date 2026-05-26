AI Coding Instructions & Project Guidelines

This document serves as the "Source of Truth" for AI assistants and human collaborators. It defines the coding standards, workflow protocols, and architectural principles for this project.

---

## 1. Architectural Principles

### 1.1 Specification-Driven Development (SDD)
- **Contract First:** Always consult API definitions (`OpenAPI`, `GraphQL Schema`, or `types.ts`) before writing implementation code.
- **SSOT:** The documentation is the Single Source of Truth. If the code deviates from the documentation, the code is considered technical debt.

### 1.2 Modularity & Logic Separation
- **Thin Controllers:** Keep entry points (routes/controllers) focused on request validation and response mapping.
- **Service Layer:** All business logic must reside in dedicated service modules.
- **Utility Purity:** Utility functions should be pure, stateless, and side-effect-free.

---

## 2. Coding Conventions

### 2.1 General Naming Rules
- **Variables/Functions:** `camelCase` (TypeScript/JS) or `snake_case` (Python).
- **Classes/Interfaces:** `PascalCase`.
- **Constants:** `SCREAMING_SNAKE_CASE`.
- **Boolean Variables:** Must use prefixes like `is`, `has`, `should`, or `can` (e.g., `is_active`).

### 2.2 Documentation
- **Public APIs:** Every public function/method must have a Docstring/JSDoc.
- **The "Why" Rule:** Comments should explain *why* a complex logic exists, not *what* the code is doing (the code should be self-describing).

---

## 3. Git & Workflow Strategy

### 3.1 Branching Strategy (Feature-Branch Workflow)
- **`main`**: Protected branch. Only merges via Pull Requests (PRs).
- **`feat/`**: New features (e.g., `feat/user-auth`).
- **`fix/`**: Bug fixes (e.g., `fix/login-loop`).
- **`refactor/`**: Improvements without changing functionality.

### 3.2 Commit Message Convention
We follow the **Conventional Commits** specification:
`type(scope): description`

- **feat**: A new feature.
- **fix**: A bug fix.
- **docs**: Documentation updates.
- **style**: Formatting, missing semi-colons, etc (no code changes).
- **refactor**: Code change that neither fixes a bug nor adds a feature.
- **perf**: A code change that improves performance.
- **test**: Adding or correcting tests.
- **chore**: Updating build tasks, package manager configs, etc.

---
