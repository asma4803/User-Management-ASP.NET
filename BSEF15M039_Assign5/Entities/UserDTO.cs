using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSEF15M039_Assign5.Entities
{
    public class UserDTO
    {
        public int userid { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int age { get; set; }
        public string NIC { get; set; }
        public DateTime DOB { get; set; }
         public bool cricket { get; set; }
        public bool hockey { get; set; }
        public bool chess { get; set; }
        public string imageName { get; set; }
        public DateTime createdon { get; set; }
    }
}