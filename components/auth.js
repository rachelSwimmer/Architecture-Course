// ======================================
// AUTHENTICATION SYSTEM
// ======================================

// Mock User Database
const MOCK_USERS = [
    { 
        id: 'admin123',
        username: 'admin', 
        password: 'password123', 
        email: 'admin@taskmanager.com',
        role: 'admin' 
    },
    { 
        id: 'user456',
        username: 'user', 
        password: 'user123', 
        email: 'user@taskmanager.com',
        role: 'user' 
    }
];

// Authentication State
let currentUser = null;
let authToken = null;

// Storage Keys
const STORAGE_KEYS = {
    AUTH_TOKEN: 'taskManager_authToken',
    USER_SESSION: 'taskManager_userSession',
    LOGIN_TIME: 'taskManager_loginTime'
};

// Authentication Class
class AuthManager {
    constructor() {
        this.initializeAuth();
    }

    /**
     * Initialize authentication system
     */
    initializeAuth() {
        this.loadUserSession();
        this.validateSession();
    }

    /**
     * Authenticate user with username/email and password
     * @param {string} usernameOrEmail - Username or email
     * @param {string} password - User password
     * @returns {Promise<Object>} Authentication result
     */
    async login(usernameOrEmail, password) {
        try {
            if (!usernameOrEmail || !password) {
                throw new Error('Username and password are required');
            }

            // Simulate network delay
            await this.simulateDelay(500);

            // Find user in mock database
            const user = this.findUser(usernameOrEmail, password);

            if (!user) {
                throw new Error('Invalid username or password');
            }

            // Generate mock JWT token
            const token = this.generateMockToken(user);

            // Set authentication state
            currentUser = {
                id: user.id,
                username: user.username,
                email: user.email,
                role: user.role,
                isAuthenticated: true,
                loginTime: new Date().toISOString()
            };

            authToken = token;

            // Store session data
            this.saveUserSession();

            return {
                success: true,
                user: currentUser,
                token: authToken
            };

        } catch (error) {
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Logout current user
     */
    logout() {
        // Clear in-memory state
        currentUser = null;
        authToken = null;

        // Clear stored session data
        this.clearUserSession();

        // Announce logout to screen readers
        this.announceToScreenReader('Successfully logged out');

        // Redirect to login page if not already there
        if (!window.location.pathname.includes('login.html')) {
            this.redirectToLogin();
        }
    }

    /**
     * Get current authenticated user
     * @returns {Object|null} Current user or null
     */
    getCurrentUser() {
        return currentUser;
    }

    /**
     * Check if user is authenticated
     * @returns {boolean} Authentication status
     */
    isAuthenticated() {
        return currentUser !== null && currentUser.isAuthenticated === true;
    }

    /**
     * Check authentication status and redirect if needed
     */
    checkAuthStatus() {
        if (!this.isAuthenticated()) {
            this.redirectToLogin();
            return false;
        }
        return true;
    }

    /**
     * Redirect to login page
     */
    redirectToLogin() {
        if (!window.location.pathname.includes('login.html')) {
            window.location.href = 'login.html';
        }
    }

    /**
     * Redirect to main application
     */
    redirectToApp() {
        if (window.location.pathname.includes('login.html')) {
            window.location.href = 'index.html';
        }
    }

    /**
     * Find user in mock database
     * @param {string} usernameOrEmail - Username or email
     * @param {string} password - Password
     * @returns {Object|null} User object or null
     */
    findUser(usernameOrEmail, password) {
        const normalizedInput = usernameOrEmail.toLowerCase().trim();
        
        return MOCK_USERS.find(user => {
            const matchesUsername = user.username.toLowerCase() === normalizedInput;
            const matchesEmail = user.email.toLowerCase() === normalizedInput;
            const matchesPassword = user.password === password; // In real app, use bcrypt comparison
            
            return (matchesUsername || matchesEmail) && matchesPassword;
        });
    }

    /**
     * Generate mock JWT token
     * @param {Object} user - User object
     * @returns {string} Mock token
     */
    generateMockToken(user) {
        const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
        const payload = btoa(JSON.stringify({
            sub: user.id,
            username: user.username,
            email: user.email,
            role: user.role,
            iat: Date.now(),
            exp: Date.now() + (24 * 60 * 60 * 1000) // 24 hours
        }));
        const signature = btoa('mock-signature-' + user.id);
        
        return `${header}.${payload}.${signature}`;
    }

    /**
     * Validate current session
     */
    validateSession() {
        if (currentUser && authToken) {
            try {
                // In a real app, validate token with backend
                const tokenParts = authToken.split('.');
                if (tokenParts.length !== 3) {
                    throw new Error('Invalid token format');
                }

                const payload = JSON.parse(atob(tokenParts[1]));
                
                // Check if token is expired
                if (Date.now() > payload.exp) {
                    throw new Error('Token expired');
                }

                // Token is valid, keep session
                return true;

            } catch (error) {
                console.warn('Invalid session detected:', error.message);
                this.logout();
                return false;
            }
        }
        return false;
    }

    /**
     * Save user session to localStorage
     */
    saveUserSession() {
        try {
            localStorage.setItem(STORAGE_KEYS.AUTH_TOKEN, authToken);
            localStorage.setItem(STORAGE_KEYS.USER_SESSION, JSON.stringify(currentUser));
            localStorage.setItem(STORAGE_KEYS.LOGIN_TIME, new Date().toISOString());
        } catch (error) {
            console.error('Failed to save user session:', error);
        }
    }

    /**
     * Load user session from localStorage
     */
    loadUserSession() {
        try {
            const storedToken = localStorage.getItem(STORAGE_KEYS.AUTH_TOKEN);
            const storedUser = localStorage.getItem(STORAGE_KEYS.USER_SESSION);

            if (storedToken && storedUser) {
                authToken = storedToken;
                currentUser = JSON.parse(storedUser);
            }
        } catch (error) {
            console.error('Failed to load user session:', error);
            this.clearUserSession();
        }
    }

    /**
     * Clear user session from localStorage
     */
    clearUserSession() {
        try {
            localStorage.removeItem(STORAGE_KEYS.AUTH_TOKEN);
            localStorage.removeItem(STORAGE_KEYS.USER_SESSION);
            localStorage.removeItem(STORAGE_KEYS.LOGIN_TIME);
        } catch (error) {
            console.error('Failed to clear user session:', error);
        }
    }

    /**
     * Get session info for debugging
     * @returns {Object} Session information
     */
    getSessionInfo() {
        return {
            isAuthenticated: this.isAuthenticated(),
            user: currentUser,
            hasToken: !!authToken,
            loginTime: localStorage.getItem(STORAGE_KEYS.LOGIN_TIME)
        };
    }

    /**
     * Simulate network delay for realistic UX
     * @param {number} ms - Delay in milliseconds
     */
    simulateDelay(ms = 1000) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    /**
     * Announce message to screen readers
     * @param {string} message - Message to announce
     */
    announceToScreenReader(message) {
        const announcement = document.createElement('div');
        announcement.setAttribute('role', 'status');
        announcement.setAttribute('aria-live', 'polite');
        announcement.className = 'visually-hidden';
        announcement.textContent = message;
        
        document.body.appendChild(announcement);
        
        setTimeout(() => {
            if (document.body.contains(announcement)) {
                document.body.removeChild(announcement);
            }
        }, 1000);
    }
}

// Create global auth manager instance
const authManager = new AuthManager();

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { AuthManager, authManager };
} else {
    window.authManager = authManager;
}