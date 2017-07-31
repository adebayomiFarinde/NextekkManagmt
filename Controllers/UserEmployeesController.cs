using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NextekkManagmt.Models;
using Microsoft.AspNet.Identity.Owin;
using NextekkManagmt.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using NextekkManagmt;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    //Handling SignIn and User Account 
    public class UserEmployeesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
  

        public UserEmployeesController()
        {

        }
        public UserEmployeesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        //Handling Sign In 
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        //Handling User Manager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //This is the welcome page displayed when new user(Employee) signs up
        //Welcome Page
        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }

        //This loads the list of the user(Employee) from the UserEmployee table 
        // GET: UserEmployees only Admin
        [MyAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var userEmployees = db.UserEmployees
                .Include(u => u.EducationQualification)
                .Include(u => u.Gender)
                .Include(u => u.MaritalStatus);
            return View(userEmployees.ToList());
        }

        //This shows the details of the Data of the selected user(Employee) at request
        // GET: UserEmployees/Details/5 for Users and Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmployee userEmployee = db
                .UserEmployees
                .Find(id);
            if (userEmployee == null)
            {
                return HttpNotFound();
            }
            return View(userEmployee);
        }

        //This loads the page for registering the user(Employee)
        // GET: UserEmployees/Create
        public ActionResult Create()

        {
            ViewBag
                .EducationQualificationID = new SelectList(db.EducationQualifications, "ID", "Level");
            ViewBag
                .GenderID = new SelectList(db.Genders, "ID", "GenderType");
            ViewBag
                .MaritalStatusID = new SelectList(db.MaritalStatuss, "ID", "Status");
            return View();
        }

        //Posting the Data inputted during registration to the both the UserEmployee table and runs the Register Action methos
        // POST: UserEmployees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserEmployeeViewModel userVM)
        {
            UserEmployee employee = new UserEmployee();
            if (ModelState.IsValid)
            {
                employee.ID = Guid.NewGuid();

                employee.FirstName = userVM.FirstName;

                employee.LastName = userVM.LastName;

                employee.Email = userVM.Email;

                employee.DOB = userVM.DOB;

                employee.NumberOfChildren = userVM.NumberOfChildren;

                employee.AnnualSalary = 0;

                employee.GenderID = userVM.GenderID;

                employee.EducationQualificationID = db
                    .EducationQualifications
                    .Where(r => r.Level.Contains("B.Sc"))
                    .Single().ID;

                employee.MaritalStatusID = userVM.MaritalStatusID;

                employee.Active = false;

                employee.DateOfEmployment = DateTime.Now;

                employee.NameOfInstitution = userVM.NameOfInstitution;

                employee.Position = userVM.Position;

                employee.PromotedAt = DateTime.Now;
                try
                {
                    //Create a user account in the AspNetUser table
                    var userid = Register(new RegisterViewModel()
                    {
                        Email = userVM.Email,
                        Password = userVM.Password,
                        ConfirmPassword = userVM.ConfirmPassword
                    });


                    if (!string.IsNullOrWhiteSpace(userid))
                    {

                        employee.RegistrationID = userid;
                        db.UserEmployees.Add(employee);
                        db.SaveChanges();

                    }
                }
                catch (Exception e)
                {

                    ModelState
                        .AddModelError("", "Unable to create user. Please contact administrator" + e.Message);
                }

                return RedirectToAction("Welcome");
            }


            ViewBag
                .GenderID = new SelectList(db.Genders, "ID", "GenderType");
            ViewBag
                .MaritalStatusID = new SelectList(db.MaritalStatuss, "ID", "Status");
            ViewBag
                .EductionQualificationID = new SelectList(db.EducationQualifications, "ID", "Level");
            return View(employee);
        }

        //This shows the detail view for the User(Employee) that has active as true
        [Authorize]
        public ActionResult DetailEmployee(Guid? id)
            
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmployee userEmployee = db
                .UserEmployees
                .Find(id);
            if (userEmployee == null)
            {
                return HttpNotFound();
            }
            return View(userEmployee);
        }

        //Authorization for different users
        public class MyAuthorizeAttribute : AuthorizeAttribute
        {
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                if (!filterContext
                    .HttpContext
                    .User
                    .Identity
                    .IsAuthenticated)
                {
                    // The user is not authenticated
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else if (!this
                    .Roles.Split(',')
                    .Any(filterContext
                    .HttpContext
                    .User
                    .IsInRole))
                {
                    // The user is not in any of the listed roles => 
                    // show the unauthorized view
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/Unauthorized.cshtml"
                    };
                }
                else
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
            }
        }
        //Redirecting to this if a User(Employee) has not has not being made active yet by the Admin
        [Authorize]
        public ActionResult Redirect()
        {
            return View();
        }

        //Authorized Users checking if Admin or user(Employee) to redirect to appropriate view either for the Admin or Employee
        [Authorize]
        public ActionResult Useraccount()

        {

            var userId = User
                .Identity
                .GetUserId();
            var Usernames = User
                .Identity
                .GetUserName();
            var isAdmin = UserManager
                .IsInRole(userId, "Admin") ? true : false;
            if (isAdmin)
            {
                return RedirectToAction("Index");
            }
            var user = db.UserEmployees
                .Include(x => x.MaritalStatus)
                .Include(x => x.Gender)
                .Include(x => x.EducationQualification)
                .Where(x => x.RegistrationID == userId)
                .ToList();
            var status = db.UserEmployees
                .Where(x => x.RegistrationID == userId)
                .Single()
                .Active;
            if (status == true)
            {
                return View(user);
            }
            return RedirectToAction("Redirect", "UserEmployees");
        }

        //Register and Assignment of Roles where neccesary
        public string Register(RegisterViewModel model)
        {
            var userRoleName = db.Roles
                .Where(r => r.Name.Contains("User"))
                .Single()
                .Name;

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = UserManager
                .CreateAsync(user, model.Password)
                .Result;
            if (result.Succeeded)
            {

                SignInManager
                    .SignIn(user, isPersistent: false, rememberBrowser: false);

                this.UserManager
                    .AddToRole(user.Id, userRoleName);
                return user.Id;
            }
            else
            {
                return "";
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: UserEmployees/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmployee userEmployee = db
                .UserEmployees
                .Find(id);
            if (userEmployee == null)
            {
                return HttpNotFound();
            }
            ViewBag
                .EducationQualificationID = new SelectList(db.EducationQualifications, "ID", "Level", userEmployee.EducationQualificationID);
            ViewBag
                .GenderID = new SelectList(db.Genders, "ID", "GenderType", userEmployee.GenderID);
            ViewBag
                .MaritalStatusID = new SelectList(db.MaritalStatuss, "ID", "Status", userEmployee.MaritalStatusID);
            return View(userEmployee);
        }
        //Posting the Edited Data of the Employee back to the UserEmployee table right given to the Admin alone
        // POST: UserEmployees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEmployee userEmployee)
        {
            if (ModelState.IsValid)
            {

                db.Entry(userEmployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag
                .EducationQualificationID = new SelectList(db.EducationQualifications, "ID", "Level", userEmployee.EducationQualificationID);
            ViewBag
                .GenderID = new SelectList(db.Genders, "ID", "GenderType", userEmployee.GenderID);
            ViewBag
                .MaritalStatusID = new SelectList(db.MaritalStatuss, "ID", "Status", userEmployee.MaritalStatusID);
            return View(userEmployee);
        }
        //Displaying the List of the Data of the Employee to be deleted from the UserEmployee Table given to the Admin alone
        // GET: UserEmployees/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEmployee userEmployee = db.UserEmployees.Find(id);
            if (userEmployee == null)
            {
                return HttpNotFound();
            }
            return View(userEmployee);
        }
        //Checking the Users table and the UserEmployee Table given to the Admin alone
        // POST: UserEmployees/Delete/5/ 
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserEmployee userEmployee = db
                .UserEmployees
                .Find(id);

            var userid = db
                .Users
                .Find(userEmployee.RegistrationID);

            if (id == null)
                return HttpNotFound();
            if (userid == null)
                return HttpNotFound();
            db.Users
                .Remove(userid);
            db.UserEmployees
                .Remove(userEmployee);
            db.SaveChanges();
            

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
