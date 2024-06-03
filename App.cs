namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using Autodesk.Revit.ApplicationServices;
    using System;
    using System.Reflection;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Events;
    using System.IO;
    using System.Windows.Media.Imaging;

    public class App : IExternalApplication
    {
        static AddInId addinId = new AddInId(new Guid("CDE4EA5A-2933-430B-926B-82BEA5E3A069"));

        public Result OnStartup(UIControlledApplication application)
        {
            // Сбор логов при запуске Revit
            CollectLogs();


            // Создание панели
            string tabName = "KiPRO";
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception) { /* Вкладка уже существует */ }

            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Панель");

            // Путь к сборке
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // Добавление кнопки для обращения к базе семейств
            AddButton(panel, "FamilyDatabaseButton", "Обращение к базе семейств", assemblyPath, "KIPRO.BIM.RevitPlugin.FamilyDatabaseCommand", "family_database_icon.png");

            // Добавление кнопки для обращения к базе знаний
            AddButton(panel, "KnowledgeBaseButton", "Обращение к базе знаний", assemblyPath, "KIPRO.BIM.RevitPlugin.KnowledgeBaseCommand", "knowledge_base_icon.png");

            // Добавление кнопки для ручного сбора логов
            AddButton(panel, "CollectLogsButton", "Сбор логов", assemblyPath, "KIPRO.BIM.RevitPlugin.CollectLogsCommand", "collect_logs_icon.png");


            return Result.Succeeded;
        }

        private void AddButton(RibbonPanel panel, string buttonName, string buttonText, string assemblyPath, string className, string iconName)
        {
            string iconPath = Path.Combine(Path.GetDirectoryName(assemblyPath), "Resources", iconName);
            PushButtonData buttonData = new PushButtonData(buttonName, buttonText, assemblyPath, className);

            // Проверяем, существует ли файл иконки
            if (File.Exists(iconPath))
            {
                Uri iconUri = new Uri(iconPath, UriKind.Absolute);
                BitmapImage iconImage = new BitmapImage(iconUri);
                buttonData.Image = ResizeImage(iconImage, 16, 16);
                buttonData.LargeImage = ResizeImage(iconImage, 32, 32);
            }

            panel.AddItem(buttonData);
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            //application.ControlledApplication.ApplicationInitialized -= OnApplicationInitialized;
            return Result.Succeeded;
        }


        private void CollectLogs()
        {
            // Логика сбора логов
            string userName = Environment.UserName;
            string projectName = "ProjectName"; // Здесь нужно получить реальное имя проекта
            string date = DateTime.Now.ToString("yyyyMMdd");
            string logFileName = $"{projectName}_{userName}_{date}.init";

            // Путь для сохранения логов
            string logDirectory = @"C:\Logs"; // Здесь нужно предусмотреть настройку пути
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFilePath = Path.Combine(logDirectory, logFileName);
            File.WriteAllText(logFilePath, "Log init"); // Здесь нужно собрать реальные логи
        }

        private BitmapImage ResizeImage(BitmapImage originalImage, int width, int height)
        {
            // Создание нового изображения с заданными размерами
            BitmapImage resizedImage = new BitmapImage();
            resizedImage.BeginInit();
            resizedImage.UriSource = originalImage.UriSource;
            resizedImage.DecodePixelWidth = width;
            resizedImage.DecodePixelHeight = height;
            resizedImage.EndInit();
            return resizedImage;
        }
    }
}
