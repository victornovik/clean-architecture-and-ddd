namespace Api.FunctionalTests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected HttpClient HttpClient { get; set; }
}
