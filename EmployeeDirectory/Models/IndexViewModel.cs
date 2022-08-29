using System.Collections.Generic;

namespace EmployeeDirectory.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
