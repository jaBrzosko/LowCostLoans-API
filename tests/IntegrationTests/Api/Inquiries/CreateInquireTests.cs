using System.Net;
using Contracts.Api.Inquiries;
using Contracts.Common;
using Contracts.Shared.Users;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Api.Inquiries;
using Xunit;

namespace IntegrationTests.Api.Inquiries;


public class InquireIsCreated : TestBase
{
    public InquireIsCreated(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
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
}

public class ValidationFailsBecauseOfInvalidPesel : TestBase
{
    public ValidationFailsBecauseOfInvalidPesel(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
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
