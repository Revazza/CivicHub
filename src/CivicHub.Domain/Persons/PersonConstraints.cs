namespace CivicHub.Domain.Persons;

public static class PersonConstraints
{
    public const int FirstNameMinLength = 2;
    
    public const int FirstNameMaxLength = 50;
    
    public const int LastNameMinLength = 2;
    
    public const int LastNameMaxLength = 50;
    
    public const int PersonalNumberLength = 11;
    
    public const int PicturePathMaxLength = 1024;
    
    public const int PictureMaxSize = 10;
}