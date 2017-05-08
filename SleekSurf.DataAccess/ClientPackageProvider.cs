using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SleekSurf.FrameWork;
using SleekSurf.Entity;
using System.Data;
using System.Data.Common;

namespace SleekSurf.DataAccess
{
    public abstract class ClientPackageProvider : DataAccess
    {
        static ClientPackageProvider _instance = null;
        static public ClientPackageProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (ClientPackageProvider)Activator.CreateInstance(Type.GetType(Globals.Settings.Package.ProviderType));
                return _instance;
            }
        }

        public ClientPackageProvider()
        {
            this.ConnectionString = Globals.Settings.Package.ConnectionString;
            this.EnableCaching = Globals.Settings.Package.EnableCaching;
            this.CacheDuration = Globals.Settings.Package.CacheDuration;
        }

        #region  METHODS THAT WORK WITH Packages

        public abstract PackageDetails SelectPackage(string packageCode);
        public abstract PackageDetails SelectPackageByFeatureType(string featureType);

        public abstract List<PackageDetails> SelectAllPackages(PagingDetails pgObj);

        public abstract List<PackageDetails> SelectAllPackagesWithoutPaging();

        public abstract List<PackageDetails> SelectPackagesByPublication(bool published, PagingDetails pgObj);

        public abstract List<PackageDetails> SelectPackagesByPublication(bool published);

        public abstract int InsertPackage(PackageDetails package);

        public abstract int UpdatePackage(PackageDetails package);

        public abstract int UpdatePackageByPublication(string packageCode, bool published);

        public abstract int DeletePackage(string packageCode);

        protected virtual PackageDetails GetPackageFromReader(IDataReader reader)
        {
            PackageDetails package = new PackageDetails();

            package.PackageCode = reader["PackageCode"].ToString();
            package.PackageName = reader["PackageName"].ToString();
            package.Description = reader["Description"].ToString();
            package.Published = (bool)reader["Published"];
            package.Status = reader["Status"].ToString();
            package.FeatureType = reader["FeatureType"].ToString();
            package.CreatedDate = (DateTime?)reader["CreatedDate"];
            package.CreatedBy = reader["CreatedBy"].ToString();
            if (reader["UpdatedDate"] != DBNull.Value)
                package.UpdatedDate = (DateTime?)reader["UpdatedDate"];
            package.UpdatedBy = reader["UpdatedBy"].ToString();

            return package;
        }

        protected virtual List<PackageDetails> GetPackageCollectionFromReader(IDataReader reader)
        {
            List<PackageDetails> packages = new List<PackageDetails>();
            while (reader.Read())
                packages.Add(GetPackageFromReader(reader));
            return packages;
        }

        #endregion

        #region METHODS THAT WORK WITH PACKAGEOPTION

        public abstract int InsertPackageOption(PackageOptionDetails option);

        public abstract int UpdatePackageOption(PackageOptionDetails option);

        public abstract int DeletePackageOption(int packageOptionID);

        public abstract PackageOptionDetails SelectPackageOption(int packageOptionID);

        public abstract PackageOptionDetails SelectPackageOption(string promoCode);

        public abstract List<PackageOptionDetails> SelectPackageOptionDetailsByPackage(string packageCode);

        public abstract List<PackageOptionDetails> SelectPackageOptionsByPublication(string packageCode, bool published);

        protected virtual PackageOptionDetails GetPackageOptionFromReader(IDataReader reader)
        {
            PackageOptionDetails options = new PackageOptionDetails();

            options.PackageOptionID = (int)reader["PackageOptionID"];
            options.PackageCode = reader["PackageCode"].ToString();
            options.Duration = reader["Duration"].ToString();
            if (reader["StandardPrice"] != DBNull.Value)
                options.StandardPrice = Convert.ToDecimal(reader["StandardPrice"]);
            if (reader["DiscountPercentage"] != DBNull.Value)
                options.DiscountPercentage = Convert.ToDouble(reader["DiscountPercentage"]);
            options.PromoCode = reader["PromoCode"].ToString();
            options.Published = (bool)reader["Published"];
            if (reader["PromoCodeStartDate"] != DBNull.Value)
                options.PromoCodeStartDate = (DateTime?)reader["PromoCodeStartDate"];
            if (reader["PromoCodeEndDate"] != DBNull.Value)
                options.PromoCodeEndDate = (DateTime?)reader["PromoCodeEndDate"];
            options.Comments = reader["Comments"].ToString();
            return options;
        }

        protected virtual List<PackageOptionDetails> GetPackageOptionCollectionFromReader(IDataReader reader)
        {
            List<PackageOptionDetails> packagesOptions = new List<PackageOptionDetails>();
            while (reader.Read())
                packagesOptions.Add(GetPackageOptionFromReader(reader));
            return packagesOptions;
        }
        #endregion

        #region METHODS THAT WORK WITH PackageOrders

        public abstract int InsertPackageOrder(PackageOrderDetails packageOrder);
        public abstract int UpdatePackageOrder(PackageOrderDetails packageOrder);
        public abstract List<PackageOrderDetails> SelectPackageOrderByClientID(string clientID, PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectPackageOrderByStatusAndDays(string orderStatus, int days, PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectLatestPackageOrderPerClient(string orderStatus, DateTime fromDate, PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectExpiringPackageOrders(PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectPackageOrdersWithExpiryNoticeRange(PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectExpiringPackageOrdersWithClientName(string clientID, PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectPackageOrdersWithExpirationNoticeRangeByClientName(string clientName, PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectRecentDistinctOrdersByClientID(string clientID);//SELECTS ALL DISTINCT ORDERS OF CLIENT BY PACKAGECODE
        public abstract PackageOrderDetails SelectPackageOrder(string orderID, string clientID);
        public abstract PackageOrderDetails SelectPackageOrder(string orderID);
        public abstract PackageOrderDetails SelectRecentPackageOrder(string clientID, string packageCode);//VERIFIED ONLY
        public abstract PackageOrderDetails SelectMostRecentPackageOrder(string clientID);//ANY ORDER
        public abstract PackageOrderDetails SelectMostRecentIncompletePackageOrder(string clientID); //ONLY INCOMPLETE ORDERS
        public abstract PackageOrderDetails SelectPackageorderWithHighestExpiryDate(string clientID);
        public abstract int DeletePackageOrder(string orderID);
        public abstract DataTable RemindForRenewalOfPackageOrder();
        public abstract List<PackageOrderDetails> SelectExpiredAccounts();
        public abstract List<PackageOrderDetails> SelectExpiredPackageOrders(PagingDetails pgObj);
        public abstract List<PackageOrderDetails> SelectExpiredPackageOrdersWithClientName(string clientName, PagingDetails pgObj);
        public abstract int UpdateAccountExpiryNotice(string orderID, string clientID, int nextNoticeDaysLeft);

        protected virtual PackageOrderDetails GetPackageOrderFromReader(IDataReader reader)
        {
            PackageOrderDetails packageOrder = new PackageOrderDetails();
            packageOrder.OrderID = reader["OrderID"].ToString();
            packageOrder.CreatedDate = (DateTime)reader["CreatedDate"];
            packageOrder.CreatedBy = reader["CreatedBy"].ToString();
            packageOrder.OrderStatus = reader["OrderStatus"].ToString();
            packageOrder.Client = new ClientDetails() { ClientID = reader["ClientID"].ToString() };
            if (reader["ReceiptNo"] != DBNull.Value)
                packageOrder.ReceiptNo = reader["ReceiptNo"].ToString();
            packageOrder.TransactionID = reader["TransactionID"].ToString();
            packageOrder.PaymentOption = reader["PaymentOption"].ToString();
            if (reader["StandardPrice"] != DBNull.Value)
                packageOrder.StandardPrice = decimal.Parse(reader["StandardPrice"].ToString());
            if (reader["DiscountPercentage"] != DBNull.Value)
                packageOrder.DiscountPercentage = double.Parse(reader["DiscountPercentage"].ToString());
            if (reader["FinalPrice"] != DBNull.Value)
                packageOrder.FinalPrice = decimal.Parse(reader["FinalPrice"].ToString());
            if (reader["AmountDeducted"] != DBNull.Value)
                packageOrder.AmountDeducted = decimal.Parse(reader["AmountDeducted"].ToString());
            if (reader["FinalPriceAfterDeduction"] != DBNull.Value)
                packageOrder.FinalPriceAfterDeduction = decimal.Parse(reader["FinalPriceAfterDeduction"].ToString());
            packageOrder.RegistrationDate = (DateTime)reader["RegistrationDate"];
            packageOrder.ExpiryDate = (DateTime)reader["ExpiryDate"];
            packageOrder.PackageCode = reader["PackageCode"].ToString();
            packageOrder.PackageName = reader["PackageName"].ToString();
            packageOrder.Duration = reader["Duration"].ToString();
            if (reader["PromoCodeStartDate"] != DBNull.Value)
                packageOrder.PromoCode = reader["PromoCode"].ToString();
            if (reader["PromoCodeStartDate"] != DBNull.Value)
                packageOrder.PromoCodeStartDate = (DateTime)reader["PromoCodeStartDate"];
            if (reader["PromoCodeEndDate"] != DBNull.Value)
                packageOrder.PromoCodeEndDate = (DateTime)reader["PromoCodeEndDate"];
            packageOrder.Comments = reader["Comments"].ToString();

            return packageOrder;
        }

        protected virtual List<PackageOrderDetails> GetPackageOrderCollectionFromReader(IDataReader reader)
        {
            List<PackageOrderDetails> packageOrders = new List<PackageOrderDetails>();
            while (reader.Read())
                packageOrders.Add(GetPackageOrderFromReader(reader));
            return packageOrders;
        }
        #endregion

        #region METHODS THAT WORK WITH PACKAGEPICTURES

        public abstract int InsertPackagePicture(string pictureID, string pictureDescription);
        public abstract int UpdatePackagePicture(string pictureID, string packageCode, string pictureDescription, int displayOrder);
        public abstract int DeletePackagePicture(string pictureID);
        public abstract int AssignPictureToPackage(string pictureID, string packageCode, int displayOrder);
        public abstract int RemovePictureFromPackage(string pictureID, string packageCode);
        public abstract DataTable SelectPackagePicture(string pictureID);
        public abstract DataTable SelectAllPackagePicture();
        public abstract DataTable SelectPackagePicturesByPackage(string packageCode);
        public abstract int UpdatePackagePictureDisplayOrder(string pictureID, string packageCode, int displayOrder);

        #endregion

    }
}
