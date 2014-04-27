using System;
using System.IO;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores.Base;
using UseAbilities.XML.Serialization;

namespace ASofCP.Cashier.Stores
{
    public class ChequeQueueStore : XmlStore<ModuleSettings>
    {
        #region Singleton implementation

        private ChequeQueueStore()
        {
            //
        }

        private static readonly ChequeQueueStore SingleInstance = new ChequeQueueStore();
        public static ChequeQueueStore Instance {get { return SingleInstance; }}

        #endregion

        public bool IsAddToFileSuffixDate { get; set; }
        public string FileSuffix {get { return IsAddToFileSuffixDate ? DateTime.Now.ToString("yy.mm.dd") : String.Empty; }}
        private const string FILE_EXT = "xml";
        private const string FILE_NAME = "ChequeQueue";
        public override string FileName
        {
            get
            {
                return String.Format("{0}{1}.{2}", FILE_NAME, FileSuffix, FILE_EXT);
            }
        }

        public override ModuleSettings Load()
        {
            return File.Exists(FileName) ? SerializationUtility.Deserialize<ModuleSettings>(FileName) : null;
        }
    }
}
