using System;

namespace NetCoreBoot.ViewModel
{
    public class PageModel
    {
        public int PageSize { get; set; } = 40;

        public int PageIndex { get; set; } = 1;

        public string Key { get; set; }
    }
}
