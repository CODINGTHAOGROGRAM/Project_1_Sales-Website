using asm.Models;
using asm.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace asm.Services.EmployeeSvc
{
    public interface IEmployeeService
    {
        public List<Employee> GetAllList();
        public Employee GetById(int id);
        public int AddEmployee(Employee employee);
        public int EditEmployee(int id, Employee employee);
        public Employee Login(ViewLogin viewLogin);

    }
}
