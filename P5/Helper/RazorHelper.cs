using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace The_Car_Hub.Helper
{
    public class RazorHelper
    {
        public static string RenderRazorViewToString(Controller controller,string viewName,object model = null)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext,viewName,false);

                viewResult.View.RenderAsync(new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                ));

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}