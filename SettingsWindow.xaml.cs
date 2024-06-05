namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.UI;
    using System.Windows;
    using System.Windows.Forms;

    public partial class SettingsWindow : Window
    {
        private UserSettingsHelper settings;

        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            settings = UserSettingsHelper.Load();
            LogDirectoryTextBox.Text = settings.LogDirectory;
            KnowledgeBaseDirectoryTextBox.Text = settings.KnowledgeBaseDirectory;
            FamilyDirectoryTextBox.Text = settings.FamilyDirectory;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            settings = new UserSettingsHelper();
            LogDirectoryTextBox.Text = settings.LogDirectory;
            KnowledgeBaseDirectoryTextBox.Text = settings.KnowledgeBaseDirectory;
            FamilyDirectoryTextBox.Text = settings.FamilyDirectory;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            settings.LogDirectory = LogDirectoryTextBox.Text;
            settings.KnowledgeBaseDirectory = KnowledgeBaseDirectoryTextBox.Text;
            settings.FamilyDirectory = FamilyDirectoryTextBox.Text;
            settings.Save();

            TaskDialog.Show("Настройки", "Настройки успешно сохранены!");
            this.Close();
        }

        private void LogDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = SelectFolder();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                LogDirectoryTextBox.Text = selectedPath;
            }
        }

        private void KnowledgeBaseDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = SelectFolder();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                KnowledgeBaseDirectoryTextBox.Text = selectedPath;
            }
        }

        private void FamilyDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = SelectFolder();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                FamilyDirectoryTextBox.Text = selectedPath;
            }
        }

        private string SelectFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Укажите директорию";
                dialog.ShowNewFolderButton = true;
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return null;
        }
    }
}
