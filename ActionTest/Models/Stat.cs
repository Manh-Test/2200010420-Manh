namespace ActionTest.Models
{
    public class Stat
    {
        public int? Id { get; set; }
        public int? DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;

        public int TotalEmployee { get; set; }

        public int TotalMale { get; set; }

        public int TotalFemale { get; set; }
    }
}
