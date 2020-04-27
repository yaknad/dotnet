using System.ComponentModel.DataAnnotations;

namespace OdeToFood.Core
{
    public class Resturant //: IValidatableObject - allows to write custom code to perform validation check
    {
        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Name { get; set; }
        [Required, StringLength(255)]
        public string Location { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
