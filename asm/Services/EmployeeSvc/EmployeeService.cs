using asm.Helpers;
using asm.Models;
using asm.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace asm.Services.EmployeeSvc
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _dataContext;
        private readonly IDataEncode _dataEncode;
        public EmployeeService(IDataEncode dataEncode, DataContext dataContext)
        {
            _dataEncode = dataEncode;
            _dataContext = dataContext;
        }
        public int AddEmployee(Employee employee)
        {
            int st = 0;
            try
            {
                employee.Password = _dataEncode.EnCode(employee.Password);
                _dataContext.Add(employee);
                _dataContext.SaveChanges();
                st = employee.EmployeeID;
            }catch (Exception ex)
            {
                return st = 0;
            }
            return st;
        }

        public int EditEmployee(int id, Employee employee)
        {
            int st = 0;
            var exitsEmpl = _dataContext.employees.FirstOrDefault(emp => emp.EmployeeID == id);
            try
            {
                if(exitsEmpl == null)
                {
                    return 0;
                }
                exitsEmpl.UserName = employee.UserName;
                exitsEmpl.FullName = employee.FullName;
                exitsEmpl.Email = employee.Email;
                exitsEmpl.Position = employee.Position;
                exitsEmpl.Gender = employee.Gender;
                exitsEmpl.DateOfBirth = employee.DateOfBirth;
                exitsEmpl.PhoneNumber = employee.PhoneNumber;
                exitsEmpl.Locked = employee.Locked;
                exitsEmpl.Image = employee.Image;
                exitsEmpl.Note = employee.Note;
                if(exitsEmpl.Password != null)
                {
                    employee.Password = _dataEncode.EnCode(employee.Password);
                    exitsEmpl.Password = employee.Password;
                }
                _dataContext.Update(exitsEmpl);
                _dataContext.SaveChanges();
                st = employee.EmployeeID;
            }catch (Exception ex)
            {
                st = 0;
            }
            return st;
        }

        public List<Employee> GetAllList()
        {
            try
            {
                return _dataContext.employees.ToList();
            }catch (Exception ex)
            {
                return new List<Employee>();
            }
        }

        public Employee GetById(int id)
        {
            try
            {
                return _dataContext.employees.Where(empl => empl.EmployeeID == id).FirstOrDefault();
            }catch (Exception ex)
            {
                return new Employee();
            }
        }
        public Employee Login(ViewLogin viewLogin)
        {
            var login = _dataContext.employees.Where(e => e.UserName.Equals(viewLogin.Username)
           && e.Password.Equals(_dataEncode.EnCode(viewLogin.Password))).FirstOrDefault();
            return login;
        }
    }
}
