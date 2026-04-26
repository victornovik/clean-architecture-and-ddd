using Products.Api.Products;

namespace Products.Api.IntegrationTests;

public class ProductTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldCreateProduct()
    {
        const decimal PRICE = 99.9m;
        const string NAME = "Intel Pentium";
        const string CATEGORY = "Hardware";

        // Arrange
        var command = new CreateProduct.Command
        {
            Name = NAME,
            Category = CATEGORY,
            Price = PRICE
        };

        // Act
        var productId = await Sender.Send(command);

        // Assert
        var product = DbContext.Products.FirstOrDefault(p => p.Id == productId);

        Assert.NotNull(product);
        Assert.Equal(NAME, product.Name);
        Assert.Equal(CATEGORY, product.Category);
        Assert.Equal(PRICE, product.Price);
    }

    [Fact]
    public async Task Get_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = await CreateProduct();
        var query = new GetProduct.Query { Id = productId };

        // Act
        var productResponse = await Sender.Send(query);

        // Assert
        Assert.NotNull(productResponse);
        Assert.Equal(productId, productResponse.Id);
    }

    [Fact]
    public async Task Get_ShouldThrow_WhenQueryNonExistingProduct()
    {
        // Arrange
        var query = new GetProduct.Query { Id = Guid.NewGuid() };

        // Act
        Task Action() => Sender.Send(query);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(Action);
    }

    [Fact]
    public async Task Update_ShouldUpdateProduct_WhenProductExists()
    {
        const decimal PRICE = 55.5m;
        const string NAME = "Fridge";
        const string CATEGORY = "Utilities";

        // Arrange
        var productId = await CreateProduct();
        var command = new UpdateProduct.Command
        {
            Id = productId,
            Name = NAME,
            Category = CATEGORY,
            Price = PRICE
        };

        // Act
        await Sender.Send(command);

        // Assert
        var product = DbContext.Products.FirstOrDefault(p => p.Id == productId);
        Assert.NotNull(product);
        Assert.Equal(NAME, product.Name);
        Assert.Equal(CATEGORY, product.Category);
        Assert.Equal(PRICE, product.Price);
    }

    [Fact]
    public async Task Update_ShouldThrow_WhenProductIsNull()
    {
        // Arrange
        var command = new UpdateProduct.Command
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Category = "Test category",
            Price = 100.0m
        };

        // Act
        Task Action() => Sender.Send(command);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(Action);
    }

    [Fact]
    public async Task Delete_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var productId = await CreateProduct();
        var command = new DeleteProduct.Command { Id = productId };

        // Act
        await Sender.Send(command);

        // Assert
        var product = DbContext.Products.FirstOrDefault(p => p.Id == productId);

        Assert.Null(product);
    }

    [Fact]
    public async Task Delete_ShouldThrow_WhenProductIsNull()
    {
        // Arrange
        var command = new DeleteProduct.Command { Id = Guid.NewGuid() };

        // Act
        Task Action() => Sender.Send(command);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(Action);
    }

    private async Task<Guid> CreateProduct()
    {
        var command = new CreateProduct.Command
        {
            Name = "Test",
            Category = "Test category",
            Price = 100.0m
        };

        var productId = await Sender.Send(command);

        return productId;
    }
}