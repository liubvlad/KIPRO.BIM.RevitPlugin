namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using System.Diagnostics;
    using System.IO;
    using System;

    [Transaction(TransactionMode.Manual)]
    public class FamilyDatabaseCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Process.Start("https://link_to_family_database");
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class KnowledgeBaseCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            KnowledgeBaseWindow knowledgeBaseWindow = new KnowledgeBaseWindow();
            knowledgeBaseWindow.ShowDialog();

            return Result.Succeeded;
        }
    }

    
    [Transaction(TransactionMode.Manual)]
    public class CollectLogsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var logDirectory = UserSettingsHelper.Load().LogDirectory;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            
            string projectName = doc.Title;
            string userName = Environment.UserName;
            
            string logFileName = $"{projectName}_{userName}_{DateTime.Now:yyyy-MM-dd}.log";
            string filePath = Path.Combine(logDirectory, logFileName);
            if (!File.Exists(filePath))
            {
                TaskDialog.Show("О логах", "Файл логирования отсутствует");
            }
            else
            {
                TaskDialog.Show("О логах", $"Файл содержит {File.ReadAllLines(filePath).Length} записей логирования");
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class SettingsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
    /*
    public Result PickUpExample(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;
        Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
        Element element = doc.GetElement(myRef);
        ElementId id = element.Id;

        TaskDialog.Show("Hello world!", id.ToString());

        return Result.Succeeded;
    }
    */
}
