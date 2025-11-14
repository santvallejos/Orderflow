# AI Assistant Instructions - OrderFlow Project

## Project Overview

**Project Name:** Orderflow 
**Type:** Multi-tenant Restaurant Ordering Platform  
**Team Size:** 5 developers  

### Mission
Build a scalable, customizable ordering platform that allows restaurants to have their own branded ordering system without paying commissions to third-party delivery apps. Think "Shopify for restaurants" - same core functionality, but each restaurant gets their own branded experience.

### Core Philosophy
- **Multi-tenancy first**: Every feature must consider multiple restaurants using the same platform
- **Customization over uniformity**: Each restaurant should feel unique to their customers
- **Scalability**: Code for 100 restaurants, not just 1
- **Commercial viability**: This should be production-ready, not just a school project

---

## Technology Stack

### Backend
- **.NET 9** (Web API)
- **Entity Framework Core** (ORM)
- **PostgreSQL** or **SQL Server** (Database)
- **Clean Architecture** pattern
- **JWT** for authentication
- **SignalR** for real-time updates

### Frontend
- **React 18+** with TypeScript
- **Vite** as build tool
- **TailwindCSS** for styling
- **React Query** for data fetching
- **Zustand** or **Context API** for state management
- **React Router** for navigation

### Infrastructure
- **Git** for version control
- **GitHub Actions** for CI/CD
- **Docker** for containerization
- Cloud storage for images (AWS S3)

---

## Architecture Principles

### Backend Architecture - Clean Architecture

```
Core Layer (Domain)
├── Entities (Restaurant, MenuItem, Order, Customer, Theme)
├── Interfaces (Repositories, Services)
└── Value Objects

Application Layer
├── DTOs (Data Transfer Objects)
├── Services (Business logic)
├── Validators (FluentValidation)
└── Mappings (AutoMapper)

Infrastructure Layer
├── Data (EF Core, DbContext)
├── Repositories (Data access)
├── External Services (Payment, Notifications, Storage)
└── Identity (Authentication/Authorization)

API Layer
├── Controllers (Endpoints)
├── Middleware (Tenant resolution, error handling)
├── Filters
└── Configuration
```

### Frontend Architecture - Feature-Based

```
src/
├── features/           # Feature modules
│   ├── menu/
│   ├── orders/
│   ├── theme-customization/
│   └── auth/
├── components/         # Shared components
├── hooks/             # Custom hooks
├── services/          # API calls
├── store/             # State management
├── types/             # TypeScript types
└── utils/             # Helper functions
```

---

## Multi-Tenancy Implementation

### Strategy: Single Database with Tenant Isolation

**Every entity must have a `RestaurantId`:**

```csharp
public abstract class TenantEntity
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
}

public class MenuItem : TenantEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    // ... other properties
}
```

**Automatic tenant filtering:**

```csharp
// Global query filter in DbContext
modelBuilder.Entity<MenuItem>()
    .HasQueryFilter(m => m.RestaurantId == _currentTenantId);
```

**Tenant Resolution:**
- Admin Panel: Via JWT claim (RestaurantId)
- Customer App: Via subdomain or custom domain
  - `restaurant-slug.orderflow.com`
  - `www.pizzeriaelpaso.com` (future)

---

## Coding Standards

### C# / .NET Backend

#### Naming Conventions
```csharp
// Classes: PascalCase
public class OrderService { }

// Interfaces: I + PascalCase
public interface IOrderRepository { }

// Methods: PascalCase
public async Task<Order> CreateOrderAsync(CreateOrderDto dto) { }

// Private fields: _camelCase
private readonly IOrderRepository _orderRepository;

// Parameters: camelCase
public void ProcessOrder(Guid orderId, bool sendNotification) { }

// Constants: PascalCase or UPPER_SNAKE_CASE
public const int MaxItemsPerOrder = 50;
```

#### Async/Await
- **ALWAYS** use async/await for I/O operations
- **ALWAYS** suffix async methods with `Async`
- **NEVER** use `.Result` or `.Wait()`

```csharp
// ✅ GOOD
public async Task<Order> GetOrderAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ❌ BAD
public Order GetOrder(Guid id)
{
    return _repository.GetByIdAsync(id).Result; // NEVER DO THIS
}
```

#### SOLID Principles
- **Single Responsibility**: One class, one purpose
- **Dependency Injection**: Always inject dependencies via constructor
- **Interface Segregation**: Small, focused interfaces

