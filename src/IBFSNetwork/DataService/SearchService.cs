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
    public class SearchService
    {
        private readonly ApplicationDbContext _context;
        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

 //       @firstName varchar(50),
 //@lastName varchar(50),
 //@idNumber varchar(50),
 //@locationId int,
 //@stateId int,
 //@city varchar(50),
 //@address varchar(128),
 //@phone varchar(50),
 //@email varchar(128),
 //@company varchar(128)

        public Models.SearchResulttWithPage GetSearchResult(string firstName, string lastName, string idNumber,
                                        int locationId, int stateId, string city, string address, string phone,string email,string company,string alias, int page, int pagesize)
        {
            Models.SearchResulttWithPage res = new Models.SearchResulttWithPage();

            SqlConnection conn = new SqlConnection();
            try
            { 
                SqlDataReader rdr = null;
                AlertForLanding alert;
                
                conn.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                conn.Open();
                SqlCommand cmd = new SqlCommand("SearchAlertsListWithPage", conn);
             
                if (string.IsNullOrWhiteSpace(city))
                    city ="";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@firstName", string.IsNullOrWhiteSpace(firstName) ? "":firstName));
                cmd.Parameters.Add(new SqlParameter("@lastName", string.IsNullOrWhiteSpace(lastName) ? "" : lastName));
                cmd.Parameters.Add(new SqlParameter("@idNumber", string.IsNullOrWhiteSpace(idNumber) ? "" : idNumber));
                cmd.Parameters.Add(new SqlParameter("@locationId", locationId));
                cmd.Parameters.Add(new SqlParameter("@stateId", stateId));
                cmd.Parameters.Add(new SqlParameter("@city", string.IsNullOrWhiteSpace(city) ? "":city));
                cmd.Parameters.Add(new SqlParameter("@address", string.IsNullOrWhiteSpace(address) ? "" : address));
                cmd.Parameters.Add(new SqlParameter("@phone", string.IsNullOrWhiteSpace(phone) ? "" : phone));
                cmd.Parameters.Add(new SqlParameter("@email", string.IsNullOrWhiteSpace(email) ? "" : email));
                cmd.Parameters.Add(new SqlParameter("@company", string.IsNullOrWhiteSpace(company) ? "" : company));
                cmd.Parameters.Add(new SqlParameter("@alias", string.IsNullOrWhiteSpace(alias) ? "" : alias));
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@pagesize", pagesize));

                rdr = cmd.ExecuteReader();
                int total = 0;

                while (rdr.Read())
                {
                    alert = new AlertForLanding();
                    total = Convert.ToInt32(rdr["TotalItems"]);
                    alert.AlertId = Convert.ToInt32( rdr["AlertId"]);
                    alert.AlertDate = Convert.ToDateTime(rdr["AlertDate"]);
                    alert.BankType = rdr["BankType"].ToString();
                    alert.BankSize = rdr["BankSize"].ToString();
                    alert.FraudType = rdr["FraudType"].ToString();

                    alert.FirstName = rdr["FirstName"].ToString();
                    alert.LastName = rdr["LastName"].ToString();
                    alert.Location = rdr["Location"].ToString();
                    alert.LocationState = rdr["State"].ToString();
                    alert.City = rdr["City"].ToString();
                    alert.LostAmount =Convert.ToInt32(rdr["LostAmount"]);
                    alert.ApplicationUserId = rdr["ApplicationUserId"].ToString();
                    res.Alerts.Add(alert);
                }
                rdr.Dispose();
                Models.PageInfo pageInfo = new Models.PageInfo(total, page, pagesize);
                res.PageInfo = pageInfo;
                return res;

            }
            catch (Exception ex)
            {
                return res;
            }
            finally
            { if (conn != null) conn.Close(); }
        }

 
    }
}
