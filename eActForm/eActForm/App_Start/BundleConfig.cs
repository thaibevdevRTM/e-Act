using System.Web.Optimization;

namespace eActForm
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
          "~/Scripts/jquery-ui-{version}.js",
          "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/vendors/jquery/dist/jquery.min.js",
                        "~/Content/vendors/datatables.net/js/jquery.dataTables.min.js"));

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
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/pnotify/dist/pnotify.js",
                        "~/Scripts/pnotify/dist/pnotify.buttons.js",
                        "~/Scripts/pnotify/dist/pnotify.nonblock.js",
                        "~/Content/vendors/multiselect/bootstrap-multiselect.js",
                        "~/Scripts/bootstrap-select.min.js"
                        ));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //   "~/Scripts/jquery-ui.min.css",
            //   "~/content/vendors/bootstrap/dist/css/bootstrap.min.css",
            //   "~/content/vendors/font-awesome/css/font-awesome.min.css",
            //   "~/Content/vendors/datatables.net-bs/css/dataTables.bootstrap.min.css",
            //   "~/Content/vendors/iCheck/skins/flat/green.css",
            //   "~/Content/vendors/bootstrap-datepicker/datepicker3.css",
            //   "~/Content/Site.css",
            //   "~/content/vendors/build/css/custom.min.css",
            //   "~/Scripts/pnotify/dist/pnotify.nonblock.css",
            //   "~/Scripts/pnotify/dist/pnotify.buttons.css",
            //   "~/Scripts/pnotify/dist/pnotify.css",
            //   "~/Content/vendors/multiselect/bootstrap-multiselect.css",
            //   "~/Scripts/bootstrap-select.css"
            //   ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
               "~/Scripts/jquery-ui.min.css",
               "~/content/vendors/bootstrap/dist/css/bootstrap.min.css",
               "~/content/vendors/font-awesome/css/font-awesome.min.css",
               "~/Content/vendors/datatables.net-bs/css/dataTables.bootstrap.min.css",
               "~/Content/vendors/iCheck/skins/flat/green.min.css",
               "~/Content/vendors/bootstrap-datepicker/datepicker3.min.css",
               "~/Content/Site.min.css",
               "~/content/vendors/build/css/custom.min.css",
               "~/Scripts/pnotify/dist/pnotify.min.css",
               "~/Content/vendors/multiselect/bootstrap-multiselect.css",
               "~/Scripts/bootstrap-select.min.css"
               ));

            //~/Content/vendors/bootstrap-datepicker/datepicker3.css
        }
    }
}