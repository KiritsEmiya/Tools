using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Acmilan.Models;

namespace Acmilan.Services
{
    public class CompanyServiceImpl : ICompanyService
    {

        public async Task<IEnumerable<CompanyServices>> GetServiceAsync()
        {
            using (var ctx = new Data.DbContext())
            {
                return await ctx.ServicesCollection.GetAll();
            }
        }

        public async Task AddAsync(Company model)
        {
            using (var ctx = new Data.DbContext())
            {
                await ctx.CompanyCollection.Add(model);
            }
        }

        private const string FILE_NAME = @"C:\Users\Tempest绝园\Desktop\data.txt";
        public void AddItem()
        {
            StreamReader sr = new StreamReader(FILE_NAME);
            List<string> LineList;
            string sLine = "";

            var model = new ModelItem();
            sLine = sr.ReadLine();
            LineList = new List<string>(sLine.Split('$'));
            sr.Close();
            //string str = LineList[1].Replace("%", "\r\n");
            using (var ctx = new Data.DbContext())
            {
                foreach (string st in LineList)
                {
                    model.Item = st.Replace("%", "\r\n"); ;
                    ctx.CompanyCollection.AddItem(model);
                }
            }
            //Console.WriteLine(str);
            //Console.ReadLine();
            //return str;
        }

        public async Task<Company> GetByIdAsync(int companyId)
        {
            using (var ctx = new Data.DbContext())
            {
                return await ctx.CompanyCollection.GetById(companyId);
            }
        }

        public async Task<IEnumerable<Company>> GetCompanyAsync()
        {
            using (var ctx = new Data.DbContext())
            {
                return await ctx.CompanyCollection.GetAll();
            }
        }

        public async Task<IEnumerable<Company>> GetCompanyAsync(int value)
        {
            using (var ctx = new Data.DbContext())
            {
                return await ctx.CompanyCollection.GetAll(value);
            }
        }

        public IEnumerable<Category> SelectList()
        {
            using (var ctx = new Data.DbContext())
            {
                return ctx.CompanyCollection.SelectList();
            }
        }

        public async Task AddServiceList(ViewServiceList lists)
        {
            using (var ctx = new Data.DbContext())
            {
                IDbTransaction tran = ctx.Conn.BeginTransaction();
                try
                {
                    foreach (var list in lists.Lists)
                    {
                        //var category_id = await ctx.ServicesCollection.GetCategoryIdByName(list.Category, tran);
                        var serviceType = new ServiceType
                        {
                            Category_id = list.Category_id,
                            Company_id = lists.Company_id
                        };
                        var serviceType_id = await ctx.ServicesCollection.AddServiceType(serviceType, tran);

                        foreach (var item in list.ListB)
                        {
                            var serviceItem = new ServiceItem
                            {
                                ServiceName = item.ServiceName,
                                ServiceType_id = serviceType_id
                            };
                            await ctx.ServicesCollection.AddServiceItem(serviceItem, tran);
                        }

                    }
                tran.Commit();
                }
                catch(Exception  ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<ViewServiceList> GetServiceList(int company_id)
        {
            using (var ctx = new Data.DbContext())
            {
                var serviceTypes = await ctx.ServicesCollection.GetServiceType(company_id);
                var viewServiceTypes = new List<ViewServiceType>(serviceTypes.Count());
                foreach (var type in serviceTypes)
                {
                    string categoryName = await ctx.ServicesCollection.GetCategoryNameById(type.Category_id);
                    var serviceItems = await ctx.ServicesCollection.GetServicesByTypeId(type.ServiceType_id);
                    var viewServiceType = new ViewServiceType
                    {
                        Category_id = type.Category_id,
                        Category = categoryName,
                        ListB = new List<ViewServiceItem>(serviceItems.Count())
                    };
                    foreach (var item in serviceItems)
                    {
                        var viewServiceItem = new ViewServiceItem
                        {
                            Service_id = item.Service_id,
                            ServiceName = item.ServiceName
                        };
                        viewServiceType.ListB.Add(viewServiceItem);
                    }
                    viewServiceTypes.Add(viewServiceType);
                }
                var serviceLists = new ViewServiceList
                {
                    Company_id = company_id,
                    Lists = viewServiceTypes
                };
                return serviceLists;
            }

        }

        public async Task EditServiceList(ViewServiceList lists)
        {
            using (var ctx = new Data.DbContext())
            {
                IDbTransaction tran = ctx.Conn.BeginTransaction();
                try
                {
                    int company_id = lists.Company_id;
                    var serviceTypes = await ctx.ServicesCollection.GetServiceTypes(company_id, tran);
                    foreach (var type in serviceTypes)
                    {
                        await ctx.ServicesCollection.DeleteServiceItems(type.ServiceType_id, tran);
                    }
                    await ctx.ServicesCollection.DeleteServiceTypes(company_id, tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                await AddServiceList(lists);
            }
        }

        public async Task AddSiteView(string code)
        {
            var model = new SiteView
            {
                View = code
            };
            using (var ctx = new Data.DbContext())
            {
                await ctx.ServicesCollection.AddSiteView(model);
            }
        }

        public async Task<string> GetSiteView(int id)
        {
            using (var ctx = new Data.DbContext())
            {
                return await ctx.ServicesCollection.GetSiteView(id);
            }
        }
    }
}
