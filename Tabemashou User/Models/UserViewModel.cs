using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tabemashou_User.Models
{
    public class FollowersViewModel
    {
        public List<ProfileModel> Customers;
    }


    public class JsonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ProfileModel
    {
        public string Username { get; set; }
        public decimal IdCard { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string Nationality { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public decimal AccountNumber { get; set; }
        public decimal Followers { get; set; }
        public decimal Following { get; set; }
        public decimal Reviews { get; set; }
        public List<Review> Timeline { get; set; }
    }
}