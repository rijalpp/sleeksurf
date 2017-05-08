using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SleekSurf.Entity;
using SleekSurf.DataAccess;
using SleekSurf.FrameWork;
using System.Data;

namespace SleekSurf.Manager
{
    public class ClientPackageManager : BasePackage
    {
        #region ALL METHODS OF PACKAGE

        public static Result<PackageDetails> SelectPackage(string packageCode)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            string key = "packages_package_" + packageCode;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageDetails>)BizObject.Cache[key];
                }
                else
                {
                    PackageDetails packageDetails = SiteProvider.Packages.SelectPackage(packageCode);
                    if (packageDetails != null)
                    {
                        result.EntityList.Add(SiteProvider.Packages.SelectPackage(packageCode));
                        BasePackage.CacheData(key, result.EntityList);
                    }
                }
                if (result != null && result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                    result.Status = ResultStatus.Success;
                }
                else if (result != null && result.EntityList.Count == 0)
                    result.Message = "No entry in Database";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> SelectPackageByFeatureType(string featureType)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            string key = "packages_package_" + featureType;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageDetails>)BizObject.Cache[key];
                }
                else
                {
                    PackageDetails package = SiteProvider.Packages.SelectPackageByFeatureType(featureType);
                    if (package != null)
                    {
                        result.EntityList.Add(package);
                        BasePackage.CacheData(key, result.EntityList);
                        result.Status = ResultStatus.Success;
                        result.Message = "Records successfully retrieved from Database.";
                    }
                    else
                        result.Message = "No entry in Database";
                }  
                
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> SelectAllPackage(PagingDetails pgObj)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            string key = "packages_package_All_" + pgObj.StartRowIndex + pgObj.PageSize;
            string keyCount = "packages_package_All_Count";
            try
            {
                if (BasePackage.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PackageDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectAllPackages(pgObj);
                    BasePackage.CacheData(key, result.EntityList);
                    BasePackage.CacheData(key, pgObj.TotalNumber);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> SelectAllPackageWithoutPaging()
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            string key = "packages_package_All_";
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectAllPackagesWithoutPaging();
                    BasePackage.CacheData(key, result.EntityList);
                }

                if (result.EntityList.Count > 0)
                {
                    result.Message = "Records successfully retrieved from Database.";
                }
                else if (result.EntityList.Count == 0)
                {
                    result.Message = "No entry in Database.";
                }
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> SelectPackagesByPublication(bool published, PagingDetails pgObj)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            string key = "packages_package_IsPublished_" + published.ToString() + "_" + pgObj.StartRowIndex.ToString() + "_" + pgObj.PageSize.ToString();
            string keyCount = "packages_package_IsPublished_Count_" + published.ToString() + "_" + pgObj.StartRowIndex.ToString() + "_" + pgObj.PageSize.ToString();
            try
            {
                if (BasePackage.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PackageDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectPackagesByPublication(published, pgObj);
                    BasePackage.CacheData(key, result.EntityList);
                    BasePackage.CacheData(keyCount, pgObj.TotalNumber);
                }
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from the database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No entry in the Database.";
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> SelectPackagesByPublication(bool published)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            string key = "packages_package_IsPublished_" + published.ToString();
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectPackagesByPublication(published);
                    BasePackage.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from the database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No entry in the Database.";
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> InsertPackage(PackageDetails package)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            try
            {
                int i = SiteProvider.Packages.InsertPackage(package);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(package);
                    result.Message = "Congratulations! The Package details have been added.";
                    BizObject.PurgeCacheItems("packages_package");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }

            return result;
        }

        public static Result<PackageDetails> UpdatePackage(PackageDetails package)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            try
            {
                int i = SiteProvider.Packages.UpdatePackage(package);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(package);
                    result.Message = "Congratulations! The Package Details have been updated.";
                    BizObject.PurgeCacheItems("packages_package");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> UpdatePackageByPublication(string packageCode, bool published)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            try
            {
                int i = SiteProvider.Packages.UpdatePackageByPublication(packageCode, published);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "Congratulations! The Package Details have been updated.";
                    BizObject.PurgeCacheItems("packages_package");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageDetails> DeletePackage(string packageCode)
        {
            Result<PackageDetails> result = new Result<PackageDetails>();
            try
            {
                int i = SiteProvider.Packages.DeletePackage(packageCode);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Package details have been successfully deleted.";
                    BizObject.PurgeCacheItems("packages_package");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }
        #endregion

        #region ALL METHODS OF PACKAGEOPTION

        public static Result<PackageOptionDetails> InsertPackageOption(PackageOptionDetails option)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            try
            {
                int i = SiteProvider.Packages.InsertPackageOption(option);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(option);
                    result.Message = "Congratulations! The Package Option details have been added.";
                    BizObject.PurgeCacheItems("packageoptions_option");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }

            return result;
        }

        public static Result<PackageOptionDetails> UpdatePackageOption(PackageOptionDetails option)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            try
            {
                int i = SiteProvider.Packages.UpdatePackageOption(option);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(option);
                    result.Message = "Congratulations! The Package Option Details have been updated.";
                    BizObject.PurgeCacheItems("packageoptions_option");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOptionDetails> DeletePackageOption(int packageOptionID)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            try
            {
                int i = SiteProvider.Packages.DeletePackageOption(packageOptionID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The Package Option details have been successfully deleted.";
                    BizObject.PurgeCacheItems("packageoptions_option");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOptionDetails> SelectPackageOption(int packageOptionID)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            try
            {
                result.EntityList.Add(SiteProvider.Packages.SelectPackageOption(packageOptionID));

                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No entry in Database";
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOptionDetails> SelectPackageOption(string promoCode)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            string key = "packageoptions_option_" + promoCode;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageOptionDetails>)BizObject.Cache[key];
                }
                else
                {
                    result.EntityList.Add(SiteProvider.Packages.SelectPackageOption(promoCode));
                    BasePackage.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No entry in Database";
                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOptionDetails> SelectPackageOptionsByPackage(string packageCode)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            string key = "packageoptions_option_" + packageCode;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                    result.EntityList = (List<PackageOptionDetails>)BizObject.Cache[key];
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectPackageOptionDetailsByPackage(packageCode);
                    if (result.EntityList.Count > 0)
                        BasePackage.CacheData(key, result.EntityList);
                    else
                        BasePackage.CacheData(key, null);
                }
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No entry in Database";
                result.Status = ResultStatus.Success;


            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }

            return result;
        }

        public static Result<PackageOptionDetails> SelectPackageOptionsByPublication(string packageCode, bool published)
        {
            Result<PackageOptionDetails> result = new Result<PackageOptionDetails>();
            string key = "packageoptions_option_" + packageCode + published.ToString();
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                    result.EntityList = (List<PackageOptionDetails>)BizObject.Cache[key];
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectPackageOptionsByPublication(packageCode, published);
                    BasePackage.CacheData(key, result.EntityList);
                }
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No entry in Database";
                result.Status = ResultStatus.Success;


            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }

            return result;
        }
        #endregion

        #region ALL METHODS OF PACKAGEORDERS

        public static Result<PackageOrderDetails> InsertPackageOrder(PackageOrderDetails orderDetails)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                int i = SiteProvider.Packages.InsertPackageOrder(orderDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(orderDetails);
                    result.Message = "Congratulations! The Package Order Details have been added.";
                    BizObject.PurgeCacheItems("packageorderdetails_packageorderdetail");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> UpdatePackageOrder(PackageOrderDetails orderDetails)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                int i = SiteProvider.Packages.UpdatePackageOrder(orderDetails);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.EntityList.Add(orderDetails);
                    result.Message = "Congratulations! The Package Order Details have been updated.";
                    BizObject.PurgeCacheItems("packageorderdetails_packageorderdetail");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageOrderByClientID(string clientID, PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            string key = "packageorderdetails_packageorderdetail_" + clientID + pgObj.StartRowIndex.ToString() + pgObj.PageSize.ToString();
            string keyCount = "packageorderdetails_packageorderdetail_count_" + clientID + pgObj.StartRowIndex.ToString() + pgObj.PageSize.ToString();
            try
            {
                if (BasePackage.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PackageOrderDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectPackageOrderByClientID(clientID, pgObj);

                    BasePackage.CacheData(key, result.EntityList);
                    BasePackage.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";

            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageOrderByStatusAndDays(string orderStatus, int days, PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            string key = "packageorderdetails_packageorderdetail_" + orderStatus + days.ToString() + pgObj.StartRowIndex.ToString() + pgObj.PageSize.ToString();
            string keyCount = "packageorderdetails_packageorderdetail_count_" + orderStatus + days.ToString() + pgObj.StartRowIndex.ToString() + pgObj.PageSize.ToString();
            try
            {
                if (BasePackage.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PackageOrderDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectPackageOrderByStatusAndDays(orderStatus, days, pgObj);

                    BasePackage.CacheData(key, result.EntityList);
                    BasePackage.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";

            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectLatestPackageOrderPerClient(string orderStatus, DateTime fromDate, PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            string key = "packageorderdetails_packageorderdetail_" + orderStatus + fromDate.ToString() + pgObj.StartRowIndex.ToString() + pgObj.PageSize.ToString();
            string keyCount = "packageorderdetails_packageorderdetail_count_" + orderStatus + fromDate.ToString() + pgObj.StartRowIndex.ToString() + pgObj.PageSize.ToString();
            try
            {
                if (BasePackage.Settings.EnableCaching && (BizObject.Cache[key] != null && BizObject.Cache[keyCount] != null))
                {
                    result.EntityList = (List<PackageOrderDetails>)BizObject.Cache[key];
                    pgObj.TotalNumber = (int)BizObject.Cache[keyCount];
                }
                else
                {
                    result.EntityList = SiteProvider.Packages.SelectLatestPackageOrderPerClient(orderStatus, fromDate, pgObj);

                    BasePackage.CacheData(key, result.EntityList);
                    BasePackage.CacheData(keyCount, pgObj.TotalNumber);
                }
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";

            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectExpiringPackageOrders(PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectExpiringPackageOrders(pgObj);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageOrdersWithExpiryNoticeRange(PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectPackageOrdersWithExpiryNoticeRange(pgObj);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectExpiringPackageOrdersWithClientName(string clientName, PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectExpiringPackageOrdersWithClientName(clientName, pgObj);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageOrdersWithExpirationNoticeRangeByClientName(string clientName, PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectPackageOrdersWithExpirationNoticeRangeByClientName(clientName, pgObj);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectRecentDistinctOrdersByClientID(string clientID)//SELECTS DISTINCT ORDERS BY PACKAGECODE 
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectRecentDistinctOrdersByClientID(clientID);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved from the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record found in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageOrder(string orderID, string clientID)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            PackageOrderDetails orderDetail = new PackageOrderDetails();
            string key = "packageorderdetails_packageorderdetail_" + orderID + clientID;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageOrderDetails>)BizObject.Cache[key];
                    result.Message = "Records retrieved from Cache.";
                }
                else
                {
                    //GET THE ORDER DETAIL FOR DATAACCESS
                    orderDetail = SiteProvider.Packages.SelectPackageOrder(orderID, clientID);
                    //FINALLY ADD THE ORDER DETAIL TO THE RESULT ENTITYLIST
                    result.EntityList.Add(orderDetail);
                    BasePackage.CacheData(key, result.EntityList);
                    result.Message = "Records retrieved from Database.";
                }

                result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageOrder(string orderID)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            PackageOrderDetails orderDetail = new PackageOrderDetails();
            string key = "packageorderdetails_packageorderdetail_" + orderID;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    result.EntityList = (List<PackageOrderDetails>)BizObject.Cache[key];
                    result.Status = ResultStatus.Success;
                }
                else
                {
                    //GET THE ORDER DETAIL FOR DATAACCESS
                    orderDetail = SiteProvider.Packages.SelectPackageOrder(orderID);
                    //FINALLY ADD THE ORDER DETAIL TO THE RESULT ENTITYLIST
                    if (orderDetail != null)
                    {
                        result.EntityList.Add(orderDetail);
                        BasePackage.CacheData(key, result.EntityList);
                        result.Status = ResultStatus.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectRecentPackageOrder(string clientID, string packageCode)//VERIFIED ONLY
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            PackageOrderDetails orderDetail = new PackageOrderDetails();
            try
            {
                orderDetail = SiteProvider.Packages.SelectRecentPackageOrder(clientID, packageCode);
                //FINALLY ADD THE ORDER DETAIL TO THE RESULT ENTITYLIST
                if (orderDetail != null)
                {
                    result.EntityList.Add(orderDetail);
                    result.Status = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectPackageorderWithHighestExpiryDate(string clientID)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            PackageOrderDetails orderDetail = new PackageOrderDetails();
            try
            {
                orderDetail = SiteProvider.Packages.SelectPackageorderWithHighestExpiryDate(clientID);
                //FINALLY ADD THE ORDER DETAIL TO THE RESULT ENTITYLIST
                if (orderDetail != null)
                {
                    result.EntityList.Add(orderDetail);
                    result.Status = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectMostRecentPackageOrder(string clientID)//ANY TRANSACTION
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            PackageOrderDetails orderDetail = new PackageOrderDetails();
            try
            {
                orderDetail = SiteProvider.Packages.SelectMostRecentPackageOrder(clientID);
                //FINALLY ADD THE ORDER DETAIL TO THE RESULT ENTITYLIST
                if (orderDetail != null)
                {
                    result.EntityList.Add(orderDetail);
                    result.Status = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectMostRecentIncompletePackageOrder(string clientID)//ONLY INCOMPLETE 
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            PackageOrderDetails orderDetail = new PackageOrderDetails();
            try
            {
                orderDetail = SiteProvider.Packages.SelectMostRecentIncompletePackageOrder(clientID);
                //FINALLY ADD THE ORDER DETAIL TO THE RESULT ENTITYLIST
                if (orderDetail != null)
                {
                    result.EntityList.Add(orderDetail);
                    result.Status = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> DeletePackageOrder(string orderID)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                int i = SiteProvider.Packages.DeletePackageOrder(orderID);
                if (i > 0)
                {
                    result.Status = ResultStatus.Success;
                    result.Message = "The OrderDetail have been deleted.";
                    BizObject.PurgeCacheItems("packageorderdetails_packageorderdetail");
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }

            return result;
        }

        public static string GetPayPalPaymentUrl(PackageOrderDetails order)
        {
            //ShoppingCart cart = new ShoppingCart();
            string serverUrl = (Globals.Settings.Package.SandBoxMode ? "https://www.sandbox.paypal.com/us/cgi-bin/webscr" : "https://www.paypal.com/us/cgi-bin/webscr");

            string amount = order.FinalPrice.ToString("N2").Replace(',', '.');
            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "") + HttpContext.Current.Request.ApplicationPath;
            if (!baseUrl.EndsWith("/"))
                baseUrl += "/";

            string notifyUrl = HttpUtility.UrlEncode(baseUrl + "WebPages/PayPal/PayPalIPNHandler.ashx");
            string returnUrl = HttpUtility.UrlEncode(baseUrl + "Admin/Client/AccountManagement.aspx?ID=" + order.OrderID);
            string cancelUrl = HttpUtility.UrlEncode(baseUrl + "WebPages/PayPal/OrderCancelled.aspx");
            string business = HttpUtility.UrlEncode(Globals.Settings.Package.BusinessEmail);
            string itemName = HttpUtility.UrlEncode("Order #" + order.OrderID);

            StringBuilder url = new StringBuilder();
            url.AppendFormat(
                  "{0}?cmd=_xclick&upload=1&rm=2&no_shipping=1&no_note=1&currency_code={1}&" +
                    "business={2}&item_number={3}&custom={3}&item_name={4}&amount={5}" +
                    "&notify_url={6}&return={7}&cancel_return={8}", serverUrl, Globals.Settings.Package.CurrencyCode, business, order.OrderID, itemName, amount, notifyUrl, returnUrl, cancelUrl);
            return url.ToString();
        }

        public static DataTable RemindForRenewalOfPackageOrder()
        {
            return SiteProvider.Packages.RemindForRenewalOfPackageOrder();
        }

        public static Result<PackageOrderDetails> SelectExpiredAccounts()
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectExpiredAccounts();
                if (result.EntityList != null)
                    result.Status = ResultStatus.Success;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectExpiredPackageOrders(PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectExpiredPackageOrders(pgObj);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static Result<PackageOrderDetails> SelectExpiredPackageOrdersWithClientName(string clientName, PagingDetails pgObj)
        {
            Result<PackageOrderDetails> result = new Result<PackageOrderDetails>();
            try
            {
                result.EntityList = SiteProvider.Packages.SelectExpiredPackageOrdersWithClientName(clientName, pgObj);
                result.Status = ResultStatus.Success;
                if (result.EntityList.Count > 0)
                    result.Message = "Records successfully retrieved fro the Database.";
                else if (result.EntityList.Count == 0)
                    result.Message = "No record entry in the Database.";
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result.Status = ResultStatus.Error;
                result.Message = "ERROR!! " + ex.Message;
            }
            return result;
        }

        public static bool UpdateAccountExpiryNotice(string orderID, string clientID, int expiryNotice)
        {
            bool result = false;
            try
            {
                int i = SiteProvider.Packages.UpdateAccountExpiryNotice(orderID, clientID, expiryNotice);
                if (i > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
                result = false;
            }
            return result;
        }

        #endregion

        #region METHODS THAT WORK WITH PACKAGEPICTURES

        public static int InsertPackagePicture(string pictureID, string pictureDescription)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Packages.InsertPackagePicture(pictureID, pictureDescription);
                PurgeCacheItems("packagepictures_picture");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static int UpdatePackagePicture(string pictureID, string packageCode, string pictureDescription, int displayOrder)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Packages.UpdatePackagePicture(pictureID, packageCode, pictureDescription, displayOrder);
                PurgeCacheItems("packagepictures_picture");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static int DeletePackagePicture(string pictureID)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Packages.DeletePackagePicture(pictureID);
                PurgeCacheItems("packagepictures_picture");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static int AssignPictureToPackage(string pictureID, string packageCode, int displayOrder)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Packages.AssignPictureToPackage(pictureID, packageCode, displayOrder);
                PurgeCacheItems("packagepictures_picture");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static int UpadePackagePictureDisplayOrder(string pictureID, string packageCode, int displayOrder)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Packages.UpdatePackagePictureDisplayOrder(pictureID, packageCode, displayOrder);
                PurgeCacheItems("packagepictures_picture");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static int RemovePictureFromPackage(string pictureID, string packageCode)
        {
            int i = 0;
            try
            {
                i = SiteProvider.Packages.RemovePictureFromPackage(pictureID, packageCode);
                PurgeCacheItems("packagepictures_picture");
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }

            return i;
        }

        public static DataTable SelectPackagePicture(string pictureID)
        {
            DataTable dt = new DataTable();
            string key = "packagepictures_picture_" + pictureID;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    dt = (DataTable)BizObject.Cache[key];
                }
                else
                {
                    dt = SiteProvider.Packages.SelectPackagePicture(pictureID);
                    BasePackage.CacheData(key, dt);
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }
            return dt;
        }

        public static DataTable SelectAllPackagePicture()
        {
            DataTable dt = new DataTable();
            string key = "packagepictures_picture";
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    dt = (DataTable)BizObject.Cache[key];
                }
                else
                {
                    dt = SiteProvider.Packages.SelectAllPackagePicture();
                    BasePackage.CacheData(key, dt);
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }
            return dt;
        }

        public static DataTable SelectPackagePicturesByPackage(string packageCode)
        {
            DataTable dt = new DataTable();
            string key = "packagepictures_picture_" + packageCode;
            try
            {
                if (BasePackage.Settings.EnableCaching && BizObject.Cache[key] != null)
                {
                    dt = (DataTable)BizObject.Cache[key];
                }
                else
                {
                    dt = SiteProvider.Packages.SelectPackagePicturesByPackage(packageCode);
                    BasePackage.CacheData(key, dt);
                }
            }
            catch (Exception ex)
            {
                Helpers.LogError(ex);
            }
            return dt;
        }

        #endregion

    }
}
