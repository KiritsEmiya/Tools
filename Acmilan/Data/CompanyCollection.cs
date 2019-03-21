using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acmilan.Models;
using Dapper;

namespace Acmilan.Data
{
    public class CompanyCollection
    {
        private readonly DbContext _context;

        public CompanyCollection(DbContext context) {
            _context = context;
        }

        public void AddItem(ModelItem model)
        {
            string sQuery = "INSERT INTO MODELITEM (Id, Item)"
                            + " VALUES(@Id, @Item)";
            //_context.Conn.ExecuteAsync(sQuery, model);
            _context.Conn.Execute(sQuery, model);
        }

        public IEnumerable<Category> SelectList()
        {
            string sQuery = "SELECT * FROM CATEGORY";
            //_context.Conn.ExecuteAsync(sQuery, model);
            var result = _context.Conn.Query<Category>(sQuery);
            return result;
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            string sQuery = "SELECT * FROM COMPANY ";
            var result = await _context.Conn.QueryAsync<Company>(sQuery);
            return result;
        }

        public async Task<IEnumerable<Company>> GetAll(int value)
        {
            string stat = (value == 0 ? "1=1" : "Company_category_id = @Category_id");
            string sQuery = "SELECT * FROM COMPANY WHERE " + stat;
            var result = await _context.Conn.QueryAsync<Company>(sQuery, new { Category_id = value });
            return result;
        }

        public async Task<Company> GetById(int id)
        {

            string sQuery = "SELECT * FROM COMPANY WHERE id = @Id";
            var result = await _context.Conn.QueryAsync<Company>(sQuery,new { id });
            return result.FirstOrDefault();
        }

        public async Task Add(Company model)
        {
            string sQuery = "INSERT INTO COMPANY (Company_id, CompanyName, Address,ZipCode,Telephone,Facsimile,Email,Website,Services,Company_category_id)"
                            + " VALUES(@Company_id, @CompanyName, @Address,@ZipCode,@Telephone,@Facsimile,@Email,@Website,@Services,@Company_category_id)";
            await _context.Conn.ExecuteAsync(sQuery, model);
        }

        public static implicit operator CompanyCollection(ServicesCollection v)
        {
            throw new NotImplementedException();
        }
    }
}
