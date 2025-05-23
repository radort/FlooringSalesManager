using System.Windows;
using FlooringSalesManager.Data;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        DatabaseHelper.EnsureTables();
        base.OnStartup(e);
        
    }
}