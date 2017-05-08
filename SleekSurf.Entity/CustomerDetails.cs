using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    public class CustomerDetails
    {
        public string CustomerID { get; set; }
        public string AvatarUrl { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string ContactHome { get; set; }
        public string ContactMobile { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public CountryDetails CountryID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public ClientDetails ClientID { get; set; }
        public string Comments { get; set; }
        public string CustomerGroupID { get; set; }
        public bool SubscriptionEmail { get; set; }
        public bool SubscriptionSMS { get; set; }

        public string FullName
        {
            get { return this.FirstName + " " + this.MiddleName + " " + this.LastName; }
        }

        public string Address
        {
            get
            {
                string thisAddress;
                thisAddress = AddressLine1;
                if (!string.IsNullOrEmpty(AddressLine2))
                {
                    thisAddress += " \n" + AddressLine2;
                    if (!string.IsNullOrEmpty(AddressLine3))
                    {
                        thisAddress += "\n" + AddressLine3;
                    }
                }
                if (!string.IsNullOrEmpty(City))
                    thisAddress += ", " + City;
                if (!string.IsNullOrEmpty(State))
                    thisAddress += ", " + State;
                if (!string.IsNullOrEmpty(PostCode))
                    thisAddress += ", " + PostCode;
                return thisAddress;
            }
        }
    }
}
