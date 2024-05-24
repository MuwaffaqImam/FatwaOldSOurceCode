using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.IServices.SystemDefinition;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Repository;

namespace TeamWork.Service.SystemDefinition
{
    internal class CountryService : ICountryService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<Country> _countryRepository;

        internal CountryService(IMapper imapper, IRepository<Country> countryRepository)
        {
            _imapper = imapper;
            _countryRepository = countryRepository;
        }

        public List<CountryModel> GetAllCountrys()
        {
            try
            {
                List<Country> countrys = _countryRepository.GetAll();
                return _imapper.Map<List<CountryModel>>(countrys);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public CountryModel GetCountry(int id)
        {
            try
            {
                Country country = _countryRepository.GetSingle(id);
                return _imapper.Map<CountryModel>(country);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool AddCountry(CountryModel model)
        {
            try
            {
                Country country = _imapper.Map<Country>(model);
                return _countryRepository.Insert(country) == 0 ? false : true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateCountry(CountryModel model)
        {
            try
            {
                Country country = _countryRepository.GetSingle(model.Id);
                return _countryRepository.Update(country);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteCountry(int id)
        {
            try
            {
                Country country = _countryRepository.GetSingle(id);
                return _countryRepository.Delete(country);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
