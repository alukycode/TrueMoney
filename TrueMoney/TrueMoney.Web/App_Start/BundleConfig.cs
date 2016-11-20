using System.Web.Optimization;

namespace TrueMoney.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                        "~/Scripts/lib/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/lib/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/lib/bootstrap.js",
                      "~/Scripts/lib/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/DealDetails").Include(
                      "~/Scripts/DealDetails.js"));

            bundles.Add(new StyleBundle("~/Styles/css").Include(
                      "~/Styles/lib/bootstrap.lumen.css",
                      "~/Styles/site.css"));
        }
    }
}
