using Domain.Inquiries;
using Domain.Users;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Inquiries;

public class InquireCreationTests
{
    private PersonalData personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);

    [Fact]
    public void Create_from_PersonalData()
    {
        TestCreation(personalData, 11231, 123312);
    }

    [Fact]
    public void Personal_data_is_null()
    {
        TestThrowingArgumentException(null, 11231, 123312);
    }
    
    [Fact]
    public void Money_is_zero()
    {
        TestThrowingArgumentException(personalData, 0, 123312);
    }
    
    [Fact]
    public void Money_is_negative()
    {
        TestThrowingArgumentException(personalData, -123, 123312);
    }
    
    [Fact]
    public void NumberOfInstallments_is_zero()
    {
        TestThrowingArgumentException(personalData, 123, 0);
    }
    
    [Fact]
    public void NumberOfInstallments_is_negative()
    {
        TestThrowingArgumentException(personalData, 123, -123);
    }

    private void TestThrowingArgumentException(PersonalData? personalDataL, int money, int numberOfInstallments)
    {
        var action = () => { new Inquire(personalDataL, money, numberOfInstallments); };
        action.Should().Throw<ArgumentException>();
    }

    private void TestCreation(PersonalData? personalDataL, int money, int numberOfInstallments)
    {
        var actualInquire = new Inquire(personalDataL, money, numberOfInstallments);

        actualInquire.PersonalData.Should().BeEquivalentTo(personalDataL);
        actualInquire.MoneyInSmallestUnit.Should().Be(money);
        actualInquire.NumberOfInstallments.Should().Be(numberOfInstallments);
    }
}
