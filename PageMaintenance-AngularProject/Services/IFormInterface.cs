using PageMaintenance_AngularProject.Models;

namespace PageMaintenance_AngularProject.Services
{
    public interface IFormInterface
    {
        Task<Form> AddForm(Form form);
        Task<Form> DeleteFormById(Guid formId);
        Task<Form> EditFormById(Form newForm);
        //Task<List<Form>?> GetAllForms();
        Task<List<Form>?> GetAllForms(int pageNumber, int pageSize);
        Task<List<Form>?> GetFormByFormName(string formName, int pageNumber, int pageSize);
        Task<List<Form>?> GetFormByFormNumber(string formNumber, int pageNumber, int pageSize);

        //Task<List<Form>?> GetFormByFormName(string formName);
        //Task<List<Form>?> GetFormByFormNumber(string formNumber);
        Task<Form> GetFormById(Guid id);
    }
}
