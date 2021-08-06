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
    public class FraudsterService
    {
        private readonly ApplicationDbContext _context;
        private List<IdsJonsonWithId> _ids;
        public FraudsterService(ApplicationDbContext context)
        {
            _context = context;
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

        public IEnumerable<Fraudster> GetFraudstersByAlert(int id)
        {
            var query = from fr in _context.Fraudsters
                        join alf in _context.AlertFraudsters on  fr.FraudsterId equals alf.FraudsterId
                        where alf.AlertId==id
                        orderby alf.isMain descending, fr.LastName
                        select fr;

            return query;
        }

        public List<FraudsterJson> GetFraudstersForJson()
        {
            CreateJsonIDS();

            List<FraudsterJson> list = new List<FraudsterJson>();
            FraudsterJson frj;

            var frs = _context.Fraudsters;
            
            foreach(var row  in frs)
            {
                frj = new FraudsterJson();
                frj.FraudsterId = row.FraudsterId;
                frj.LastName = row.LastName;
                frj.FirstName = row.FirstName;
                frj.MiddleName = row.MiddleName;
                frj.BOD = row.BOD;
                frj.Address = row.Alias;
                frj.Alias = row.Alias;
                frj.Company = row.Company;
                frj.Email = row.Email;
                frj.Gender = row.Gender;
                frj.PhoneNumber = row.PhoneNumber;
                frj.IDS = GetJsonIDS(row.FraudsterId);
                list.Add(frj);
            }
            return list;
        }

        private void CreateJsonIDS()
        {
            _ids = (from fi in _context.FraudsterIDs
                   join t in _context.IDTypes on fi.IDTypeId equals t.IDTypeId
                   select new IdsJonsonWithId
                   {
                       FraudsterId = fi.FraudsterId,
                       IDType = t.IDTypeName,
                       PassportNumber = fi.PassportNumber,
                       DateOfIssue = fi.DateOfIssue,
                       ExpirationDate = fi.ExpirationDate,
                       IssuingAuthority = fi.IssuingAuthority,
                       IssuingCountry = fi.IssuingCountry
                   }).ToList();
            }

        private List<IdsJonson> GetJsonIDS(int fraudsterId)
        {
            var query = _ids.Where(i => i.FraudsterId == fraudsterId);
            List<IdsJonson> list = new List<IdsJonson>();
            IdsJonson ids;
            foreach (var row in query)
            {
                ids = new IdsJonson();
                ids.IDType = row.IDType;
                ids.PassportNumber = row.PassportNumber;
                ids.DateOfIssue = row.DateOfIssue;
                ids.ExpirationDate = row.ExpirationDate;
                ids.IssuingAuthority = row.IssuingAuthority;
                ids.IssuingCountry = row.IssuingCountry;
                list.Add(ids);
            }
            return list;
        }

        public List<Fraudster> GetMainFraudstersByAlert(int id)
        {
            var query = from fr in _context.Fraudsters
                        join alf in _context.AlertFraudsters on fr.FraudsterId equals alf.FraudsterId
                        where alf.AlertId == id && alf.isMain
                        select fr;

            return query.ToList();
        }

        public int GetAlertIdByFraudsterId(int fraudsterId)
        {
            var query = from alf in _context.AlertFraudsters
                        where alf.FraudsterId == fraudsterId //&& alf.isMain
                        select alf;
            var temp = query.Where(f => f.isMain).SingleOrDefault();
            if (temp != null)
                return temp.AlertId;
            else
                return query.FirstOrDefault().AlertId;
        }

        public int GetFraudsterIdByName(string fname,string lname)
        {
            var query = from fr in _context.Fraudsters
                        where fr.FirstName==fname && fr.LastName==lname
                        select fr;
            if (query.Count() == 1)
                return query.SingleOrDefault().FraudsterId;
            else
                return query.First().FraudsterId;
        }

        public List<AutocomleteFraudster> GetFraudsterTerms()
        {
            var query = from fr in _context.Fraudsters

                        select new AutocomleteFraudster
                        {
                            FraudsterId = fr.FraudsterId,
                            FullName = fr.LastName + "," + fr.FirstName
                        };

            return query.ToList();
        }

        public AlertFraudster GetAlertFraudsterModel(int alertid,int fraudsterid)
        {
            var query = from af in _context.AlertFraudsters
                        where af.AlertId == alertid && af.FraudsterId == fraudsterid
                        select af;
            return query.ToList().SingleOrDefault();
        } 
    }
}
