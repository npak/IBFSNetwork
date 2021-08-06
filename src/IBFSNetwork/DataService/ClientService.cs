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
    public class ClientService
    {
        private readonly ApplicationDbContext _context;
        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }
    } 
}
