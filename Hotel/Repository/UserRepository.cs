using Hotel.Data;
using Hotel.Itnerfaces;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(Guid placeId, User user)
        {
            var userPlaceEntity = _context.Places.Where(p => p.Id == placeId).FirstOrDefault();

            var reservation = new Reservation
            {
                Place = userPlaceEntity,
                User = user
            };

            _context.Add(reservation);
            _context.Add(user);

            return Save();
        }

        public bool UpdateUser(Guid? placeId, Guid userId, User user)
        {
            if (placeId.HasValue)
            {
                var placeExists = _context.Places.Any(p => p.Id == placeId);

                if (!placeExists)
                    return false;

                var existingUserPlaces = _context.Reservations.Where(u => u.UserId == userId).ToList();

                _context.Reservations.RemoveRange(existingUserPlaces);

                var newUserReserve = new Reservation { UserId = userId, PlaceId = placeId.Value };

                _context.Reservations.Add(newUserReserve);
            }

            _context.Update(user);
            return Save();
        }

        public ICollection<Place> GetPlacesByUser(Guid userId)
        {
            var reservations = _context.Reservations.Where(r => r.UserId == userId).ToList();

            var placeIds = reservations.Select(r => r.PlaceId).ToList();

            var places = _context.Places.Where(p => placeIds.Contains(p.Id)).ToList();

            return places;
        }

        public bool DeleteUser(User user)
        {
            var reservationsToDelete = _context.Reservations.Where(r => r.UserId == user.Id);

            _context.Reservations.RemoveRange(reservationsToDelete);

            _context.Users.Remove(user);

            return Save();
            //_context.Remove(user);
            //return Save();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(Guid userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByPhone(string phone)
        {
            return _context.Users.FirstOrDefault(u => u.PhoneNumber == phone);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(u => u.CreatedAt).ToList();
        }

        public bool UserByIdExist(Guid userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        public bool UserByEmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UserByPhoneExist(string phone)
        {
            return _context.Users.Any(u => u.PhoneNumber == phone);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
