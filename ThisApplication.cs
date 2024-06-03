using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System;
using System.IO;
using Autodesk.Revit.DB.Events;

public class ThisApplication : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        application.ControlledApplication.ApplicationInitialized += new EventHandler<ApplicationInitializedEventArgs>(OnApplicationInitialized);
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        application.ControlledApplication.ApplicationInitialized -= OnApplicationInitialized;
        return Result.Succeeded;
    }

    private void OnApplicationInitialized(object sender, ApplicationInitializedEventArgs e)
    {
        // Сбор логов
        string userName = Environment.UserName;
        string projectName = "ProjectName"; // Здесь нужно получить реальное имя проекта
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
    }
}
