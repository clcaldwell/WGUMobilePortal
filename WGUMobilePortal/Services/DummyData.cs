using System.Collections.Generic;

using WGUMobilePortal.Models;

namespace WGUMobilePortal.Services
{
    public class DummyData
    {
        public static void Main()
        {
            IEnumerable<Term> terms = (IEnumerable<Term>)DBService.GetAllTerm();
            foreach(Term term in terms)
            {
                DBService.RemoveTerm(term.Id);
            };
        }
    }
}
