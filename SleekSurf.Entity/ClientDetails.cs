using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.Entity
{
    [Serializable]
    public class ClientDetails
    {
        public string ClientID { get; set; }
        public string UniqueIdentity { get; set; }
        public string ABN { get; set; }
        public string ClientName { get; set; }
        public Guid ContactPerson { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string ContactOffice { get; set; }
        public string ContactFax { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessUrl { get; set; }
        public string LogoUrl { get; set; }
        public Nullable<DateTime> EstablishedDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public CountryDetails CountryID { get; set; }
        public int? SearchHit { get; set; }
        public int? PageHit { get; set; }
        public int? FreeSearchHit { get; set; }
        public int? SMSCredit { get; set; }
        //should be changed
        public CategoryDetails Category { get; set; }
        public bool Published { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string UniqueDomain { get; set; }
        public string Theme { get; set; }

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
