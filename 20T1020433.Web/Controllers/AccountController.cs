using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using _20T1020433.BusinessLayers;

namespace _20T1020433.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            ViewBag.UserName = userAccount.UserName;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        //tranh chay lap vi co authorize
        [AllowAnonymous]
        //chi dinh
        [HttpPost]
        public ActionResult Login(string userName = "", string password = "")
        {
            ViewBag.UserName = userName;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {

                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!");
                return View();
            }

            var userAccount = UserAccountService.Authorize(AccountTypes.Employee, userName, password);

            if (userAccount == null)
            {
                ModelState.AddModelError("", "Đăng nhập thất bại!");
                return View();
            }

            // -> JSON
            string cookieValue = Newtonsoft.Json.JsonConvert.SerializeObject(userAccount);
            FormsAuthentication.SetAuthCookie(cookieValue, false);
            return RedirectToAction("Index", "Home");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            //Xoa thong tin dang nhap cua nguoi dung
            Session.Clear();

            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string userName = "", string oldPassword = "", string newPassword = "")
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);                   
            ViewBag.UserName = userAccount.UserName;
            userName = userAccount.UserName;
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!");
                return View();
            }                
            var check = UserAccountService.ChangePassword(AccountTypes.Employee, userName, oldPassword, newPassword);
            if(check == false)
            {
                ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                return View();
            }
            return RedirectToAction("Index", "Home");         
        }
          
    }
}