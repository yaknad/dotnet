using Microsoft.Extensions.Logging;
using OdeToFood.Core;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFood.Data
{
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
        public IEnumerable<Resturant> GetAll()
        {
            return from r in resturants
                   orderby r.Name
                   select r;
        }
    }
}