```csharp
// ✅ GOOD - Injected dependencies
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly INotificationService _notificationService;
    
    public OrderService(
        IOrderRepository orderRepository,
        INotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _notificationService = notificationService;
    }
}
```

#### Error Handling
```csharp
// Use custom exceptions
public class RestaurantNotFoundException : Exception
{
    public RestaurantNotFoundException(Guid restaurantId) 
        : base($"Restaurant with ID {restaurantId} not found") { }
}

// Global exception handling in middleware
public class ErrorHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (RestaurantNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}
```

#### DTOs and Validation
```csharp
// Always use DTOs for API inputs/outputs
public record CreateMenuItemDto(
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    bool IsAvailable
);

// FluentValidation
public class CreateMenuItemDtoValidator : AbstractValidator<CreateMenuItemDto>
{
    public CreateMenuItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

### TypeScript / React Frontend

#### Naming Conventions
```typescript
// Components: PascalCase
export const MenuCard: React.FC<MenuCardProps> = ({ item }) => { }

// Hooks: camelCase with 'use' prefix
export const useRestaurantTheme = () => { }

// Types/Interfaces: PascalCase
interface MenuItem {
  id: string;
  name: string;
  price: number;
}

// Constants: UPPER_SNAKE_CASE
const API_BASE_URL = import.meta.env.VITE_API_URL;

// Functions: camelCase
const calculateOrderTotal = (items: OrderItem[]) => { }
```

#### Component Structure
```typescript
// ✅ GOOD - Functional component with TypeScript
interface MenuCardProps {
  item: MenuItem;
  onAddToCart: (item: MenuItem) => void;
}

export const MenuCard: React.FC<MenuCardProps> = ({ item, onAddToCart }) => {
  // Hooks first
  const [quantity, setQuantity] = useState(1);
  const theme = useRestaurantTheme();
  
  // Event handlers
  const handleAddToCart = () => {
    onAddToCart({ ...item, quantity });
  };
  
  // Early returns
  if (!item) return null;
  
  // JSX
  return (
    <div className="menu-card" style={{ borderColor: theme.primaryColor }}>
      <h3>{item.name}</h3>
      <p>{item.description}</p>
      <button onClick={handleAddToCart}>Add to Cart</button>
    </div>
  );
};
```

#### Custom Hooks Pattern
```typescript
// ✅ GOOD - Extract logic into custom hooks
export const useMenuItems = (restaurantId: string) => {
  return useQuery({
    queryKey: ['menu-items', restaurantId],
    queryFn: () => menuService.getItems(restaurantId),
  });
};

// Usage in component
const { data: menuItems, isLoading } = useMenuItems(restaurantId);
```

#### State Management Rules
- **Local state**: `useState` for component-specific state
- **Server state**: React Query for API data
- **Global state**: Context API or Zustand for theme, auth, cart

```typescript
// ✅ GOOD - React Query for server state
const { data: orders } = useQuery({
  queryKey: ['orders', restaurantId],
  queryFn: () => orderService.getOrders(restaurantId),
  refetchInterval: 30000, // Refetch every 30s
});

// ✅ GOOD - Zustand for cart
interface CartStore {
  items: CartItem[];
  addItem: (item: MenuItem) => void;
  removeItem: (itemId: string) => void;
  clearCart: () => void;
}

export const useCart = create<CartStore>((set) => ({
  items: [],
  addItem: (item) => set((state) => ({ 
    items: [...state.items, item] 
  })),
  // ...
}));
```

#### API Service Pattern
```typescript
// services/orderService.ts
class OrderService {
  private baseUrl = '/api/orders';
  
