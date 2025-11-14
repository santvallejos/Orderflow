# AI Assistant Instructions - OrderFlow Project

## Project Overview

**Project Name:** OrderFlow  
**Type:** Multi-tenant Restaurant Ordering Platform  
**Team Size:** 5 developers  
**Context:** Bootcamp final project with commercial viability focus

### Mission
Build a "Shopify for restaurants" - scalable platform where each restaurant gets their own branded ordering system without commission fees. Same core functionality, but each restaurant's customer app feels unique to them.

### Core Philosophy
- **Multi-tenancy first**: Every feature must support multiple restaurants
- **Customization**: Each restaurant should have unique branding/design
- **Scalability**: Design for 100+ restaurants, not just 1
- **Production-ready**: Commercial quality, not just academic

---

## Technology Stack

### Backend
- **.NET 9** - Web API with Clean Architecture
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **JWT** - Authentication
- **SignalR** - Real-time updates

### Frontend
- **Next.js +16+** - React framework with App Router
- **TypeScript** - Strict mode required
- **TailwindCSS** - Styling
- **shadcn/ui** - Component library
- **TanStack Query** - Server state management
- **Zustand** - Client state (cart, UI)

### Infrastructure
- **Turborepo** (optional) - Monorepo management
- **pnpm** - Package manager
- **Docker** - Containerization
- **GitHub Actions** - CI/CD
- **Vercel** - Frontend hosting
- **AWS S3** - Image storage

---

## Project Structure

```
orderflow/
├── backend/
│   └── src/
│       ├── OrderFlow.API/
│       ├── OrderFlow.Core/          # Domain entities, interfaces
│       ├── OrderFlow.Application/   # Business logic, DTOs
│       └── OrderFlow.Infrastructure/# Data access, external services
│
├── apps/
│   ├── admin/                       # Restaurant dashboard
│   │   └── src/
│   │       ├── app/
│   │       │   ├── (auth)/         # Login, register
│   │       │   ├── (dashboard)/    # Menu, orders, analytics, theme
│   │       │   └── actions/        # Server Actions
│   │       ├── components/
│   │       ├── lib/
│   │       └── types/
│   │
│   └── customer/                    # Customer ordering app
│       └── src/
│           ├── app/
│           │   ├── [restaurantSlug]/  # Dynamic per-restaurant
│           │   │   ├── page.tsx       # Menu
│           │   │   ├── cart/
│           │   │   ├── checkout/
│           │   │   └── layout.tsx     # Theme wrapper
│           │   └── actions/
│           ├── components/
│           ├── store/              # Zustand stores
│           └── lib/
│
└── packages/                       # Shared code (optional)
    ├── types/
    └── ui/
```

---

## Architecture Principles

### Backend - Clean Architecture

**Layers:**
1. **Core** - Domain entities, interfaces, value objects
2. **Application** - Business logic, DTOs, services, validators
3. **Infrastructure** - Database, external APIs, file storage
4. **API** - Controllers, middleware, authentication

**Key Rules:**
- Dependencies point inward (API → Application → Core)
- Core has no dependencies on other layers
- Use dependency injection everywhere
- All entities inherit from `TenantEntity` (includes `RestaurantId`)

### Frontend - Next.js App Router

**Component Strategy:**
- **Default to Server Components** - Fetch data directly, no client bundle
- **Client Components only when needed** - Interactivity, browser APIs, state
- **Server Actions** - For mutations (create, update, delete)
- **React Query** - For real-time data in client components

**Key Patterns:**
- Dynamic routes for multi-tenancy: `[restaurantSlug]`
- Route groups for layout organization: `(auth)`, `(dashboard)`
- ISR (Incremental Static Regeneration) for performance
- Theme customization via CSS variables

---

## Multi-Tenancy Implementation

### Core Concept
Every restaurant is a "tenant". All data is isolated by `RestaurantId`.

### Backend
```csharp
// Base entity with tenant isolation
public abstract class TenantEntity
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }  // CRITICAL: Always required
}

// Automatic filtering in DbContext
modelBuilder.Entity<MenuItem>()
    .HasQueryFilter(m => m.RestaurantId == _currentTenantId);
```

