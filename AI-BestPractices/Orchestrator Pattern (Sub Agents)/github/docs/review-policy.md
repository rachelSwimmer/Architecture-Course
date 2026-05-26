# Code Review Policy & Guidelines

This document defines the standards and expectations for the code review process. The goal of code reviews is to ensure code quality, share knowledge, and maintain consistency across the codebase.

---

## 1. Review Objectives
- **Code Quality:** Ensure the code is readable, maintainable, and follows project standards.
- **Defect Detection:** Identify logical errors, security vulnerabilities, and performance bottlenecks.
- **Knowledge Sharing:** Facilitate collective ownership of the codebase and mentor team members.
- **Consistency:** Maintain uniform patterns, naming conventions, and architectural styles.

---

## 2. Reviewer Expectations

### 2.1 Technical Checklist
- **Functionality:** Does the code actually do what it is supposed to do?
- **Readability:** Is the code easy to understand? Are variables well-named?
- **Simplicity:** Is there a simpler way to achieve the same result? (Avoid over-engineering).
- **Security:** Are inputs validated? Is sensitive data handled correctly?
- **Performance:** Are there obvious inefficiencies (e.g., N+1 queries, heavy loops)?
- **Tests:** Are there sufficient tests? Do they pass?

### 2.2 Communication Tone
- **Be Respectful:** Critique the code, not the person. Use "we" instead of "you".
- **Be Constructive:** If you find an issue, suggest a specific improvement or ask a clarifying question.
- **Appreciation:** Give "Kudos" for particularly clean or clever solutions.

---

## 3. Author Expectations

### 3.1 Submission Standards
- **Small PRs:** Keep Pull Requests under 400 lines of code to ensure thorough reviews.
- **Self-Review:** Review your own code before assigning others. Check for console logs and temporary comments.
- **Context:** Provide a clear description of *what* changed and *why*. Link to relevant tickets/issues.
- **Screenshots:** For UI changes, attach screenshots or a short screen recording.

### 3.2 Response to Feedback
- **Openness:** Be open to suggestions. If you disagree, explain your reasoning professionally.
- **Resolution:** Address all comments before merging. Use "Resolve" buttons only after the change is made.

---

## 4. The Approval Process

- **Minimum Approvals:** At least 1 (or 2 for critical systems) approved review is required before merging.
- **Blocking Comments:** If a reviewer finds a critical issue, they should use "Request Changes."
- **Nitpicks:** Use the prefix `nit:` for minor suggestions that don't necessarily block a merge.
- **Emergency Merges:** In case of hotfixes, the requirement can be bypassed with a follow-up review scheduled immediately.
