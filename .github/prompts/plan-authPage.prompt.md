# Plan: Add Authentication Page to Task Manager

Create a brutalist-styled authentication page allowing users to sign up and log in, enabling user-specific task management with persistent sessions using localStorage.

## Steps

1. **Create auth.html** with brutalist login/signup forms following the existing design system (lime accents, hard shadows, thick borders, uppercase typography)
2. **Add authentication logic to script.js** including `login()`, `signup()`, `logout()`, and `checkAuth()` functions with localStorage-based session management
3. **Update index.html** to add user profile navbar section, logout button, and display current user's name
4. **Modify task storage in script.js** to scope tasks per user (e.g., `tasks_username` instead of global `tasks` key)
5. **Extend styles.css** with authentication form styles matching brutalist design (form inputs, buttons, error messages, profile section)

## Further Considerations

1. **Navigation approach?** Option A: Separate `auth.html` file with redirect / Option B: Modal overlay on `index.html` / Option C: Toggle between auth and app sections on same page
2. **Password handling?** Since this uses localStorage (not a real backend), should we hash passwords client-side for demonstration, or keep plain text with a disclaimer?
3. **Session persistence?** Should users stay logged in after closing the browser, or require re-authentication each session?
