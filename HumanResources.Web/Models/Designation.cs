namespace HumanResources.Web.Models
{
    public class Designation
    {
        public int Id { get; set; }

        public string Name { get; set; }    

        public string Description { get; set; }

        public List<Employee>? Employees{ get; set; }
    }
}
