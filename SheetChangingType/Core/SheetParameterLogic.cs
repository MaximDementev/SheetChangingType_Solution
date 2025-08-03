using Autodesk.Revit.DB;
using KRGPMagic.Plugins.SheetChangingType.Commands;
using KRGPMagic.Plugins.SheetChangingType.Data;
using ChangeType = KRGPMagic.Plugins.SheetChangingType.Commands.ChangeType;

namespace KRGPMagic.Plugins.SheetChangingType.Core
{
    // Содержит основную логику обработки значения параметра статуса листа.
    public class SheetParameterLogic
    {
        // Обновляет параметр статуса для указанного листа.
        public bool UpdateSheetStatus(ViewSheet sheet, ChangeType changeType, int newValue)
        {
            Parameter statusParam = sheet.LookupParameter(Constants.SheetStatusParameterName);
            if (statusParam == null || statusParam.IsReadOnly) return false;

            int paramValue = statusParam.AsInteger();
            string currentValue = paramValue.ToString("D2");

            // Парсим текущее значение
            int firstDigit = int.Parse(currentValue.Substring(0, 1));
            int secondDigit = int.Parse(currentValue.Substring(1, 1));

            // Применяем новое значение
            if (changeType == ChangeType.First)
            {
                firstDigit = newValue;
                // Если первое изменение 0 или 4, второе автоматически сбрасывается
                if (firstDigit == 0 || firstDigit == 4)
                {
                    secondDigit = 0;
                }
            }
            else // Second
            {
                // Проверка на ограничение
                if ((firstDigit == 0 || firstDigit == 4) && newValue != 0)
                {
                    return false; // Возвращаем false, чтобы показать ошибку
                }
                secondDigit = newValue;
            }

            int newStatus = int.Parse($"{firstDigit}{secondDigit}");
            statusParam.Set(newStatus);

            return true;
        }
    }
}