### Frontend
**Admin Panel:** Identify restaurant via JWT token claim  
**Customer App:** Identify restaurant via URL slug (`/pizzeria-el-paso`)

```typescript
// apps/customer/src/app/[restaurantSlug]/layout.tsx
export default async function RestaurantLayout({ 
  params 
}: { 
  params: Promise<{ restaurantSlug: string }> 
}) {
  const { restaurantSlug } = await params;
  const restaurant = await getRestaurantBySlug(restaurantSlug);
  
  return (
    <ThemeProvider theme={restaurant.theme}>
      {children}
    </ThemeProvider>
  );
}
```

---

## Coding Standards

### General Rules

**Backend (.NET):**
- PascalCase for classes, methods, properties
- _camelCase for private fields
- Always use `async`/`await`, never `.Result` or `.Wait()`
- Always use DTOs for API input/output (never expose entities)
- Always use `Guid` for IDs (never `string` or `int`)
- Always inject dependencies via constructor

**Frontend (TypeScript):**
- PascalCase for components, types, interfaces
- camelCase for functions, variables, hooks
- UPPER_SNAKE_CASE for constants
- Always type everything (no `any`)
- Always use Server Components unless interactivity needed
- Always use Server Actions for mutations

### Critical Next.js Patterns

**1. Server Components (Default)**
```typescript
// No 'use client' directive
export default async function MenuPage({ params }) {
  const data = await fetchMenuItems(); // Direct fetch
  return <MenuGrid items={data} />;
}
```

**2. Client Components (Only when needed)**
```typescript
'use client'; // Required for: useState, useEffect, event handlers, browser APIs

export function AddToCartButton({ item }) {
  const [loading, setLoading] = useState(false);
  const handleClick = () => { /* ... */ };
  return <button onClick={handleClick}>Add</button>;
}
```

**3. Server Actions (For mutations)**
```typescript
// app/actions/menu.ts
'use server';
import { revalidatePath } from 'next/cache';

export async function createMenuItem(formData: FormData) {
  // Validate, call API, revalidate cache
  await apiClient.post('/menu-items', data);
  revalidatePath('/menu');
}
```

**4. Data Fetching**
- **Server Components**: Direct `fetch` with caching
- **Client Components**: React Query for real-time updates
- **ISR**: `export const revalidate = 60;` for automatic cache refresh

---

## Theme Customization System

### Backend
```csharp
public class Theme : TenantEntity
{
    public string PrimaryColor { get; set; }    // Hex color
    public string SecondaryColor { get; set; }
    public string FontHeading { get; set; }
    public string LogoUrl { get; set; }
    public string? CustomCss { get; set; }      // Advanced
}
```

### Frontend
Dynamic CSS variables applied per restaurant:
```typescript
// ThemeProvider sets CSS vars based on restaurant theme
root.style.setProperty('--primary', theme.primaryColor);
```

Tailwind uses these variables:
```css
:root { --primary: 219 100% 50%; }
```
```typescript
// Tailwind config
colors: { primary: 'hsl(var(--primary))' }
```

---

## State Management

**Server State (API data):**
- Server Components: Direct fetch
- Client Components: React Query

**Client State (UI, cart):**
- Zustand with persist middleware
- Example: Shopping cart stored in localStorage

```typescript
// store/cart.ts
export const useCart = create<CartStore>()(
  persist(
    (set) => ({
      items: [],
      addItem: (item) => set((state) => ({ items: [...state.items, item] })),
      clearCart: () => set({ items: [] }),
    }),
    { name: 'cart-storage' }
  )
);
```

---

## Database Guidelines

### Entity Design
- All entities extend `TenantEntity` (includes `RestaurantId`)
- Use `Guid` for all IDs
- Index all foreign keys and `RestaurantId`
- Use enums for status fields (`OrderStatus`, etc.)

### Relationships
```csharp
Restaurant 1 → Many MenuItems
Restaurant 1 → Many Orders
Order 1 → Many OrderItems
OrderItem Many → 1 MenuItem
```

### Migrations
```bash
dotnet ef migrations add AddFeatureName
dotnet ef database update
```
Always review migrations before applying.

