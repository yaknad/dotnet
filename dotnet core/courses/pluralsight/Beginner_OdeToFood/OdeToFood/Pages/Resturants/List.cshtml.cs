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
    // Model binding (both input and output model binding - in query string and in routing parameters)
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IResturantData resturantData;

        // Output Model
        public string LocalMessage { get; set; }
        public string ExternalMessage { get; set; }
        public IEnumerable<Resturant> Resturants { get; set; }

        [BindProperty(SupportsGet = true)] // enables to use this property as both input and output model
        public string SearchTerm { get; set; }

        public ListModel(IConfiguration config, IResturantData resturantData)
        {
            this.config = config;
            this.resturantData = resturantData;
        }

        // the default void return type returns a View (Page in dotnet core) - the corresponding cshtml
        // required for Option 2:
        //public void OnGet(string searchTerm /*Input Model - Model Binding will bind it here from the query string / form data/ http headers */)
        public void OnGet()
        {
            LocalMessage = "Hello, World";
            ExternalMessage = config["Message"];

            // Option 1: (no method parameter needed)
            //string resturantName = null;
            //if (HttpContext.Request.Query.ContainsKey("searchTerm")) ;
            //resturantName = HttpContext.Request.Query["searchTerm"];
            //Resturants = resturantData.GetResturantByName(resturantName);

            // Option 2:
            // Resturants = resturantData.GetResturantByName(searchTerm - a method param. in Option 3 we use a Property);

            // Option 3:
            Resturants = resturantData.GetResturantByName(SearchTerm);
        }
    }
}