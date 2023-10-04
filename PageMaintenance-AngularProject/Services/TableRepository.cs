using PageMaintenance_AngularProject.Data;
using PageMaintenance_AngularProject.Models;
using Microsoft.EntityFrameworkCore;

namespace PageMaintenance_AngularProject.Services
{
    public class TableRepository:ITableInterface
    {
        private readonly PolAdminSysContext _polAdminSysContext;
        public TableRepository(PolAdminSysContext dbContext)
        {
            this._polAdminSysContext = dbContext;
        }
        public async Task<List<TableNames>> GetTableNames( )
        {
            var tableNames = await _polAdminSysContext.Aotables.Select(
                table => new TableNames{
                   Id = table.Id,
                   Name = table.Name
               }).ToListAsync();
            return tableNames.Count >0 ? tableNames : (List<TableNames>?)null;
        }

        public async Task<Aotable> GetTableById(Guid id)
        {

            var table = await _polAdminSysContext.Aotables.FindAsync(id);
            return table != null ? table : null;
        }
    }
}
