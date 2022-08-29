using EmployeeDirectory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDirectory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        EmployeeContext _employeeContext;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _employeeContext = new EmployeeContext();
        }

        public async Task<IActionResult> Index()
        {
            EmployeeTable();
            return View();
        }

        public PartialViewResult EmployeeTable(int page = 1, string searchFullName = "", string searchPhoneNumber = "")
        {
            int pageSize = 30;

            IQueryable<Employee> source = _employeeContext.Employees.Where(a => a.FullName.Contains(searchFullName ?? "") && a.PhoneNumber.Contains(searchPhoneNumber ?? ""));
            var count = source.CountAsync().Result;
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync().Result;

            var PageViewModel = new PageViewModel(count, page, pageSize);
            
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = PageViewModel,
                Employees = items
            };

            return PartialView("EmployeeTable", viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee emp, IFormFile Avatar)
        {
            try
            {
                if (Avatar != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Avatar.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        emp.Avatar = fileBytes;
                    }
                }
                _employeeContext.Employees.Add(emp);
                _employeeContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var employee = _employeeContext.Employees.Single(m => m.Id == id);
            return View(employee);
        }

        [HttpPost]
        public ActionResult Delete(Employee employee)
        {
            try
            {
                var e = _employeeContext.Employees.Find(employee.Id);
                _employeeContext.Employees.Remove(e);
                _employeeContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var employee = _employeeContext.Employees.Single(m => m.Id == id);
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee, IFormFile avatar)
        {
            try
            {
                byte[] image = null;
                if (avatar != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        avatar.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        image = fileBytes;
                    }
                }

                var e = _employeeContext.Employees.Find(employee.Id);
                e.FullName = employee.FullName;
                e.PhoneNumber = employee.PhoneNumber;
                e.Department = employee.Department;
                if (image != null) e.Avatar = image;

                _employeeContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
