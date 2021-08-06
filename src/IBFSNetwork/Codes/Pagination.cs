using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBFSNetwork.Codes
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public int PageItemsAmount { get; set; } // показывать количество записей на странице
        public int ItemsAmount { get; set; } // всего записей в бд
        public int CurrentPageItemsAmount { get; set; } // количествл записей на текущей странице
        public int PrevPage { get; set; } // предыдущая страница относительно текущей
        public int NextPage { get; set; } // следующая страница относительно текущей
        public List<Page> Pages { get; set; } // данные о страницах которые будут показаны
        public bool ShowLastAndFirstPages { get; set; } // всегда показываем первую и последнюю страница

        public int Offset { get; set; } // смещение записей относительно первой текущей записи. (CurrentPage - 1) * PageItemsAmount

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Dictionary<string, object> RouteParams { get; set; } // параметры которые относятся к маршруту
        public Dictionary<string, object> Params { get; set; } // get параметры
        public Pagination()
        {
            CurrentPage = 1;
            LastPage = 1;
            PageItemsAmount = 10;
            ItemsAmount = 0;
            CurrentPageItemsAmount = 0;
            PrevPage = 1;
            NextPage = 1;
            Pages = new List<Page>();
            ShowLastAndFirstPages = true;

            Offset = 0;

            ControllerName = "TestPagination";
            ActionName = "TestPagination";
            RouteParams = new Dictionary<string, object>();
            Params = new Dictionary<string, object>();

            ComputePages();
        }
        public void Refresh()
        {
            // устанавливаем значения по умолчанию, еслм это будет необходимо
            if (CurrentPage <= 0)
                CurrentPage = 1;
            if (LastPage <= 0)
                LastPage = 1;
            if (PageItemsAmount <= 0)
                PageItemsAmount = 10;
            if (ItemsAmount < 0)
                ItemsAmount = 0;
            // установим расчетные параметры
            Offset = (CurrentPage - 1) * PageItemsAmount;

            if (ItemsAmount > 0 && ItemsAmount > PageItemsAmount)
                LastPage = (int)Math.Ceiling((double)ItemsAmount / PageItemsAmount);

            if (CurrentPage < LastPage)
                CurrentPageItemsAmount = PageItemsAmount;
            else
                CurrentPageItemsAmount = ItemsAmount - (LastPage - 1) * PageItemsAmount;
            //
            ComputePages();
        }
        private void ComputePages()
        {
            Pages.Clear();
            bool leftDotPage = false;
            bool rightDotPage = false;
            if (LastPage > 15)
                rightDotPage = true;
            // если страниц меньше чем 15(вкл) или текущая страница меньше 8(вкл), то покажем все начальные страницы
            if (LastPage <= 15)
            {
                for (int i = 1; i <= LastPage; i++)
                    Pages.Add(new Page
                    {
                        Value = i,
                        Caption = i.ToString(),
                        IsCurrenPage = i == CurrentPage,
                        RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", i } }, RouteParams, Params)
                    });
            }
            else if (CurrentPage <= 8)
            {
                for (int i = 1; i <= 15; i++)
                    Pages.Add(new Page
                    {
                        Value = i,
                        Caption = i.ToString(),
                        IsCurrenPage = i == CurrentPage,
                        RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", i } }, RouteParams, Params)
                    });
            }
            // иначе сдвигаем страницы на разницу текущей страницы от 8
            else
            {
                leftDotPage = true;
                int n = LastPage - 15; // количество позиций на которые можно сдвинуть влево
                int c = CurrentPage - 8; // количество позиций относительно текущей страницы
                if (c > n)
                {
                    c = n;
                    rightDotPage = false;
                }

                for (int i = 1 + c, l = c + 15; i <= l; i++)
                    Pages.Add(new Page
                    {
                        Value = i,
                        Caption = i.ToString(),
                        IsCurrenPage = i == CurrentPage,
                        RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", i } }, RouteParams, Params)
                    });
            }

            // если не нужно показывать первые и последние страницы, то покажем просто точки. Картина будет такая:
            //  ____ ___ ___ ___ ___ ____
            // | .. | 6 | 7 | 8 | 9 | .. |
            //
            if (!ShowLastAndFirstPages)
            {
                for (var i = 0; i < Pages.Count; i++)
                {
                    if (i == 0 && leftDotPage || i == 14 && rightDotPage)
                    {
                        Pages[i].Caption = "..";
                    }
                }
            }
            // если нужно показывать первые и последние страницы, то покажем 1, 2, предпоследнюю и последнюю страницы. То есть картина будет такая:
            // ___ ___ ____ ___ ___ ___ ___ ____ ____ ____
            // | 1 | 2 | .. | 6 | 7 | 8 | 9 | .. | 14 | 15 |
            //
            else
            {
                for (var i = 0; i < Pages.Count; i++)
                {
                    if (i <= 2 && leftDotPage)
                    {
                        if (i == 0)
                        {
                            Pages[i].Caption = "1";
                            Pages[i].Value = 1;
                            Pages[i].RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", 1 } }, RouteParams, Params);
                        }
                        else if (i == 1)
                        {
                            Pages[i].Caption = "2";
                            Pages[i].Value = 2;
                            Pages[i].RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", 2 } }, RouteParams, Params);
                        }
                        else if (i == 2)
                        {
                            Pages[i].Caption = "..";
                        }
                    }
                    else if (i >= 12 && rightDotPage)
                    {
                        if (i == 14)
                        {
                            Pages[i].Caption = LastPage.ToString();
                            Pages[i].Value = LastPage;
                            Pages[i].RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", LastPage } }, RouteParams, Params);
                        }
                        else if (i == 13)
                        {
                            Pages[i].Caption = (LastPage - 1).ToString();
                            Pages[i].Value = LastPage - 1;
                            Pages[i].RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", LastPage - 1 } }, RouteParams, Params);
                        }
                        else if (i == 12)
                        {
                            Pages[i].Caption = "..";
                        }
                    }
                }
            }

            // добавим предыдущую и следующую странцы. Получится картина такая:
            // ____ ___ ___ ____ ___ ___ ___ ___ ____ ____ ____ ____
            // | << | 1 | 2 | .. | 6 | 7 | 8 | 9 | .. | 14 | 15 | >> |
            //
            PrevPage = CurrentPage - 1;
            if (PrevPage <= 0)
                PrevPage = 1;
            Pages.Insert(0, new Page
            {
                Value = PrevPage,
                Caption = "&laquo;",
                Disabled = PrevPage == CurrentPage,
                RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", PrevPage } }, RouteParams, Params)
            });
            NextPage = CurrentPage + 1;
            if (NextPage >= LastPage)
                NextPage = LastPage;
            Pages.Add(new Page
            {
                Value = NextPage,
                Caption = "&raquo;",
                Disabled = NextPage == LastPage,
                RouteValues = Merge(new Dictionary<string, object> { { "controller", ControllerName }, { "action", ActionName }, { "Page", NextPage } }, RouteParams, Params)
            });
        }
        private Dictionary<string, object> Merge(params Dictionary<string, object>[] extValues)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            for (int i = 0; i < extValues.Length; i++)
            {
                if (extValues[i] != null && extValues[i].Count > 0)
                    extValues[i].ToList().ForEach(x => { values[x.Key] = x.Value; });
            }
            return values;
        }
    }
    public class Page
    {
        public int Value { get; set; } // числовое значение
        public string Caption { get; set; } // отображаемое значение
        public bool Disabled { get; set; } // актуально для PrevPage и NextPage
        public bool IsCurrenPage { get; set; }
        public Dictionary<string, object> RouteValues { get; set; } // совокупность всех параметров ControllerName, ActionName, RouteParams, Params и Value
    }
    public static class PaginationUtils
    {
        public static RouteValueDictionary ToRouteValueDictionary(this Dictionary<string, object> values)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            if (values != null && values.Count > 0)
                values.ToList().ForEach(x => { routeValues[x.Key] = x.Value; });
            return routeValues;
        }
    }
}
