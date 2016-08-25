using System.Web;
using System.Web.Optimization;

namespace SecondHandStoreApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


                //TEMPLATE CUSTOM STYLES AND SCRIPTS    
            bundles.Add(new StyleBundle("~/Content/template-css").Include(
                   "~/Content/Template/font-awesome.css",
                   "~/Content/Template/bootstrap.min.css",
                   "~/Content/Template/animate.min.css",
                   "~/Content/Template/owl.carousel.css",
                   "~/Content/Template/owl.theme.css",
                   "~/Content/Template/style.default.css",
                   "~/Content/Template/custom.css"
                     ));

  
            bundles.Add(new ScriptBundle("~/bundles/template-scripts").Include(
                   "~/Scripts/Template/jquery-1.11.0.min.js",
                   "~/Scripts/Template/bootstrap.min.js",
                   "~/Scripts/Template/waypoints.min.js",
                   "~/Scripts/Template/modernizr.js",
                   "~/Scripts/Template/bootstrap-hover-dropdown.js",
                   "~/Scripts/Template/owl.carousel.min.js",
                   "~/Scripts/Template/front.js"));


        }
    }
}
