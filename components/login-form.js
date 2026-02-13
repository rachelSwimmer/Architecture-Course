// ======================================
// LOGIN FORM COMPONENT
// ======================================

// Login Form Component Class
class LoginForm {
    constructor(formSelector = '#loginForm') {
        this.form = document.querySelector(formSelector);
        this.isLoading = false;
        this.initializeComponent();
    }

    /**
     * Initialize login form component
     */
    initializeComponent() {
        if (!this.form) {
            console.error('Login form not found');
            return;
        }

        this.setupFormElements();
        this.setupValidation();
        this.attachEventListeners();
        this.checkInitialAuthState();
    }

    /**
     * Setup form element references
     */
    setupFormElements() {
        this.elements = {
            form: this.form,
            usernameField: this.form.querySelector('#username'),
            passwordField: this.form.querySelector('#password'),
            passwordToggle: this.form.querySelector('#passwordToggle'),
            submitButton: this.form.querySelector('#submitButton'),
            loadingSpinner: this.form.querySelector('#loadingSpinner'),
            errorContainer: this.form.querySelector('#loginError'),
            demoCredentials: document.querySelector('#demoCredentials')
        };

        // Validate required elements
        if (!this.elements.usernameField || !this.elements.passwordField || !this.elements.submitButton) {
            console.error('Required form elements not found');
            return;
        }
    }

    /**
     * Setup form validation
     */
    setupValidation() {
        this.validationRules = {
            username: ['required', 'usernameOrEmail'],
            password: ['required', 'password']
        };

        this.formValidator = new FormValidator(this.form, this.validationRules);
    }

    /**
     * Attach event listeners
     */
    attachEventListeners() {
        // Form submission
        this.form.addEventListener('submit', this.handleSubmit.bind(this));

        // Password visibility toggle
        if (this.elements.passwordToggle) {
            this.elements.passwordToggle.addEventListener('click', this.togglePasswordVisibility.bind(this));
        }

        // Demo credentials buttons
        if (this.elements.demoCredentials) {
            this.elements.demoCredentials.addEventListener('click', this.handleDemoCredentials.bind(this));
        }

        // Real-time validation feedback
        this.elements.usernameField.addEventListener('input', this.clearLoginError.bind(this));
        this.elements.passwordField.addEventListener('input', this.clearLoginError.bind(this));

        // Keyboard shortcuts
        document.addEventListener('keydown', this.handleKeyboardShortcuts.bind(this));

        // Focus management
        this.setupFocusManagement();
    }

    /**
     * Setup focus management for accessibility
     */
    setupFocusManagement() {
        // Auto-focus username field on page load
        if (this.elements.usernameField) {
            setTimeout(() => {
                this.elements.usernameField.focus();
            }, 100);
        }

        // Focus management for errors
        this.form.addEventListener('focusin', (e) => {
            if (e.target.classList.contains('error')) {
                this.announceToScreenReader(`${e.target.labels[0]?.textContent}: ${e.target.getAttribute('aria-describedby')}`);
            }
        });
    }

    /**
     * Handle form submission
     * @param {Event} e - Submit event
     */
    async handleSubmit(e) {
        e.preventDefault();

        if (this.isLoading) {
            return;
        }

        // Clear any previous errors
        this.clearLoginError();

        // Validate form
        if (!this.formValidator.validateForm()) {
            this.announceToScreenReader('Please correct the errors in the form');
            this.focusFirstError();
            return;
        }

        // Get form data
        const formData = this.formValidator.getFormData();
        
        // Start loading state
        this.setLoadingState(true);

        try {
            // Attempt login
            const result = await authManager.login(formData.username, formData.password);
            
            if (result.success) {
                this.handleLoginSuccess(result);
            } else {
                this.handleLoginError(result.error);
            }
        } catch (error) {
            console.error('Login error:', error);
            this.handleLoginError('An unexpected error occurred. Please try again.');
        } finally {
            this.setLoadingState(false);
        }
    }

    /**
     * Handle successful login
     * @param {Object} result - Login result
     */
    handleLoginSuccess(result) {
        this.announceToScreenReader(`Welcome back, ${result.user.username}!`);
        
        // Clear form
        this.form.reset();
        
        // Show success feedback (briefly)
        this.showSuccessMessage(`Welcome back, ${result.user.username}!`);
        
        // Redirect to main app after brief delay
        setTimeout(() => {
            authManager.redirectToApp();
        }, 1000);
    }

    /**
     * Handle login error
     * @param {string} errorMessage - Error message
     */
    handleLoginError(errorMessage) {
        this.showLoginError(errorMessage);
        this.announceToScreenReader(`Login failed: ${errorMessage}`);
        
        // Focus back to username field for retry
        this.elements.usernameField.focus();
        this.elements.usernameField.select();
    }

