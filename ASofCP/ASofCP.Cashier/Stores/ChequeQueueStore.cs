using System;
using System.IO;
using System.Linq;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores.Base;
using log4net;
using UseAbilities.XML.Serialization;

namespace ASofCP.Cashier.Stores
{
    public class ChequeQueueStore : XmlStore<ChequeQueue>, IQueueStore<ChequeQueue>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ChequeQueueStore));

        #region Singleton implementation

        private ChequeQueueStore()
        {
            //
        }

        private static readonly ChequeQueueStore SingleInstance = new ChequeQueueStore();
        public static ChequeQueueStore Instance {get { return SingleInstance; }}

        #endregion

        private const string FOLDER = "Queue";
        private const string FILE_EXT = "xml";

        public override void Save(ChequeQueue storeObject)
        {
            if (!Directory.Exists(FOLDER)) Directory.CreateDirectory(FOLDER);

            if (storeObject.IsNull() || storeObject.GetAllNew().IsNullOrEmpty()) return;
            
            try
            {
                foreach (var newElement in storeObject.GetAllNew())
                {
                    FileName = Path.Combine(FOLDER, "{0}.{1}".F(newElement.GetGuid(), FILE_EXT));
                    base.Save(newElement);
                    newElement.SetAllOld();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                throw;
            }
        }

        public void Delete(ChequeQueue storeObject)
        {
            if (storeObject.IsNull() || storeObject.GetAllDelete().IsNullOrEmpty()) return;

            try
            {
                foreach (var deleteElement in storeObject.GetAllDelete())
                {
                    FileName = Path.Combine(FOLDER, "{0}.{1}".F(deleteElement.GetGuid(), FILE_EXT));
                    if (File.Exists(FileName)) File.Delete(FileName);
                }

                storeObject.RemoveAllDeleted();
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                throw;
            }
        }

        public override ChequeQueue Load()
        {
            throw new NotImplementedException();
        }

        public ChequeQueue Load(string fileName)
        {
            return File.Exists(fileName) ? SerializationUtility.Deserialize<ChequeQueue>(fileName) : null;
        }

        public ChequeQueue LoadAll()
        {
            if (!Directory.Exists(FOLDER) ||
                Directory.EnumerateFiles(FOLDER, "*.{0}".F(FILE_EXT)).IsNullOrEmpty()) return new ChequeQueue();

            var files = Directory.EnumerateFiles(FOLDER, "*.{0}".F(FILE_EXT));

            var result = new ChequeQueue();
            foreach (var queue in files.Select(SerializationUtility.Deserialize<ChequeQueue>))
                result.AddRange(queue.Elements);
            
            return result;
        }
    }

    public interface IQueueStore<T>
    {
        void Save(T storeObject);
        T Load(string fileName);
        T LoadAll();
        void Delete(T storeObject);
    }
}
