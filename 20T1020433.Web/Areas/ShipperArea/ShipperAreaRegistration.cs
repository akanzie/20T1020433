using System.Web.Mvc;

namespace _20T1020433.Web.Areas.ShipperArea
{
    public class ShipperAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ShipperArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Shipper_default",                                
                "ShipperArea/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "_20T1020433.Web.Areas.ShipperArea.Controllers" }
            );
        }
    }
}