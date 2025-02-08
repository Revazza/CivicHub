using CivicHub.Application.Common.Responses;
using CivicHub.Domain.Persons.Enums;

namespace CivicHub.Application.Features.Persons.Queries.GetFullInformation;

public class GetFullInformationResponse
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string PersonalNumber { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public CityResponse City { get; set; }
    public List<PhoneNumberResponse> PhoneNumbers { get; set; }
    public List<PersonConnectionResponse> Connections { get; set; }
}