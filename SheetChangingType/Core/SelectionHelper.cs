using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using KRGPMagic.Plugins.SheetChangingType.Data;

namespace KRGPMagic.Plugins.SheetChangingType.Core
{
    // Вспомогательный класс для работы с выбором элементов в Revit.
    public static class SelectionHelper
    {
        // Получает выбранные листы или активный лист, если выбор пустой.
        public static List<ViewSheet> GetSelectedSheets(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;
            Selection selection = uiDoc.Selection;

            // Пытаемся получить выбранные листы
            ICollection<ElementId> selectedIds = selection.GetElementIds();
            var sheets = selectedIds
                .Select(id => doc.GetElement(id))
                .OfType<ViewSheet>()
                .ToList();

            if (sheets.Any())
            {
                return sheets;
            }

            // Если выбор пустой — проверяем активный вид
            if (uiDoc.ActiveView is ViewSheet activeSheet)
            {
                return new List<ViewSheet> { activeSheet };
            }

            // Ничего не выбрано и активный вид — не лист
            return new List<ViewSheet>();
        }
    }

    // Фильтр для выбора только элементов типа "Лист".
    public class SheetSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is ViewSheet;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
