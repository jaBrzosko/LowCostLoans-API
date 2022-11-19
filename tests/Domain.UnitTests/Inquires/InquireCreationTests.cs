using Domain.Inquires;
using Domain.Users;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Inquires;

public class InquireCreationTests
{
    [Fact]
    public void Create_from_UserId()
    {
        TestCreation(Guid.NewGuid(), null, 11231, 123312);
    }
    
    [Fact]
    public void Create_from_PersonalData()
    {
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
        TestCreation(null, personalData, 11231, 123312);
    }

    [Fact]
    public void UserId_and_Personal_data_are_not_null()
    {
        var personalData = new PersonalData("first name", "last name", "pesel", GovernmentIdType.Pesel, JobType.SomeJobType);
        TestThrowingArgumentException(Guid.NewGuid(), personalData, 11231, 123312);
    }
    
    [Fact]
    public void UserId_and_Personal_data_are_null()
    {
        TestThrowingArgumentException(null, null, 11231, 123312);
    }
    
    [Fact]
    public void Money_is_zero()
    {
        TestThrowingArgumentException(Guid.NewGuid(), null, 0, 123312);
    }
    
    [Fact]
    public void Money_is_negative()
    {
        TestThrowingArgumentException(Guid.NewGuid(), null, -123, 123312);
    }
    
    [Fact]
    public void NumberOfInstallments_is_zero()
    {
        TestThrowingArgumentException(Guid.NewGuid(), null, 123, 0);
    }
    
    [Fact]
    public void MNumberOfInstallments_is_negative()
    {
        TestThrowingArgumentException(Guid.NewGuid(), null, 123, -123);
    }

    private void TestThrowingArgumentException(Guid? userId, PersonalData? personalData, int money, int numberOfInstallments)
    {
        var action = () => { new Inquire(userId, personalData, money, numberOfInstallments); };
        action.Should().Throw<ArgumentException>();
    }

    private void TestCreation(Guid? userId, PersonalData? personalData, int money, int numberOfInstallments)
    {
        var actualInquire = new Inquire(userId, personalData, money, numberOfInstallments);

        actualInquire.UserId.Should().Be(userId);
        actualInquire.PersonalData.Should().BeEquivalentTo(personalData);
        actualInquire.MoneyInSmallestUnit.Should().Be(money);
        actualInquire.NumberOfInstallments.Should().Be(numberOfInstallments);
    }
}
