using Microsoft.EntityFrameworkCore;
using PageMaintenance_AngularProject.Data;
using PageMaintenance_AngularProject.Models;

namespace PageMaintenance_AngularProject.Services
{
    public class FormRepository : IFormInterface
    {
        private readonly PolAdminSysContext _polAdminSysContext;

        public FormRepository(PolAdminSysContext adminSysContext)
        {
            _polAdminSysContext = adminSysContext;
        }

        //Get functions
        public async Task<Form> GetFormById(Guid id)
        {
            var form = await _polAdminSysContext.Forms.FindAsync(id);
            return form != null ? form : null;
        }
        public async Task<List<Form>?> GetAllForms()
        {
            var forms = await _polAdminSysContext.Forms.ToListAsync();
            return forms.Any() ? forms : null;
        }
        public async Task<List<Form>> GetFormByFormName(string formName)
        {

            var forms = await _polAdminSysContext.Forms
                .Where(x => x.Name.Contains(formName))
                .ToListAsync();
            return forms.Any() ? forms : (List<Form>?)null;
        }

        public async Task<List<Form>> GetFormByFormNumber(string formNumber)
        {
            var forms = await _polAdminSysContext.Forms
                .Where(x => x.Number.Contains(formNumber))
                .ToListAsync();
            return forms.Any() ? forms : (List<Form>?)null;
        }

        //Add Form
        public async Task<Form> AddForm(Form form)
        {
            var result = _polAdminSysContext.Forms.AddAsync(form);
            if (result == null)
            {
                return null;
            }
            await _polAdminSysContext.SaveChangesAsync();
            return form;
        }

        //Update functions

        public async Task<Form> EditFormById(Guid formId, Form newForm)
        {
            var currentForm = await _polAdminSysContext.Forms.FindAsync(formId);
            if(currentForm == null)
            {
                return null;
            }

            currentForm.RatebookId = newForm.RatebookId;
            currentForm.TableId = newForm.TableId;
            currentForm.AddChangeDeleteFlag = newForm.AddChangeDeleteFlag;
            currentForm.Sequence = newForm.Sequence;
            currentForm.SubSequence = newForm.SubSequence;
            currentForm.Type = newForm.Type;
            currentForm.MinOccurs = newForm.MinOccurs;
            currentForm.MaxOccurs = newForm.MaxOccurs;
            currentForm.Number = newForm.Number;
            currentForm.Name = newForm.Name;
            currentForm.Comment = newForm.Comment;
            currentForm.HelpText = newForm.HelpText;
            currentForm.Condition = newForm.Condition;
            currentForm.HidePremium = newForm.HidePremium;
            currentForm.TemplateFile = newForm.TemplateFile;
            currentForm.Hidden = newForm.Hidden;
            currentForm.TabCondition = newForm.TabCondition;
            currentForm.TabResourceName = newForm.TabResourceName;
            currentForm.BtnResAdd = newForm.BtnResAdd;
            currentForm.BtnResModify = newForm.BtnResModify;
            currentForm.BtnResDelete = newForm.BtnResDelete;
            currentForm.BtnResViewDetail = newForm.BtnResViewDetail;
            currentForm.BtnResRenumber = newForm.BtnResRenumber;
            currentForm.BtnResView = newForm.BtnResView;
            currentForm.BtnResCopy = newForm.BtnResCopy;
            currentForm.BtnCndAdd = newForm.BtnCndAdd;
            currentForm.BtnCndModify = newForm.BtnCndModify;
            currentForm.BtnCndDelete = newForm.BtnCndDelete;
            currentForm.BtnCndViewDetail = newForm.BtnCndViewDetail;
            currentForm.BtnCndRenumber = newForm.BtnCndRenumber;
            currentForm.BtnCndView = newForm.BtnCndView;
            currentForm.BtnCndCopy = newForm.BtnCndCopy;
            currentForm.BtnLblAdd = newForm.BtnLblAdd;
            currentForm.BtnLblModify = newForm.BtnLblModify;
            currentForm.BtnLblDelete = newForm.BtnLblDelete;
            currentForm.BtnLblViewDetail = newForm.BtnLblViewDetail;
            currentForm.BtnLblRenumber = newForm.BtnLblRenumber;
            currentForm.BtnLblView = newForm.BtnLblView;
            currentForm.BtnLblCopy = newForm.BtnLblCopy;
            currentForm.FormType = newForm.FormType;
            currentForm.ScriptBefore = newForm.ScriptBefore;
            currentForm.ScriptAfter = newForm.ScriptAfter;

            await _polAdminSysContext.SaveChangesAsync();

            return currentForm;



        }

        //Delete functions

        public async Task<Form> DeleteFormById(Guid formId)
        {
            var form = await _polAdminSysContext.Forms.FindAsync(formId);
            if(form == null)
            {
                return null;
            }
            _polAdminSysContext.Forms.Remove(form);
            await _polAdminSysContext.SaveChangesAsync();
            return form;
        }
    }
}
