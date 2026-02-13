// ======================================
// STORAGE UTILITIES
// ======================================

// Storage Keys Constants
const STORAGE_KEYS = {
    AUTH_TOKEN: 'taskManager_authToken',
    USER_SESSION: 'taskManager_userSession',
    LOGIN_TIME: 'taskManager_loginTime',
    TASKS: 'tasks',
    SETTINGS: 'taskManager_settings'
};

// Storage Manager Class
class StorageManager {
    constructor() {
        this.isAvailable = this.checkStorageAvailability();
    }

    /**
     * Check if localStorage is available
     * @returns {boolean} Storage availability status
     */
    checkStorageAvailability() {
        try {
            const test = '__storage_test__';
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (error) {
            console.warn('localStorage is not available:', error);
            return false;
        }
    }

    /**
     * Set item in storage with error handling
     * @param {string} key - Storage key
     * @param {*} value - Value to store
     * @returns {boolean} Success status
     */
    setItem(key, value) {
        if (!this.isAvailable) {
            console.warn('Storage not available, using in-memory fallback');
            return this.setInMemory(key, value);
        }

        try {
            const jsonValue = JSON.stringify({
                value: value,
                timestamp: Date.now(),
                type: typeof value
            });
            localStorage.setItem(key, jsonValue);
            return true;
        } catch (error) {
            console.error(`Failed to set storage item "${key}":`, error);
            return false;
        }
    }

    /**
     * Get item from storage with parsing
     * @param {string} key - Storage key
     * @param {*} defaultValue - Default value if not found
     * @returns {*} Retrieved value or default
     */
    getItem(key, defaultValue = null) {
        if (!this.isAvailable) {
            return this.getInMemory(key, defaultValue);
        }

        try {
            const storedItem = localStorage.getItem(key);
            if (storedItem === null) {
                return defaultValue;
            }

            const parsed = JSON.parse(storedItem);
            
            // Handle legacy items (plain values without metadata)
            if (!parsed.hasOwnProperty('value')) {
                return storedItem === 'undefined' ? defaultValue : JSON.parse(storedItem);
            }

            return parsed.value;
        } catch (error) {
            console.error(`Failed to get storage item "${key}":`, error);
            return defaultValue;
        }
    }

    /**
     * Remove item from storage
     * @param {string} key - Storage key
     * @returns {boolean} Success status
     */
    removeItem(key) {
        if (!this.isAvailable) {
            return this.removeInMemory(key);
        }

        try {
            localStorage.removeItem(key);
            return true;
        } catch (error) {
            console.error(`Failed to remove storage item "${key}":`, error);
            return false;
        }
    }

    /**
     * Clear all storage items
     * @returns {boolean} Success status
     */
    clear() {
        if (!this.isAvailable) {
            this.clearInMemory();
            return true;
        }

        try {
            localStorage.clear();
            return true;
        } catch (error) {
            console.error('Failed to clear storage:', error);
            return false;
        }
    }

    /**
     * Get all keys with optional prefix filter
     * @param {string} prefix - Key prefix to filter by
     * @returns {Array} Array of matching keys
     */
    getKeys(prefix = '') {
        if (!this.isAvailable) {
            return this.getInMemoryKeys(prefix);
        }

        try {
            const keys = [];
            for (let i = 0; i < localStorage.length; i++) {
                const key = localStorage.key(i);
                if (key && key.startsWith(prefix)) {
                    keys.push(key);
                }
            }
            return keys;
        } catch (error) {
            console.error('Failed to get storage keys:', error);
            return [];
        }
    }

    /**
     * Get storage size information
     * @returns {Object} Storage size information
     */
    getStorageInfo() {
        if (!this.isAvailable) {
            return {
                used: 0,
                total: 0,
                remaining: 0,
                itemCount: Object.keys(this.inMemoryStorage || {}).length
            };
        }

        try {
            let totalSize = 0;
            let itemCount = 0;

            for (let key in localStorage) {
                if (localStorage.hasOwnProperty(key)) {
                    totalSize += localStorage[key].length + key.length;
                    itemCount++;
                }
            }

            // Estimate total localStorage size (typically 5-10MB)
            const estimatedTotal = 5 * 1024 * 1024; // 5MB
            const remaining = Math.max(0, estimatedTotal - totalSize);

            return {
                used: totalSize,
                total: estimatedTotal,
                remaining: remaining,
                itemCount: itemCount
            };
        } catch (error) {
            console.error('Failed to get storage info:', error);
            return {
                used: 0,
                total: 0,
                remaining: 0,
                itemCount: 0
            };
        }
    }

    // In-Memory Storage Fallback
    inMemoryStorage = {};

    setInMemory(key, value) {
        this.inMemoryStorage[key] = {
            value: value,
            timestamp: Date.now()
        };
        return true;
    }

    getInMemory(key, defaultValue = null) {
        const stored = this.inMemoryStorage[key];
        return stored ? stored.value : defaultValue;
    }

    removeInMemory(key) {
        delete this.inMemoryStorage[key];
        return true;
    }

    clearInMemory() {
        this.inMemoryStorage = {};
    }

    getInMemoryKeys(prefix = '') {
        return Object.keys(this.inMemoryStorage).filter(key => key.startsWith(prefix));
    }
}

// Authentication-specific storage functions
class AuthStorage extends StorageManager {
    /**
     * Set authentication token
     * @param {string} token - JWT token
     * @returns {boolean} Success status
     */
    setAuthToken(token) {
        return this.setItem(STORAGE_KEYS.AUTH_TOKEN, token);
    }

