using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using KRGPMagic.Plugins.SheetChangingType.Data;
using System.IO;
using System.Linq;
using Microsoft.Win32;

public class ParameterManager
{
    private static ParameterManager _instance;

    private readonly Document _doc;
    private DefinitionFile _sharedParamFile; // nullable до момента необходимости

    private ParameterManager(Document doc)
    {
        _doc = doc;
    }

    public static ParameterManager GetInstance(Document doc)
    {
        if (_instance == null)
        {
            _instance = new ParameterManager(doc);                
        }
        return _instance;
    }

    public bool EnsureParameterExistsAndCorrect()
    {
        var parameterElement = new FilteredElementCollector(_doc)
            .OfClass(typeof(SharedParameterElement))
            .Cast<SharedParameterElement>()
            .FirstOrDefault(p => p.GetDefinition().Name == Constants.SheetStatusParameterName);

        if (parameterElement == null)
        {
            TaskDialog.Show(Constants.RibbonPanelName, "Параметр \"KRGPMagic\" не найден. " +
                "Необходимо добавить параметр из текущего ФОП и добавить для него категорию \"Листы\"" +
                "\nОбратитесь в ОИМ");
            return false;
        }

        var definition = parameterElement.GetDefinition();

        if (!HasAnyCategoryBinding(definition))
        {
            TaskDialog.Show(Constants.RibbonPanelName, "Параметр \"KRGPMagic\" не найден. " +
                "Необходимо добавить параметр из текущего ФОП и добавить для него категорию \"Листы\"" +
                "\nОбратитесь в ОИМ");
            return false;
        }

        if (!IsCategoryBindingCorrect(definition))
        {
            return EnsureCategoryBinding(definition);
        }

        return true;
    }




    private bool EnsureCategoryBinding(Definition definition)
    {
        var dialogResult = TaskDialog.Show(Constants.RibbonPanelName, UIMessages.GetCategoryNotAssignedDialogMessage(), TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No);
        if (dialogResult != TaskDialogResult.Yes) return false;

        using (var transaction = new Transaction(_doc, "Назначение категории параметру"))
        {
            transaction.Start();
            var newBinding = _doc.Application.Create.NewInstanceBinding(CreateCategorySet());
            _doc.ParameterBindings.ReInsert(definition, newBinding, BuiltInParameterGroup.PG_IDENTITY_DATA);
            transaction.Commit();
        }

        TaskDialog.Show(Constants.RibbonPanelName, UIMessages.GetCategoryAssignedSuccessMessage());
        return true;
    }
    private bool HasAnyCategoryBinding(Definition definition)
    {
        var binding = _doc.ParameterBindings.get_Item(definition) as InstanceBinding;
        return binding != null &&
               binding.Categories != null &&
               !binding.Categories.IsEmpty;
    }


    private bool IsCategoryBindingCorrect(Definition definition)
    {
        var binding = _doc.ParameterBindings.get_Item(definition) as InstanceBinding;
        return binding != null &&
               binding.Categories.Contains(_doc.Settings.Categories.get_Item(BuiltInCategory.OST_Sheets));
    }

    private CategorySet CreateCategorySet()
    {
        var categorySet = _doc.Application.Create.NewCategorySet();
        categorySet.Insert(_doc.Settings.Categories.get_Item(BuiltInCategory.OST_Sheets));
        return categorySet;
    }
}
