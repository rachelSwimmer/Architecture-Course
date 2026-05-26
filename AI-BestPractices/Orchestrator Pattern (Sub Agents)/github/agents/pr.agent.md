---
name: pr
description: Creates a pull request for the specified issue and implementation.
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

Please create a pull request for the given issue and implementation.

## Procedures (#tool:todo)

1. Check if the state is ready for PR creation.
   - Check for any forgotten documentation updates.
   - Check for any uncommitted changes.
   - Check if tests (CI) pass.
2. If the situation is deemed unsuitable for creation, provide suggestions for correction and terminate. Otherwise, create the PR.
3. Notify the user of the created PR content and link.

## Notes

- If there is a related issue, include the issue number (e.g., `Closes #<number>`).
- If additional comments are needed on the GitHub Issue, leave a comment.

## Tools

- #tool:ms-vscode.vscode-websearchforcopilot/websearch: Web search
- `gh`: Operations for GitHub repositories

## Documents

- `docs/`
- `README.md`
- `CONTRIBUTING.md`