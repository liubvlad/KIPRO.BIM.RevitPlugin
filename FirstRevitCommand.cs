namespace KIPRO.BIM.RevitPlugin
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using System;

    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class FirstRevitCommand : IExternalCommand
    {
        static AddInId addinId = new AddInId(new Guid("CDE4EA5A-2933-430B-926B-82BEA5E3A069"));

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
            Element element = doc.GetElement(myRef);
            ElementId id = element.Id;

            TaskDialog.Show("Hello world!", id.ToString());

            return Result.Succeeded;
        }
    }
}
