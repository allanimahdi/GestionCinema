using System.Web;
using System.Web.Optimization;

namespace Web
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
                 
                    "~/Content/font-awesome.min.css"));

            //css && js AdminTemplate
            bundles.Add(new StyleBundle("~/Admin/css").Include(
                    "~/Content/Admin/site.css",
                    "~/Content/Admin/metisMenu.min.css",
                    "~/Content/Admin/timeline.css",
                    "~/Content/Admin/sb-admin-2.css",
                    "~/Content/Admin/morris.css"));

            bundles.Add(new ScriptBundle("~/bundles/Admin").Include(
                      "~/Scripts/Admin/metisMenu.min.js",
                      "~/Scripts/Admin/raphael-min.js",
                      "~/Scripts/Admin/morris.min.js",
                      "~/Scripts/Admin/morris-data.js",
                      "~/Scripts/Admin/sb-admin-2.js"));

            //data-table
            bundles.Add(new ScriptBundle("~/bundles/DataTable").Include(
                    "~/Scripts/Admin/jquery.dataTables.min.js",
                    "~/Scripts/Admin/dataTables.bootstrap.min.js"

                      ));
            bundles.Add(new StyleBundle("~/DataTable/css").Include(
                    "~/Content/dataTables.bootstrap.css"));

            //css && js Home
            bundles.Add(new ScriptBundle("~/bundles/Home").Include(
                  "~/Scripts/Home/jquery.mousewheel.js",
                  "~/Scripts/Home/jquery.contentcarousel.js",
                  "~/Scripts/Home/jquery.easing.1.3.js",
                  "~/Scripts/Home/jquery.magnific-popup.js"
                         ));
            bundles.Add(new StyleBundle("~/Home/css").Include(
                  "~/Content/site.css",
                  "~/Content/magnific-popup.css"));
        }
    }
}
