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

        public IActionResult OnGet(int? resturantId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            if (resturantId.HasValue)
            {
                Resturant = resturantData.GetById(resturantId.Value);
            }
            else
            {
                Resturant = new Resturant();
            }
            if(Resturant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        //public IActionResult OnGet()
        //{
        //    return null;
        //}

        public IActionResult OnPost()
        {
            //ModelState["location"].Errors
            if(!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();
            }   
            
            if(Resturant.Id > 0)
            {
                resturantData.Update(Resturant);
            }
            else
            {
                resturantData.Add(Resturant);
            }
            resturantData.Commit();
            // PRG (Post Redirect Get) pattern: after successful post redirect to another page that uses a GET verb. If we stay in this page, 
            // it may be refreshed and the POST will re-occure, causing a non intended action like another payment etc. 
            // Another important note: don't include in the query string any flag that you don't want the user to bookmark. 
            // for example, any error flag etc - because when the user will call the page (by the bookmark) he will get an irrelevant error message. 

            TempData["message"] = "Resturant saved!";
            return RedirectToPage("./Detail", new { resturantId = Resturant.Id });
        }
    }
}