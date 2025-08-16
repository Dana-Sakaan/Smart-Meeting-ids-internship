using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Smart_Meeting.DTOs
{
    public class CreateEmployeeDto
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        public string? Email { get; set; }

        public string? Password { get; set; }
      
        public required string Job { get; set; }

        public required string Avatar { get; set; }

        public string? Role { get; set; }   
    }
}
