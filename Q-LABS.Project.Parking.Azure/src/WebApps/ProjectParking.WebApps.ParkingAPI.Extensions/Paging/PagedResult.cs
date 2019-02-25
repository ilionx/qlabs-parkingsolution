using Microsoft.AspNetCore.Mvc;
using ProjectParking.WebApps.ParkingAPI.Extensions.Hateoas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectParking.WebApps.ParkingAPI.Extensions.Paging
{
    public class PagedResult<T>
    {
        public List<T> Items { get; }
        public int TotalItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling(this.TotalItems / (double)this.PageSize);
        public bool HasPreviousPage => this.PageNumber > 0;
        public bool HasNextPage => this.PageNumber < this.TotalPages - 1;
        public int NextPageNumber => this.HasNextPage ? this.PageNumber + 1 : this.TotalPages - 1;
        public int PreviousPageNumber => this.HasPreviousPage ? this.PageNumber - 1 : 0;

        public PagedResult(IQueryable<T> source, int pageNumber, int pageSize)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.TotalItems = source.Count();
            this.Items = source
                            .Skip(pageSize * (pageNumber - 1))
                            .Take(pageSize)
                            .ToList();
        }



    }

    public static class PagedResultExtensions
    {

        public static LinkInfoWrapper<List<T>> BuildLinks<T>(this PagedResult<T> pagedResult, IUrlHelper uriHelper, string method)
        {
            return new LinkInfoWrapper<List<T>>
            {
                Value = pagedResult.Items,
                Links = BuildUrls(pagedResult, uriHelper, method)
            };
        }

        private static List<LinkInfo> BuildUrls<T>(PagedResult<T> pagedResult, IUrlHelper uriHelper, string method)
        {

            var result = new List<LinkInfo> { };

            result.Add(new LinkInfo
            {
                Rel = "self",
                Method = "GET",
                Href = uriHelper.Link(method, new { PageNumber = pagedResult.PageNumber, PageSize = pagedResult.PageSize })
            });

            if (pagedResult.HasPreviousPage)
            {
                result.Add(new LinkInfo
                {
                    Rel = "previous-page",
                    Method = "GET",
                    Href = uriHelper.Link(method, new { PageNumber = pagedResult.PreviousPageNumber, PageSize = pagedResult.PageSize })
                });
            }


            if (pagedResult.HasNextPage)
            {
                result.Add(new LinkInfo
                {
                    Rel = "next-page",
                    Method = "GET",
                    Href = uriHelper.Link(method, new { PageNumber = pagedResult.NextPageNumber, PageSize = pagedResult.PageSize })
                });
            }

            return result;

        }
    }
}
