using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AfterTaste.Models
{
	public class ContactUs
	{
		[Required]
		[Key]
		public int contactId { get; set; }
		[Required]
		[ForeignKey("userId")]
		public int userId { get; set; }
		[Required]
		public string fullName { get; set; }
		[Required]
		public string email { get; set; }
		[Required]
		public string subject { get; set; }
		[Required]
		public string message { get; set; }
	}
}