using Microsoft.Extensions.Logging;
using OdeToFood.Core;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFood.Data
{
    // Redundnat - we used it before we developed a Sql Server based data repository
    public class InMemoryResturantData : IResturantData
    {
        private List<Resturant> resturants;
        private ILogger<InMemoryResturantData> logger;
        private ICustomService service;

        public InMemoryResturantData(ILogger<InMemoryResturantData> logger, ICustomService service)
        {
            this.service = service;
            service.DoNothing();
            this.logger = logger;
            resturants = new List<Resturant>()
            {
                new Resturant { Id = 1 , Cuisine = CuisineType.Indian, Location = "Bnei Braq", Name = "Tsipori" },
                new Resturant { Id = 2, Cuisine = CuisineType.Italian, Location = "Ramat Gan", Name = "Pitzale" },
                new Resturant { Id = 3, Cuisine = CuisineType.None, Location = "Airport City", Name = "Neeman" }
            };
        }
        public IEnumerable<Resturant> GetResturantByName(string name = null)
        {
            return from r in resturants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }

        public Resturant GetById(int id)
        {
            return resturants.SingleOrDefault(r => r.Id == id);
        }

        public Resturant Update(Resturant updatedResturant)
        {
            var resturant = resturants.SingleOrDefault(r => r.Id == updatedResturant.Id);
            if(resturant != null)
            {
                resturant.Name = updatedResturant.Name;
                resturant.Location = updatedResturant.Location;
                resturant.Cuisine = updatedResturant.Cuisine;
            }
            return resturant;
        }

        public Resturant Add(Resturant newResturant)
        {
            resturants.Add(newResturant);
            newResturant.Id = resturants.Max(r => r.Id) + 1;
            return newResturant;
        }

        public int Commit()
        {
            return 0;
        }
    }
}
