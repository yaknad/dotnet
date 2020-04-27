using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public interface IResturantData
    {
        IEnumerable<Resturant> GetResturantByName(string name);
        Resturant GetById(int id);
        Resturant Update(Resturant updatedResturant);
        int Commit();
    }
}
