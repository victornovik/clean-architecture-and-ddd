using MediatR;

namespace Products.Api.Products;

public class UpdateProduct
{
    public sealed record Command : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.Id == request.Id);

            if (product is null)
            {
                throw new ApplicationException("Product not found");
            }

            product.Name = request.Name;
            product.Category = request.Category;
            product.Price = request.Price;

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
