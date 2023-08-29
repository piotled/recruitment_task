namespace FrontendBlazor.Model;

public interface IContactsDAO
{
    Task<List<Contact>> GetAll();
    Task<Contact?> Get(int id);
    Task<bool> Create(Contact contact);
    Task<bool> Delete(int contactId);
}