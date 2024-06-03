using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

[Transaction(TransactionMode.Manual)]
public class FamilyDatabaseCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        System.Diagnostics.Process.Start("https://link_to_family_database");
        return Result.Succeeded;
    }
}

[Transaction(TransactionMode.Manual)]
public class KnowledgeBaseCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        System.Diagnostics.Process.Start("https://link_to_knowledge_base");
        return Result.Succeeded;
    }
}
