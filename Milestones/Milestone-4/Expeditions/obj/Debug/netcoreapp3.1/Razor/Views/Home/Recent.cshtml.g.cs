#pragma checksum "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c7134cacb32e2a222a23a1d654beab8e192a7b48"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Recent), @"mvc.1.0.view", @"/Views/Home/Recent.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\_ViewImports.cshtml"
using Expeditions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\_ViewImports.cshtml"
using Expeditions.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c7134cacb32e2a222a23a1d654beab8e192a7b48", @"/Views/Home/Recent.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68c8790d80df532bcc590db3da29847bb186fa7c", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Recent : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Expeditions.Models.Expedition>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
  
    ViewData["Title"] = "Recent";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Recent Expenditions</h1>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 12 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayNameFor(model => model.Season));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 15 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayNameFor(model => model.Year));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 18 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayNameFor(model => model.StartDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n\r\n            <th>\r\n                ");
#nullable restore
#line 22 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayNameFor(model => model.Peak));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 25 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayNameFor(model => model.TerminationReason));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 30 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr class=\"btn-outline-primary\"");
            BeginWriteAttribute("onclick", " onclick=\"", 792, "\"", 874, 5);
            WriteAttributeValue("", 802, "location.href", 802, 13, true);
            WriteAttributeValue(" ", 815, "=", 816, 2, true);
            WriteAttributeValue(" ", 817, "\'", 818, 2, true);
#nullable restore
#line 31 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
WriteAttributeValue("", 819, Url.Action("Details", "Home", new { id = item.Id }), 819, 54, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 873, "\'", 873, 1, true);
            EndWriteAttribute();
            WriteLiteral("> \r\n            <td>\r\n                ");
#nullable restore
#line 33 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayFor(modelItem => item.Season));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 36 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayFor(modelItem => item.Year));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 39 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayFor(modelItem => item.StartDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 42 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayFor(modelItem => item.Peak.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 45 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
           Write(Html.DisplayFor(modelItem => item.TerminationReason));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 48 "C:\Users\cj-21\OneDrive\سطح المكتب\HAL\HAL-Development\Milestones\Milestone-3\Expeditions\Views\Home\Recent.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Expeditions.Models.Expedition>> Html { get; private set; }
    }
}
#pragma warning restore 1591
