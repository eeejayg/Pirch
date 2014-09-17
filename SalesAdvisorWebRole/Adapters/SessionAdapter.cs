using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWebRole.Adapters
{
	public class SessionAdapter
	{
        private static readonly String COOKIE_PATH = "/";
        private static readonly String COOKIE_STORE = "StoreCookie";
        private static readonly String COOKIE_STORE_STORENAME = "StoreCookieStoreName";
        private static readonly String COOKIE_STORE_STORECODE = "StoreCookieStoreCode";
        private static readonly String COOKIE_LAST_LOGGED_IN_USER = "LastLoggedInUser";

        private static readonly SessionAdapter instance = new SessionAdapter();

        public static SessionAdapter getInstance()
        {
            return instance;
        }

        public static void setSessionStateBase(HttpSessionStateBase session)
        {
            instance.session = session;
        }

        private HttpSessionStateBase session = null;

        private SessionAdapter()
        {
            
        }

        public User LoggedInUser
        {
            get { return (User)session["User"]; }
            set { session["User"] = value; }
        }

        public StoreInfo GetCurrentStore(HttpRequestBase request)
        {
            StoreInfo result = null;
            HttpCookie storeCookie = request.Cookies[COOKIE_STORE];
            if (storeCookie != null) {
                result = new StoreInfo();
                result.StoreName = storeCookie.Values[COOKIE_STORE_STORENAME];
                result.StoreCode = storeCookie.Values[COOKIE_STORE_STORECODE];
            }
            return result;
        }

        public void SetCurrentStore(StoreInfo store, HttpResponseBase response)
        {
            HttpCookie storeCookie = new HttpCookie(COOKIE_STORE);
            storeCookie.Path = COOKIE_PATH;
            storeCookie.Expires = DateTime.MaxValue;
            storeCookie.Values[COOKIE_STORE_STORECODE] = store.StoreCode;
            storeCookie.Values[COOKIE_STORE_STORENAME] = store.StoreName;
            response.Cookies.Add(storeCookie);
        }

        public void SetLastLoggedInUser(String username, HttpResponseBase response)
        {
            HttpCookie userCookie = new HttpCookie(COOKIE_LAST_LOGGED_IN_USER);
            userCookie.Expires = DateTime.MaxValue;
            userCookie.Path = COOKIE_PATH;
            response.Cookies.Add(userCookie);
        }
    }
}