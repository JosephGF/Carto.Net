using System;
using NetCarto.Core.Extensions;

namespace NetCarto.Map.Common
{

    public abstract class MapJsHelper : IMapHelper
    {
        protected const string JS_NAMESPACE = "window.net.cartodb";
        public abstract object Execute(string function, params object[] args);
        public abstract object Execute(string function);
        public abstract object ExecuteAsync(string function, params object[] args);
        public abstract object ExecuteAsync(string function);

        public object SetZoom(int number = -1)
        {
            return Execute(JS_NAMESPACE + ".setZoom({0})", number);
        }

        public object GetZoom()
        {
            return Execute(JS_NAMESPACE + ".getZoom()");
        }

        public object SumZoom(int number = 1 )
        {
            return Execute(JS_NAMESPACE + ".sumZoom({0})", number);
        }

        public void Create(MapOptions options)
        {
            ExecuteAsync(JS_NAMESPACE + ".create({0})", options.ToJson());
        }
    }
}
