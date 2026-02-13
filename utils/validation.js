// ======================================
// VALIDATION UTILITIES
// ======================================

// Validation Rules Constants
const VALIDATION_RULES = {
    USERNAME: {
        MIN_LENGTH: 2,
        MAX_LENGTH: 30,
        PATTERN: /^[a-zA-Z0-9_.-]+$/
    },
    PASSWORD: {
        MIN_LENGTH: 6,
        MAX_LENGTH: 128
    },
    EMAIL: {
        PATTERN: /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    }
};

// Validation Error Messages
const ERROR_MESSAGES = {
    REQUIRED: 'This field is required',
    INVALID_EMAIL: 'Please enter a valid email address',
    INVALID_USERNAME: 'Username can only contain letters, numbers, dots, dashes, and underscores',
    USERNAME_TOO_SHORT: `Username must be at least ${VALIDATION_RULES.USERNAME.MIN_LENGTH} characters`,
    USERNAME_TOO_LONG: `Username cannot exceed ${VALIDATION_RULES.USERNAME.MAX_LENGTH} characters`,
    PASSWORD_TOO_SHORT: `Password must be at least ${VALIDATION_RULES.PASSWORD.MIN_LENGTH} characters`,
    PASSWORD_TOO_LONG: `Password cannot exceed ${VALIDATION_RULES.PASSWORD.MAX_LENGTH} characters`,
    INVALID_USERNAME_OR_EMAIL: 'Please enter a valid username or email address'
};

// Validation Manager Class
class ValidationManager {
    constructor() {
        this.validators = new Map();
        this.setupDefaultValidators();
    }

    /**
     * Setup default validation functions
     */
    setupDefaultValidators() {
        this.validators.set('required', this.validateRequired);
        this.validators.set('email', this.validateEmail);
        this.validators.set('username', this.validateUsername);
        this.validators.set('password', this.validatePassword);
        this.validators.set('usernameOrEmail', this.validateUsernameOrEmail);
    }

    /**
     * Validate required field
     * @param {string} value - Field value
     * @returns {Object} Validation result
     */
    validateRequired(value) {
        const isValid = value !== null && value !== undefined && value.toString().trim().length > 0;
        return {
            isValid,
            error: isValid ? null : ERROR_MESSAGES.REQUIRED
        };
    }

    /**
     * Validate email format
     * @param {string} value - Email value
     * @returns {Object} Validation result
     */
    validateEmail(value) {
        if (!value) {
            return { isValid: true, error: null }; // Allow empty if not required
        }

        const isValid = VALIDATION_RULES.EMAIL.PATTERN.test(value.trim());
        return {
            isValid,
            error: isValid ? null : ERROR_MESSAGES.INVALID_EMAIL
        };
    }

    /**
     * Validate username format and length
     * @param {string} value - Username value
     * @returns {Object} Validation result
     */
    validateUsername(value) {
        if (!value) {
            return { isValid: true, error: null }; // Allow empty if not required
        }

        const trimmedValue = value.trim();

        // Check length
        if (trimmedValue.length < VALIDATION_RULES.USERNAME.MIN_LENGTH) {
            return {
                isValid: false,
                error: ERROR_MESSAGES.USERNAME_TOO_SHORT
            };
        }

        if (trimmedValue.length > VALIDATION_RULES.USERNAME.MAX_LENGTH) {
            return {
                isValid: false,
                error: ERROR_MESSAGES.USERNAME_TOO_LONG
            };
        }

        // Check format
        const isValid = VALIDATION_RULES.USERNAME.PATTERN.test(trimmedValue);
        return {
            isValid,
            error: isValid ? null : ERROR_MESSAGES.INVALID_USERNAME
        };
    }

    /**
     * Validate password length
     * @param {string} value - Password value
     * @returns {Object} Validation result
     */
    validatePassword(value) {
        if (!value) {
            return { isValid: true, error: null }; // Allow empty if not required
        }

        const length = value.length;

        if (length < VALIDATION_RULES.PASSWORD.MIN_LENGTH) {
            return {
                isValid: false,
                error: ERROR_MESSAGES.PASSWORD_TOO_SHORT
            };
        }

        if (length > VALIDATION_RULES.PASSWORD.MAX_LENGTH) {
            return {
                isValid: false,
                error: ERROR_MESSAGES.PASSWORD_TOO_LONG
            };
        }

        return { isValid: true, error: null };
    }

    /**
     * Validate username or email (flexible input)
     * @param {string} value - Username or email value
     * @returns {Object} Validation result
     */
    validateUsernameOrEmail(value) {
        if (!value) {
            return { isValid: true, error: null }; // Allow empty if not required
        }

        const trimmedValue = value.trim();

        // If contains @, validate as email
        if (trimmedValue.includes('@')) {
            return this.validateEmail(trimmedValue);
        }

        // Otherwise, validate as username
        return this.validateUsername(trimmedValue);
    }

    /**
     * Validate field with multiple rules
     * @param {string} value - Field value
     * @param {Array} rules - Array of validation rule names
     * @returns {Object} Validation result
     */
    validateField(value, rules) {
        const errors = [];

        for (const rule of rules) {
            const validator = this.validators.get(rule);
            if (validator) {
                const result = validator.call(this, value);
                if (!result.isValid) {
                    errors.push(result.error);
                    break; // Stop at first error
                }
            }
        }

        return {
            isValid: errors.length === 0,
            errors: errors,
            error: errors[0] || null
        };
    }

