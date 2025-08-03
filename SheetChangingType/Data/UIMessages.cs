using KRGPMagic.Plugins.SheetChangingType.Data;

namespace KRGPMagic.Plugins.SheetChangingType.Data
{
    // Хранит тексты сообщений для пользователя.
    public static class UIMessages
    {
        public const string PluginErrorTitle = "Ошибка плагина";
        public const string NoSheetsSelectedError = "Не выбрано ни одного листа. Выберите листы в проекте и повторите операцию.";
        public const string SecondChangeConstraintError = "Операция не может быть выполнена. Если первое изменение 'Сброшено' (0) или 'Аннулировано' (4), второе изменение не может быть установлено.";
        
        public static string GetParameterNotFoundDialogMessage()
        {
            return $"В проекте не найден необходимый параметр '{Constants.SheetStatusParameterName}'.\n" +
                   "Добавить ли его из текущего ФОП перед выполнением операции?\n\n" +
                   $"Путь к ФОП: {Constants.FopFilePath}";
        }

        public static string GetCategoryNotAssignedDialogMessage()
        {
            return $"Параметру '{Constants.SheetStatusParameterName}' не назначена категория 'Листы'.\n" +
                   "Добавить эту категорию параметру перед выполнением операции?";
        }

        public static string GetParameterAddedSuccessMessage()
        {
            return $"Параметр '{Constants.SheetStatusParameterName}' добавлен из текущего ФОПа и ему назначена категория 'Листы', параметр для экземпляров в группе '{Constants.ParameterGroupName}'.\n\n" +
                   "Также выполнена запрашиваемая операция.";
        }

        public static string GetCategoryAssignedSuccessMessage()
        {
            return $"Параметру '{Constants.SheetStatusParameterName}' назначена категория 'Листы', параметр для экземпляров в группе '{Constants.ParameterGroupName}'.\n\n" +
                   "Также выполнена запрашиваемая операция.";
        }

        public static string GetTransactionName(int sheetCount, string sheetNumber)
        {
            if (sheetCount == 1)
            {
                return $"Тип ИЗМа для листа №{sheetNumber}";
            }
            return $"Тип ИЗМа для {sheetCount} листов";
        }
    }
}
