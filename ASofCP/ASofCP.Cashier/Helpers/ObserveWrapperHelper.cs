using System;
using System.Collections.Generic;
using UseAbilities.IoC.Helpers;
using UseAbilities.MVVM.Base;

namespace ASofCP.Cashier.Helpers
{
    public class ObserveWrapperHelper
    {
        #region Singleton implementation

        private ObserveWrapperHelper()
        {
            WrappedViewModels = new Dictionary<Type, Type>();
        }

        private static readonly ObserveWrapperHelper Instance = new ObserveWrapperHelper();
        public static ObserveWrapperHelper GetInstance()
        {
            return Instance;
        }

        #endregion

        public Dictionary<Type, Type> WrappedViewModels { get; set; } 

        public void WrapType<T>()
        {
            var tType = typeof (T);
            if (WrappedViewModels.ContainsKey(tType)) WrappedViewModels[tType] = Wrap<T>();
            else WrappedViewModels.Add(tType, Wrap<T>());
        }

        public Type GetWrappedType<T>(bool isNewAdd = false)
        {
            var tType = typeof(T);
            if (WrappedViewModels.ContainsKey(tType)) return WrappedViewModels[tType];
            if (!isNewAdd) return null;
            WrapType<T>();
            return WrappedViewModels[tType];
        }

        private static Type Wrap<T>()
        {
            return ObserveWrapper.Wrap(typeof (T));
        }

        public T Resolve<T>()
        {
            return (T)StaticHelper.IoCcontainer.Resolve(GetWrappedType<T>(true));
        }
    }
}
