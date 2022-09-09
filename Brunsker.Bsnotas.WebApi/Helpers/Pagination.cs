using System;
using System.Collections.Generic;
using System.Linq;

namespace Brunsker.Bsnotas.WebApi.Helpers
{
    public class Pagination<T> where T : class
    {
        public int Total { get; set; }
        public double TotalPage { get; set; }
        public IEnumerable<T> Data { get; set; }
        
        public Pagination(int index, int length, IEnumerable<T> data)
        {
            Total = data.Count();
            TotalPage = Math.Ceiling((double)data.Count() / length);
            Data = data.Skip((index - 1) * length).Take(length);
        }
    }
}