namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System;
    using System.Reflection;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Events;
    using System.IO;
    using System.Windows.Media.Imaging;
    using KIPRO.BIM.RevitPlugin.Properties;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    public class App : IExternalApplication
    {
        static AddInId addinId = new AddInId(new Guid("CDE4EA5A-2933-430B-926B-82BEA5E3A069"));
        private string logDirectory;

        public Result OnStartup(UIControlledApplication application)
        {
            // Загрузка пути из файла настроек
            logDirectory = UserSettingsHelper.Load().LogDirectory;

            // Сбор логов при запуске Revit
            if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
            application.ControlledApplication.DocumentChanged += LoggingOnDocumentChanged;

            // Создание панели с кнопочками
            CreateUIPanel(application);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            //application.ControlledApplication.ApplicationInitialized -= OnApplicationInitialized;

            application.ControlledApplication.DocumentChanged -= LoggingOnDocumentChanged;

            return Result.Succeeded;
        }

        private void CreateUIPanel(UIControlledApplication application)
        {
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
            AddButton(
                panel,
                "FamilyDatabaseButton",
                "Обращение к базе семейств",
                assemblyPath,
                "KIPRO.BIM.RevitPlugin.FamilyDatabaseCommand",
                Resources.icon_family);

            // Добавление кнопки для обращения к базе знаний
            AddButton(
                panel,
                "KnowledgeBaseButton",
                "Обращение к базе знаний",
                assemblyPath,
                "KIPRO.BIM.RevitPlugin.KnowledgeBaseCommand",
                Resources.icon_knowledge);

            // Добавление кнопки для проверки собранных логов
            AddButton(
                panel,
                "CollectLogsButton",
                "Сбор логов",
                assemblyPath,
                "KIPRO.BIM.RevitPlugin.CollectLogsCommand",
                Resources.icon_logs);

            // Добавление кнопки настроек
            AddButton(
                panel,
                "SettingsButton",
                "Настройки",
                assemblyPath,
                "KIPRO.BIM.RevitPlugin.SettingsCommand",
                Resources.icon_settings);
        }

        private void AddButton(
            RibbonPanel panel,
            string buttonName,
            string buttonText,
            string assemblyPath,
            string className,
            Bitmap resourceIconName)
        {
            PushButtonData buttonData = new PushButtonData(buttonName, buttonText, assemblyPath, className);

            try
            {
                buttonData.Image = LoadBitmapImageFromResources(resourceIconName, 16, 16);
                buttonData.LargeImage = LoadBitmapImageFromResources(resourceIconName, 24, 24);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Set image error!", ex.Message);
            }

            panel.AddItem(buttonData);
        }

        private void LoggingOnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();
            string projectName = doc.Title;
            string userName = doc.Application.Username;
            ///string userName = Environment.UserName;
            var timespan = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");


            foreach (ElementId id in e.GetAddedElementIds())
            {
                Element element = doc.GetElement(id);
                if (element == null) continue;

                string elementInfo = GetElementInfo(element);
                LogChange(projectName, timespan, "Add", userName, elementInfo);
            }

            foreach (ElementId id in e.GetModifiedElementIds())
            {
                Element element = doc.GetElement(id);
                if (element == null) continue;

                string elementInfo = GetElementInfo(element);
                LogChange(projectName, timespan, "Mod", userName, elementInfo);
            }

            foreach (ElementId id in e.GetDeletedElementIds())
            {
                string elementInfo = $"id={id}";
                LogChange(projectName, timespan, "Del", userName, elementInfo);
            }
        }

        private string GetElementInfo(Element element)
        {
            // Закоментировано из-за излишек информации
            /*
            StringBuilder parametersInfo = new StringBuilder();
            foreach (Parameter param in element.Parameters)
            {
                string paramName = param.Definition.Name;
                string paramValue = param.AsValueString(); // AsValueString() возвращает значение параметра в виде строки
                parametersInfo.Append($"{paramName}={paramValue}; ");
            }*/

            string locationInfo = "None";
            if (element.Location is LocationPoint locationPoint)
            {
                XYZ point = locationPoint.Point;
                locationInfo = $"Point(X={point.X}, Y={point.Y}, Z={point.Z})";
            }
            else if (element.Location is LocationCurve locationCurve)
            {
                Curve curve = locationCurve.Curve;
                locationInfo = $"Curve(X0={curve.GetEndPoint(0).X}, Y0={curve.GetEndPoint(0).Y}, Z0={curve.GetEndPoint(0).Z}, " +
                               $"X1={curve.GetEndPoint(1).X}, Y1={curve.GetEndPoint(1).Y}, Z1={curve.GetEndPoint(1).Z})";
            }

            string elementInfo = 
                $"Id={element.Id.IntegerValue}: " +
                $"Name={element.Name}," +
                $"Category={element.Category.Name}," +
                ///$"Parameters={parametersInfo}, " +
                $"Location={locationInfo}";

            return elementInfo;
        }

        private void LogChange(
            string projectName,
            string datetime,
            string type,
            string userName,
            string elementInfo)
        {
            string logFileName = $"{projectName}_{userName}_{DateTime.Now:yyyy-MM-dd}.log";

            string logEntry = $"{datetime},{type},{userName},{elementInfo}";
            File.AppendAllText(Path.Combine(logDirectory, logFileName), logEntry + Environment.NewLine);
        }

        private BitmapImage LoadBitmapImageFromResources(Bitmap resource, int width, int height)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                resource.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.DecodePixelWidth = width;
                bitmapImage.DecodePixelHeight = height;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
