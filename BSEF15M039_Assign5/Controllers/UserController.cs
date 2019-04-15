using BSEF15M039_Assign5.Entities;
using BSEF15M039_Assign5.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BSEF15M039_Assign5.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Validate Admin
        [ActionName("Login")]
        [HttpPost]
        public ActionResult Login2()
        {
            var username = Request["txtLogin"];
            var password = Request["txtPass"];
            if (username == "" || password == "") {
                if (username == "") {
                    ViewBag.userError = "*Required";
                }
                if (password == "")
                {
                    ViewBag.username = username;
                    ViewBag.passError = "*Required";
                }
                Response.Write("<script>alert('Some Fields are empty')</script>");
                return View("Login");
            }

            var admin = DAL.validateAdmin(username, password);
            if (admin != null)
            {
                Session["adminLogin"] = admin.login;
                return Redirect("~/User/UserList");
            }
            else
            {
                ViewBag.username = username;
                ViewBag.message = "Wrong username or password";
                return View("Login");
            }

        }

        //return Admin Login Screen
        public ActionResult Admin()
        {
            return View("Login");
        }

        //Return User List to Admin
        public ActionResult UserList()
        {
            if (Session["adminLogin"] == null)
            {
                Response.Write("<script>alert('Login first to continue')</script>");
                return View("Login");
            }
            else {
                List<UserDTO> list = DAL.getAllUsers();
                return View(list);
            }

        }

        //New User Screen
        public ActionResult NewUser()
        {
            UserDTO u = new UserDTO();
            return View(u);
        }

        //Logout Admin
        public ActionResult Logout()
        {
            Session["adminLogin"] = null;
            Session.Abandon();
            return View("Login");
        }

        //Save or Update User
        [ActionName("NewUser")]
        [HttpPost]
        public ActionResult SaveUser(UserDTO user)
        {
            //Getting Sports
            if (Request["cricket"] == "on")
            {
                user.cricket = true;
            }
            if (Request["hockey"] == "on")
            {
                user.hockey = true;
            }
            if (Request["chess"] == "on")
            {
                user.chess = true;
            }

            //Getting DOB
            string val = Request["DOB"];
            if (val != null)
            {
                user.DOB = DateTime.Parse(Request["dob"]);
                if (user.DOB >= DateTime.Now) {
                    ViewBag.DateError = "*Invalide Date";
                    return View("NewUser", user);
                }
            }

            //Check in the case of New User
            if (user.userid == 0) {
                if ((DAL.emailAlreadyExists(user.email) == 1) || (DAL.loginAlreadyExists(user.login) == 1) || (DAL.NICAlreadyExists(user.NIC) == 1))
                {
                    if (DAL.emailAlreadyExists(user.email) == 1)
                    {
                        ViewBag.EmailError = "*Email already exists";
                    }
                    if (DAL.loginAlreadyExists(user.login) == 1)
                    {
                        ViewBag.loginError = "*Login already exists";
                    }
                    if (DAL.NICAlreadyExists(user.NIC) == 1)
                    {
                        ViewBag.NICError = "*NIC already exists";
                    }
                    return View("NewUser", user);
                }
            }
           
            //Check if User change picture or upload for new user
            if ((user.imageName == null && Request.Files["image"].FileName != "") || (user.imageName != null && Request.Files["image"].FileName != ""))
            {
                var image = Request.Files["image"];
                var uniqueName = "";
                if (Request.Files["image"] != null)
                {
                    if (image.FileName != null)
                    {
                        var extension = System.IO.Path.GetExtension(image.FileName);
                        uniqueName = Guid.NewGuid().ToString() + extension;
                        var rootPath = Server.MapPath("~/uploadedFiles");
                        var saveFilePath = System.IO.Path.Combine(rootPath, uniqueName);
                        image.SaveAs(saveFilePath);
                    }
                }
                user.imageName = uniqueName;
            }

            //Image not changed in case of Edit
            else if (user.imageName != null && Request.Files["image"].FileName == "")
            {
                user.imageName = user.imageName;
            }

            //If No image selected
            else if (user.imageName == null && Request.Files["image"].FileName == "") {
                Response.Write("<script>alert('Select image please')</script>");
                return View("NewUser", user);
            }
            
            //Saving or Updating in database
            int id = DAL.saveUser(user);
            if (id != -1)
            {
                Session["userLogin"] = user.userid;
                if (id == 1)
                {
                    Response.Write("<script>alert('User Updated Successfully');</script>");
                    if (Session["adminLogin"] != null)
                    {
                        List<UserDTO> userList = DAL.getAllUsers();
                        return View("UserList", userList);
                    }
                    else if (Session["userLogin"] != null) {
                        return View("HomeUser", user);
                    }
                    
                }
                else {
                    Response.Write("<script>alert('Data successfully added');</script>");
                    UserDTO user1 = DAL.getUser(id);
                    TempData["user"] = user1;
                    return View("HomeUser", user);
                }
                return null;
                
            }
            else
            {
                Response.Write("<script>alert('Some problem occured');</script>");
                ViewBag.id = id;
                UserDTO user1 = DAL.getUser(id);
                TempData["user"] = user1;
                return View("HomeUser", user1);

            }
                
        }

        //Return New User Screen for Edit 
        public ActionResult EditUser() {
            UserDTO user = (UserDTO)TempData["User"];
            return View("NewUser",user);
        }

        //Edit user from List
        public ActionResult EditList( int id) {
            UserDTO user = DAL.getUser(id);
            return View("NewUser", user);
        }

        //User Home Screen
        public ActionResult HomeUser() {
            if (Session["userLogin"] == null ) {
                Response.Write("<script>alert('Login first to proceed')</script>");
                return View("Login");
            }
            return View();
        }

        //User Login Screen
        public ActionResult UserLogin() {
            return View();
        }

        //Validate User Login
        [HttpPost]
        [ActionName("UserLogin")]
        public ActionResult UserLogin2() {
            var username = Request["txtLogin"];
            var password = Request["txtPass"];
            if (username == "" || password == "")
            {
                if (username == "")
                {
                    ViewBag.userError = "*Required";
                }
                if (password == "")
                {
                    ViewBag.username = username;
                    ViewBag.passError = "*Required";
                }
                Response.Write("<script>alert('Some Fields are empty')</script>");
                return View("UserLogin");
            }
            UserDTO user = DAL.validateUser(username, password);
            if (user != null)
            {
                Session["userLogin"] = user.userid;
                TempData["user"] = user;
                return View("HomeUser", user);
            }
            else {
                ViewBag.username = username;
                ViewBag.message = "Wrong username or password";
                return View("UserLogin");
            }
            
        }

        //User Logout
        public ActionResult UserLogout() {
            Session["userLogin"] = null;
            Session.Abandon();
            return View("UserLogin");
        }

        //Sending Email
        public ActionResult SendEmail() {
            string to = Request["e_eamil"];
            int id = DAL.findUserByEmail(to);
            if (id == 0)
            {
                Response.Write("<script>alert('No such Email exists');</script>");
                return View("UserLogin");
            }
            Random rand = new Random();
            int num = rand.Next(2000,10000);
            if (BAL.SendEmail(to, "Password Reset", num.ToString()) == true)
            {
                TempData["code"] = num;
                Session["pass_id"] = id;
                return View("CodeScreen");
            }
            Response.Write("<script>alert('Some problem accured');</script>");
            return View("UserLogin");
        }

        //Return Code Screen
        public ActionResult CodeScreen() {
            var code = Request["code"];
            if (code == TempData["code"].ToString()) {
                return View("ChangePassword");
            }
            Response.Write("<script>alert('enter code first')</script>");
            return View("CodeScreen");
        }

        //Return Change Passowrd Screen
        public ActionResult ChangePassword() {
            return View();
        }


        //Validate Change Password
        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePassword2()
        {
            var new_pass = Request["NewPassword"];
            int id = Convert.ToInt32(Session["pass_id"]);
            if (DAL.updatePassword(id, new_pass) == true)
            {
                Response.Write("<script>alert('Password updated successfully');</script>");
                return View("UserLogin");
            }
            Response.Write("<script>alert('Some problem occured');</script>");
            return View();
        }

        //User Delete
        public ActionResult DeleteUser() {
            int i = Convert.ToInt32(Request["id"]);
            int result = DAL.deleteUser(i);
            if (result == 1) {
                return Json(i, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}