    /**
     * Validate entire form
     * @param {Object} formData - Form data object
     * @param {Object} validationRules - Validation rules for each field
     * @returns {Object} Validation result
     */
    validateForm(formData, validationRules) {
        const results = {};
        let isFormValid = true;

        for (const [fieldName, rules] of Object.entries(validationRules)) {
            const fieldValue = formData[fieldName];
            const result = this.validateField(fieldValue, rules);
            
            results[fieldName] = result;
            
            if (!result.isValid) {
                isFormValid = false;
            }
        }

        return {
            isValid: isFormValid,
            fields: results,
            errors: Object.entries(results)
                .filter(([_, result]) => !result.isValid)
                .map(([fieldName, result]) => ({ field: fieldName, error: result.error }))
        };
    }

    /**
     * Sanitize input to prevent XSS
     * @param {string} input - Input value
     * @returns {string} Sanitized input
     */
    sanitizeInput(input) {
        if (typeof input !== 'string') {
            return input;
        }

        return input
            .trim()
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#x27;')
            .replace(/\//g, '&#x2F;');
    }

    /**
     * Add custom validator
     * @param {string} name - Validator name
     * @param {Function} validator - Validator function
     */
    addValidator(name, validator) {
        this.validators.set(name, validator);
    }
}

// Form Field Validation Helper
class FieldValidator {
    constructor(fieldElement) {
        this.field = fieldElement;
        this.errorElement = null;
        this.setupErrorElement();
    }

    /**
     * Setup error display element
     */
    setupErrorElement() {
        const fieldId = this.field.id;
        let errorElement = document.getElementById(`${fieldId}-error`);
        
        if (!errorElement) {
            errorElement = document.createElement('div');
            errorElement.id = `${fieldId}-error`;
            errorElement.className = 'field-error';
            errorElement.setAttribute('role', 'alert');
            errorElement.setAttribute('aria-live', 'polite');
            
            this.field.parentNode.appendChild(errorElement);
        }
        
        this.errorElement = errorElement;
        
        // Associate field with error element for accessibility
        this.field.setAttribute('aria-describedby', errorElement.id);
    }

    /**
     * Show field error
     * @param {string} message - Error message
     */
    showError(message) {
        if (this.errorElement && message) {
            this.errorElement.textContent = message;
            this.errorElement.classList.add('visible');
            this.field.classList.add('error');
            this.field.setAttribute('aria-invalid', 'true');
        }
    }

    /**
     * Clear field error
     */
    clearError() {
        if (this.errorElement) {
            this.errorElement.textContent = '';
            this.errorElement.classList.remove('visible');
            this.field.classList.remove('error');
            this.field.setAttribute('aria-invalid', 'false');
        }
    }

    /**
     * Validate field with given rules
     * @param {Array} rules - Validation rules
     * @param {ValidationManager} validator - Validator instance
     * @returns {boolean} Validation result
     */
    validate(rules, validator) {
        const result = validator.validateField(this.field.value, rules);
        
        if (result.isValid) {
            this.clearError();
        } else {
            this.showError(result.error);
        }
        
        return result.isValid;
    }
}

// Form Validator Class
class FormValidator {
    constructor(formElement, validationRules) {
        this.form = formElement;
        this.rules = validationRules;
        this.validator = new ValidationManager();
        this.fieldValidators = new Map();
        this.setupFieldValidators();
        this.attachEventListeners();
    }

    /**
     * Setup field validators
     */
    setupFieldValidators() {
        for (const fieldName of Object.keys(this.rules)) {
            const field = this.form.querySelector(`[name="${fieldName}"], #${fieldName}`);
            if (field) {
                this.fieldValidators.set(fieldName, new FieldValidator(field));
            }
        }
    }

    /**
     * Attach event listeners for real-time validation
     */
    attachEventListeners() {
        // Validate on blur
        for (const [fieldName, fieldValidator] of this.fieldValidators) {
            const field = fieldValidator.field;
            field.addEventListener('blur', () => {
                this.validateField(fieldName);
            });

            // Clear errors on input
            field.addEventListener('input', () => {
                if (field.classList.contains('error')) {
                    fieldValidator.clearError();
                }
            });
        }
    }

    /**
     * Validate single field
     * @param {string} fieldName - Field name
     * @returns {boolean} Validation result
     */
    validateField(fieldName) {
        const fieldValidator = this.fieldValidators.get(fieldName);
        const rules = this.rules[fieldName];
        
        if (fieldValidator && rules) {
            return fieldValidator.validate(rules, this.validator);
        }
        
        return true;
    }

    /**
     * Validate entire form
     * @returns {boolean} Validation result
     */
    validateForm() {
        let isValid = true;
        
        for (const fieldName of Object.keys(this.rules)) {
            const fieldValid = this.validateField(fieldName);
            if (!fieldValid) {
                isValid = false;
            }
        }
        
        return isValid;
    }

    /**
     * Get form data as object
     * @returns {Object} Form data
     */
    getFormData() {
        const formData = new FormData(this.form);
        const data = {};
        
        for (const [key, value] of formData.entries()) {
            data[key] = this.validator.sanitizeInput(value);
        }
        
        return data;
    }

    /**
     * Clear all field errors
     */
    clearAllErrors() {
        for (const fieldValidator of this.fieldValidators.values()) {
            fieldValidator.clearError();
        }
    }
}

// Create global validator instance
const validationManager = new ValidationManager();

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        ValidationManager,
        FieldValidator,
        FormValidator,
        validationManager,
        VALIDATION_RULES,
        ERROR_MESSAGES
    };
} else {
    // Global access for browser
    window.ValidationManager = ValidationManager;
    window.FieldValidator = FieldValidator;
    window.FormValidator = FormValidator;
    window.validationManager = validationManager;
    window.VALIDATION_RULES = VALIDATION_RULES;
    window.ERROR_MESSAGES = ERROR_MESSAGES;
}