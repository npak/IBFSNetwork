using IBFSNetwork.Data;
using IBFSNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Models.FeedModels;


namespace IBFSNetwork.DataService
{
    public class DocumentService
    {
        private readonly ApplicationDbContext _context;
        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<Models.AlertViewModels.Document> DocToDelete(int fraudsterid,List<string> flist)
        {
            var dlist = from d in _context.Documents
                        where (d.FraudsterId == fraudsterid) && !flist.Contains(d.DocName)
                        select d;
            return dlist.ToList();
           
        }

        public List<Models.AlertViewModels.Document> DocToStore(int fraudsterId, List<string> flist)
        {
            var list = from d in _context.Documents
                        where  (d.FraudsterId== fraudsterId) && flist.Contains(d.DocName)
                        select d;
            return list.ToList();

        }
    }
}
