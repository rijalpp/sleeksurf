using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SleekSurf.Entity;

namespace SleekSurf.DataAccess.SqlClient
{
    public class SqlClientPackageProvider : ClientPackageProvider
    {
        #region ALL METHODS OF PACKAGE

        public override PackageDetails SelectPackage(string packageCode)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageFromReader(reader);
                else
                    return null;
            }
        }

        public override PackageDetails SelectPackageByFeatureType(string featureType)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageSelectByFeatureType", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@featureType", SqlDbType.NVarChar).Value = featureType;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageFromReader(reader);
                else
                    return null;
            }
        }

        public override List<PackageDetails> SelectAllPackages(PagingDetails pgObj)
        {
            List<PackageDetails> packageList = new List<PackageDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                packageList = GetPackageCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageCountAll", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return packageList;
        }

        public override List<PackageDetails> SelectAllPackagesWithoutPaging()
        {
            List<PackageDetails> packageList = new List<PackageDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageSelectAllWithoutPaging", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                packageList = GetPackageCollectionFromReader(ExecuteReader(cmd));
            }

            return packageList;
        }

        public override List<PackageDetails> SelectPackagesByPublication(bool published, PagingDetails pgObj)
        {
            List<PackageDetails> packageList = new List<PackageDetails>();

            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageSelectByPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                packageList = GetPackageCollectionFromReader(ExecuteReader(cmd));
            }

            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageCountByPublication", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }

            return packageList;
        }

        public override List<PackageDetails> SelectPackagesByPublication(bool published)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagesSelectByPublicationWithoutPaging", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                return GetPackageCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override int InsertPackage(PackageDetails package)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = package.PackageCode;
                cmd.Parameters.Add("@packageName", SqlDbType.NVarChar).Value = package.PackageName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = package.Description;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = package.Published;
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = package.Status;
                cmd.Parameters.Add("@featureType", SqlDbType.NVarChar).Value = package.FeatureType;
                cmd.Parameters.Add("@createdBy", SqlDbType.NVarChar).Value = package.CreatedBy;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdatePackage(PackageDetails package)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = package.PackageCode;
                cmd.Parameters.Add("@packageName", SqlDbType.NVarChar).Value = package.PackageName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = package.Description;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = package.Published;
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = package.Status;
                cmd.Parameters.Add("@featureType", SqlDbType.NVarChar).Value = package.FeatureType;
                cmd.Parameters.Add("@updatedBy", SqlDbType.NVarChar).Value = package.UpdatedBy;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdatePackageByPublication(string packageCode, bool published)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageUpdateByPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeletePackage(string packageCode)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region ALL METHODS OF PACKAGE OPTIONS

        public override int InsertPackageOption(PackageOptionDetails option)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageOptionID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = option.PackageCode;
                cmd.Parameters.Add("@duration", SqlDbType.NVarChar).Value = option.Duration;
                cmd.Parameters.Add("@standardPrice", SqlDbType.Money).Value = option.StandardPrice;
                cmd.Parameters.Add("@discountPercentage", SqlDbType.Float).Value = option.DiscountPercentage;
                cmd.Parameters.Add("@promoCode", SqlDbType.NVarChar).Value = option.PromoCode;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = option.Published;
                cmd.Parameters.Add("@promoCodeStartDate", SqlDbType.DateTime).Value = option.PromoCodeStartDate;
                cmd.Parameters.Add("@promoCodeEndDate", SqlDbType.DateTime).Value = option.PromoCodeEndDate;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = option.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdatePackageOption(PackageOptionDetails option)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageOptionID", SqlDbType.Int).Value = option.PackageOptionID;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = option.PackageCode;
                cmd.Parameters.Add("@duration", SqlDbType.NVarChar).Value = option.Duration;
                cmd.Parameters.Add("@standardPrice", SqlDbType.Money).Value = option.StandardPrice;
                cmd.Parameters.Add("@discountPercentage", SqlDbType.Float).Value = option.DiscountPercentage;
                cmd.Parameters.Add("@promoCode", SqlDbType.NVarChar).Value = option.PromoCode;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = option.Published;
                cmd.Parameters.Add("@promoCodeStartDate", SqlDbType.DateTime).Value = option.PromoCodeStartDate;
                cmd.Parameters.Add("@promoCodeEndDate", SqlDbType.DateTime).Value = option.PromoCodeEndDate;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = option.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int DeletePackageOption(int packageOptionID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageOptionID", SqlDbType.Int).Value = packageOptionID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override PackageOptionDetails SelectPackageOption(int packageOptionID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageOptionID", SqlDbType.Int).Value = packageOptionID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOptionFromReader(reader);
                else
                    return null;
            }
        }

        public override PackageOptionDetails SelectPackageOption(string promoCode)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionSelectByPromoCode", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@promoCode", SqlDbType.NVarChar).Value = promoCode;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOptionFromReader(reader);
                else
                    return null;
            }
        }

        public override List<PackageOptionDetails> SelectPackageOptionDetailsByPackage(string packageCode)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionSelectByPackage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cn.Open();
                return GetPackageOptionCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<PackageOptionDetails> SelectPackageOptionsByPublication(string packageCode, bool published)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOptionSelectByPublication", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cmd.Parameters.Add("@published", SqlDbType.Bit).Value = published;
                cn.Open();
                return GetPackageOptionCollectionFromReader(ExecuteReader(cmd));
            }
        }
        #endregion

        #region ALL METHODS THAT WORK WITH PACKAGEORDER

        public override int InsertPackageOrder(PackageOrderDetails packageOrder)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderID", SqlDbType.NVarChar).Value = packageOrder.OrderID;
                cmd.Parameters.Add("@createdBy", SqlDbType.NVarChar).Value = packageOrder.CreatedBy;
                cmd.Parameters.Add("@orderStatus", SqlDbType.NVarChar).Value = packageOrder.OrderStatus;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = packageOrder.Client.ClientID;
                cmd.Parameters.Add("@receiptNo", SqlDbType.NVarChar).Value = packageOrder.ReceiptNo;
                cmd.Parameters.Add("@transactionID", SqlDbType.NVarChar).Value = packageOrder.TransactionID;
                cmd.Parameters.Add("@standardPrice", SqlDbType.Money).Value = packageOrder.StandardPrice;
                cmd.Parameters.Add("@discountPercentage", SqlDbType.Float).Value = packageOrder.DiscountPercentage;
                cmd.Parameters.Add("@finalPrice", SqlDbType.Money).Value = packageOrder.FinalPrice;
                cmd.Parameters.Add("@amountDeducted", SqlDbType.Decimal).Value = packageOrder.AmountDeducted;
                cmd.Parameters.Add("@finalPriceAfterDeduction", SqlDbType.Decimal).Value = packageOrder.FinalPriceAfterDeduction;
                cmd.Parameters.Add("@paymentOption", SqlDbType.NVarChar).Value = packageOrder.PaymentOption;
                cmd.Parameters.Add("@registrationDate", SqlDbType.DateTime).Value = packageOrder.RegistrationDate;
                cmd.Parameters.Add("@expiryDate", SqlDbType.DateTime).Value = packageOrder.ExpiryDate;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageOrder.PackageCode;
                cmd.Parameters.Add("@packageName", SqlDbType.NVarChar).Value = packageOrder.PackageName;
                cmd.Parameters.Add("@duration", SqlDbType.NVarChar).Value = packageOrder.Duration;
                cmd.Parameters.Add("@promoCode", SqlDbType.NVarChar).Value = packageOrder.PromoCode;
                cmd.Parameters.Add("@promoCodeStartDate", SqlDbType.DateTime).Value = packageOrder.PromoCodeStartDate;
                cmd.Parameters.Add("@promoCodeEndDate", SqlDbType.DateTime).Value = packageOrder.PromoCodeEndDate;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = packageOrder.Comments;

                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdatePackageOrder(PackageOrderDetails packageOrder)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderID", SqlDbType.NVarChar).Value = packageOrder.OrderID;
                cmd.Parameters.Add("@orderStatus", SqlDbType.NVarChar).Value = packageOrder.OrderStatus;
                cmd.Parameters.Add("@receiptNo", SqlDbType.NVarChar).Value = packageOrder.ReceiptNo;
                cmd.Parameters.Add("@transactionID", SqlDbType.NVarChar).Value = packageOrder.TransactionID;
                cmd.Parameters.Add("@standardPrice", SqlDbType.Money).Value = packageOrder.StandardPrice;
                cmd.Parameters.Add("@discountPercentage", SqlDbType.Float).Value = packageOrder.DiscountPercentage;
                cmd.Parameters.Add("@finalPrice", SqlDbType.Money).Value = packageOrder.FinalPrice;
                cmd.Parameters.Add("@paymentOption", SqlDbType.NVarChar).Value = packageOrder.PaymentOption;
                cmd.Parameters.Add("@amountDeducted", SqlDbType.Decimal).Value = packageOrder.AmountDeducted;
                cmd.Parameters.Add("@finalPriceAfterDeduction", SqlDbType.Decimal).Value = packageOrder.FinalPriceAfterDeduction;
                cmd.Parameters.Add("@registrationDate", SqlDbType.DateTime).Value = packageOrder.RegistrationDate;
                cmd.Parameters.Add("@expiryDate", SqlDbType.DateTime).Value = packageOrder.ExpiryDate;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageOrder.PackageCode;
                cmd.Parameters.Add("@packageName", SqlDbType.NVarChar).Value = packageOrder.PackageName;
                cmd.Parameters.Add("@duration", SqlDbType.NVarChar).Value = packageOrder.Duration;
                cmd.Parameters.Add("@promoCode", SqlDbType.NVarChar).Value = packageOrder.PromoCode;
                cmd.Parameters.Add("@promoCodeStartDate", SqlDbType.DateTime).Value = packageOrder.PromoCodeStartDate;
                cmd.Parameters.Add("@promoCodeEndDate", SqlDbType.DateTime).Value = packageOrder.PromoCodeEndDate;
                cmd.Parameters.Add("@comments", SqlDbType.NVarChar).Value = packageOrder.Comments;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override List<PackageOrderDetails> SelectPackageOrderByClientID(string clientID, PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountOrdersByClientID", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectPackageOrderByStatusAndDays(string orderStatus, int days, PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectOrdersByOrderStatusAndDays", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@orderStatus", SqlDbType.VarChar).Value = orderStatus;
                cmd.Parameters.Add("@days", SqlDbType.Int).Value = days;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageorderCountOrdersByStatusAndDate", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderStatus", SqlDbType.VarChar).Value = orderStatus;
                cmd.Parameters.Add("@days", SqlDbType.Int).Value = days;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectLatestPackageOrderPerClient(string orderStatus, DateTime fromDate, PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectLatestPerClient", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@orderStatus", SqlDbType.VarChar).Value = orderStatus;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountLatestPerClient", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderStatus", SqlDbType.VarChar).Value = orderStatus;
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectExpiringPackageOrders(PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectExpiringPackageOrders", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountExpiringPackageOrder", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectPackageOrdersWithExpiryNoticeRange(PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectExpirationStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountExpirationStatus", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectExpiringPackageOrdersWithClientName(string clientName, PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectByClientName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientName", SqlDbType.NVarChar).Value = clientName;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountExpiringPackageOrdersByClientName", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientName", SqlDbType.NVarChar).Value = clientName;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectPackageOrdersWithExpirationNoticeRangeByClientName(string clientName, PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectExpirationStatusWithClientName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientName", SqlDbType.NVarChar).Value = clientName;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountExpirationStatusWithClientName", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientName", SqlDbType.NVarChar).Value = clientName;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectRecentDistinctOrdersByClientID(string clientID)//SELECTS DISTINCT ORDERS BY PACKAGECODE 
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectRecentOrders", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            return orderDetailList;
        }

        public override PackageOrderDetails SelectPackageOrder(string orderID, string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderID", SqlDbType.NVarChar).Value = orderID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOrderFromReader(reader);
                else
                    return null;
            }
        }

        public override PackageOrderDetails SelectPackageOrder(string orderID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectByOrderID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderID", SqlDbType.NVarChar).Value = orderID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOrderFromReader(reader);
                else
                    return null;
            }
        }

        public override PackageOrderDetails SelectRecentPackageOrder(string clientID, string packageCode)//VERIFIED ONLY TRANSACTION
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectRecentTransactionByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOrderFromReader(reader);
                else
                    return null;

            }
        }

        public override PackageOrderDetails SelectPackageorderWithHighestExpiryDate(string clientID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectHighestExpiryDateTransactionByClientID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOrderFromReader(reader);
                else
                    return null;

            }
        }

        public override PackageOrderDetails SelectMostRecentPackageOrder(string clientID)//ANY TRANSACTION
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectMostRecentTransaction", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOrderFromReader(reader);
                else
                    return null;
            }
        }

        public override PackageOrderDetails SelectMostRecentIncompletePackageOrder(string clientID)//ONLY INCOMPLETE TRANSACTION
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectMostRecentIncompleteTransaction", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cn.Open();
                IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                if (reader.Read())
                    return GetPackageOrderFromReader(reader);
                else
                    return null;
            }
        }

        public override int DeletePackageOrder(string orderID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderID", SqlDbType.NVarChar).Value = orderID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override DataTable RemindForRenewalOfPackageOrder()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderRemindForRenewal", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }

        public override List<PackageOrderDetails> SelectExpiredAccounts()
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectExpired", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                return GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
        }

        public override List<PackageOrderDetails> SelectExpiredPackageOrders(PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectExpiredPackageOrders", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountExpiredPackageOrders", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override List<PackageOrderDetails> SelectExpiredPackageOrdersWithClientName(string clientName, PagingDetails pgObj)
        {
            List<PackageOrderDetails> orderDetailList = new List<PackageOrderDetails>();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderSelectExpiredPackageOrdersWithClientName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@startRowIndex", SqlDbType.Int).Value = pgObj.StartRowIndex;
                cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pgObj.PageSize;
                cmd.Parameters.Add("@clientName", SqlDbType.NVarChar).Value = clientName;
                cn.Open();
                orderDetailList = GetPackageOrderCollectionFromReader(ExecuteReader(cmd));
            }
            using (SqlConnection cn1 = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderCountExpiredPackageOrderWithClientName", cn1);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@clientName", SqlDbType.NVarChar).Value = clientName;
                cn1.Open();
                pgObj.TotalNumber = (int)ExecuteScalar(cmd);
            }
            return orderDetailList;
        }

        public override int UpdateAccountExpiryNotice(string orderID, string clientID, int expiryNotice)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackageOrderUpdateAccountExpiryNotice", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@orderID", SqlDbType.NVarChar).Value = orderID;
                cmd.Parameters.Add("@clientID", SqlDbType.VarChar).Value = clientID;
                cmd.Parameters.Add("@expiryNotice", SqlDbType.Int).Value = expiryNotice;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        #endregion

        #region METHODS THAT WORK WITH DATEEXTENDER

        #endregion

        #region METHODS THAT WORK WITH PACKAGEPICTURES

        public override int InsertPackagePicture(string pictureID, string pictureDescription)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagePictureInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cmd.Parameters.Add("@pictureDescription", SqlDbType.VarChar).Value = pictureDescription;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int UpdatePackagePicture(string pictureID, string packageCode, string pictureDescription, int displayOrder)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagePictureUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cmd.Parameters.Add("@pictureDescription", SqlDbType.VarChar).Value = pictureDescription;
                cmd.Parameters.Add("@displayOrder", SqlDbType.Int).Value = displayOrder;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int DeletePackagePicture(string pictureID)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagePictureDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int AssignPictureToPackage(string pictureID, string packageCode, int displayOrder)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spJointPackagePictureInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cmd.Parameters.Add("@displayOrder", SqlDbType.Int).Value = displayOrder;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override int UpdatePackagePictureDisplayOrder(string pictureID, string packageCode, int displayOrder)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spJointPackagePictureUpdateDisplayOrder", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cmd.Parameters.Add("@displayOrder", SqlDbType.Int).Value = displayOrder;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }
        public override int RemovePictureFromPackage(string pictureID, string packageCode)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spJointPackagePictureDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cn.Open();
                return ExecuteNonQuery(cmd);
            }
        }

        public override DataTable SelectPackagePicture(string pictureID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagePictureSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pictureID", SqlDbType.NVarChar).Value = pictureID;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }

        public override DataTable SelectAllPackagePicture()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagePictureSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }

        public override DataTable SelectPackagePicturesByPackage(string packageCode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spPackagePictureSelectByPackage", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@packageCode", SqlDbType.NVarChar).Value = packageCode;
                cn.Open();
                dt.Load(ExecuteReader(cmd));
                return dt;
            }
        }

        #endregion

    }
}
