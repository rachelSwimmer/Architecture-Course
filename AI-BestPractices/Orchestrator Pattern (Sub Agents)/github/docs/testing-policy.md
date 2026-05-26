# Testing Policy & Quality Standards

This document outlines the mandatory testing procedures, coverage requirements, and quality gates for all code contributions.

---

## 1. Testing Philosophy
- **Safety First:** No code is merged without verified tests.
- **Isolation:** Tests should be deterministic and independent of external state (use mocks/stubs for APIs and DBs).
- **Fast Feedback:** Unit tests must be optimized for speed to provide immediate developer feedback.

---

## 2. The Testing Pyramid

### 2.1 Unit Tests (Base)
- **Scope:** Individual functions, methods, or classes.
- **Requirement:** 100% of business logic branches must be covered.
- **Constraint:** Must not perform I/O (Disk, Network).

### 2.2 Integration Tests (Middle)
- **Scope:** Interaction between modules, database queries, and third-party service adapters.
- **Requirement:** Mandatory for all API endpoints and database repository methods.

### 2.3 End-to-End (E2E) Tests (Top)
- **Scope:** Critical user journeys (e.g., Auth Flow, Checkout, Data Processing).
- **Tooling:** Playwright, Cypress, or Selenium.
- **Requirement:** Smoke tests for every major release.

---

## 3. Tool-Specific Guidelines

### AI Agent & LLM Testing
- **Deterministic Checks:** Use structured output validation (e.g., Pydantic) to verify agent responses.
- **Prompt Regression:** Maintain a set of "Golden Prompts" to ensure prompt tweaks don't degrade performance.
- **Evaluation Frameworks:** Utilize tools like Ragas or LangSmith for measuring agent accuracy and retrieval quality.

### Python (FastAPI/Flask)
- **Runner:** `pytest`
- **Mocking:** `unittest.mock` or `pytest-mock`.
- **Async:** Use `pytest-asyncio` for all asynchronous route testing.

---

## 4. Coverage & Quality Gates

| Metric | Target | Action on Failure |
| :--- | :--- | :--- |
| **Total Test Coverage** | > 80% | PR Blocked |
| **Critical Logic Coverage** | 100% | PR Blocked |
| **Flaky Tests** | 0 | Immediate quarantine/fix |
| **Linting (Ruff/ESLint)** | Pass | PR Blocked |

---

## 5. Test Writing Checklist
- [ ] Does the test follow the **Arrange-Act-Assert** pattern?
- [ ] Are edge cases (null values, empty strings, max limits) included?
- [ ] Does the test name clearly describe the expected behavior?
- [ ] Are all external secrets and API keys properly mocked?