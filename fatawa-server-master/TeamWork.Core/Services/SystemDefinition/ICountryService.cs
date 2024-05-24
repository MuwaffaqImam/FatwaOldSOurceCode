using System.Collections.Generic;
using TeamWork.Core.Models.SystemDefinition;

namespace TeamWork.Core.IServices.SystemDefinition
{
    public interface ICountryService
    {
        List<CountryModel> GetAllCountrys();
        CountryModel GetCountry(int id);
        bool AddCountry(CountryModel model);
        bool UpdateCountry(CountryModel model);
        bool DeleteCountry(int id);
    }
}
