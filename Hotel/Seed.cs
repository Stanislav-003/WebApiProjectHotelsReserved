using Hotel.Data;
using Hotel.Models;

namespace Hotel
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (dataContext.Users.Any() || dataContext.Places.Any() || dataContext.Reservations.Any())
            {
                return; 
            }

            var users = new User[]
            {
                new User { Id = Guid.NewGuid(), FirstName = "Іван", LastName = "Петров", PhoneNumber = "+380977537422", Email = "ivan@example.com", Password = GenerateRandomPassword(), CreatedAt = DateTime.Now },
                new User { Id = Guid.NewGuid(), FirstName = "Марія", LastName = "Іванова", PhoneNumber = "+380674818936", Email = "maria@example.com", Password = GenerateRandomPassword(), CreatedAt = DateTime.Now }
            };
            dataContext.Users.AddRange(users);

            var places = new Place[]
            {
                new Place { Id = Guid.NewGuid(), Name = "Готель А", Description = "Затишний готель у центрі міста", Address = "вул. Центральна 1", PriceOneNight = 100, AvailableFrom = DateTime.Now, AvailableTo = DateTime.Now.AddDays(7) },
                new Place { Id = Guid.NewGuid(), Name = "Квартира Б", Description = "Простора квартира з усіма зручностями", Address = "вул. Паркова 10", PriceOneNight = 80, AvailableFrom = DateTime.Now, AvailableTo = DateTime.Now.AddDays(14) }
            };
            dataContext.Places.AddRange(places);

            var reservations = new Reservation[]
            {
                new Reservation { UserId = users[0].Id, PlaceId = places[0].Id },
                new Reservation { UserId = users[1].Id, PlaceId = places[1].Id }
            };
            dataContext.Reservations.AddRange(reservations);

            dataContext.SaveChanges();
        }

        private string GenerateRandomPassword()
        {
            // Генеруємо випадковий пароль з 8 символів
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }
    }
}
