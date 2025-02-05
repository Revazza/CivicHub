namespace CivicHub.Domain.Cities;

public static class CityConstraints
{
    public const int MinCityCodeLength = 2;
    public const int MaxCityCodeLength = 3;
    
    public const int MinCityNameLength = 3;
    public const int MaxCityNameLength = 50;
}