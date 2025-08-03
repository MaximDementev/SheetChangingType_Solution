using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using KRGPMagic.Core.Interfaces;
using KRGPMagic.Core.Models;

namespace KRGPMagic.Plugins.SheetChangingType.Commands.FirstChange
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class FirstChangeCommand2 : IExternalCommand, IPlugin
    {
        #region IPlugin Implementation

        public PluginInfo Info { get; set; }
        public bool IsEnabled { get; set; }

        public bool Initialize()
        {
            // ������ �������������, ���� ���������
            return true;
        }

        public void Shutdown()
        {
            // ������ ���������� ������, ���� ���������
        }

        #endregion

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            BaseChangeCommand command = BaseChangeCommand.Instance;

            // ������ ����� �������� ������ ������
            return command.Execute(commandData, ChangeType.First, 2);
        }

    }
}
