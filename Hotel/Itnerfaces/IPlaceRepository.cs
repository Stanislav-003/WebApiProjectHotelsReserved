using Hotel.Models;

namespace Hotel.Itnerfaces
{
    public interface IPlaceRepository
    {
        ICollection<Place> GetPlaces();
        Place GetPlace(Guid placeId);
        ICollection<User> GetUsersByPlace(Guid placeId);
        bool PlaceExists(Guid placeId);
        bool CreatePlace(Place place);
        bool UpdatePlace(Place place);
        bool DeletePlace(Place place);
        bool Save();
    }
}
