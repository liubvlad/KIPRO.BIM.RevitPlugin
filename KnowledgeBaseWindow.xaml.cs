namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public partial class KnowledgeBaseWindow : Window
    {
        private const string FileExtensionPattern = "*.doc";
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

                string[] files = Directory.GetFiles(_userSettingsHelper.KnowledgeBaseDirectory, FileExtensionPattern, SearchOption.AllDirectories);

                if (files.Length > 0)
                {
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

            foreach (var file in rootDirectory.GetFiles(FileExtensionPattern))
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

            foreach (var file in directoryInfo.GetFiles(FileExtensionPattern))
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
                selectedItem.Background = Brushes.LightBlue;
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
