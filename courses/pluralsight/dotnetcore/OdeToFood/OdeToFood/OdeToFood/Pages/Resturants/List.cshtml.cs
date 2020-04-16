using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IResturantData resturantData;

        public string LocalMessage { get; set; }
        public string ExternalMessage { get; set; }
        public IEnumerable<Resturant> Resturants { get; set; }

        public ListModel(IConfiguration config, IResturantData resturantData)
        {
            this.config = config;
            this.resturantData = resturantData;
        }
        
        public void OnGet()
        {
            LocalMessage = "Hello, World";
            ExternalMessage = config["Message"];
            Resturants = resturantData.GetAll();
        }
    }
}