namespace UnoApp.Presentation.Views.Contacts;

public partial record ContactsListModel(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string Branche
);
