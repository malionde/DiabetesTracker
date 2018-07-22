using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MVCExample.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/Js").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery-{version}.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.slimscroll.min.js",
                      "~/Scripts/fastclick.js",
                      "~/Scripts/app.min.js",
                      "~/Scripts/demo.js",
                      "~/Scripts/Chart.js",
                      "~/Scripts/Common.js"
                      ));

            bundles.Add(new StyleBundle("~/Css").Include(
                      "~/Content/bootstrap/css/bootstrap.min.css",
                      "~/Content/bootstrap/css/font-awesome.css",
                      "~/Content/bootstrap/css/font-awesome.min.css",
                      "~/Content/dist/css/DiabetTracker.min.css",
                      "~/Content/dist/css/skins/_all-skins.min.css",
                      "~/Content/dist/css/alt/bootstrap-social.min.css",
                      "~/Content/dist/css/alt/bootstrap-social.css",
                      "~/Content/dist/css/alt/fullcalendar.min.css",
                      "~/Content/dist/css/alt/fulcalandar.css",
                      "~/Content/dist/css/alt/select2.css",
                      "~/Content/dist/css/alt/select2.min.css",
                      "~/Content/dist/css/alt/without-plugins.min.css",
                      "~/Content/dist/css/alt/without-plugins.css"


                      ));
            bundles.Add(new StyleBundle("~/Fonts").Include(
            "~/Content/bootstrap/fonts/glyphicons-halflings-regular.ttf"
            ).Include("~/Content/fontawesome5/css/fontawesome.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/build/less").Include("~/Content/build/*.less"));




        }
    }
}