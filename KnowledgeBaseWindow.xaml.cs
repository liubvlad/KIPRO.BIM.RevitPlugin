namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Markup;

    public partial class KnowledgeBaseWindow : Window
    {
        private string baseDirectory = @"C:\Logs\LocalInstructions";

        public KnowledgeBaseWindow()
        {
            InitializeComponent();

            InitializeDocsFilesList();
        }

        [Obsolete("Temporery Solution")]
        private void InitializeDocsFilesList()
        {
            var filesList = new List<string>();
            var directories = Directory.GetDirectories(baseDirectory);

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
            if (FilesListBox.SelectedItem != null)
            {
                string selectedFile = FilesListBox.SelectedItem.ToString();
                string[] parts = selectedFile.Split('-');
                string directory = parts[0].Trim();
                string fileName = parts[1].Trim();

                string filePath = Path.Combine(baseDirectory, directory, fileName);
                if (File.Exists(filePath))
                {
                    string fileContent = File.ReadAllText(filePath);
                    ContentTextBox.Text = fileContent;
                }
                else
                {
                    ContentTextBox.Text = string.Empty;
                    TaskDialog.Show("Knowledge Base", "File not found.");
                }
            }
        }
    }
}
