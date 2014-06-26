using System.IO;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores.Base;
using UseAbilities.XML.Serialization;

namespace ASofCP.Cashier.Stores
{
    public class SettingsStore : XmlStore<ModuleSettings>
    {
        #region Singleton implementation

        private SettingsStore()
        {
            //
        }

        private static readonly SettingsStore Instance = new SettingsStore();
        public static SettingsStore GetInstance()
        {
            return Instance;
        }

        #endregion

        private const string FILE_NAME = "ModuleSettings.xml";
        public override string FileName
        {
            get
            {
                return FILE_NAME;
            }
        }

        public override ModuleSettings Load()
        {
            if (!File.Exists(FileName)) base.Save(new ModuleSettings
            {
                IP = "localhost",
                Port = "8080",
                Id = "id123",
                PrinterName = "Zebra",
                PathToTemplate = "ZebraTemplate.xml",
                PathToZpl = "ZebraTemplate.zpl",
                IsCheckPrinterQueue = true,
                PrinterBusyMessage = "Необходимо выключить и включить принтер!"
            });

            return SerializationUtility.Deserialize<ModuleSettings>(FileName);
        }
    }
}
