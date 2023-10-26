namespace AfterTaste.Models
{
    public class Recipe
    {

        public int userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string userPassword { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string profilePicture { get; set; }
        public int contactNumber { get; set; }
        public DateTime birthdate { get; set; }
        public string address { get; set; }

    }
}
