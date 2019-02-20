using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SoftwareDevelopmentTest
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl link = new HtmlGenericControl("link");
            link.Attributes["type"] = "text/css";
            link.Attributes["rel"] = "stylesheet";
            link.Attributes["href"] = "/App_Themes/Main/Main.css";

            head.Controls.Add(link);
        }
    }
}