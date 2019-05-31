using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone6.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string msg = "")
        {
            using (TaskListModel ORM = new TaskListModel())
            {
                ViewBag.msg = msg;

                if (Session["Logged"] == null)
                {//if not logged in, go log in
                    return RedirectToAction("LoginRegister");
                }


                User logged = (User)Session["Logged"];
                ViewBag.Valid = ORM.Users.ToList();//for selecting from a list of employees to assign a task to, to ensure it's idiot proof
                ViewBag.Tasks = ORM.Tasks.ToList().Where(t => t.Assigned == logged.Name);
                return View();
            }
        }

        public ActionResult AddTask(string whom, string desc, DateTime? due, bool complete = false)
        {
            using (TaskListModel ORM = new TaskListModel())
            {
                Task task = new Task();
                User user = ORM.Users.First(u => u.Name == whom);
                //gonna have validation elsewhere
                task.Assigned = whom;
                task.Desc = desc;
                task.Due = due;
                task.Complete = complete;
                task.User = user;

                ORM.Tasks.Add(task);
                ORM.SaveChanges();

                return RedirectToAction("Index", new { msg = "Task assigned." });
            }
        }

        public ActionResult DelTask(int id)
        {
             using(TaskListModel ORM = new TaskListModel())
            {
                Task toDel = ORM.Tasks.Single(t => t.ID == id);
                ORM.Tasks.Attach(toDel);
                ORM.Tasks.Remove(toDel);
                ORM.SaveChanges();
                return RedirectToAction("Index", new { msg = "Task removed." });
            }
        }

        public ActionResult ToggleComplete(int id)
        {
            using(TaskListModel ORM = new TaskListModel())
            {
                Task toToggle = ORM.Tasks.Single(t => t.ID == id);
                ORM.Tasks.Attach(toToggle);

                toToggle.Complete = !toToggle.Complete;

                ORM.SaveChanges();
                return RedirectToAction("Index", new { msg = "Completion toggled." });
            }
        }

        public ActionResult LoginRegister(string msg = "")
        {
            ViewBag.msg = msg;//msg comes from the login/register methods
            return View();
        }
        
        public bool CanRegister(string user)
        {
            using (TaskListModel ORM = new TaskListModel())
            {
                if(ORM.Users.Any(u => u.Name == user))
                {
                    return false;
                }
                return true;
            }
        }

        public ActionResult Register(string user, string password, string email)
        {
            using (TaskListModel ORM = new TaskListModel())
            {
                if (CanRegister(user))
                {
                    User register = new User();
                    register.Name = user;
                    register.Password = password;
                    register.Email = email;
                    Session["Logged"] = register;
                    ORM.Users.Add(register);
                    ORM.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("LoginRegister", new { msg = "That username is already taken!" });
            }
        }

        public bool TryLogin(string user, string password, out string error)
        {
            error = "";
            using (TaskListModel ORM = new TaskListModel())
            {
                if(ORM.Users.Any(u => u.Name == user))
                {//2 if statements bc im lazy but this at least provides user with feedback on what they messed up
                    if(ORM.Users.Any(u => u.Name == user && u.Password == password))
                    {
                        return true;
                    }

                    error = "Incorrect password!";
                }
                error = "That username doesn't exist.";
                return false;
            }
        }

        public ActionResult Login(string user, string password)
        {
            using (TaskListModel ORM = new TaskListModel())
            {
                string error;//message gotten from TryLogin
                if (TryLogin(user, password, out error))
                {
                    User logging = ORM.Users.Single(u => u.Name == user);
                    Session["Logged"] = logging;
                    return RedirectToAction("Index");
                }

                return RedirectToAction("LoginRegister", new { msg = error });
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}