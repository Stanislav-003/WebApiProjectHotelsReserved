using Hotel.Itnerfaces;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;

namespace Hotel.Repository
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly DataContext _context;

        public PlaceRepository(DataContext context) 
        {
            _context = context;
        }

        public bool CreatePlace(Place place)
        {
            _context.Places.Add(place);
            return Save();
        }

        public bool UpdatePlace(Place place)
        {
            _context.Update(place);
            return Save();
        }

        public bool DeletePlace(Place place)
        {
            _context.Remove(place);
            return Save();
        }

        public Place GetPlace(Guid placeId)
        {
            return _context.Places.Where(p => p.Id == placeId).FirstOrDefault();
        }

        public ICollection<Place> GetPlaces()
        {
            return _context.Places.ToList();
        }

        public bool PlaceExists(Guid placeId)
        {
            return _context.Places.Any(p => p.Id == placeId);
        }

        public ICollection<User> GetUsersByPlace(Guid placeId)
        {
            var reservations = _context.Reservations.Where(r => r.PlaceId == placeId).ToList();

            var usersIds = reservations.Select(r => r.UserId).ToList();

            var users = _context.Users.Where(u => usersIds.Contains(u.Id)).ToList();

            return users;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
