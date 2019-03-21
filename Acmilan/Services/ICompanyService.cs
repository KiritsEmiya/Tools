using Acmilan.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acmilan.Services
{
    public interface ICompanyService
    {
        Task AddAsync(Company model);
        Task<string> GetSiteView(int id);
        Task AddSiteView(string code);
        Task AddServiceList(ViewServiceList lists);
        Task EditServiceList(ViewServiceList lists);
        void AddItem();
        Task<ViewServiceList> GetServiceList(int company_id);
        IEnumerable<Category> SelectList();
        Task<IEnumerable<Company>> GetCompanyAsync();
        Task<IEnumerable<CompanyServices>> GetServiceAsync();
        Task<IEnumerable<Company>> GetCompanyAsync(int value);
        Task<Company> GetByIdAsync(int companyId);

    }
}
