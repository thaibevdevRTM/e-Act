using System.Web.Optimization;

namespace eActForm
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
          "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/vendors/jquery/dist/jquery.min.js",
                        "~/Content/vendors/datatables.net/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js",
                       "~/Content/vendors/fastclick/lib/fastclick.js",
                       "~/Content/vendors/fastclick/lib/fastclick.js",
                       "~/Content/vendors/bootstrap-datepicker/bootstrap-datepicker.js",
                       "~/Scripts/bootbox.js",
                       "~/Content/vendors/iCheck/icheck.min.js",
                       "~/Content/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js",
                       "~/Scripts/jquery-migrate-1.2.1.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

             bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Scripts/jquery-ui.css",
                "~/content/vendors/bootstrap/dist/css/bootstrap.min.css",
                "~/content/vendors/font-awesome/css/font-awesome.min.css",
                "~/Content/vendors/datatables.net-bs/css/dataTables.bootstrap.min.css",
                "~/Content/vendors/iCheck/skins/flat/green.css",
                "~/Content/vendors/bootstrap-datepicker/datepicker3.css",
                "~/Content/Site.css",
                "~/content/vendors/build/css/custom.min.css"));

            //~/Content/vendors/bootstrap-datepicker/datepicker3.css
        }
    }
}