    /**
     * Set loading state
     * @param {boolean} loading - Loading state
     */
    setLoadingState(loading) {
        this.isLoading = loading;
        
        // Update submit button
        if (this.elements.submitButton) {
            this.elements.submitButton.disabled = loading;
            this.elements.submitButton.textContent = loading ? 'LOGGING IN...' : 'LOGIN';
            this.elements.submitButton.setAttribute('aria-busy', loading.toString());
        }

        // Show/hide loading spinner
        if (this.elements.loadingSpinner) {
            this.elements.loadingSpinner.style.display = loading ? 'block' : 'none';
        }

        // Disable form fields during loading
        this.elements.usernameField.disabled = loading;
        this.elements.passwordField.disabled = loading;
        
        if (this.elements.passwordToggle) {
            this.elements.passwordToggle.disabled = loading;
        }
    }

    /**
     * Toggle password visibility
     */
    togglePasswordVisibility() {
        const passwordField = this.elements.passwordField;
        const toggleButton = this.elements.passwordToggle;
        
        if (passwordField.type === 'password') {
            passwordField.type = 'text';
            toggleButton.textContent = 'HIDE';
            toggleButton.setAttribute('aria-label', 'Hide password');
            this.announceToScreenReader('Password is now visible');
        } else {
            passwordField.type = 'password';
            toggleButton.textContent = 'SHOW';
            toggleButton.setAttribute('aria-label', 'Show password');
            this.announceToScreenReader('Password is now hidden');
        }
        
        // Return focus to password field
        passwordField.focus();
    }

    /**
     * Handle demo credentials selection
     * @param {Event} e - Click event
     */
    handleDemoCredentials(e) {
        if (e.target.matches('[data-demo-user]')) {
            const userType = e.target.dataset.demoUser;
            
            let username, password;
            
            switch (userType) {
                case 'admin':
                    username = 'admin';
                    password = 'password123';
                    break;
                case 'user':
                    username = 'user';
                    password = 'user123';
                    break;
                default:
                    return;
            }
            
            // Fill form with demo credentials
            this.elements.usernameField.value = username;
            this.elements.passwordField.value = password;
            
            // Clear any existing errors
            this.clearLoginError();
            this.formValidator.clearAllErrors();
            
            // Focus submit button
            this.elements.submitButton.focus();
            
            // Announce to screen readers
            this.announceToScreenReader(`Demo credentials loaded for ${userType}`);
        }
    }

    /**
     * Handle keyboard shortcuts
     * @param {KeyboardEvent} e - Keyboard event
     */
    handleKeyboardShortcuts(e) {
        // Enter key submits form
        if (e.key === 'Enter' && (e.target === this.elements.usernameField || e.target === this.elements.passwordField)) {
            this.handleSubmit(e);
        }

        // Escape key clears errors
        if (e.key === 'Escape') {
            this.clearLoginError();
        }

        // Ctrl/Cmd + K focuses username field
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            this.elements.usernameField.focus();
            this.elements.usernameField.select();
        }
    }

    /**
     * Show login error
     * @param {string} message - Error message
     */
    showLoginError(message) {
        if (this.elements.errorContainer) {
            this.elements.errorContainer.textContent = message;
            this.elements.errorContainer.classList.add('visible');
            this.elements.errorContainer.setAttribute('role', 'alert');
        }
    }

    /**
     * Clear login error
     */
    clearLoginError() {
        if (this.elements.errorContainer) {
            this.elements.errorContainer.textContent = '';
            this.elements.errorContainer.classList.remove('visible');
            this.elements.errorContainer.removeAttribute('role');
        }
    }

    /**
     * Show success message
     * @param {string} message - Success message
     */
    showSuccessMessage(message) {
        if (this.elements.errorContainer) {
            this.elements.errorContainer.textContent = message;
            this.elements.errorContainer.className = 'login-success visible';
            this.elements.errorContainer.setAttribute('role', 'status');
        }
    }

    /**
     * Focus first error field
     */
    focusFirstError() {
        const firstErrorField = this.form.querySelector('.error');
        if (firstErrorField) {
            firstErrorField.focus();
        }
    }

    /**
     * Check initial authentication state
     */
    checkInitialAuthState() {
        // If user is already authenticated, redirect to app
        if (authManager.isAuthenticated()) {
            authManager.redirectToApp();
        }
    }

    /**
     * Announce to screen readers
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

    /**
     * Destroy component and clean up event listeners
     */
    destroy() {
        // Remove event listeners
        this.form.removeEventListener('submit', this.handleSubmit.bind(this));
        
        if (this.elements.passwordToggle) {
            this.elements.passwordToggle.removeEventListener('click', this.togglePasswordVisibility.bind(this));
        }
        
        if (this.elements.demoCredentials) {
            this.elements.demoCredentials.removeEventListener('click', this.handleDemoCredentials.bind(this));
        }

        document.removeEventListener('keydown', this.handleKeyboardShortcuts.bind(this));
        
        // Clear form reference
        this.form = null;
        this.elements = null;
        this.formValidator = null;
    }
}

// Auto-initialize when DOM is ready
function initializeLoginForm() {
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            window.loginForm = new LoginForm();
        });
    } else {
        window.loginForm = new LoginForm();
    }
}

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { LoginForm };
} else {
    // Global access for browser
    window.LoginForm = LoginForm;
    
    // Auto-initialize if on login page
    if (document.body.classList.contains('login-page') || window.location.pathname.includes('login')) {
        initializeLoginForm();
    }
}