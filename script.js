// ======================================
// TASK MANAGER - JAVASCRIPT
// ======================================

// State Management
let tasks = [];
let currentFilter = 'all';
let currentUser = null;

// DOM Elements
const taskForm = document.getElementById('taskForm');
const taskInput = document.getElementById('taskInput');
const taskList = document.getElementById('taskList');
const emptyState = document.getElementById('emptyState');
const filterButtons = document.querySelectorAll('.filter-btn');
const clearCompletedBtn = document.getElementById('clearCompleted');
const emailCompletedBtn = document.getElementById('emailCompleted');
const countAll = document.getElementById('countAll');
const countActive = document.getElementById('countActive');
const countCompleted = document.getElementById('countCompleted');

// Initialize App
function init() {
    // Get current authenticated user
    currentUser = authManager.getCurrentUser();
    
    if (!currentUser) {
        console.error('No authenticated user found');
        return;
    }
    
    loadTasks();
    renderTasks();
    updateCounts();
    attachEventListeners();
}

// Event Listeners
function attachEventListeners() {
    taskForm.addEventListener('submit', handleAddTask);
    clearCompletedBtn.addEventListener('click', handleClearCompleted);
    
    if (emailCompletedBtn) {
        emailCompletedBtn.addEventListener('click', handleEmailCompleted);
    }
    
    filterButtons.forEach(btn => {
        btn.addEventListener('click', handleFilterChange);
    });
}

// Add Task
function handleAddTask(e) {
    e.preventDefault();
    
    const taskText = taskInput.value.trim();
    
    if (taskText === '') {
        taskInput.focus();
        return;
    }
    
    // Ensure user is still authenticated
    if (!authManager.isAuthenticated()) {
        authManager.redirectToLogin();
        return;
    }
    
    const newTask = {
        id: Date.now(),
        text: taskText,
        completed: false,
        createdAt: new Date().toISOString(),
        userId: currentUser.id // Associate task with current user
    };
    
    tasks.push(newTask);
    saveTasks();
    renderTasks();
    updateCounts();
    
    // Reset form
    taskInput.value = '';
    taskInput.focus();
    
    // Announce to screen readers
    announceToScreenReader(`Task "${taskText}" added successfully`);
}

// Delete Task
function handleDeleteTask(taskId) {
    const task = tasks.find(t => t.id === taskId);
    const taskText = task ? task.text : '';
    
    tasks = tasks.filter(task => task.id !== taskId);
    saveTasks();
    renderTasks();
    updateCounts();
    
    announceToScreenReader(`Task "${taskText}" deleted`);
}

// Toggle Task Completion
function handleToggleTask(taskId) {
    const task = tasks.find(task => task.id === taskId);
    
    if (task) {
        task.completed = !task.completed;
        saveTasks();
        renderTasks();
        updateCounts();
        
        const status = task.completed ? 'completed' : 'marked as active';
        announceToScreenReader(`Task "${task.text}" ${status}`);
    }
}

// Filter Tasks
function handleFilterChange(e) {
    const filterValue = e.target.dataset.filter;
    currentFilter = filterValue;
    
    // Update button states
    filterButtons.forEach(btn => {
        const isActive = btn.dataset.filter === filterValue;
        btn.classList.toggle('active', isActive);
        btn.setAttribute('aria-pressed', isActive);
    });
    
    renderTasks();
}

// Clear Completed Tasks
function handleClearCompleted() {
    const completedCount = tasks.filter(task => task.completed).length;
    
    if (completedCount === 0) {
        return;
    }
    
    tasks = tasks.filter(task => !task.completed);
    saveTasks();
    renderTasks();
    updateCounts();
    
    announceToScreenReader(`${completedCount} completed task${completedCount !== 1 ? 's' : ''} cleared`);
}

// Email Completed Tasks
function handleEmailCompleted() {
    const completedTasks = tasks.filter(task => task.completed);
    
    if (completedTasks.length === 0) {
        alert('No completed tasks to email!');
        return;
    }
    
    // Build email content
    const subject = encodeURIComponent(`Completed Tasks - ${new Date().toLocaleDateString()}`);
    
    let bodyText = `Here are my completed tasks from ${new Date().toLocaleDateString()}:\n\n`;
    completedTasks.forEach((task, index) => {
        const completedDate = task.completedAt 
            ? new Date(task.completedAt).toLocaleString() 
            : 'N/A';
        bodyText += `${index + 1}. ${task.text}\n`;
    });
    
    bodyText += `\n\nTotal completed: ${completedTasks.length} task${completedTasks.length !== 1 ? 's' : ''}`;
    
    const body = encodeURIComponent(bodyText);
    
    // Open default email client
    window.location.href = `mailto:?subject=${subject}&body=${body}`;
    
    announceToScreenReader(`Email prepared with ${completedTasks.length} completed task${completedTasks.length !== 1 ? 's' : ''}`);
}

