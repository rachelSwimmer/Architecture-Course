---
name: plan
description: Analyzes the repository, collects necessary information, and formulates an implementation plan for the specified issue.
tools:
  [
    "execute",
    "read",
    "search",
    "todo",
    "web",
    "ms-vscode.vscode-websearchforcopilot/websearch",
  ]
---

Please create an implementation plan for the given issue.

## Procedures (#tool:todo)

1. Check the current repository status and synchronize with the remote.
2. Confirm the details of the specified issue. If the issue does not exist, stop the process and notify the user.
3. Check the repository (code and documentation).
4. Collect information via web search.
5. Present the implementation plan to the user.

## Tools

- #tool:ms-vscode.vscode-websearchforcopilot/websearch: Web search
- `gh`: Operations for GitHub repositories

## Documents

- `docs/`
- `README.md`
- `CONTRIBUTING.md`

## Branching Strategy

- Create a branch for each new task and include the GitHub Issue number (e.g., `feature/issue-123-description`).
- Regularly rebase from or merge with the `main` branch to stay up-to-date.
- Direct commits to the `main` branch are not permitted.