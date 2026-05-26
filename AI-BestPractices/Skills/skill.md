---
name: react-component-development
description: Build React components with TypeScript using modern patterns and best practices. Use when creating React components, working with props and state, implementing hooks, or when the user asks about React component structure, TypeScript typing, or component patterns.
---

# React Component Development with TypeScript

## Component Structure

Use functional components with TypeScript. Follow this structure:

```typescript
import { FC, useState, useEffect } from 'react';

interface ComponentNameProps {
  // Props definition
}

export const ComponentName: FC<ComponentNameProps> = ({ prop1, prop2 }) => {
  // Hooks
  // Event handlers
  // Render logic
  
  return (
    // JSX
  );
};
```

## TypeScript Typing Patterns

### Props Interface

```typescript
// Basic props
interface ButtonProps {
  label: string;
  onClick: () => void;
  disabled?: boolean;
  variant?: 'primary' | 'secondary';
}

// Props with children
interface CardProps {
  title: string;
  children: React.ReactNode;
}

// Props extending HTML attributes
interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
}
```

### Event Handlers

```typescript
// Form events
const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
  e.preventDefault();
};

const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  setValue(e.target.value);
};

// Click events
const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
  console.log('Clicked');
};
```

### State Typing

```typescript
// Primitive types
const [count, setCount] = useState<number>(0);
const [name, setName] = useState<string>('');

// Complex types
interface User {
  id: string;
  name: string;
  email: string;
}

const [user, setUser] = useState<User | null>(null);
const [users, setUsers] = useState<User[]>([]);

// With initial undefined
const [data, setData] = useState<User>();
```

## Component Patterns

### Container/Presentational Pattern

**Presentational Component** (UI only):
```typescript
interface UserCardProps {
  name: string;
  email: string;
  avatar: string;
  onEdit: () => void;
}

export const UserCard: FC<UserCardProps> = ({ name, email, avatar, onEdit }) => (
  <div className="user-card">
    <img src={avatar} alt={name} />
    <h3>{name}</h3>
    <p>{email}</p>
    <button onClick={onEdit}>Edit</button>
  </div>
);
```

**Container Component** (logic):
```typescript
export const UserCardContainer: FC<{ userId: string }> = ({ userId }) => {
  const [user, setUser] = useState<User | null>(null);
  
  useEffect(() => {
    fetchUser(userId).then(setUser);
  }, [userId]);
  
  const handleEdit = () => {
    // Edit logic
  };
  
  if (!user) return <Spinner />;
  
  return <UserCard {...user} onEdit={handleEdit} />;
};
```

### Compound Components

```typescript
interface TabsProps {
  children: React.ReactNode;
  defaultTab?: string;
}

interface TabProps {
  id: string;
  label: string;
  children: React.ReactNode;
}

export const Tabs: FC<TabsProps> & { Tab: FC<TabProps> } = ({ children, defaultTab }) => {
  const [activeTab, setActiveTab] = useState(defaultTab);
  // Implementation
};

Tabs.Tab = ({ id, label, children }) => {
  // Implementation
};

// Usage:
// <Tabs defaultTab="profile">
//   <Tabs.Tab id="profile" label="Profile">...</Tabs.Tab>
//   <Tabs.Tab id="settings" label="Settings">...</Tabs.Tab>
// </Tabs>
```

### Render Props Pattern

```typescript
interface DataFetcherProps<T> {
  url: string;
  children: (data: T | null, loading: boolean, error: Error | null) => React.ReactNode;
}

function DataFetcher<T>({ url, children }: DataFetcherProps<T>) {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  
  useEffect(() => {
    fetch(url)
      .then(res => res.json())
      .then(setData)
      .catch(setError)
      .finally(() => setLoading(false));
  }, [url]);
  
  return <>{children(data, loading, error)}</>;
}
```

### Higher-Order Component (HOC)

```typescript
function withAuth<P extends object>(Component: React.ComponentType<P>) {
  return (props: P) => {
    const { user, loading } = useAuth();
    
    if (loading) return <Spinner />;
    if (!user) return <Redirect to="/login" />;
    
    return <Component {...props} />;
  };
}

// Usage:
const ProtectedPage = withAuth(MyComponent);
```

## Custom Hooks

Extract reusable logic into custom hooks:

```typescript
// Form handling
function useForm<T>(initialValues: T) {
  const [values, setValues] = useState<T>(initialValues);
  
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setValues({
      ...values,
      [e.target.name]: e.target.value,
    });
  };
  
  const reset = () => setValues(initialValues);
  
  return { values, handleChange, reset };
}

// API data fetching
function useApi<T>(url: string) {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  
  useEffect(() => {
    fetch(url)
      .then(res => res.json())
      .then(setData)
      .catch(setError)
      .finally(() => setLoading(false));
  }, [url]);
  
  return { data, loading, error };
}

// Toggle state
function useToggle(initialValue = false): [boolean, () => void] {
  const [value, setValue] = useState(initialValue);
  const toggle = () => setValue(v => !v);
  return [value, toggle];
}
```

