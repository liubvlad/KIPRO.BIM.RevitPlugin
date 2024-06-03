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
            Process.Start("https://link_to_knowledge_base");
            return Result.Succeeded;
        }
    }

    
    [Transaction(TransactionMode.Manual)]
    public class CollectLogsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Получение текущего документа
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Получение названия проекта
            string projectName = doc.Title;

            string userName = Environment.UserName;
            string date = DateTime.Now.ToString("yyyyMMdd");
            string logFileName = $"{projectName}_{userName}_{date}.log";

            // Путь для сохранения логов
            string logDirectory = @"C:\Logs"; // Здесь нужно предусмотреть настройку пути
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFilePath = Path.Combine(logDirectory, logFileName);
            File.WriteAllText(logFilePath, "Log content"); // Здесь нужно собрать реальные логи

            TaskDialog.Show("Log Collection", "Log collected and saved at " + logFilePath);
            return Result.Succeeded;
        }
    }
    
}
