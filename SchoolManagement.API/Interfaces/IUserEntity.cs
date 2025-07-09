namespace SchoolManagement.API.Interfaces;

public interface IUserEntity
{
    string UserId { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
}