using System;
using System.Collections.Generic;

namespace ActionTest.Data;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Address { get; set; }

    public string? OfficePhone { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
