using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;
using KRGPMagic.Plugins.SheetChangingType.Core;
using KRGPMagic.Plugins.SheetChangingType.Data;

namespace KRGPMagic.Plugins.SheetChangingType.Commands
{
    // Перечисление для типа изменения (первая или вторая цифра).
    public enum ChangeType { First, Second }

    public class BaseChangeCommand
    {
        private static BaseChangeCommand _instance;

        // Приватный конструктор
        private BaseChangeCommand() { }

        // Публичное свойство для получения инстанса
        public static BaseChangeCommand Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BaseChangeCommand();                        
                }
                return _instance;
            }
        }

        // Основной метод выполнения команды.
        public Result Execute(ExternalCommandData commandData, ChangeType TypeOfChange, int NewValue)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uiDoc = uiapp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // 1. Проверка и создание параметра
            var parameterManager = ParameterManager.GetInstance(doc);
            if (!parameterManager.EnsureParameterExistsAndCorrect())
            {
                return Result.Cancelled; // Пользователь отказался создавать параметр
            }

            // 2. Получение выбранных листов
            var selectedSheets = SelectionHelper.GetSelectedSheets(uiDoc);
            if (!selectedSheets.Any())
            {
                return Result.Cancelled; // Листы не выбраны
            }

            // 3. Выполнение основной логики в транзакции
            var logic = new SheetParameterLogic();
            string transactionName = UIMessages.GetTransactionName(selectedSheets.Count, selectedSheets.FirstOrDefault()?.SheetNumber);

            using (var transaction = new Transaction(doc, transactionName))
            {
                transaction.Start();

                foreach (var sheet in selectedSheets)
                {
                    bool success = logic.UpdateSheetStatus(sheet, TypeOfChange, NewValue);
                    if (!success)
                    {
                        // Если логика вернула false, это означает нарушение правила (второе изм. при первом = 0 или 4)
                        TaskDialog.Show(UIMessages.PluginErrorTitle, UIMessages.SecondChangeConstraintError);
                        transaction.RollBack();
                        return Result.Failed;
                    }
                }

                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
