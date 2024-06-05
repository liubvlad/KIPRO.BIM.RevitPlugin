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
            // Локальная папка с инструкциями
            string instructionsDirectory = @"C:\Logs\LocalInstructions";

            // Получение всех файлов .doc в директории и поддиректориях
            string[] files = Directory.GetFiles(instructionsDirectory, "*.doc", SearchOption.AllDirectories);

            // Открытие первого найденного файла (можно адаптировать для выбора файла пользователем)
            if (files.Length > 0)
            {
                /*Process.Start(new ProcessStartInfo
                {
                    FileName = files[0],
                    UseShellExecute = true
                });*/

                KnowledgeBaseWindow knowledgeBaseWindow = new KnowledgeBaseWindow();
                knowledgeBaseWindow.ShowDialog();
            }
            else
            {
                TaskDialog.Show("Knowledge Base", "No instruction files found.");
            }

            return Result.Succeeded;
        }
    }

    
    [Transaction(TransactionMode.Manual)]
    public class CollectLogsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            
            string projectName = doc.Title;
            string userName = Environment.UserName;
            
            string logFileName = $"{projectName}_{userName}_{DateTime.Now:yyyy-MM-dd}.log";
            string filePath = Path.Combine(@"C:\Logs", logFileName);
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
