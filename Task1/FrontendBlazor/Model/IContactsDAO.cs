namespace FrontendBlazor.Model;

public interface IContactsDAO
{
    Task<bool> Create(Contact contact);
    Task<bool> Delete(int contactId);
    Task<List<Contact>> GetAll();
}