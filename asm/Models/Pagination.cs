using System.ComponentModel.DataAnnotations.Schema;

namespace asm.Models
{
    [NotMapped]
    public class Pagination
    {
        public int ToltalItems { get; set;}
        public int CurrentPage{ get; set;}
        public int PageSize { get; set;}
        public int TotalPage { get ; set;}
        public int StartPage { get; set; }
        public int EndPage{get; set; }


        public Pagination() { }
        public Pagination( int toltalItems, int page , int pageSize = 10)
        {
            int toltalPages = (int)Math.Ceiling((decimal)toltalItems/(decimal)pageSize);
            int currentPage = page;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;
            if(startPage <= 0)
            {
                endPage = endPage - (startPage  - 1);
                startPage = 1;
            }
            if(endPage > toltalPages)
            {
                endPage = toltalPages;
                if(endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            ToltalItems = toltalItems;
            CurrentPage = startPage;
            PageSize = pageSize;
            TotalPage = toltalPages;
            StartPage = startPage;
            EndPage = endPage;

        }

    } 
}
