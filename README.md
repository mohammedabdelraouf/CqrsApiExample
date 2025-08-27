# CQRS Pattern with MediatR in ASP.NET Core - Product Catalog API

## Overview
This API demonstrates the Command Query Responsibility Segregation (CQRS) pattern using MediatR in ASP.NET Core. The implementation separates read and write operations for a product catalog, making the code more maintainable, testable, and scalable.

## Project Structure
```
CqrsApiExample/
├── Features/
│   └── Products/
│       ├── Commands/
│       │   └── CreateProduct/
│       │       ├── CreateProductCommand.cs
│       │       └── CreateProductCommandHandler.cs
│       └── Queries/
│           └── GetProductById/
│               ├── GetProductByIdQuery.cs
│               └── GetProductByIdQueryHandler.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
│   └── Product.cs
├── Controllers/
│   └── ProductsController.cs
└── Program.cs
```

## Implementation Details

### 1. Data Model and Context

**Models/Product.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace CqrsApiExample.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public decimal Price { get; set; }
    }
}
```

**Data/AppDbContext.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using CqrsApiExample.Models;

namespace CqrsApiExample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
```

### 2. Command (Write Operation)

**Features/Products/Commands/CreateProduct/CreateProductCommand.cs**
```csharp
using MediatR;
using CqrsApiExample.Models;

namespace CqrsApiExample.Features.Products.Commands
{
    // Command to create a new product
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Handler for the create product command
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            
            return product.Id;
        }
    }
}
```

### 3. Query (Read Operation)

**Features/Products/Queries/GetProductById/GetProductByIdQuery.cs**
```csharp
using MediatR;
using CqrsApiExample.Models;

namespace CqrsApiExample.Features.Products.Queries
{
    // Query to get a product by ID
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
    
    // Handler for the get product by ID query
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly AppDbContext _context;

        public GetProductByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        }
    }
}
```

### 4. API Controller

**Controllers/ProductsController.cs**
```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CqrsApiExample.Features.Products.Commands;
using CqrsApiExample.Features.Products.Queries;
using CqrsApiExample.Models;

namespace CqrsApiExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = productId }, command);
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await _mediator.Send(query);
            
            if (product == null)
            {
                return NotFound();
            }
            
            return Ok(product);
        }
    }
}
```

### 5. Application Configuration

**Program.cs**
```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using CqrsApiExample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure in-memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductDb"));

// Register MediatR with the current assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

## Setup Instructions

1. **Create the project:**
   ```sh
   dotnet new webapi -n CqrsApiExample
   cd CqrsApiExample
   ```

2. **Install required packages:**
   ```sh
   dotnet add package MediatR
   dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
   dotnet add package Microsoft.EntityFrameworkCore.InMemory
   ```

3. **Create the folder structure** as shown above.

4. **Add the code files** with the provided content.

5. **Run the application:**
   ```sh
   dotnet run
   ```

## Testing the API

1. **Access Swagger UI:** Open your browser and navigate to `https://localhost:<port>/swagger` to see the API endpoints.

2. **Create a product:**
   - **Endpoint:** POST `/api/Products`
   - **Body:**
     ```json
     {
       "name": "Example Product",
       "price": 10.99
     }
     ```

3. **Get a product:**
   - **Endpoint:** GET `/api/Products/{id}`
   - Replace `{id}` with the ID returned from the create operation

## Benefits of This Approach

- **Separation of concerns:** Commands and queries are handled separately
- **Clean controllers:** Controllers remain thin with minimal logic
- **Testability:** Each handler can be tested in isolation
- **Scalability:** Read and write operations can be scaled independently
- **Maintainability:** Changes to business logic are localized to specific handlers

This implementation demonstrates a clean, maintainable approach to building APIs using the CQRS pattern with MediatR in ASP.NET Core.