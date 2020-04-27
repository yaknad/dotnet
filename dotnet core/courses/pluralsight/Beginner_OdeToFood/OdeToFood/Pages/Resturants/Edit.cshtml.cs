using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood
{
    public class EditModel : PageModel
    {
        private readonly IResturantData resturantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Resturant Resturant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IResturantData resturantData, IHtmlHelper htmlHelper)
        {
            this.resturantData = resturantData;
            this.htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(int resturantId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            Resturant = resturantData.GetById(resturantId);
            if(Resturant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            //ModelState["location"].Errors
            if(ModelState.IsValid)
            {
                resturantData.Update(Resturant);
                resturantData.Commit();
                // PRG pattern: after successful post redirect to another page that uses a GET verb. If we stay in this page, 
                // it may be refreshed and the POST will re-occure, causing a non intended actino like another payment etc. 
                return RedirectToPage("./Detail", new { resturantId = Resturant.Id });
            }            
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            return Page();
        }
    }
}