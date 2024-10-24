﻿using System.Web;
using System.Web.Optimization;

namespace Church
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery-{version}.js", 
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/sweetalert/sweetalert2.min.js",
                        "~/Scripts/jquery.mask.js",
                        "~/Scripts/ChurchScript.js",
                        "~/Scripts/jquery.fancybox.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js",
                      
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Scripts/sweetalert/sweetalert2.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                     "~/Content/themes/base/core.css",
                     "~/Content/themes/base/resizable.css",
                     "~/Content/themes/base/selectable.css",
                     "~/Content/themes/base/accordion.css",
                     "~/Content/themes/base/autocomplete.css",
                     "~/Content/themes/base/button.css",
                     "~/Content/themes/base/dialog.css",
                     "~/Content/themes/base/slider.css",
                     "~/Content/themes/base/tabs.css",
                     "~/Content/themes/base/datepicker.css",
                     "~/Content/themes/base/progressbar.css",
                     "~/Content/themes/base/theme.css",
                     "~/Content/themes/base/tooltip.css",
                     "~/Content/pretty-checkbox.min.css",
                     "~/Content/material-icons/css/materialdesignicons.css",
                     "~/Content/jquery.fancybox.css",
                     "~/Content/Loader.css"
                     ));


        }
    }
}
