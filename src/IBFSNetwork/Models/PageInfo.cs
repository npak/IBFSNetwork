using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBFSNetwork.Models
{
    //public class PageInfo
    //{
    //    public int PageNumber { get; set; } // номер текущей страницы
    //    public int PageSize { get; set; } // кол-во объектов на странице
    //    public int TotalItems { get; set; } // всего объектов
    //    public int TotalPages  // всего страниц
    //    {
    //        get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
    //    }
    //}
    public class PageInfo
    {
        public PageInfo(int totalItems,int? page, int pageSize = 20)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public PageInfo(int totalItems, string dateSortDirection, string locationSortDirection, int? page, int pageSize = 20)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            if (string.IsNullOrWhiteSpace(dateSortDirection))
                DateSortDirection = "desc";
            else
                DateSortDirection = dateSortDirection;

            if (string.IsNullOrWhiteSpace(locationSortDirection))
                LocationSortDirection = "asc";
            else
                LocationSortDirection = locationSortDirection;

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public string DateSortDirection { get; private set; }
        public string LocationSortDirection { get; private set; }

    }

    public class AlertWithPage
    {
        public List<Models.AlertViewModels.AlertForLanding> Alerts { get; set; }
        public PageInfo PageInfo { get; set; }
        public SelectList PageSizeListl { get; set; }
        public string UserId { get; set; }
        public AlertWithPage()
        {
            Alerts = new List<AlertViewModels.AlertForLanding>();
        }
    }

    public class SearchResulttWithPage
    {
        public List<Models.AlertViewModels.AlertForLanding> Alerts { get; set; }
        public PageInfo PageInfo { get; set; }
        public string UserId { get; set; }
        public SearchResulttWithPage()
        {
            Alerts = new List<AlertViewModels.AlertForLanding>();
        }
    }

    public class FaqResultWithPage
    {
        public List<Models.FaqModels.FAQ> Faqs { get; set; }
        public PageInfo PageInfo { get; set; }
        public FaqResultWithPage()
        {
            Faqs = new List<FaqModels.FAQ>();
        }
    }
}