---

## Security & Validation

### Authentication
- JWT tokens with restaurant ID in claims
- Verify tenant ownership in every API call:
```csharp
if (entity.RestaurantId != currentRestaurantId)
    throw new UnauthorizedException();
```

### Validation
**Backend:** FluentValidation  
**Frontend:** Zod schemas

Always validate:
- User inputs (both client and server)
- File uploads (size, type)
- Price ranges, quantity limits
- Tenant ownership

---

## Performance Guidelines

### Backend
- Use pagination (default 20 items)
- Use eager loading with `.Include()` to avoid N+1 queries
- Cache frequently accessed data (menus, restaurant info)

### Frontend
- Use ISR for static-like pages (menus)
- Lazy load images with Next.js `<Image>`
- Use React.memo for expensive components
- Debounce search inputs (500ms)

---

## Testing Requirements

### Backend
```csharp
// xUnit + Moq
[Fact]
public async Task CreateOrder_ValidData_ReturnsOrder()
{
    // Arrange, Act, Assert
}
```

### Frontend
```typescript
// Vitest + React Testing Library
describe('MenuCard', () => {
  it('renders item details', () => {
    render(<MenuCard item={mockItem} />);
    expect(screen.getByText('Pizza')).toBeInTheDocument();
  });
});
```

---

## Git Workflow

### Branch Naming
```
feature/menu-crud
fix/cart-bug
refactor/theme-system
```

### Commit Messages (Conventional Commits)
```
feat: add menu item creation
fix: resolve cart total calculation
refactor: improve theme loading
docs: update API documentation
```

### Pull Request Process
1. Create feature branch
2. Implement + test
3. Open PR with description
4. Code review required
5. Merge to main after approval

---

## DO NOT

### Backend
❌ Return entities directly from controllers (use DTOs)  
❌ Use `.Result` or `.Wait()` (use `async`/`await`)  
❌ Skip tenant validation checks  
❌ Hardcode connection strings or secrets  

### Frontend
❌ Use `any` type in TypeScript  
❌ Add 'use client' without reason (default to Server Components)  
❌ Store sensitive data in localStorage  
❌ Fetch data in `useEffect` (use Server Components or React Query)  
❌ Mutate state directly  

### General
❌ Commit to main directly  
❌ Push `.env` files  
❌ Merge without code review  
❌ Leave TODO comments (create Trello cards instead)  

---

## AI Assistant Behavior

When generating code:

1. **Always consider multi-tenancy** - Include `RestaurantId` checks
2. **Choose correct component type** - Server Component unless interactivity needed
3. **Include validation** - Both frontend (Zod) and backend (FluentValidation)
4. **Add error handling** - Try/catch with proper error messages
5. **Follow naming conventions** - As specified above
6. **Add comments** - Explain complex logic
7. **Think scalability** - Code for 100+ restaurants

When asked to:
- **"Create feature X"** → Provide backend + frontend code
- **"Add tests"** → Include unit tests with examples
- **"Optimize this"** → Explain what and why
- **"Review code"** → Point out issues and suggest improvements

### Code Format
```typescript
// File: src/app/menu/page.tsx
// Purpose: Display restaurant menu
// Type: Server Component

export default async function MenuPage() {
  // Implementation
}
```

---

## Business Rules

1. Orders cannot be modified after status = `Preparing`
2. Menu items use soft delete (mark unavailable, don't delete)
3. Restaurant slugs must be unique
4. Theme colors must be valid hex codes
5. Prices stored as `decimal` with 2 decimal places
6. Orders require minimum 1 item
7. Payment must confirm before order status = `Confirmed`

---

## Quick Reference

### Common Commands
```bash
# Backend
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet run --project OrderFlow.API

# Frontend
pnpm dev
pnpm build
pnpm test
```

### Project Values
- **Multi-tenancy:** Every feature considers multiple restaurants
- **Customization:** Unique branding per restaurant
- **Performance:** Fast load times, efficient queries
- **Security:** Always validate tenant ownership
- **Quality:** Production-ready code, not MVP hacks

---

**Last Updated:** November 2024  
**Version:** 1.0 - Next.js +16 Migration