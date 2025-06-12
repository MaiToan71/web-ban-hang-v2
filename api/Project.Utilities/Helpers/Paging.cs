using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utilities.Helper
{
    public class Pagination
    {
        public static PaginationResponse<T> getPaging<T>(List<T> data, int page = 1, int size = 8) where T : class
        {
            PaginationResponse<T> response = new PaginationResponse<T>();
            if (page > 0 && size > 0)
            {
                response.CurrentPage = page;
                response.ItemsPerPage = size;
                if (data != null && data.Count() > 0)
                {
                    response.TotalItems = data.Count();

                    int page_number = data.Count() / size;

                    response.TotalPages = data.Count() % size > 0 ? page_number + 1 : page_number;

                    data = data.Skip((page - 1) * size).Take(size).ToList();
                }
            }
            response.Data = data;
            return response;
        }
        public static PaginationResponse<T> getPaging<T>(IQueryable<T> data, int page = 1, int size = 8) where T : class
        {
            PaginationResponse<T> response = new PaginationResponse<T>();
            if (page > 0 && size > 0)
            {
                response.CurrentPage = page;
                response.ItemsPerPage = size;
                if (data != null && data.Count() > 0)
                {
                    response.TotalItems = data.Count();

                    int page_number = data.Count() / size;

                    response.TotalPages = data.Count() % size > 0 ? page_number + 1 : page_number;

                    data = data.Skip((page - 1) * size).Take(size);
                }
            }
            response.Data = data?.ToList() ?? new List<T>();
            return response;
        }
    }
    public class PaginationResponse<T>
    {
        public int CurrentPage = 1;
        public int ItemsPerPage = 1;
        public int TotalItems = 0;
        public int ItemCount = 1;
        public int TotalPages = 0;
        public List<T> Data = new List<T>();
    }
}
