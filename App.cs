using Autodesk.Revit.UI;
using System.Reflection;

public class App : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        // Создание панели
        string tabName = "MyCustomTab";
        application.CreateRibbonTab(tabName);
        RibbonPanel panel = application.CreateRibbonPanel(tabName, "MyCustomPanel");

        // Кнопка для обращения к базе семейств
        PushButtonData buttonData1 = new PushButtonData("FamilyDatabaseButton", "Обращение к базе семейств", Assembly.GetExecutingAssembly().Location, "Namespace.FamilyDatabaseCommand");
        panel.AddItem(buttonData1);

        // Кнопка для обращения к базе знаний
        PushButtonData buttonData2 = new PushButtonData("KnowledgeBaseButton", "Обращение к базе знаний", Assembly.GetExecutingAssembly().Location, "Namespace.KnowledgeBaseCommand");
        panel.AddItem(buttonData2);

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}
