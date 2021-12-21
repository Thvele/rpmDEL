using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rpmDEL.Models
{
    public class SortViewModel
    {
        public SortState IdSort { get; private set; }
        public SortState LoginSort { get; private set; }
        public SortState EmailSort { get; private set; }
        public SortState _CurrentSort { get; private set;  }

        public SortViewModel(SortState sortOrder)
        {
            IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
            LoginSort = sortOrder == SortState.LoginAsc ? SortState.LoginDesc : SortState.LoginAsc;
            EmailSort = sortOrder == SortState.EmailAsc ? SortState.EmailDesc : SortState.EmailAsc;

            _CurrentSort = sortOrder;
        }
    }
}
