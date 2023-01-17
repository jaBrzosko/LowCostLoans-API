using System.Net;
using Contracts.Api.Inquiries;
using Contracts.Common;
using Contracts.Shared.Users;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Api.Inquiries;
using Xunit;

namespace IntegrationTests.Api.Inquiries;

public class CreateInquireTests : TestBase
{
    public CreateInquireTests(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }

    [Fact]
    public async Task Inquire_is_created()
    {
        var result = await ApiClient.POSTAsync<PostCreateAnonymousInquireEndpoint, PostCreateAnonymousInquire, PostResponseWithIdDto>(new PostCreateAnonymousInquire()
        {
            MoneyInSmallestUnit = 1000,
            NumberOfInstallments = 12,
            PersonalData = new()
            {
                FirstName = "name",
                GovernmentId = "00000000000",
                GovernmentIdType = GovernmentIdTypeDto.Pesel,
                JobType = JobTypeDto.SomeJobType,
                LastName = "lastname",
            },
        });

        result.response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Validation_fails_because_of_invalid_pesel()
    {
        var result = await ApiClient.POSTAsync<PostCreateAnonymousInquireEndpoint, PostCreateAnonymousInquire, PostResponseWithIdDto>(new PostCreateAnonymousInquire()
        {
            MoneyInSmallestUnit = 1000,
            NumberOfInstallments = 12,
            PersonalData = new()
            {
                FirstName = "name",
                GovernmentId = "0000000000",
                GovernmentIdType = GovernmentIdTypeDto.Pesel,
                JobType = JobTypeDto.SomeJobType,
                LastName = "lastname",
            },
        });

        result.response.StatusCode.Should().Be((HttpStatusCode)400);
    }
}
