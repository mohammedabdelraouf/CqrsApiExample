namespace CqrsApiExample.Features.Products.Queries
{
    using CqrsApiExample.Data;
    using CqrsApiExample.Models;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetProductById : IRequest<ResultViewModel<Product>>
    {
        public int Id { get; set; }

        public GetProductById(int id)
        {
            Id = id;
        }
    }

    public class GetProductByIdHandler : IRequestHandler<GetProductById, ResultViewModel<Product>>
    {
        private readonly AppDbContext _context;

        public GetProductByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<Product>> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == request.Id, cancellationToken);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
                }
                var res = new ResultViewModel<Product>
                {
                    Data = product,
                    Message = "Product retrieved successfully",
                    Success = true
                };
                return res;

            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product: {ex.Message}", ex);
            }
        }
    }
}