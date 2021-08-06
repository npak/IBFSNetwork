using IBFSNetwork.Data;

using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Models.FeedModels;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;


namespace IBFSNetwork.DataService
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public Models.AccountViewModels.ClientUserName GetUserName(string clientcode)
        {
            Models.AccountViewModels.ClientUserName clientInfor = new Models.AccountViewModels.ClientUserName();
            var client = _context.AplicationClients.Where(cl => cl.ClientCode == clientcode).SingleOrDefault();
            if (client == null)
                return clientInfor;

            int? val = _context.Users.Where(u => u.ClientId == client.ClientId && u.UserNumberCod != null).Max(u => u.UserNumberCod);
            int maxCode = val.GetValueOrDefault();

            clientInfor.ClientId = client.ClientId;

            if (maxCode == 0)
            {
                clientInfor.Username = client.ClientCode + "0001";
                clientInfor.UserNumber = 1;
            }
            else if (maxCode < client.UserCount)
            {
                clientInfor.Username = client.ClientCode + (maxCode + 1).ToString("0000");
                clientInfor.UserNumber = maxCode + 1;
            }
            else clientInfor.Username = "";

            return clientInfor;
        }

        public bool ClientUserLimit(int id)
        {
            ApplicationClient client = _context.AplicationClients.FirstOrDefault(t => t.ClientId == id);
            int? cnt = _context.Users.Where(u=>u.ClientId == id).Max(u=> u.UserNumberCod);
            if (cnt==null ||cnt < client.UserCount)
                return true;
            else
                return false;
        }

        public bool UpdateUserProfile(string userid,string location, bool setupEmail)
        {
            var currentuser = _context.Users.Where(u => u.Id == userid).SingleOrDefault();
            if (currentuser!=null)
            {
                currentuser.Location = location;
                currentuser.IsSetUpEmailAlert = setupEmail;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    } 
}
