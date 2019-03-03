using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchool.Common
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class PaginatedList<T> : List<T>
    {


        public PaginatedList(List<T> item, int count, int pageindex, int pagesize) {

            PageIndex = pageindex;

            TotalPages = (int)Math.Ceiling(count / (decimal)pagesize);

            this.AddRange(item);
        }

        /// <summary>
        /// 当前在第一页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 一共有多少页
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否存在上一页
        /// </summary>
        public bool PreviousPage => PageIndex > 1;

        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool NextPage => PageIndex<TotalPages;

        public static async Task<PaginatedList<T>> CreatePagng
            (IQueryable<T> source, int pageindex, int pagesize)
        {
            var count = await source.CountAsync();

            var item = await source.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
            var dtos = new PaginatedList<T>(item, count, pageindex, pagesize);
            return dtos;
        }
    }
}
