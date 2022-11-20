using Contracts.Users;
using Domain.Users;

namespace Services.Data.DataMappers;

public static class UserDataMapper
{
    public static PersonalData? ToEntity(this PersonalDataDto? personalDataDto)
    {
        if (personalDataDto is null)
        {
            return null;
        }

        return new PersonalData(
            personalDataDto.FirstName,
            personalDataDto.LastName,
            personalDataDto.GovernmentId,
            (GovernmentIdType)personalDataDto.GovernmentIdType,
            (JobType)personalDataDto.JobType
        );
    }
}
