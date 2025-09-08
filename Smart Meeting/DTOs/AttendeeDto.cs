using AutoMapper;

namespace Smart_Meeting.DTOs
{
    public class AttendeeDto
    {
        public string EmployeeID { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string Email {  get; set; }
        public string Avatar { get ; set; } 
        public string Job {  get; set; }
    }
}
