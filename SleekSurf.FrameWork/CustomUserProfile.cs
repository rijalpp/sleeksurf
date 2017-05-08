using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;

namespace SleekSurf.FrameWork
{
    public class CustomUserProfile : ProfileBase
    {
        public static CustomUserProfile GetUserProfile(string username)
        {
            return Create(username) as CustomUserProfile;
        }

        [SettingsAllowAnonymous(false)]
        public string ProfileUrl
        {
            get { return base["ProfileUrl"] as string; }
            set { base["ProfileUrl"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string Title
        {
            get { return base["Title"] as string; }
            set { base["Title"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string FirstName
        {
            get { return base["FirstName"] as string; }
            set { base["FirstName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string MiddleName
        {
            get { return base["MiddleName"] as string; }
            set { base["MiddleName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string LastName
        {
            get { return base["LastName"] as string; }
            set { base["LastName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public DateTime? DOB
        {
            get { return base["DOB"] as DateTime?; }
            set { base["DOB"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string Gender
        {
            get { return base["Gender"] as string; }
            set { base["Gender"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string Occupation
        {
            get { return base["Occupation"] as string; }
            set { base["Occupation"] = value; }
        }
        [SettingsAllowAnonymous(false)]
        public string WebSiteUrl
        {
            get { return base["WebSiteUrl"] as string; }
            set { base["WebSiteUrl"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public DateTime? CreatedDate
        {
            get { return base["CreatedDate"] as DateTime?; }
            set { base["CreatedDate"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public Guid? CreatedBy
        {
            get { return base["CreatedBy"] as Guid?; }
            set { base["CreatedBy"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public Guid? UpdatedBy
        {
            get { return base["UpdatedBy"] as Guid?; }
            set { base["UpdatedBy"] = value; }
        }
        [SettingsAllowAnonymous(false)]
        public string IPAddress
        {
            get { return base["IPAddress"].ToString(); }
            set { base["IPAddress"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public UserAddress Address
        {
            get { return base["Address"] as UserAddress; }
            set { base["Address"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public UserContacts Contacts
        {
            get { return base["Contacts"] as UserContacts; }
            set { base["Contacts"] = value; }
        }

        [SettingsAllowAnonymous(true)]
        public UserPreferences Preferences
        {
            get { return base["Preferences"] as UserPreferences; }
            set { base["Preferences"] = value; }
        }

        public class UserAddress : ProfileGroupBase
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostCode { get; set; }
            public string Country { get; set; }
        }

        public class UserContacts : ProfileGroupBase
        {
            public string ContactHome { get; set; }
            public string ContactMobile { get; set; }
        }

        public class UserPreferences : ProfileGroupBase
        {
            public string Theme { get; set; }
            public string Culture { get; set; }
        }
    }
}
