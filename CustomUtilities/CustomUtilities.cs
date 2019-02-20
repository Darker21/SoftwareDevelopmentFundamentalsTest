using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml;

namespace CustomUtilities
{
    public class CustomUtilities
    {

        public static Image GetQuestionImage(string sReplacement)
        {
            Image i = new Image();
            string sUnitName = sReplacement.Split('_')[0];
            string sImageNumber = sReplacement.Split('_')[1];
            i.ImageUrl = "~/Images/" + sUnitName + "/" + sImageNumber + ".png";
            return i;
        }

    }
}
