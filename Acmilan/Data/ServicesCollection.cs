using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Acmilan.Models;
using Dapper;

namespace Acmilan.Data
{
    public class ServicesCollection
    {
        private readonly DbContext _context;
        public ServicesCollection(DbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCategoryIdByName(string categoryName, IDbTransaction tran)
        {
            string sql = "SELECT * FROM CATEGORY WHERE CategoryName = @CategoryName";
            var result = await _context.Conn.QueryAsync<Category>(sql, new { categoryName }, tran);
            return result.FirstOrDefault().Category_id;
        }

        public async Task<IEnumerable<CompanyServices>> GetAll()
        {
            string sql = "SELECT * FROM COMPANYSERVICES ";
            var result = await _context.Conn.QueryAsync<CompanyServices>(sql);
            return result;
        }

        public async Task<int> AddServiceType(ServiceType serviceType, IDbTransaction tran)
        {
            string sql = "INSERT INTO SERVICETYPE (ServiceType_id, Category_id, Company_id)"
                            + " VALUES (@ServiceType_id, @Category_id, @Company_id);"
                            + " SELECT LAST_INSERT_ID();";
            var ids = await _context.Conn.QueryAsync<int>(sql, serviceType, tran);
            return ids.FirstOrDefault();
        }

        public async Task AddServiceItem(ServiceItem serviceItem, IDbTransaction tran)
        {
            string sql = "INSERT INTO SERVICEITEM (Service_id, ServiceName, ServiceType_id)"
                            + " VALUES (@Service_id, @ServiceName, @ServiceType_id)";
            await _context.Conn.ExecuteAsync(sql, serviceItem, tran);
        }

        public async Task<IEnumerable<ServiceType>> GetServiceType(int company_id)
        {
            string sql = "SELECT * FROM SERVICETYPE WHERE Company_id = @Company_id";
            var result = await _context.Conn.QueryAsync<ServiceType>(sql, new { company_id });
            return result;
        }

        public async Task<string> GetCategoryNameById(int category_id)
        {
            string sql = "SELECT * FROM CATEGORY WHERE Category_id = @Category_id";
            var result = await _context.Conn.QueryAsync<Category>(sql, new { category_id });
            return result.FirstOrDefault().CategoryName;
        }

        public async Task<IEnumerable<ServiceItem>> GetServicesByTypeId(int serviceType_id)
        {
            string sql = "SELECT * FROM SERVICEITEM WHERE ServiceType_id = @ServiceType_id";
            var result = await _context.Conn.QueryAsync<ServiceItem>(sql, new { serviceType_id });
            return result;
        }

        public async Task<IEnumerable<ServiceType>> GetServiceTypes(int company_id, IDbTransaction tran)
        {
            string sql = "SELECT * FROM SERVICETYPE WHERE Company_id = @Company_id";
            var result = await _context.Conn.QueryAsync<ServiceType>(sql, new { company_id }, tran);
            return result;
        }

        public async Task DeleteServiceItems(int serviceType_id, IDbTransaction tran)
        {
            string sql = "DELETE FROM SERVICEITEM WHERE ServiceType_id = @ServiceType_id";
            await _context.Conn.ExecuteAsync(sql, new { serviceType_id }, tran);
        }

        public async Task DeleteServiceTypes(int company_id, IDbTransaction tran)
        {
            string sql = "DELETE FROM SERVICETYPE WHERE Company_id = @Company_id";
            await _context.Conn.ExecuteAsync(sql, new { company_id }, tran);
        }

        public async Task AddSiteView(SiteView model)
        {
            string sql = "INSERT INTO SITEVIEW (Id, View) VALUES (@Id, @View)";
            await _context.Conn.ExecuteAsync(sql, model);
        }

        public async Task<string> GetSiteView(int id)
        {
            string sql = "SELECT * FROM SITEVIEW WHERE Id = @id";
            var result = await _context.Conn.QueryAsync(sql, new { id });
            return result.FirstOrDefault().View;
        }

    }
}
