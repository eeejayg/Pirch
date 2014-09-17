using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Dapper;

using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWebRole.Models;
using SalesAdvisorWebRole.Adapters;
using SalesAdvisorWebRole.Filters;
using SalesAdvisorWebRole.Util.Auth;

namespace SalesAdvisorWebRole.Controllers
{
    public class LoginController : BaseRequirementsController
    {

        public List<User> users = new List<User>();
        public List<StoreInfo> storeCodes = new List<StoreInfo>();

        //
        // GET: /Login/

        [AllowAnonymous]
        public ActionResult Index()
        {
            StoreInfo store = SessionAdapter.getInstance().GetCurrentStore(Request);
            try
            {
                if (store.StoreCode == null)
                {
                    throw new Exception("Storecode is null, we need storecode to continue");
                }
                this.users = DBAdapter.getInstance().GetUsersByStoreCode(store.StoreCode);
                @ViewBag.users = this.users;
                @ViewBag.storeCodes = this.storeCodes;
            }
            catch (Exception e)
            {
                return this.RedirectToAction("ChooseStore", "Login");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ChooseStore()
        {
            try
            {
                this.storeCodes = DBAdapter.getInstance().GetAllStoreCodes();
                @ViewBag.storeCodes = this.storeCodes;
            } catch (Exception e) 
            {
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SetStore(String storeCode)
        {
            try
            {
                // Check the length of the store code. Limiting to 50 characters.
                if (storeCode.Length > 50) {
                    throw new Exception("Bad store code");
                }
                // Check the store code to see if it's real. SQL Injection is handled by using a stored procedure.
                List<StoreInfo> storeList = DBAdapter.getInstance().GetStoreInfoByStoreCode(storeCode);
                // If the store code is a real store code, list size will be > 0
                if (storeList.Count() > 0) {
                    // real store, set the cookie, and redirect to login page
                    SessionAdapter.getInstance().SetCurrentStore(storeList[0], Response);
                } else {
                    // not a real store, redirect to ChooseStore
                    throw new Exception("Bad store code");
                }
            } catch (Exception e) 
            {
                return RedirectToAction("ChooseStore", "Login");
            }
            // clear last logged in user.
            SessionAdapter.getInstance().SetLastLoggedInUser(null, this.Response);
            return this.RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogUserIn(LoginModel login)
        {
            string adPath = ConfigurationManager.AppSettings["connection_ldap"]; //Path to your LDAP directory server
            ViewBag.ldapaddress = adPath;
            LdapAuthentication adAuth = new LdapAuthentication(adPath);
            try
            {
                bool authed = false;
                authed = adAuth.IsAuthenticated("flinc", login.UserName, login.Password);
                if (authed)
                {
                    // authorized
                    User loggedInUser = DBAdapter.getInstance().GetUserByUsername(login.UserName);
                    SessionAdapter.getInstance().LoggedInUser = loggedInUser;
                    // Set the cookie
                    FormsAuthentication.SetAuthCookie(login.UserName, true);
                    ViewBag.hasBeenLoggedIn = "true";
                }
                
            }
            catch (Exception ex)
            {
                // fail 
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                ViewBag.hasBeenLoggedIn = "false";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
