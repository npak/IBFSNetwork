using IBFSNetwork.Data;
using IBFSNetwork.Models.AlertViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using Sakura.AspNetCore;


namespace IBFSNetwork.DataService
{
    public class AlertService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationAdoContext _adocontext;
        public AlertService(ApplicationDbContext context, ApplicationAdoContext adocontext)
        {
            _context = context;
            _adocontext = adocontext;
        }

        public AlertDisplayModel GetAlertDisplayModel()
        {
            AlertDisplayModel res = new AlertDisplayModel();
            var dbalerts = _context.Alerts.Include(bs => bs.BankSize).Include(bt => bt.BankType).Include(ft => ft.FraudType).Include(lo => lo.Location).Include(fr => fr.Fraudsters).Include(lst => lst.LocationState).OrderByDescending(t => t.AlertDate).Take(6);

            AlertForLanding alert;
            List<AlertForLanding> alerts = new List<AlertForLanding>();

            foreach (var v in dbalerts)
            {
                alert = new AlertForLanding();
                alert.AlertId = v.AlertId;
                alert.FirstName = v.Fraudsters[0].FirstName;
                alert.LastName = v.Fraudsters[0].LastName;
                alert.Location = v.Location.Caption;
                alert.LocationState = v.LocationState==null ? "": v.LocationState.Caption;
                alert.City = v.City;
                alert.BankSize = v.BankSize.Caption;
                alert.BankType = v.BankType.Caption;
                alert.FraudType = v.FraudType.Caption;

                alert.LostAmount = v.LostAmount;
                alerts.Add(alert);
            }
            res.alerts = alerts;
            return res;
        }

   
        public List<AlertForLanding> GetFilteredAlertsDel(int location, int countries, int state, string city)
        {
            List<AlertForLanding> list = new List<Models.AlertViewModels.AlertForLanding>();
            SqlConnection conn = new SqlConnection();
            try
            {
                SqlDataReader rdr = null;
                AlertForLanding alert;
                
                conn.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetFilteredAlertsList", conn);
                if (string.IsNullOrWhiteSpace(city))
                    city ="";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(
                    new SqlParameter("@locationId", location));
                cmd.Parameters.Add(new SqlParameter("@countryId",countries));
                cmd.Parameters.Add(new SqlParameter("@stateId", state));
                cmd.Parameters.Add(new SqlParameter("@city", city));

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    alert = new AlertForLanding();
                    alert.AlertId = Convert.ToInt32( rdr["AlertId"]);
                    alert.BankType = rdr["BankType"].ToString();
                    alert.BankSize = rdr["BankSize"].ToString();
                    alert.FraudType = rdr["FraudType"].ToString();
                    alert.FirstName = rdr["FirstName"].ToString();
                    alert.LastName = rdr["LastName"].ToString();
                    alert.Location = rdr["Location"].ToString();
                    alert.LocationState = rdr["State"].ToString();
                    alert.City = rdr["City"].ToString();
                    alert.LostAmount =Convert.ToInt32(rdr["LostAmount"]);
                    list.Add(alert);
                }
                rdr.Dispose();
                return list;

            }
            catch (Exception ex)
            {
                return list;

            }
            finally
            { if (conn!=null) conn.Close(); }
        }

        public Models.AlertWithPage GetFilteredAlertsWithPage(int location, int countries, int state, string dateSortDirection, string locationSortDirection, string city,int page, int pagesize)
        {
           

            Models.AlertWithPage awp = new Models.AlertWithPage();
             SqlConnection conn = new SqlConnection();
            try
            {
                SqlDataReader rdr = null;
                AlertForLanding alert;

                conn.ConnectionString = _adocontext.Database.GetDbConnection().ConnectionString;
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetFilteredAlertsWithPage", conn);
                if (string.IsNullOrWhiteSpace(city))
                    city = "";
                if (string.IsNullOrWhiteSpace(dateSortDirection))
                    dateSortDirection = "desc";
                if (string.IsNullOrWhiteSpace(locationSortDirection))
                    locationSortDirection = "asc";
                    cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(
                    new SqlParameter("@locationId", location));
                cmd.Parameters.Add(new SqlParameter("@countryId", countries));
                cmd.Parameters.Add(new SqlParameter("@stateId", state));
                cmd.Parameters.Add(new SqlParameter("@city", city));
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@pagesize", pagesize));
                cmd.Parameters.Add(new SqlParameter("@dateSortDirection", dateSortDirection));
                cmd.Parameters.Add(new SqlParameter("@locationSortDirection", locationSortDirection));


                rdr = cmd.ExecuteReader();
                // int reccount = Convert.ToInt32(outPutParameter.Value);
                // pi.TotalItems = reccount;
                int total = 0;
                while (rdr.Read())
                {
                    alert = new AlertForLanding();
                    total= Convert.ToInt32(rdr["TotalItems"]);
                    alert.AlertId = Convert.ToInt32(rdr["AlertId"]);
                    alert.AlertDate = Convert.ToDateTime(rdr["AlertDate"]);

                    alert.BankType = rdr["BankType"].ToString();
                    alert.BankSize = rdr["BankSize"].ToString();
                    alert.FraudType = rdr["FraudType"].ToString();

                    alert.FirstName = rdr["FirstName"].ToString();
                    alert.LastName = rdr["LastName"].ToString();
                    alert.Location = rdr["Location"].ToString();
                    alert.LocationState = rdr["State"].ToString();
                    alert.City = rdr["City"].ToString();
                    alert.LostAmount = Convert.ToInt32(rdr["LostAmount"]);
                    alert.ApplicationUserId = rdr["ApplicationUserId"].ToString();
                    awp.Alerts.Add(alert);
                }
                rdr.Dispose();
                 Models.PageInfo pageInfo = new Models.PageInfo(total, dateSortDirection,locationSortDirection, page, pagesize);
                awp.PageInfo = pageInfo;
                return awp;

            }
            catch (Exception ex)
            {
                return awp;
            }
            finally
            { if (conn != null) conn.Close(); }
        }

        public Models.AlertWithPage GetAlertsWithPageById(int alertid,int page, int pagesize)
        {


            Models.AlertWithPage awp = new Models.AlertWithPage();
            SqlConnection conn = new SqlConnection();
            try
            {
                SqlDataReader rdr = null;
                AlertForLanding alert;

                conn.ConnectionString =  _adocontext.Database.GetDbConnection().ConnectionString;
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetAlertsWithPageById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(
                    new SqlParameter("@alertId", alertid));
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@pagesize", pagesize));
                
                rdr = cmd.ExecuteReader();
                int total = 0;
                while (rdr.Read())
                {
                    alert = new AlertForLanding();
                    total = Convert.ToInt32(rdr["TotalItems"]);
                    alert.AlertId = Convert.ToInt32(rdr["AlertId"]);
                    alert.AlertDate = Convert.ToDateTime(rdr["AlertDate"]);

                    alert.BankType = rdr["BankType"].ToString();
                    alert.BankSize = rdr["BankSize"].ToString();
                    alert.FraudType = rdr["FraudType"].ToString();

                    alert.FirstName = rdr["FirstName"].ToString();
                    alert.LastName = rdr["LastName"].ToString();
                    alert.Location = rdr["Location"].ToString();
                    alert.LocationState = rdr["State"].ToString();
                    alert.City = rdr["City"].ToString();
                    alert.LostAmount = Convert.ToInt32(rdr["LostAmount"]);
                    awp.Alerts.Add(alert);
                }
                rdr.Dispose();
                Models.PageInfo pageInfo = new Models.PageInfo(total,page, pagesize);
                awp.PageInfo = pageInfo;
                return awp;

            }
            catch (Exception ex)
            {
                return awp;
            }
            finally
            { if (conn != null) conn.Close(); }
        }

    }
}
