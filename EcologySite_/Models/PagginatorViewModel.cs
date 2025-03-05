namespace EcologySite.Models;
public class PagginatorViewModel<T>
{
    public const int PAGE_DIFF_COUNT = 2;

    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalRecords { get; set; }

    public bool IsStartOfPageList
    {
        get
        {
            return Page <= PAGE_DIFF_COUNT + 1;
        }
    }

    public bool IsEndOfPageList
    {
        get
        {
            return Page >= LastPage - PAGE_DIFF_COUNT;
        }
    }

    public int LastPage
    {
        get
        {
            return (int)Math.Ceiling((decimal)TotalRecords / PerPage);
        }
    }

    public List<int> Pages
    {
        get
        {
            var pageCount = PAGE_DIFF_COUNT * 2 + 1;

            var startPage = Page - PAGE_DIFF_COUNT;
            if (Page < 3)
            {
                pageCount -= PAGE_DIFF_COUNT - Page + 1;
                startPage = 1;
            }

            if (LastPage < startPage + pageCount - 1)
            {
                pageCount = LastPage - startPage + 1;
            }

            return Enumerable
                .Range(startPage, pageCount)
                .ToList();
        }
    }
}