using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Microsoft.Samples.ReportingServices.CustomSecurity.helpers;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public static class MonumentDal
    {
        public static DataTable GetDivisions(List<int> clientIdList)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                var parameters = new List<string>();
                foreach (var i in clientIdList)
                {
                    var param = string.Format("@ClientId{0}", i);
                    cmd.Parameters.AddWithValue(param, i);
                    parameters.Add(param);
                }
                cmd.CommandText = string.Format(@"SELECT   d.[division_key] as DivisionId
													,d.[division_name] as DivisionName
													,d.[client_FK] as ClientId
													,c.client_name as ClientName
													,d.[BU] as BuName
												FROM [D_Division] d INNER JOIN  [D_Client] c ON d.client_FK = c.client_key
                                                WHERE d.status = 'Active' and d.client_FK IN ({0})", string.Join(", ", parameters.ToArray()));
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void UpdateDivisionBu(string buName, int divisionId)
        {
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE [D_Division]
								   SET [BU] = @BuName
								 WHERE [division_key] = @DivisionId";
                cmd.Parameters.Add("@DivisionId", SqlDbType.Int).Value = divisionId;
                cmd.Parameters.Add("@BuName", SqlDbType.VarChar).Value = buName;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public static DataTable GetAllClientNames()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand(@"SELECT [client_key] as ClientId
											  ,[client_name] as ClientName
										  FROM [dbo].[D_Client]
                                          Where status ='Active'
                                          ORDER BY ClientName asc  ");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }

            var selectAll = dt.NewRow();

            selectAll["ClientId"] = "-1";
            selectAll["ClientName"] = "Select All";

            dt.Rows.InsertAt(selectAll, 0);
            return dt;
        }

        public static DataTable GetOfficeLocations(List<int> clientIdList)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();

                var parameters = new List<string>();
                foreach (var i in clientIdList)
                {
                    var param = string.Format("@ClientId{0}", i);
                    cmd.Parameters.AddWithValue(param, i);
                    parameters.Add(param);
                }
                var sda = new SqlDataAdapter();
                cmd.CommandText = string.Format(@"SELECT [office_location_key] as OfficeLocationId
                                                ,[client_name] as ClientName
                                              ,[office_location] as OfficeLocation
                                              ,[new_office] as NewOfficeLocation
                                          FROM [dbo].[D_OfficeLocation]
                                          where status ='Active' and client_FK IN ({0})", string.Join(", ", parameters.ToArray()));
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void UpdateOfficeLocation(string newOfficeLocation, int officeLocationId)
        {
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE [dbo].[D_OfficeLocation]
                                    SET [new_office] = @NewOfficeLocation
                                    WHERE [office_location_key] = @OfficeLocationId";
                cmd.Parameters.Add("@OfficeLocationId", SqlDbType.Int).Value = officeLocationId;
                cmd.Parameters.Add("@NewOfficeLocation", SqlDbType.VarChar).Value = newOfficeLocation;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public static DataTable GetBidStatus()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand(@"SELECT [bid_status_key] as BidStatusId
                                              ,[bid_status] as BidStatus
                                              ,[selected] as Selected
                                              ,[status]
                                          FROM [dbo].[D_BidStatus]
                                          where [status] = 'Active'");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void UpdateBidStatus(int selected, int bidStatusId)
        {
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE [dbo].[D_BidStatus]
                                   SET [selected] = @Selected
                                          WHERE [bid_status_key] =@BidStatusId";
                cmd.Parameters.Add("@Selected", SqlDbType.Int).Value = selected;
                cmd.Parameters.Add("@BidStatusId", SqlDbType.Int).Value = bidStatusId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public static DataTable GetAwardStatus()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand(@"SELECT [award_status_key] as AwardStatusId
                                              ,[award_status] as AwardStatus
                                              ,[once_engaged] as OnceEngaged
                                              ,[positive] as Positive
                                          FROM [dbo].[D_AwardStatus]
                                          where [award_status_key] != 0 and Status ='Active'");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void UpdateAwardStatus(int onceEngaged, int positive, int awardStatusId)
        {
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE [dbo].[D_AwardStatus]
                                   SET [once_engaged] = @OnceEngaged
                                       ,[positive] = @Positive
                                          WHERE [award_status_key] =@AwardStatusId";
                cmd.Parameters.Add("@OnceEngaged", SqlDbType.Int).Value = onceEngaged;
                cmd.Parameters.Add("@Positive", SqlDbType.Int).Value = positive;
                cmd.Parameters.Add("@AwardStatusId", SqlDbType.Int).Value = awardStatusId;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public static DataTable GetProjectStatus()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand(@"SELECT [project_status_key] as ProjectStatusId
                                              ,[project_status] as ProjectStatus
                                              ,[approved] as Approved
                                          FROM [dbo].[D_ProjectStatus]
                                          where [project_status_key] != 1 and Status ='Active'");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void UpdateProjectStatus(String approved, int projectStatusId)
        {
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE [dbo].[D_ProjectStatus]
                                   SET [approved] = @Approved
                                          WHERE [project_status_key] =@ProjectStatusId";
                cmd.Parameters.Add("@ProjectStatusId", SqlDbType.Int).Value = projectStatusId;
                cmd.Parameters.Add("@Approved", SqlDbType.VarChar).Value = approved;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public static DataTable GetDisEngagementStatus()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand(@"SELECT [disengage_reason_key] as DisEngagementStatusId
                                              ,[reason] as reason
                                              ,[positive_reason] as PositiveReason
                                          FROM [dbo].[D_DisengageReason]
                                          where [status] = 'Active' and [disengage_reason_key] != 0");
                var sda = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;

                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
            }
            return dt;
        }

        public static void UpdateDisEngagementStatus(int disEngagementStatusId, int positiveReason)
        {
            using (var conn = new SqlConnection(ConfigHelper.MonumentConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"UPDATE [dbo].[D_DisengageReason]
                                       SET [positive_reason] = @PositiveReason
                                     WHERE [disengage_reason_key] = @DisEngagementStatusId ";
                cmd.Parameters.Add("@DisEngagementStatusId", SqlDbType.Int).Value = disEngagementStatusId;
                cmd.Parameters.Add("@PositiveReason", SqlDbType.Int).Value = positiveReason;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

    }
    
}