## State Management

### Local State (useState)

Use for component-specific state:
```typescript
const [isOpen, setIsOpen] = useState(false);
const [formData, setFormData] = useState({ name: '', email: '' });
```

### Refs (useRef)

For DOM access and mutable values:
```typescript
const inputRef = useRef<HTMLInputElement>(null);

useEffect(() => {
  inputRef.current?.focus();
}, []);

return <input ref={inputRef} />;
```

### Context API

For shared state across components:
```typescript
interface ThemeContextValue {
  theme: 'light' | 'dark';
  toggleTheme: () => void;
}

const ThemeContext = createContext<ThemeContextValue | undefined>(undefined);

export const ThemeProvider: FC<{ children: React.ReactNode }> = ({ children }) => {
  const [theme, setTheme] = useState<'light' | 'dark'>('light');
  
  const toggleTheme = () => {
    setTheme(t => t === 'light' ? 'dark' : 'light');
  };
  
  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context) throw new Error('useTheme must be used within ThemeProvider');
  return context;
};
```

## Performance Optimization

### Memoization

```typescript
// Memoize expensive calculations
const expensiveValue = useMemo(() => {
  return computeExpensiveValue(data);
}, [data]);

// Memoize callbacks
const handleClick = useCallback(() => {
  doSomething(id);
}, [id]);

// Memoize components
const MemoizedComponent = memo(MyComponent);

// With custom comparison
const MemoizedComponent = memo(MyComponent, (prevProps, nextProps) => {
  return prevProps.id === nextProps.id;
});
```

### Code Splitting

```typescript
import { lazy, Suspense } from 'react';

const HeavyComponent = lazy(() => import('./HeavyComponent'));

function App() {
  return (
    <Suspense fallback={<Spinner />}>
      <HeavyComponent />
    </Suspense>
  );
}
```

## Best Practices

### Component Organization

```
components/
├── Button/
│   ├── Button.tsx
│   ├── Button.test.tsx
│   ├── Button.module.css
│   └── index.ts
└── Card/
    ├── Card.tsx
    └── index.ts
```

### Naming Conventions

- **Components**: PascalCase (`UserProfile`, `NavigationBar`)
- **Props interfaces**: ComponentName + "Props" (`UserProfileProps`)
- **Event handlers**: "handle" + Event (`handleClick`, `handleSubmit`)
- **Custom hooks**: "use" + Name (`useAuth`, `useLocalStorage`)
- **Boolean props**: "is", "has", "should" (`isOpen`, `hasError`, `shouldRender`)

### Props Best Practices

```typescript
// Destructure props
const Component: FC<Props> = ({ name, age, onSave }) => {
  // Good
};

// Use default values
const Component: FC<Props> = ({ variant = 'primary', size = 'medium' }) => {
  // Implementation
};

// Spread remaining props
interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
}

const Button: FC<ButtonProps> = ({ variant = 'primary', children, ...rest }) => {
  return (
    <button className={`btn btn-${variant}`} {...rest}>
      {children}
    </button>
  );
};
```

### Conditional Rendering

```typescript
// Ternary for if-else
{isLoading ? <Spinner /> : <Content />}

// Logical AND for if-only
{error && <ErrorMessage error={error} />}

// Nullish coalescing for defaults
{title ?? 'Default Title'}

// Early returns for complex conditions
if (loading) return <Spinner />;
if (error) return <Error error={error} />;
return <Content data={data} />;
```

### Lists and Keys

```typescript
// Use stable, unique keys
{users.map(user => (
  <UserCard key={user.id} user={user} />
))}

// Avoid index as key (only if list is static and never reordered)
{items.map((item, index) => (
  <Item key={index} item={item} />  // Only if list never changes
))}
```

## Common Pitfalls to Avoid

1. **Don't mutate state directly**: Use setter functions
2. **Don't forget dependency arrays**: In useEffect, useCallback, useMemo
3. **Don't use index as key**: For dynamic lists
4. **Don't define components inside components**: Causes re-creation on every render
5. **Don't call hooks conditionally**: Hooks must be at top level
6. **Don't forget to cleanup effects**: Return cleanup function from useEffect
7. **Don't over-optimize**: Only memoize when there's a performance issue

## Quick Reference Checklist

When creating a component:
- [ ] Use TypeScript with proper prop types
- [ ] Destructure props in function signature
- [ ] Use meaningful prop names (boolean props: is/has/should)
- [ ] Extract reusable logic into custom hooks
- [ ] Handle loading and error states
- [ ] Add keys to list items
- [ ] Consider accessibility (aria labels, keyboard navigation)
- [ ] Memoize only when necessary
- [ ] Keep components focused and single-purpose
- [ ] Export from index.ts for clean imports