    /**
     * Get authentication token
     * @returns {string|null} JWT token or null
     */
    getAuthToken() {
        return this.getItem(STORAGE_KEYS.AUTH_TOKEN);
    }

    /**
     * Clear authentication token
     * @returns {boolean} Success status
     */
    clearAuthToken() {
        return this.removeItem(STORAGE_KEYS.AUTH_TOKEN);
    }

    /**
     * Set user session data
     * @param {Object} userData - User session data
     * @returns {boolean} Success status
     */
    setUserSession(userData) {
        const success = this.setItem(STORAGE_KEYS.USER_SESSION, userData);
        if (success) {
            this.setItem(STORAGE_KEYS.LOGIN_TIME, new Date().toISOString());
        }
        return success;
    }

    /**
     * Get user session data
     * @returns {Object|null} User session data or null
     */
    getUserSession() {
        return this.getItem(STORAGE_KEYS.USER_SESSION);
    }

    /**
     * Clear user session data
     * @returns {boolean} Success status
     */
    clearUserSession() {
        const results = [
            this.removeItem(STORAGE_KEYS.USER_SESSION),
            this.removeItem(STORAGE_KEYS.LOGIN_TIME)
        ];
        return results.every(result => result === true);
    }

    /**
     * Check if session is valid
     * @param {number} maxAgeMs - Maximum session age in milliseconds
     * @returns {boolean} Session validity
     */
    isSessionValid(maxAgeMs = 24 * 60 * 60 * 1000) { // 24 hours default
        try {
            const loginTime = this.getItem(STORAGE_KEYS.LOGIN_TIME);
            if (!loginTime) {
                return false;
            }

            const loginTimestamp = new Date(loginTime).getTime();
            const currentTime = Date.now();
            const sessionAge = currentTime - loginTimestamp;

            return sessionAge < maxAgeMs;
        } catch (error) {
            console.error('Failed to validate session:', error);
            return false;
        }
    }

    /**
     * Clear all authentication-related data
     * @returns {boolean} Success status
     */
    clearAuthData() {
        const results = [
            this.clearAuthToken(),
            this.clearUserSession()
        ];
        return results.every(result => result === true);
    }
}

// Task-specific storage functions
class TaskStorage extends StorageManager {
    /**
     * Save tasks array
     * @param {Array} tasks - Tasks array
     * @returns {boolean} Success status
     */
    saveTasks(tasks) {
        return this.setItem(STORAGE_KEYS.TASKS, tasks);
    }

    /**
     * Load tasks array
     * @returns {Array} Tasks array
     */
    loadTasks() {
        return this.getItem(STORAGE_KEYS.TASKS, []);
    }

    /**
     * Clear all tasks
     * @returns {boolean} Success status
     */
    clearTasks() {
        return this.removeItem(STORAGE_KEYS.TASKS);
    }
}

// Create storage instances
const storageManager = new StorageManager();
const authStorage = new AuthStorage();
const taskStorage = new TaskStorage();

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        StorageManager,
        AuthStorage,
        TaskStorage,
        storageManager,
        authStorage,
        taskStorage,
        STORAGE_KEYS
    };
} else {
    // Global access for browser
    window.storageManager = storageManager;
    window.authStorage = authStorage;
    window.taskStorage = taskStorage;
    window.STORAGE_KEYS = STORAGE_KEYS;
}