// Render Tasks
function renderTasks() {
    const filteredTasks = getFilteredTasks();
    
    // Show/hide empty state
    if (tasks.length === 0) {
        emptyState.classList.remove('hidden');
        taskList.classList.add('hidden');
        clearCompletedBtn.disabled = true;
        return;
    }
    
    emptyState.classList.add('hidden');
    
    // Show/hide task list based on filtered results
    if (filteredTasks.length === 0) {
        taskList.classList.add('hidden');
        // Could show a "no tasks match filter" message here
    } else {
        taskList.classList.remove('hidden');
    }
    
    // Render task items
    taskList.innerHTML = filteredTasks.map(task => createTaskHTML(task)).join('');
    
    // Attach event listeners to task items
    attachTaskEventListeners();
    
    // Update clear button state
    const hasCompleted = tasks.some(task => task.completed);
    clearCompletedBtn.disabled = !hasCompleted;
    
    if (emailCompletedBtn) {
        emailCompletedBtn.disabled = !hasCompleted;
    }
}

// Create Task HTML
function createTaskHTML(task) {
    return `
        <li class="task-item ${task.completed ? 'completed' : ''}" data-task-id="${task.id}">
            <div class="task-checkbox">
                <input 
                    type="checkbox" 
                    id="task-${task.id}" 
                    ${task.completed ? 'checked' : ''}
                    aria-label="Mark task as ${task.completed ? 'incomplete' : 'complete'}"
                >
                <div class="checkbox-custom"></div>
            </div>
            <label for="task-${task.id}" class="task-text">${escapeHtml(task.text)}</label>
            <button 
                class="btn-delete" 
                data-task-id="${task.id}"
                aria-label="Delete task: ${escapeHtml(task.text)}"
            >
                DELETE
            </button>
        </li>
    `;
}

// Attach Event Listeners to Task Items
function attachTaskEventListeners() {
    // Checkbox listeners
    const checkboxes = taskList.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', (e) => {
            const taskId = parseInt(e.target.closest('.task-item').dataset.taskId);
            handleToggleTask(taskId);
        });
    });
    
    // Delete button listeners
    const deleteButtons = taskList.querySelectorAll('.btn-delete');
    deleteButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            const taskId = parseInt(e.target.dataset.taskId);
            handleDeleteTask(taskId);
        });
    });
}

// Get Filtered Tasks
function getFilteredTasks() {
    switch (currentFilter) {
        case 'active':
            return tasks.filter(task => !task.completed);
        case 'completed':
            return tasks.filter(task => task.completed);
        default:
            return tasks;
    }
}

// Update Counts
function updateCounts() {
    const activeCount = tasks.filter(task => !task.completed).length;
    const completedCount = tasks.filter(task => task.completed).length;
    
    countAll.textContent = tasks.length;
    countActive.textContent = activeCount;
    countCompleted.textContent = completedCount;
}

// Local Storage Functions
function saveTasks() {
    try {
        if (!currentUser) {
            console.error('Cannot save tasks: No authenticated user');
            return;
        }
        
        // Save tasks with user-specific key
        const userTaskKey = `tasks_${currentUser.id}`;
        localStorage.setItem(userTaskKey, JSON.stringify(tasks));
    } catch (error) {
        console.error('Error saving tasks to localStorage:', error);
    }
}

function loadTasks() {
    try {
        if (!currentUser) {
            console.error('Cannot load tasks: No authenticated user');
            tasks = [];
            return;
        }
        
        // Load tasks with user-specific key
        const userTaskKey = `tasks_${currentUser.id}`;
        const storedTasks = localStorage.getItem(userTaskKey);
        
        if (storedTasks) {
            tasks = JSON.parse(storedTasks);
            
            // Ensure all tasks belong to current user
            tasks = tasks.filter(task => !task.userId || task.userId === currentUser.id);
            
            // Add userId to legacy tasks that don't have it
            tasks = tasks.map(task => ({
                ...task,
                userId: task.userId || currentUser.id
            }));
            
            // Save the cleaned tasks
            saveTasks();
        } else {
            tasks = [];
        }
    } catch (error) {
        console.error('Error loading tasks from localStorage:', error);
        tasks = [];
    }
}

// Utility Functions
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Accessibility: Announce to screen readers
function announceToScreenReader(message) {
    const announcement = document.createElement('div');
    announcement.setAttribute('role', 'status');
    announcement.setAttribute('aria-live', 'polite');
    announcement.className = 'visually-hidden';
    announcement.textContent = message;
    
    document.body.appendChild(announcement);
    
    setTimeout(() => {
        document.body.removeChild(announcement);
    }, 1000);
}

// Keyboard Shortcuts (Optional Enhancement)
document.addEventListener('keydown', (e) => {
    // Focus task input with Ctrl/Cmd + K
    if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault();
        taskInput.focus();
        taskInput.select();
    }
});

// Initialize the app when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        // Wait for authentication to be initialized
        if (window.authManager) {
            init();
        } else {
            // Wait a bit for auth manager to load
            setTimeout(init, 100);
        }
    });
} else {
    // DOM already loaded
    if (window.authManager) {
        init();
    } else {
        setTimeout(init, 100);
    }
}

// Export functions for testing (if needed)
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        handleAddTask,
        handleDeleteTask,
        handleToggleTask,
        getFilteredTasks
    };
}
