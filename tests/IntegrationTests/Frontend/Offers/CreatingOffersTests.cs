using System.Net;
using Contracts.Frontend.Offers;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Frontend.Offers;
using Xunit;

namespace IntegrationTests.Frontend.Offers;

public class OffersCreated : TestBaseWithFrontendAuthentication
{
    public OffersCreated(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }

    [Fact]
    public async Task Test()
    {
        await CreateOfferTemplates();
    }

    private async Task CreateOfferTemplates()
    {
        var offerTemplateResult = await FrontendClient.POSTAsync<PostCreateOfferTemplateEndpoint, PostCreateOfferTemplate>(
            new PostCreateOfferTemplate()
            {
                InteresetRate = 12,
                MaximumNumberOfInstallments = 10,
                MinimumNumberOfInstallments = 5,
                MaximumMoneyInSmallestUnit = 10000,
                MinimumMoneyInSmallestUnit = 100,
            });

        offerTemplateResult!.StatusCode.Should().Be(HttpStatusCode.OK);

        offerTemplateResult = await FrontendClient.POSTAsync<PostCreateOfferTemplateEndpoint, PostCreateOfferTemplate>(
            new PostCreateOfferTemplate()
            {
                InteresetRate = 12,
                MaximumNumberOfInstallments = 8,
                MinimumNumberOfInstallments = 7,
                MaximumMoneyInSmallestUnit = 10000,
                MinimumMoneyInSmallestUnit = 100,
            });

        offerTemplateResult!.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
