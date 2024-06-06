namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.DB.Structure;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Linq;

    public partial class FamiliesWindow : Window
    {
        private const string FileExtensionPattern = "*.rfa";
        private string FamilyRootPath;
        private ExternalCommandData commandData;
        
        public FamiliesWindow(ExternalCommandData commandData)
        {
            InitializeComponent();
            this.commandData = commandData;

            FamilyRootPath = UserSettingsHelper.Load().FamilyDirectory;
            LoadFilesToTreeView(FamilyRootPath);
        }

        private void PlaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (FamiliesTreeView.SelectedItem != null)
            {
                var selectedItem = FamiliesTreeView.SelectedItem as TreeViewItem;
                var filePath = (selectedItem.Tag as FileInfo)?.FullName;

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    var result = InputFamilyInDocument(filePath);
                    TaskDialog.Show("Окно выбора семейств", result == Result.Succeeded ? $"Размещаем: {selectedItem.Header}" : "Ошибка при размещении семейства.");
                }
                else
                {
                    TaskDialog.Show("Окно выбора семейств", "Выберите элемент для размещения.");
                }
                this.Close();
            }
            else
            {
                TaskDialog.Show("Окно выбора семейств", "Выберите элемент для размещения.");
                this.Close();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFilesToTreeView(FamilyRootPath);
        }

        private void LoadFilesToTreeView(string rootPath)
        {
            FamiliesTreeView.Items.Clear();
            var rootDirectory = new DirectoryInfo(rootPath);
            foreach (var directory in rootDirectory.GetDirectories())
            {
                var directoryNode = new TreeViewItem { Header = directory.Name, Tag = directory };
                FamiliesTreeView.Items.Add(directoryNode);
                LoadFilesAndDirectories(directory, directoryNode);
            }

            foreach (var file in rootDirectory.GetFiles(FileExtensionPattern))
            {
                var fileNode = new TreeViewItem { Header = file.Name, Tag = file };
                FamiliesTreeView.Items.Add(fileNode);
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

        private Result InputFamilyInDocument(string familyPath)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (!File.Exists(familyPath))
            {
                return Result.Failed;
            }

            Family family = null;
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Load Family");

                if (!doc.LoadFamily(familyPath, out family))
                {
                    return Result.Failed;
                }

                tx.Commit();
            }

            if (family != null)
            {
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Place Family");

                    // Get the FamilySymbol (type) to place
                    var familySymbols = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilySymbol))
                        .OfCategoryId(family.FamilyCategoryId)
                        .Cast<FamilySymbol>()
                        .ToList();

                    if (familySymbols.Count == 0)
                    {
                        return Result.Failed;
                    }

                    // For simplicity, let's just use the first type in the family
                    FamilySymbol familySymbol = familySymbols.First();

                    if (familySymbol != null && !familySymbol.IsActive)
                    {
                        familySymbol.Activate();
                        doc.Regenerate();
                    }

                    XYZ location = new XYZ(0, 0, 0); // Set your desired location
                    doc.Create.NewFamilyInstance(location, familySymbol, StructuralType.NonStructural);

                    tx.Commit();
                }
            }

            return Result.Succeeded;
        }
    }
}
