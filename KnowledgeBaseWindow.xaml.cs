namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

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

                    ///InitializeDocsFilesList();
                    LoadFilesToTreeView(_userSettingsHelper.KnowledgeBaseDirectory);
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

        private void LoadFilesToTreeView(string rootPath)
        {
            var rootDirectory = new DirectoryInfo(rootPath);
            foreach (var directory in rootDirectory.GetDirectories())
            {
                var directoryNode = new TreeViewItem { Header = directory.Name, Tag = directory };
                FilesTreeView.Items.Add(directoryNode);
                LoadFilesAndDirectories(directory, directoryNode);
            }

            foreach (var file in rootDirectory.GetFiles("*.doc"))
            {
                var fileNode = new TreeViewItem { Header = file.Name, Tag = file };
                FilesTreeView.Items.Add(fileNode);
            }
        }

        private void LoadFilesAndDirectories(DirectoryInfo directoryInfo, TreeViewItem parentNode)
        {
            foreach (var directory in directoryInfo.GetDirectories())
            {
                var directoryNode = new TreeViewItem { Header = directory.Name, Tag = directory };
                parentNode.Items.Add(directoryNode);
                LoadFilesAndDirectories(directory, directoryNode);
            }

            foreach (var file in directoryInfo.GetFiles("*.doc"))
            {
                var fileNode = new TreeViewItem { Header = file.Name, Tag = file };
                parentNode.Items.Add(fileNode);
            }
        }

        private void FilesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = FilesTreeView.SelectedItem as TreeViewItem;
            ResetTreeViewItemColors(FilesTreeView.Items);

            if (selectedItem?.Tag is FileInfo fileInfo)
            {
                selectedItem.Background = Brushes.LightBlue; // Измените цвет на нужный
                DisplayFileContent(fileInfo.FullName);
            }
        }

        private void ResetTreeViewItemColors(ItemCollection items)
        {
            foreach (var item in items)
            {
                var treeViewItem = item as TreeViewItem;
                if (treeViewItem != null)
                {
                    treeViewItem.Background = Brushes.Transparent;
                    ResetTreeViewItemColors(treeViewItem.Items);
                }
            }
        }

        private void DisplayFileContent(string filePath)
        {
            // Здесь вы можете использовать библиотеки для чтения .doc файлов,
            // например, Microsoft Office Interop или Aspose.Words.
            ///ContentTextBox.Text = $"Содержимое файла: {filePath}";
            try
            {
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
            catch (Exception ex)
            {
                TaskDialog.Show("База знаний", $"Ошибка выбора инструкции: {ex.Message}\nОкно будет закрыто!");
                this.Close();
            }
        }
    }
}
















        /*
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
*/