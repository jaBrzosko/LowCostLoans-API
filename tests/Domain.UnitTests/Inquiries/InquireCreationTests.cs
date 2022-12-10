using Domain.Inquiries;
using Domain.Users;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Inquiries;

public class InquireCreationTests
{
    [Fact]
    public void Create_from_PersonalData()
    {
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
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
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
        TestThrowingArgumentException(personalData, 0, 123312);
    }
    
    [Fact]
    public void Money_is_negative()
    {
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
        TestThrowingArgumentException(personalData, -123, 123312);
    }
    
    [Fact]
    public void NumberOfInstallments_is_zero()
    {
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
        TestThrowingArgumentException(personalData, 123, 0);
    }
    
    [Fact]
    public void NumberOfInstallments_is_negative()
    {
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
        TestThrowingArgumentException(personalData, 123, -123);
    }

    private void TestThrowingArgumentException(PersonalData? personalData, int money, int numberOfInstallments)
    {
        var action = () => { new Inquire(personalData, money, numberOfInstallments); };
        action.Should().Throw<ArgumentException>();
    }

    private void TestCreation(PersonalData? personalData, int money, int numberOfInstallments)
    {
        var actualInquire = new Inquire(personalData, money, numberOfInstallments);

        actualInquire.PersonalData.Should().BeEquivalentTo(personalData);
        actualInquire.MoneyInSmallestUnit.Should().Be(money);
        actualInquire.NumberOfInstallments.Should().Be(numberOfInstallments);
    }
}
