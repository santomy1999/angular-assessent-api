using PageMaintenance_AngularProject.Models;

namespace PageMaintenance_AngularProject.Services
{
    public interface ITableInterface
    {
        Task<Aotable?> GetTableById(Guid id);
        Task<List<TableNames?>> GetTableNames();
    }
}
