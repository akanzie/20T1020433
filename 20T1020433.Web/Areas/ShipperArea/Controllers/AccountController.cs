﻿using _20T1020433.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _20T1020433.Web.Areas.ShipperArea.Controllers
{
    
    public class AccountController : Controller
    {
        // GET: Shipper/Account
        private const string MESSAGE = "Message";
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
            var cookie = Converter.CookieToUserAccount(User.Identity.Name);
            if (cookie != null)
                return RedirectToAction("Index", "Home", new { area = "ShipperArea" });
            ViewBag.Message = TempData[MESSAGE] ?? "";
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
            ViewBag.Message = TempData[MESSAGE] ?? "";
            ViewBag.UserName = userName;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {

                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!");
                return View();
            }

            var userAccount = UserAccountService.Authorize(AccountTypes.Shipper, userName, password);


            if (userAccount == null)
            {
                ModelState.AddModelError("", "Đăng nhập thất bại!");
                return View();
            }
            // -> JSON
            string cookieValue = Newtonsoft.Json.JsonConvert.SerializeObject(userAccount);
            FormsAuthentication.SetAuthCookie(cookieValue, false);
            return RedirectToAction("Index", "Home", new { area = "ShipperArea" });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            //Xoa thong tin dang nhap cua nguoi dung
            Session.Clear();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "ShipperArea" });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string userName = "", string oldPassword = "", string newPassword = "", string newPass = "")
        {
            var userAccount = Converter.CookieToUserAccount(User.Identity.Name);
            if (userAccount == null)
            {
                return RedirectToAction("Login");
            }
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!");
                return View();
            }
            if (newPass != newPassword)
            {
                ModelState.AddModelError("", "Mật khẩu không khớp");
                return View();
            }
            if (oldPassword == newPassword)
            {
                ModelState.AddModelError("", "Mật khẩu mới đã trùng với mật khẩu cũ!");
                return View();
            }
            var check = UserAccountService.ChangePassword(AccountTypes.Shipper, userName, oldPassword, newPassword);
            if (check == false)
            {
                ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                return View();
            }
            //Response.Write("<script>alert('Đổi mật khẩu thành công! Vui lòng đăng nhập lại!')</script>");
            TempData[MESSAGE] = "Đổi mật khẩu thành công! Vui lòng đăng nhập lại!";
            //return View("Login");
            return RedirectToAction("Logout");

        }
    }
}