using Hotel.Models;

namespace Hotel.Itnerfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUserById(Guid userId);
        User GetUserByEmail(string email);
        User GetUserByPhone(string phone);
        ICollection<Place> GetPlacesByUser(Guid userId);
        bool UserByIdExist(Guid userId);
        bool UserByEmailExist(string email);
        bool UserByPhoneExist(string phone);
        bool CreateUser(Guid placeId, User user);
        bool UpdateUser(Guid? placeId, Guid userId, User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
