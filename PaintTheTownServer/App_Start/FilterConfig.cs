using System.Web;
using System.Web.Mvc;
using PaintTheTownServer.Filter;

namespace PaintTheTownServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());

        }
    }
}
