using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class PageViewModel
    {
        public int _PageNumber { get; set; }
        public int _TotalPages { get; private set; }

        public PageViewModel(int count, int pagenum, int pagesize)
        {
            _PageNumber = pagenum;
            _TotalPages = (int)Math.Ceiling(count / (double)pagesize);
        }

        public bool HasNextPage
        {
            get { return (_PageNumber < _TotalPages); }
        }
        public bool HasPreviousPage
        {
            get { return (_PageNumber > 1); }
        }
    }
}
