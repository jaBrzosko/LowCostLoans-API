using FluentAssertions;
using Services.Services.BlobStorages;
using Xunit;

namespace Services.UnitTests.Services.BlobStorages;

public class BlobStorageConnectionStringParserTests
{
    [Fact]
    public void ConnectionString_is_normal()
    {
        string connectionString = "a=5;b=asdf;c=c";
        var expected = new Dictionary<string, string>()
        {
            { "a", "5" },
            { "b", "asdf" },
            { "c", "c" },
        };

        var actual = BlobStorageConnectionStringParser.Parse(connectionString);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Values_contain_equal_operatos()
    {
        string connectionString = "a==5;b==as=df==;c=c";
        var expected = new Dictionary<string, string>()
        {
            { "a", "=5" },
            { "b", "=as=df==" },
            { "c", "c" },
        };

        var actual = BlobStorageConnectionStringParser.Parse(connectionString);

        actual.Should().BeEquivalentTo(expected);
    }
}
