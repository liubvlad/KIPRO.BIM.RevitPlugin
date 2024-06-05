namespace KIPRO.BIM.RevitPlugin
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class UserSettingsHelper
    {
        private static readonly string ApplicationDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KIPRO.BIM.RevitPlugin");
        private static string settingsFilePath = Path.Combine(ApplicationDirectoryPath, "UserSettings.txt");
        private static char spliter = ':';

        public string LogDirectory { get; set; } = Path.Combine(ApplicationDirectoryPath, nameof(LogDirectory));
        public string KnowledgeBaseDirectory { get; set; } = Path.Combine(ApplicationDirectoryPath, nameof(KnowledgeBaseDirectory));
        public string FamilyDirectory { get; set; } = Path.Combine(ApplicationDirectoryPath, nameof(FamilyDirectory));

        // Чтение настроек из файла
        public static UserSettingsHelper Load()
        {
            var data = new UserSettingsHelper();
            if (!File.Exists(settingsFilePath))
            {
                // Возвращаем настройки по умолчанию, если файл не существует
                return data;
            }

            var lines = File.ReadAllLines(settingsFilePath);
            foreach (var line in lines)
            {
                var split = line.Split(spliter);
                switch (split[0])
                {
                    case nameof(LogDirectory): data.LogDirectory = split[1]; break;
                    case nameof(KnowledgeBaseDirectory): data.KnowledgeBaseDirectory = split[1]; break;
                    case nameof(FamilyDirectory): data.FamilyDirectory = split[1]; break;
                }
            }

            return data;
        }

        // Сохранение настроек в файл
        public void Save()
        {
            var lines = new List<string>
            {
                $"{nameof(LogDirectory)}{spliter}{LogDirectory}",
                $"{nameof(KnowledgeBaseDirectory)}{spliter}{KnowledgeBaseDirectory}",
                $"{nameof(FamilyDirectory)}{spliter}{FamilyDirectory}"
            };

            Directory.CreateDirectory(Path.GetDirectoryName(settingsFilePath));
            File.WriteAllLines(settingsFilePath, lines);
        }
    }
}
