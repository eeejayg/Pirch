using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

using Microsoft.WindowsAzure.ServiceRuntime;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Services;
using SalesAdvisorWorkerRole.Logging;

namespace SalesAdvisorWorkerRole.Adapters
{
    /*
     * Responsible for dealing with REST calls to the fixtures living backend
     */
    public class FLJsonAdapter
    {
        private static readonly FLJsonAdapter instance = new FLJsonAdapter();
        // Endpoints - No forward slash at beginning
        private static String ENDPOINT_LIST_USERS = "api/SystemUser/GetAdvisors?inventorysiteid={0}";
        private static String ENDPOINT_GET_USER_BY_NAME = "api/Account/SimpleSearch?search={0}";
        private static String ENDPOINT_UPDATE_USER = "api/Account/";

        public static FLJsonAdapter getInstance() {
            return instance;
        }

        // instance stuff
        private String serverUrl;
        private String _storeId;
        public String storeId
        {
            set { this._storeId = value; }
            get { return this._storeId; }
        }

        private FLJsonAdapter()
        {
            this.serverUrl = RoleEnvironment.GetConfigurationSettingValue("FLEndpointServer");
        }

        private String ConstructEndpointUrl(String endpoint)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.serverUrl);
            builder.Append(endpoint);
            return builder.ToString();
        }

        // get endpoint strings

        private String GetJsonStringFromEndpoint(String endpoint)
        {
            return new WebClient().DownloadString(endpoint);
        }

        private Dictionary<String, dynamic> ParseJson(String data)
        {
            return new JavaScriptSerializer().Deserialize<Dictionary<String, dynamic>>(data);
        }

        // API stuff

        /// <summary>
        /// Gets a list of users by store ID
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public List<User> GetUsersByStore(String storeId)
        {
            String endpoint = this.ConstructEndpointUrl(ENDPOINT_LIST_USERS);
            String data = this.GetJsonStringFromEndpoint(String.Format(endpoint, storeId));
            Dictionary<String, dynamic> response = this.ParseJson(data);
            // We shuld do something with roles at some point
            ArrayList roles = response["roles"];
            ArrayList titles = response["titles"];
            ArrayList users = response["value"];
            List<User> userlist = new List<User>();
            //
            UserServiceWorker worker = new UserServiceWorker();
            foreach (Dictionary<String, dynamic> userdata in users) {
                userlist.Add(this.ParseUser(userdata, titles, storeId, worker));
            }
            //
            return userlist;
        }

        private User ParseUser(Dictionary<String, dynamic> userData, ArrayList titles, String storecode, UserServiceWorker worker)
        {
            String[] usernameParts = ((String)userData["domainname"]).Split('\\');
            String username = usernameParts[usernameParts.Length - 1];
            String guid = userData["systemuserid"];
            User user = worker.GetUserByGuid(guid);
            if (user == null) {
                user = new User();
            }
            user.userguid = guid;
            user.userName = username;
            user.firstName = userData["firstname"];
            user.lastName = userData["lastname"];
            user.storeCode = storecode;
            user.imagePath = userData["portraitpath"];
            user.title = (String)titles[(int)userData["fix_title"]];
            user.primaryphone = userData["primaryphone"];
            user.email = userData["internalemailaddress"];
            //
            return user;
        }

        //--------------
        // CUSTOMERS
        //--------------

        public Customers GetCustomerByName(String name)
        {
            Customers result = null;
            //
            String endpoint = this.ConstructEndpointUrl(ENDPOINT_GET_USER_BY_NAME);
            String encodedName = HttpUtility.UrlEncode(name);
            String data = this.GetJsonStringFromEndpoint(String.Format(endpoint, encodedName));
            Dictionary<String, dynamic> response = this.ParseJson(data);
            ArrayList customers = (ArrayList)response["values"];
            if (customers.Count > 0) {
                Dictionary<String, dynamic> first = (Dictionary<String, dynamic>)customers[0];
                try
                {
                    result = this.ParseCustomer(first);
                }
                catch (KeyNotFoundException e)
                {
                    DebugLog.Log(String.Format("Error trying to parse customer response from FLAPI, message: {0}", e.Message.ToString()));
                    result = null;
                }
            }
            //
            return result;
        }

        private Customers ParseCustomer(Dictionary<String, dynamic> first)
        {
            Customers result = new Customers();
            result.Id = 0;
            result.CustomerGuid = first["accountid"];
            // deal with address
            Address address = new Address();
            result.Addresses = new List<Address>();
            result.Addresses.Add(address);
            address.AddressLine1 = first["address1_line1"];
            address.AddressLine2 = first["address1_line2"];
            address.AddressLine3 = first["address1_line3"];
            address.City = first["address1_city"];
            address.Country = first["address1_country"];
            address.PostalCode = first["address1_postalcode"];
            address.State = first["address1_stateorprovince"];
            result.DefaultShippingAddress = address;
            // deal with email
            Emails email = new Emails();
            email.Address = first["emailaddress1"];
            result.Emails = new List<Emails>();
            result.Emails.Add(email);
            // deal with name
            String[] names = ((String)first["name"]).Split(" ".ToCharArray());
            result.LastName = names[names.Count() - 1];
            List<String> poppable = new List<String>(names);
            poppable.RemoveAt(poppable.Count - 1);
            result.FirstName = String.Join(" ", poppable.ToArray<String>());
            // deal with phone number
            PhoneNumbers phone = new PhoneNumbers();
            phone.PhoneNumber = first["address1_telephone1"];
            result.PhoneNumbers = new List<PhoneNumbers>();
            result.PhoneNumbers.Add(phone);
            //
            return result;
        }

        public void UpdateCustomer(Customers customer)
        {
            this.CreateOrUpdateCustomer(customer, "PUT");
        }

        public void CreateCustomer(Customers customer)
        {
            this.CreateOrUpdateCustomer(customer, "POST");
        }

        private void CreateOrUpdateCustomer(Customers customer, String method)
        {
            String json = this.CustomerToJsonString(customer);
            HttpWebRequest rq = this.GetRequestForEndpoint(this.ConstructEndpointUrl(ENDPOINT_UPDATE_USER));
            rq.Method = method;
            this.WriteToPost(json, rq);
        }

        private String CustomerToJsonString(Customers customer)
        {
            String result = null;
            //
            Dictionary<String, String> data = new Dictionary<String, String>();
            data["name"] = customer.Name();
            data["accountid"] = customer.CustomerGuid;
            if (customer.Addresses != null) {
                if (customer.Addresses.Count > 0) {
                    Address address = customer.Addresses[0];
                    data["address1_line1"] = address.AddressLine1;
                    data["address1_line2"] = address.AddressLine2;
                    data["address1_line3"] = address.AddressLine3;
                    data["address1_city"] = address.City;
                    data["address1_country"] = address.Country;
                    data["address1_postalcode"] = address.PostalCode;
                    data["address1_stateorprovince"] = address.State;
                }
            }
            if (customer.Emails != null) {
                if (customer.Emails.Count > 0) {
                    data["emailaddress1"] = customer.Emails[0].Address;
                }
            }
            if (customer.PhoneNumbers != null) {
                if (customer.PhoneNumbers.Count > 0) {
                    data["address1_telephone1"] = customer.PhoneNumbers[0].PhoneNumber;
                }
            }
            result = new JavaScriptSerializer().Serialize(data);
            //
            return result;
        }

        private HttpWebRequest GetRequestForEndpoint(String endpoint)
        {
            HttpWebRequest rq = null;
            //
            rq = (HttpWebRequest)WebRequest.Create(endpoint);
            rq.ContentType = "application/json; charset=utf-8";
            //
            return rq;
        }

        private void WriteToPost(String data, HttpWebRequest request)
        {
            request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            byte[] dataBytes = new UTF8Encoding().GetBytes(data);
            postStream.Write(dataBytes, 0, dataBytes.Length);
            postStream.Flush();
            postStream.Close();
        }
    }
}