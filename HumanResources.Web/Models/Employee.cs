using HumanResources.Web.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResources.Web.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? Address { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Dob { get; set; }

        public DateTime JoinDate { get; set; }


        public string Department { get; set; }

        public string Designation { get; set; }


        
        


    }
}
