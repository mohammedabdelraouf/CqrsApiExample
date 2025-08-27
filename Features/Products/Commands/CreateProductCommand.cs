namespace CqrsApiExample.Features.Products.Commands
{
    using CqrsApiExample.Data;
    using CqrsApiExample.Models;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateProductCommand : IRequest<ResultViewModel<Product>>
    {
        public ProductDTO ProductDTO { get; set; }

        public CreateProductCommand(ProductDTO productDTO)
        {
            ProductDTO = productDTO;
        }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResultViewModel<Product>>
    {
        private readonly AppDbContext _context;

        public CreateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultViewModel<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.ProductDTO.Name,
                Description = request.ProductDTO.Description,
                Price = request.ProductDTO.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            var res = new ResultViewModel<Product>
            {
                Data = product,
                Message = "Product created successfully",
                Success = true
            };
            return res;
        }
    }
}