  async createOrder(data: CreateOrderDto): Promise<Order> {
    const response = await fetch(this.baseUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`,
      },
      body: JSON.stringify(data),
    });
    
    if (!response.ok) {
      throw new Error('Failed to create order');
    }
    
    return response.json();
  }
  
  // ... other methods
}

export const orderService = new OrderService();
```

---

## Theme Customization System

### Backend - Theme Entity
```csharp
public class Theme : TenantEntity
{
    public string PrimaryColor { get; set; } = "#2563eb"; // Tailwind blue-600
    public string SecondaryColor { get; set; } = "#1e40af";
    public string AccentColor { get; set; } = "#f59e0b";
    public string FontFamily { get; set; } = "Inter";
    public string LogoUrl { get; set; }
    public string? CustomCss { get; set; } // Advanced customization
    public bool DarkMode { get; set; }
}
```

### Frontend - Theme Provider
```typescript
interface RestaurantTheme {
  colors: {
    primary: string;
    secondary: string;
    accent: string;
  };
  fonts: {
    heading: string;
    body: string;
  };
  logo: string;
  darkMode: boolean;
}

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { data: theme } = useRestaurantTheme();
  
  useEffect(() => {
    if (theme) {
      // Apply CSS variables
      document.documentElement.style.setProperty('--color-primary', theme.colors.primary);
      document.documentElement.style.setProperty('--color-secondary', theme.colors.secondary);
      // ...
    }
  }, [theme]);
  
  return (
    <ThemeContext.Provider value={theme}>
      {children}
    </ThemeContext.Provider>
  );
};
```

---

## Database Guidelines

### Entity Relationships
```csharp
// One Restaurant has many MenuItems
public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; } // URL-friendly name
    public ICollection<MenuItem> MenuItems { get; set; }
    public ICollection<Order> Orders { get; set; }
    public Theme Theme { get; set; }
}

// One Order has many OrderItems (many-to-many with MenuItem)
public class Order : TenantEntity
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<OrderItem> Items { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### Migrations
- **ALWAYS** create migrations with descriptive names
- **NEVER** manually edit the database
- **ALWAYS** review migrations before applying

```bash
# Good migration names
dotnet ef migrations add AddThemeCustomization
dotnet ef migrations add AddOrderStatusEnum
dotnet ef migrations add AddRestaurantSlug
```

### Indexes
```csharp
// ALWAYS index foreign keys and frequently queried fields
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<MenuItem>()
        .HasIndex(m => m.RestaurantId);
    
    modelBuilder.Entity<Order>()
        .HasIndex(o => new { o.RestaurantId, o.CreatedAt });
    
    modelBuilder.Entity<Restaurant>()
        .HasIndex(r => r.Slug)
        .IsUnique();
}
```

---

## Testing Requirements

### Backend Tests
```csharp
// Unit Tests - xUnit
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly OrderService _service;
    
    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _service = new OrderService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task CreateOrder_ValidData_ReturnsOrder()
    {
        // Arrange
        var dto = new CreateOrderDto { /* ... */ };
        
        // Act
        var result = await _service.CreateOrderAsync(dto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Pending, result.Status);
    }
}
```

### Frontend Tests
```typescript
// Component tests - Vitest + React Testing Library
import { render, screen } from '@testing-library/react';
import { MenuCard } from './MenuCard';

describe('MenuCard', () => {
  it('renders menu item details', () => {
    const item = {
      id: '1',
      name: 'Pizza Margherita',
      price: 12.99,
    };
    
    render(<MenuCard item={item} onAddToCart={() => {}} />);
    
    expect(screen.getByText('Pizza Margherita')).toBeInTheDocument();
    expect(screen.getByText('$12.99')).toBeInTheDocument();
  });
});
```

---

## Git Workflow

### Branch Naming
```
feature/menu-crud
fix/order-status-bug
refactor/theme-system
docs/api-documentation
```

### Commit Messages (Conventional Commits)
```
feat: add menu item CRUD endpoints
fix: resolve theme not loading on customer app
refactor: improve order service performance
docs: update API documentation
test: add unit tests for OrderService
chore: update dependencies
```

### Pull Request Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Checklist
- [ ] Code follows project conventions
- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No console.logs or debugger statements
- [ ] Tested on local environment

## Related Issues
Closes #123
```

---

## Security Requirements

### Authentication & Authorization
```csharp
// ALWAYS check tenant ownership
public async Task<MenuItem> GetMenuItemAsync(Guid id, Guid restaurantId)
{
    var item = await _repository.GetByIdAsync(id);
    
    if (item.RestaurantId != restaurantId)
        throw new UnauthorizedException("Access denied");
    
    return item;
}
```

### Input Validation
- **ALWAYS** validate user inputs
- **ALWAYS** sanitize HTML content
- **NEVER** trust client-side data

```typescript
// Frontend validation
const schema = z.object({
  name: z.string().min(1).max(100),
  price: z.number().positive(),
  description: z.string().max(500),
});

const result = schema.safeParse(formData);
if (!result.success) {
  // Handle errors
}
```

---

## Performance Guidelines

### Backend
- Use **pagination** for list endpoints (default: 20 items)
- Use **eager loading** to avoid N+1 queries
- Implement **caching** for frequently accessed data

```csharp
// ✅ GOOD - Eager loading
public async Task<Order> GetOrderWithItemsAsync(Guid id)
{
    return await _context.Orders
        .Include(o => o.Items)
        .ThenInclude(i => i.MenuItem)
        .FirstOrDefaultAsync(o => o.Id == id);
}

// ❌ BAD - N+1 queries
public async Task<Order> GetOrderAsync(Guid id)
{
    var order = await _context.Orders.FindAsync(id);
    // This will trigger additional queries for each item
    foreach (var item in order.Items)
    {
        var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
    }
    return order;
}
```

### Frontend
- Use **React.memo** for expensive components
- Implement **virtual scrolling** for long lists
- Use **image optimization** (WebP, lazy loading)
- Debounce search inputs

```typescript
// Debounced search
const [searchTerm, setSearchTerm] = useState('');
const debouncedSearch = useDebouncedValue(searchTerm, 500);

useEffect(() => {
  if (debouncedSearch) {
    // Perform search
  }
}, [debouncedSearch]);
```

---

## DO NOT Do These Things

### ❌ Backend
- **NEVER** expose entity IDs in URLs without validation
- **NEVER** return entities directly from controllers (use DTOs)
- **NEVER** use `string` for IDs (use `Guid`)
- **NEVER** catch exceptions without logging them
- **NEVER** use `dynamic` types
- **NEVER** hardcode connection strings or API keys

### ❌ Frontend
- **NEVER** store sensitive data in localStorage
- **NEVER** use `any` type in TypeScript
- **NEVER** fetch data in `useEffect` (use React Query)
- **NEVER** mutate state directly
- **NEVER** use inline styles (use Tailwind classes)
- **NEVER** commit `console.log` statements

### ❌ General
- **NEVER** commit directly to `main` branch
- **NEVER** push `.env` files
- **NEVER** leave TODOs without creating a task in Trello
- **NEVER** merge PRs without code review
- **NEVER** deploy without testing

---

## AI Assistant Specific Instructions

When generating code:

1. **ALWAYS consider multi-tenancy** - Add `RestaurantId` checks
2. **ALWAYS use TypeScript** - No `any` types
3. **ALWAYS handle errors** - Try/catch blocks and proper error messages
4. **ALWAYS add comments** - Explain complex logic
5. **ALWAYS suggest tests** - Offer to write unit tests
6. **ALWAYS follow naming conventions** - As specified above
7. **ALWAYS validate inputs** - Both frontend and backend
8. **ALWAYS think about scalability** - Code for 1000 restaurants, not 1

When I ask you to:
- **"Create a feature"** → Provide both backend and frontend code
- **"Add tests"** → Include unit and integration tests
- **"Optimize this"** → Explain what you're optimizing and why
- **"Review this code"** → Point out issues, suggest improvements
- **"Generate API endpoint"** → Include controller, service, DTO, and validation

### Code Generation Format

When generating backend code:
```csharp
// File: RestaurantPlatform.API/Controllers/MenuController.cs
// Purpose: CRUD operations for menu items
// Dependencies: IMenuService, IMapper

[ApiController]
[Route("api/restaurants/{restaurantId}/menu")]
public class MenuController : ControllerBase
{
    // ... implementation
}
```

When generating frontend code:
```typescript
// File: src/features/menu/components/MenuCard.tsx
// Purpose: Display individual menu item
// Dependencies: useRestaurantTheme, useCart

export const MenuCard: React.FC<MenuCardProps> = ({ item }) => {
    // ... implementation
};
```

---

## Quick Reference

### Common Commands
```bash
# Backend
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet test
dotnet run --project RestaurantPlatform.API

# Frontend
npm run dev
npm run build
npm test
npm run lint
```

### Useful Extensions
- Backend: C# Dev Kit, NuGet Package Manager
- Frontend: ES7+ React/Redux snippets, Tailwind CSS IntelliSense
- General: GitLens, Better Comments, Error Lens

---

## Project-Specific Business Rules

1. **Orders** cannot be modified after status is `Preparing`
2. **Menu items** can be marked unavailable but never deleted (soft delete)
3. **Restaurants** must have a unique slug for subdomain
4. **Themes** must have valid hex color codes
5. **Prices** are stored as `decimal` with 2 decimal places
6. **Orders** must have at least 1 item
7. **Customers** can have multiple addresses
8. **Payment** must be confirmed before order status changes to `Confirmed`

---

**Last Updated:** November 2024  
**Project Phase:** Sprint 1 - MVP Development  
**Team:** 5 developers (Bootcamp Final Project)