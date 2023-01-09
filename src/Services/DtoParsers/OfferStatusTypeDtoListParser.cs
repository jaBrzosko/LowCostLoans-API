using Contracts.Offers;
using FastEndpoints;

namespace Services.DtoParsers;

public static class OfferStatusTypeDtoListParser
{
    public static ParseResult Parse(object? input)
    {
        try
        {
            var inputAsString = input!.ToString();
            var parts = inputAsString!.Split(',');
            var result = parts.Select(p =>
                {
                    var success = Enum.TryParse<OfferStatusTypeDto>(p, out var val);
                    return (success, val);
                })
                .ToList();
            return new(result.All(r => r.success), result.Select(r => r.val).ToList());
        }
        catch (Exception)
        {
            return new(false, new List<OfferStatusTypeDto>());
        }
    }
}
