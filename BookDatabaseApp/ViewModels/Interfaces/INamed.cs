namespace BookDatabaseApp.ViewModels;

internal interface INamed
{
    string PageName
    {
        get => this.GetType().FullName ?? this.GetType().Name;
    }
}
