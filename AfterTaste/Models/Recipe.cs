using AfterTaste.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterTaste.Models
{
    public enum Origin
    {
        Afghanistan, Albania, Algeria, AmericanSamoa, Andorra, Angola, AntiguaandBarbuda, Argentina, Armenia, Australia, Austria, Azerbaijan, Bahamas, Bahrain, Bangladesh, Barbados, Belarus, Belgium, Belize, Benin, Bermuda, Bhutan, Bolivia, BosniaandHerzegovina, Botswana, Brazil, Brunei, Bulgaria, BurkinaFaso, Burundi, Cambodia, Cameroon, Canada, CapeVerde, CaymanIslands, CentralAfricanRepublic, Chad, Chile, China, Colombia, Comoros, Congo, DemocraticRepublicoftheCongo, RepublicoftheCostaRica, CôtedIvoire, Croatia, Cuba, Curaçao, Cyprus, CzechRepublic, Denmark, Djibouti, Dominica, DominicanRepublic, EastTimor, Ecuador, Egypt, ElSalvador, EquatorialGuinea, Eritrea, Estonia, Ethiopia, FaroeIslands, Fiji, Finland, France, FrenchPolynesia, Gabon, Gambia, Georgia, Germany, Ghana, Greece, Greenland, Grenada, Guam, Guatemala, Guinea, GuineaBissau, Guyana, Haiti, Honduras, HongKong, Hungary, Iceland, India, Indonesia, Iran, Iraq, Ireland, Israel, Italy, Jamaica, Japan, Jordan, Kazakhstan, Kenya, Kiribati, NorthKorea, SouthKorea, Kosovo, Kuwait, Kyrgyzstan, Laos, Latvia, Lebanon, Lesotho, Liberia, Libya, Liechtenstein, Lithuania, Luxembourg, Macedonia, Madagascar, Malawi, Malaysia, Maldives, Mali, Malta, MarshallIslands, Mauritania, Mauritius, Mexico, Micronesia, Moldova, Monaco, Mongolia, Montenegro, Morocco, Mozambique, Myanmar, Namibia, Nauru, Nepal, Netherlands, NewZealand, Nicaragua, Niger, Nigeria, NorthernMarianaIslands, Norway, Oman, Pakistan, Palau, Palestine, StateofPanama, PapuaNewGuinea, Paraguay, Peru, Philippines, Poland, Portugal, PuertoRico, Qatar, Romania, Russia, Rwanda, SaintKittsandNevis, SaintLucia, SaintVincentandtheGrenadines, Samoa, SanMarino, SaoTomeandPrincipe, SaudiArabia, Senegal, Serbia, Seychelles, SierraLeone, Singapore, SintMaarten, Slovakia, Slovenia, SolomonIslands, Somalia, SouthAfrica, Spain, SriLanka, Sudan, South, Suriname, Swaziland, Sweden, Switzerland, Syria, Taiwan, Tajikistan, Tanzania, Thailand, Togo, Tonga, TrinidadandTobago, Tunisia, Turkey, Turkmenistan, Tuvalu, Uganda, Ukraine, UnitedArabEmirates, UnitedKingdom, UnitedStates, Uruguay, Uzbekistan, Vanuatu, VaticanCity, Venezuela, Vietnam, VirginIslands, British, Yemen, Zambia, Zimbabwe
    }

    public enum RecipeStatus
    {
        Pending, Approved, Disapproved
    }
    public class Recipe
    {
        [Key]
        public int recipeId { get; set; }
        [ForeignKey("userId")]
        public string? userId { get; set; }
        public User? User { get; set; }
        public string? recipeName { get; set; }
        public string? recipeDescription { get; set; }
        public string? recipeVideo { get; set; }
        public byte[]? recipeImage { get; set; }
        public string? recipeDirections { get; set; }
        public string? recipeIngredients { get; set; }
        public Origin Origin { get; set; }
        public int? recipeCalories { get; set; }
        public RecipeStatus Status { get; set; }
        public ICollection<UserReview>? Reviews { get; set; }

        public double AverageRating
        {
            get
            {
                if (Reviews == null || !Reviews.Any())
                    return 0;

                return Reviews.Average(review => review.Rating);
            }
        }
    }

}