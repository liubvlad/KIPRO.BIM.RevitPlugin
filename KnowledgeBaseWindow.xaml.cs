namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    public partial class KnowledgeBaseWindow : Window
    {
        private UserSettingsHelper _userSettingsHelper;

        public KnowledgeBaseWindow()
        {
            InitializeComponent();

            _userSettingsHelper = UserSettingsHelper.Load();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_userSettingsHelper.KnowledgeBaseDirectory));

                // Получение всех файлов .doc в директории и поддиректориях
                string[] files = Directory.GetFiles(_userSettingsHelper.KnowledgeBaseDirectory, "*.doc", SearchOption.AllDirectories);

                // Открытие первого найденного файла (можно адаптировать для выбора файла пользователем)
                if (files.Length > 0)
                {
                    /*Process.Start(new ProcessStartInfo
                    {
                        FileName = files[0],
                        UseShellExecute = true
                    });*/

                    InitializeDocsFilesList();
                }
                else
                {
                    TaskDialog.Show("База знаний", "Не найдено файлов инструкций.\nОкно будет закрыто!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("База знаний", $"Ошибка: {ex.Message}\nОкно будет закрыто!");
                this.Close();
            }
        }

        [Obsolete("Temporery Solution")]
        private void InitializeDocsFilesList()
        {
            var filesList = new List<string>();
            var directories = Directory.GetDirectories(_userSettingsHelper.KnowledgeBaseDirectory);

            foreach (var dir in directories)
            {
                var files = Directory.GetFiles(dir);

                foreach (var file in files)
                {
                    filesList.Add($"{Path.GetFileName(dir)} - {Path.GetFileName(file)}");
                }
            }

            FilesListBox.ItemsSource = filesList;
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FilesListBox.SelectedItem != null)
                {
                    string selectedFile = FilesListBox.SelectedItem.ToString();
                    string[] parts = selectedFile.Split('-');
                    string directory = parts[0].Trim();
                    string fileName = parts[1].Trim();

                    // TODO исп. доп класс для хранения полного пути
                    string filePath = Path.Combine(_userSettingsHelper.KnowledgeBaseDirectory, directory, fileName);
                    if (File.Exists(filePath))
                    {
                        string fileContent = File.ReadAllText(filePath);
                        ContentTextBox.Text = fileContent;
                    }
                    else
                    {
                        ContentTextBox.Text = string.Empty;
                        TaskDialog.Show("База знаний", "Файл не найден.");
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("База знаний", $"Ошибка выбора инструкции: {ex.Message}\nОкно будет закрыто!");
                this.Close();
            }
        }
    }
}
