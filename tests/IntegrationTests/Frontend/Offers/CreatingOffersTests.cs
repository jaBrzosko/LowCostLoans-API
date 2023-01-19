using System.Net;
using Contracts.Api.Inquiries;
using Contracts.Api.Offers;
using Contracts.Common;
using Contracts.Frontend.Offers;
using Contracts.Shared.Users;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Api.Inquiries;
using Services.Endpoints.Api.Offers;
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
        await CreateOfferTemplatesAsync();
        var inquireId = await CreateInquireAndEnsureCorrectnessAsync();
        var expectedOfferList = new OfferListDto()
        {
            InquireId = inquireId,
            MoneyInSmallestUnit = 1000,
            NumberOfInstallments = 8,
            Offers = new()
            {
                new()
                {
                    InterestRate = 12,
                },
                new()
                {
                    InterestRate = 12,
                },
            },
        };

        var actualOfferList = await ApiClient.GETAsync<GetOffersByInquireIdEndpoint, GetOffersByInquireId, OfferListDto?>(
            new GetOffersByInquireId()
            {
                Id = inquireId,
            });

        actualOfferList.response.StatusCode.Should().Be(HttpStatusCode.OK);

        actualOfferList.result.Should().BeEquivalentTo(expectedOfferList, options => options.Excluding(p => p.Offers));
        actualOfferList.result.Offers.Should().BeEquivalentTo(expectedOfferList.Offers,
            options => options.Excluding(p => p.Id).Excluding(p => p.CreationTime));
    }

    private async Task<Guid> CreateInquireAndEnsureCorrectnessAsync()
    {
        var result = await ApiClient.POSTAsync<PostCreateAnonymousInquireEndpoint, PostCreateAnonymousInquire, PostResponseWithIdDto>(
            new PostCreateAnonymousInquire()
            {
                MoneyInSmallestUnit = 1000,
                NumberOfInstallments = 8,
                PersonalData = new PersonalDataDto()
                {
                    FirstName = "name",
                    LastName = "lastname",
                    GovernmentId = "00000000000",
                    GovernmentIdType = GovernmentIdTypeDto.Pesel,
                    JobType = JobTypeDto.SomeJobType,
                },
            });

        result.response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.result.Id.Should().NotBeEmpty();

        return result.result.Id;
    }

    private async Task CreateOfferTemplatesAsync()
    {
        var result = await FrontendClient.POSTAsync<PostCreateOfferTemplateEndpoint, PostCreateOfferTemplate>(
            new PostCreateOfferTemplate()
            {
                InteresetRate = 12,
                MaximumNumberOfInstallments = 10,
                MinimumNumberOfInstallments = 5,
                MaximumMoneyInSmallestUnit = 10000,
                MinimumMoneyInSmallestUnit = 100,
            });

        result!.StatusCode.Should().Be(HttpStatusCode.OK);

        result = await FrontendClient.POSTAsync<PostCreateOfferTemplateEndpoint, PostCreateOfferTemplate>(
            new PostCreateOfferTemplate()
            {
                InteresetRate = 12,
                MaximumNumberOfInstallments = 8,
                MinimumNumberOfInstallments = 7,
                MaximumMoneyInSmallestUnit = 10000,
                MinimumMoneyInSmallestUnit = 100,
            });

        result!.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
