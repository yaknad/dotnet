using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood.Data
{
    public class SqlResturantData : IResturantData
    {
        private readonly OdeToFoodDbContext db;

        public SqlResturantData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public Resturant Add(Resturant resturant)
        {
            db.Add(resturant);
            return resturant;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Resturant Delete(int id)
        {
            var res = GetById(id);
            if(res != null)
            {
                db.Remove(res);
            }
            return res;
        }

        public Resturant GetById(int id)
        {
            return db.Resturants.Find(id);
        }

        public IEnumerable<Resturant> GetResturantByName(string name)
        {
            var query = from r in db.Resturants
                        where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Resturant Update(Resturant updatedResturant)
        {
            var entity = db.Resturants.Attach(updatedResturant);
            entity.State = EntityState.Modified;
            return updatedResturant;
        }
    }
}
