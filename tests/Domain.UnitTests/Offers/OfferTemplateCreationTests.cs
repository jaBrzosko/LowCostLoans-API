using Domain.Offers;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Offers;

public class OfferTemplateCreationTests
{
    [Fact]
    public void Create()
    {
        TestCreation(100_000_00, 200_000_00, 48, 60, 250);
    }

    [Fact]
    public void MinimumMoney_is_zero()
    {
        TestThrowingArgumentException(0, 200_000_00, 48, 64, 250);
    }

    [Fact]
    public void MinimumMoney_is_negative()
    {
        TestThrowingArgumentException(-1, 200_000_00, 48, 64, 250);
    }

    [Fact]
    public void MinimumMoney_is_larger_than_MaximumMoney()
    {
        TestThrowingArgumentException(200_000_00, 100_000_00, 48, 64, 250);
    }
    
    [Fact]
    public void MinimumInstallments_is_zero()
    {
        TestThrowingArgumentException(100_000_00, 200_000_00, 0, 64, 250);
    }

    [Fact]
    public void MinimumInstallments_is_negative()
    {
        TestThrowingArgumentException(100_000_00, 200_000_00, -1, 64, 250);
    }

    [Fact]
    public void MinimumInstallments_is_larger_than_MaximumInstallments()
    {
        TestThrowingArgumentException(100_000_00, 200_000_00, 64, 48, 250);
    }

    [Fact]
    public void InstallmentRate_is_negative()
    {
        TestThrowingArgumentException(100_000_00, 200_000_00, 48, 64, -1);
    }
    
    private void TestThrowingArgumentException(int minimumMoneyInSmallestUnit, int maximumMoneyInSmallestUnit,
        int minimumNumberOfInstallments, int maximumNumberOfInstallments, int interestRate)
    {
        var action = () =>
        {
            new OfferTemplate(minimumMoneyInSmallestUnit, maximumMoneyInSmallestUnit,
                minimumNumberOfInstallments, maximumNumberOfInstallments, interestRate);
        };
        action.Should().Throw<ArgumentException>();
    }

    private void TestCreation(int minimumMoneyInSmallestUnit, int maximumMoneyInSmallestUnit,
        int minimumNumberOfInstallments, int maximumNumberOfInstallments, int interestRate)
    {
        var actualOfferTemplate = new OfferTemplate(minimumMoneyInSmallestUnit, maximumMoneyInSmallestUnit,
            minimumNumberOfInstallments, maximumNumberOfInstallments, interestRate);

        actualOfferTemplate.MinimumMoneyInSmallestUnit.Should().Be(minimumMoneyInSmallestUnit);
        actualOfferTemplate.MaximumMoneyInSmallestUnit.Should().Be(maximumNumberOfInstallments);
        actualOfferTemplate.MinimumNumberOfInstallments.Should().Be(minimumNumberOfInstallments);
        actualOfferTemplate.MaximumNumberOfInstallments.Should().Be(maximumNumberOfInstallments);
        actualOfferTemplate.InterestRate.Should().Be(interestRate);
    }
}