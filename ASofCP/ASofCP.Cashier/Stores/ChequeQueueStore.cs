using System;
using System.IO;
using ASofCP.Cashier.Helpers;
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

        public void GenerateName()
        {
            _fileName = Guid.NewGuid().ToString();
        }

        private const string FOLDER = "Queue";
        private const string FILE_EXT = "xml";
        private string _fileName = null;
        public override string FileName
        {
            get
            {
                return Path.Combine(FOLDER, "{0}.{1}".F(_fileName.IsNullOrEmpty() ? Guid.NewGuid().ToString() : _fileName, FILE_EXT));
            }
        }

        public override ModuleSettings Load()
        {
            return File.Exists(FileName) ? SerializationUtility.Deserialize<ModuleSettings>(FileName) : null;
        }
    }
}
