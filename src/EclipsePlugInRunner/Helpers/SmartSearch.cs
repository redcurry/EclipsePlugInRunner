using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.Helpers
{
    public class SmartSearch
    {
        private const int MaximumResults = 20;

        private readonly IEnumerable<PatientSummary> _patients;

        public SmartSearch(IEnumerable<PatientSummary> patients)
        {
            _patients = patients;
        }

        public IEnumerable<PatientSummary> GetMatches(string searchText)
        {
            return !string.IsNullOrEmpty(searchText)
                ? _patients
                    .Where(p => IsMatch(p, searchText))
                    .OrderByDescending(p => p.CreationDateTime)
                    .Take(MaximumResults)
                : new PatientSummary[0];
        }

        private bool IsMatch(PatientSummary p, string searchText)
        {
            var searchTerms = GetSearchTerms(searchText);

            if (searchTerms.Length == 0)
            {
                return false;
            }
            else if (searchTerms.Length == 1)
            {
                return IsMatch(p.Id, searchTerms[0]) ||
                       IsMatch(p.LastName, searchTerms[0]) ||
                       IsMatch(p.FirstName, searchTerms[0]);
            }
            else
            {
                return IsMatch(p.LastName, searchTerms[0]) && IsMatch(p.FirstName, searchTerms[1]) ||
                       IsMatch(p.FirstName, searchTerms[0]) && IsMatch(p.LastName, searchTerms[1]);
            }
        }

        private string[] GetSearchTerms(string searchText)
        {
            // Split by whitespace and remove any separators
            return searchText.Split().Select(t => t.Trim(',', ';')).ToArray();
        }

        private bool IsMatch(string actual, string candidate)
        {
            return actual.ToUpper().Contains(candidate.ToUpper());
        }
    }
}
