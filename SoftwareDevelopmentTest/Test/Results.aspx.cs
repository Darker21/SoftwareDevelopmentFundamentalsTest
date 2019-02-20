using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace SoftwareDevelopmentTest.Test
{
    public partial class Results : System.Web.UI.Page
    {
        // Keeps track of users score
        int iUsersScore;

        protected void Page_Load(object sender, EventArgs e)
        {

            iUsersScore = new int();

            // Load the XML document that contains the questions and answers
            XmlDocument questionDocument = new XmlDocument();
            questionDocument.Load(Server.MapPath("~/Questions.xml"));
            XmlNode unitQuestions = questionDocument.SelectSingleNode("/questions");

            // Grab all users answers for each unit
            string[] asUsersUnit1Answers = Session["Unit1Answers"] as string[];
            string[] asUsersUnit2Answers = Session["Unit2Answers"] as string[];
            string[] asUsersUnit3Answers = Session["Unit3Answers"] as string[];

            // Grab all questions that were asked for each unit
            int[] aiUnit1Questions = Session["Unit1Questions"] as int[];
            int[] aiUnit2Questions = Session["Unit2Questions"] as int[];
            int[] aiUnit3Questions = Session["Unit3Questions"] as int[];

            // If null - force user to retake the test
            if (asUsersUnit1Answers == null ||
                asUsersUnit2Answers == null ||
                asUsersUnit3Answers == null ||
                aiUnit1Questions == null ||
                aiUnit2Questions == null ||
                aiUnit3Questions == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            // Grab specific unit's questions
            XmlNode xUnit1Questions = unitQuestions.SelectSingleNode("Unit[@Name='Core Programming']");

            // Using the for loop as we need the indexes for other functionality in the loop
            for (int i = 0; i < aiUnit1Questions.Length; i++)
            {
                // Get the Questions xml
                XmlNode xQuestion = xUnit1Questions.SelectSingleNode("Question[@Id='" + aiUnit1Questions[i] + "']");

                // Get the correct answer and explanation
                string sCorrectAnswer = xQuestion.SelectSingleNode("Correct/Option").InnerText;
                string sExplanation = xQuestion.SelectSingleNode("Correct/Explanation").InnerText;

                bool bUserCorrect = false;
                // boolean means we don't have to re-evalute the boolean expresion in other if statements
                if (asUsersUnit1Answers[i].ToLower() == sCorrectAnswer.ToLower())
                {
                    bUserCorrect = true;
                    iUsersScore++;
                }

                // Create the container for the information
                Panel panelQuestion = new Panel();

                Label l = new Label();

                string sQuestion = xQuestion.SelectSingleNode("Text").InnerText;
                // Create the Question again
                if (sQuestion.Contains("|"))
                {
                    Image iImage;
                    string sData = sQuestion.Substring(sQuestion.IndexOf("|"));
                    string[] asData = sData.Split(' ');
                    string sDataType = asData[0];
                    string sDataValue = asData[1].Substring(0, asData[1].IndexOf("|"));

                    switch (sDataType.TrimStart('|'))
                    {
                        case "img":
                            iImage = CustomUtilities.CustomUtilities.GetQuestionImage(sDataValue);
                            iImage.ToolTip = xQuestion.Attributes["Id"].ToString();
                            l = new Label();
                            l.CssClass = "SubHeaderText";
                            l.Text = (i + 1).ToString() + ". " + sQuestion.Substring(0, sQuestion.IndexOf("|"));
                            l.Text += "<br />";
                            // Make text green if correct or red if incorrect
                            if (bUserCorrect)
                            {
                                l.Style["color"] = "#72DA25 !important";
                            }
                            else
                            {
                                l.Style["color"] = "#C4140B !important";
                            }
                            panelQuestion.Controls.Add(l);
                            panelQuestion.Controls.Add(iImage);
                            l = new Label();
                            l.Text = "<br />" + sQuestion.Substring(sQuestion.LastIndexOf("|") + 1) + "<br />";
                            l.CssClass = "SubHeaderText";
                            // Make text green if correct or red if incorrect
                            if (bUserCorrect)
                            {
                                l.Style["color"] = "#72DA25 !important";
                            }
                            else
                            {
                                l.Style["color"] = "#C4140B !important";
                            }
                            panelQuestion.Controls.Add(l);
                            break;
                    }

                }
                else
                {
                    l = new Label();
                    l.CssClass = "SubHeaderText";
                    l.Text = string.Format("{0}. {1}<br />", (i + 1), sQuestion);
                    // Make text green if correct or red if incorrect
                    if (bUserCorrect)
                    {
                        l.Style["color"] = "#72DA25 !important";
                    }
                    else
                    {
                        l.Style["color"] = "#C4140B !important";
                    }
                    panelQuestion.Controls.Add(l);
                }
                RadioButton option;
                foreach (XmlElement ele in xQuestion.SelectSingleNode("Answers").ChildNodes)
                {
                    option = new RadioButton();
                    option.GroupName = "Unit1_" + xQuestion.Attributes["Id"].Value;
                    option.Text = ele.InnerText;
                    // Stops the user from changing the value
                    option.Enabled = false;
                    if (asUsersUnit1Answers != null)
                    {
                        if (ele.Name == asUsersUnit1Answers[i] && bUserCorrect)
                        {
                            option.Style["color"] = "#72DA25";
                            option.Checked = true;
                        }
                        else if (ele.Name == asUsersUnit1Answers[i] && !bUserCorrect)
                        {
                            option.Style["color"] = "#C4140B";
                            option.Checked = true;
                        }
                        else if (ele.Name.ToLower() == sCorrectAnswer.ToLower() && !bUserCorrect)
                        {
                            // Makes text blue if user has not selected this answer
                            option.Style["color"] = "#0000CC";
                            option.Checked = true;
                        }
                        else
                        {
                            option.Checked = false;
                        }
                    }
                    else
                    {
                        option.Checked = false;
                    }

                    option.AutoPostBack = false;
                    option.Attributes.Add("data-AnswerLetter", ele.Name);
                    panelQuestion.Controls.Add(option);
                    l = new Label();
                    l.Text = "<br />";
                    panelQuestion.Controls.Add(l);
                }

                Button buttonShowExplanation = new Button();
                buttonShowExplanation.Text = "Show Explanation";
                buttonShowExplanation.UseSubmitBehavior = false;
                buttonShowExplanation.CssClass = "ExplanationButton";
                // Creates a JavaScript function for the button click so the page won't refresh when wanting to show and hide an explanation
                // - It will toggle whether it's visible and hidden and the button text
                buttonShowExplanation.Attributes.Add("onclick", "if (document.getElementById('ContentMain_panelShowExplanation_Unit1_" + xQuestion.Attributes["Id"].Value + "').style.display === 'none'){document.getElementById('ContentMain_panelShowExplanation_Unit1_" + xQuestion.Attributes["Id"].Value + "').style.display = ''; this.innerText = 'Hide Explanation'; this.value = 'Hide Explanation';}else{document.getElementById('ContentMain_panelShowExplanation_Unit1_" + xQuestion.Attributes["Id"].Value + "').style.display = 'none'; this.innerText = 'Show Explanation';this.value = 'Show Explanation';};return false;");
                panelQuestion.Controls.Add(buttonShowExplanation);

                Panel p = new Panel();
                p.ID = "panelShowExplanation_Unit1_" + xQuestion.Attributes["Id"].Value;
                p.Controls.Add(new HtmlGenericControl("hr"));
                l = new Label();
                l.Text = sExplanation;
                p.Controls.Add(l);
                // Sets it to hidden by default
                p.Style["display"] = "none";

                panelQuestion.Controls.Add(p);

                l = new Label();
                l.Text = "<br /><br />";
                panelQuestion.Controls.Add(l);
                panelUnit1Questions.Controls.Add(panelQuestion);
            }

            // Grab specific unit's questions
            XmlNode xUnit2Questions = unitQuestions.SelectSingleNode("Unit[@Name='Object Orientated Programming']");

            for (int i = 0; i < aiUnit2Questions.Length; i++)
            {
                // Get the Questions xml
                XmlNode xQuestion = xUnit2Questions.SelectSingleNode("Question[@Id='" + aiUnit2Questions[i] + "']");

                // Get the correct answer and explanation
                string sCorrectAnswer = xQuestion.SelectSingleNode("Correct/Option").InnerText;
                string sExplanation = xQuestion.SelectSingleNode("Correct/Explanation").InnerText;

                bool bUserCorrect = false;
                // boolean means we don't have to re-evalute the boolean expresion in other if statements
                if (asUsersUnit2Answers[i].ToLower() == sCorrectAnswer.ToLower())
                {
                    bUserCorrect = true;
                    iUsersScore++;
                }

                // Create the container for the information
                Panel panelQuestion = new Panel();

                Label l = new Label();
                string sQuestion = xQuestion.SelectSingleNode("Text").InnerText;
                // Create the Question again
                if (sQuestion.Contains("|"))
                {
                    Image iImage;
                    string sData = sQuestion.Substring(sQuestion.IndexOf("|"));
                    string[] asData = sData.Split(' ');
                    string sDataType = asData[0];
                    string sDataValue = asData[1].Substring(0, asData[1].IndexOf("|"));

                    switch (sDataType.TrimStart('|'))
                    {
                        case "img":
                            iImage = CustomUtilities.CustomUtilities.GetQuestionImage(sDataValue);
                            iImage.ToolTip = xQuestion.Attributes["Id"].ToString();
                            l = new Label();
                            l.CssClass = "SubHeaderText";
                            l.Text = (i + 6).ToString() + ". " + sQuestion.Substring(0, sQuestion.IndexOf("|"));
                            l.Text += "<br />";
                            // Make text green if correct or red if incorrect
                            if (bUserCorrect)
                            {
                                l.Style["color"] = "#72DA25 !important";
                            }
                            else
                            {
                                l.Style["color"] = "#C4140B !important";
                            }
                            panelQuestion.Controls.Add(l);
                            panelQuestion.Controls.Add(iImage);
                            l = new Label();
                            l.Text = "<br />" + sQuestion.Substring(sQuestion.LastIndexOf("|") + 1) + "<br />";
                            l.CssClass = "SubHeaderText";
                            // Make text green if correct or red if incorrect
                            if (bUserCorrect)
                            {
                                l.Style["color"] = "#72DA25 !important";
                            }
                            else
                            {
                                l.Style["color"] = "#C4140B !important";
                            }
                            panelQuestion.Controls.Add(l);
                            break;
                    }

                }
                else
                {
                    l = new Label();
                    l.CssClass = "SubHeaderText";
                    l.Text = String.Format("{0}. {1}<br />", (i + 6), sQuestion);
                    // Make text green if correct or red if incorrect
                    if (bUserCorrect)
                    {
                        l.Style["color"] = "#72DA25 !important";
                    }
                    else
                    {
                        l.Style["color"] = "#C4140B !important";
                    }
                    panelQuestion.Controls.Add(l);
                }
                RadioButton option;
                foreach (XmlElement ele in xQuestion.SelectSingleNode("Answers").ChildNodes)
                {
                    option = new RadioButton();
                    option.GroupName = "Unit2_" + xQuestion.Attributes["Id"].Value;
                    option.Text = ele.InnerText;
                    // Stops the user from changing the value
                    option.Enabled = false;
                    if (asUsersUnit2Answers != null)
                    {
                        if (ele.Name == asUsersUnit2Answers[i] && bUserCorrect)
                        {
                            option.Style["color"] = "#72DA25";
                            option.Checked = true;
                        }
                        else if (ele.Name == asUsersUnit2Answers[i] && !bUserCorrect)
                        {
                            option.Style["color"] = "#C4140B";
                            option.Checked = true;
                        }
                        else if (ele.Name.ToLower() == sCorrectAnswer.ToLower() && !bUserCorrect)
                        {
                            // Makes text blue if user has not selected this answer
                            option.Style["color"] = "#0000CC";
                            option.Checked = true;
                        }
                        else
                        {
                            option.Checked = false;
                        }
                    }
                    else
                    {
                        option.Checked = false;
                    }

                    option.AutoPostBack = false;
                    option.Attributes.Add("data-AnswerLetter", ele.Name);
                    panelQuestion.Controls.Add(option);
                    l = new Label();
                    l.Text = "<br />";
                    panelQuestion.Controls.Add(l);
                }

                Button buttonShowExplanation = new Button();
                buttonShowExplanation.Text = "Show Explanation";
                buttonShowExplanation.UseSubmitBehavior = false;
                buttonShowExplanation.CssClass = "ExplanationButton";
                // Creates a JavaScript function for the button click so the page won't refresh when wanting to show and hide an explanation
                // - It will toggle whether it's visible and hidden and the button text
                buttonShowExplanation.Attributes.Add("onclick", "if (document.getElementById('ContentMain_panelShowExplanation_Unit2_" + xQuestion.Attributes["Id"].Value + "').style.display === 'none'){document.getElementById('ContentMain_panelShowExplanation_Unit2_" + xQuestion.Attributes["Id"].Value + "').style.display = ''; this.innerText = 'Hide Explanation'; this.value = 'Hide Explanation';}else{document.getElementById('ContentMain_panelShowExplanation_Unit2_" + xQuestion.Attributes["Id"].Value + "').style.display = 'none'; this.innerText = 'Show Explanation';this.value = 'Show Explanation';};return false;");
                panelQuestion.Controls.Add(buttonShowExplanation);

                Panel p = new Panel();
                p.ID = "panelShowExplanation_Unit2_" + xQuestion.Attributes["Id"].Value;
                p.Controls.Add(new HtmlGenericControl("hr"));
                l = new Label();
                l.Text = sExplanation;
                p.Controls.Add(l);
                p.Style["display"] = "none";

                panelQuestion.Controls.Add(p);

                l = new Label();
                l.Text = "<br /><br />";
                panelQuestion.Controls.Add(l);
                panelUnit2Questions.Controls.Add(panelQuestion);
            }

            // Grab specific unit's questions
            XmlNode xUnit3Questions = unitQuestions.SelectSingleNode("Unit[@Name='General Software Development']");

            for (int i = 0; i < aiUnit3Questions.Length; i++)
            {
                // Get the Questions xml
                XmlNode xQuestion = xUnit3Questions.SelectSingleNode("Question[@Id='" + aiUnit3Questions[i] + "']");

                // Get the correct answer and explanation
                string sCorrectAnswer = xQuestion.SelectSingleNode("Correct/Option").InnerText;
                string sExplanation = xQuestion.SelectSingleNode("Correct/Explanation").InnerText;

                bool bUserCorrect = false;
                // boolean means we don't have to re-evalute the boolean expresion in other if statements
                if (asUsersUnit3Answers[i].ToLower() == sCorrectAnswer.ToLower())
                {
                    bUserCorrect = true;
                    iUsersScore++;
                }

                // Create the container for the information
                Panel panelQuestion = new Panel();

                Label l = new Label();
                string sQuestion = xQuestion.SelectSingleNode("Text").InnerText;
                // Create the Question again
                if (sQuestion.Contains("|"))
                {
                    Image iImage;
                    string sData = sQuestion.Substring(sQuestion.IndexOf("|"));
                    string[] asData = sData.Split(' ');
                    string sDataType = asData[0];
                    string sDataValue = asData[1].Substring(0, asData[1].IndexOf("|"));

                    switch (sDataType.TrimStart('|'))
                    {
                        case "img":
                            iImage = CustomUtilities.CustomUtilities.GetQuestionImage(sDataValue);
                            iImage.ToolTip = xQuestion.Attributes["Id"].ToString();
                            l = new Label();
                            l.CssClass = "SubHeaderText";
                            l.Text = (i + 11).ToString() + ". " + sQuestion.Substring(0, sQuestion.IndexOf("|"));
                            l.Text += "<br />";
                            // Make text green if correct or red if incorrect
                            if (bUserCorrect)
                            {
                                l.Style["color"] = "#72DA25 !important";
                            }
                            else
                            {
                                l.Style["color"] = "#C4140B !important";
                            }
                            panelQuestion.Controls.Add(l);
                            panelQuestion.Controls.Add(iImage);
                            l = new Label();
                            l.Text = "<br />" + sQuestion.Substring(sQuestion.LastIndexOf("|") + 1) + "<br />";
                            l.CssClass = "SubHeaderText";
                            // Make text green if correct or red if incorrect
                            if (bUserCorrect)
                            {
                                l.Style["color"] = "#72DA25 !important";
                            }
                            else
                            {
                                l.Style["color"] = "#C4140B !important";
                            }
                            panelQuestion.Controls.Add(l);
                            break;
                    }

                }
                else
                {
                    l = new Label();
                    l.CssClass = "SubHeaderText";
                    l.Text = String.Format("{0}. {1}<br />", (i + 11), sQuestion);
                    // Make text green if correct or red if incorrect
                    if (bUserCorrect)
                    {
                        l.Style["color"] = "#72DA25 !important";
                    }
                    else
                    {
                        l.Style["color"] = "#C4140B !important";
                    }
                    panelQuestion.Controls.Add(l);
                }
                RadioButton option;
                foreach (XmlElement ele in xQuestion.SelectSingleNode("Answers").ChildNodes)
                {
                    option = new RadioButton();
                    option.GroupName = "Unit3_" + xQuestion.Attributes["Id"].Value;
                    option.Text = ele.InnerText;
                    // Stops the user from changing the value
                    option.Enabled = false;
                    if (asUsersUnit3Answers != null)
                    {
                        if (ele.Name == asUsersUnit3Answers[i] && bUserCorrect)
                        {
                            option.Style["color"] = "#72DA25";
                            option.Checked = true;
                        }
                        else if (ele.Name == asUsersUnit3Answers[i] && !bUserCorrect)
                        {
                            option.Style["color"] = "#C4140B";
                            option.Checked = true;
                        }
                        else if (ele.Name.ToLower() == sCorrectAnswer.ToLower() && !bUserCorrect)
                        {
                            // Makes text blue if user has not selected this answer
                            option.Style["color"] = "#0000CC";
                            option.Checked = true;
                        }
                        else
                        {
                            option.Checked = false;
                        }
                    }
                    else
                    {
                        option.Checked = false;
                    }

                    option.AutoPostBack = false;
                    option.Attributes.Add("data-AnswerLetter", ele.Name);
                    panelQuestion.Controls.Add(option);
                    l = new Label();
                    l.Text = "<br />";
                    panelQuestion.Controls.Add(l);
                }

                Button buttonShowExplanation = new Button();
                buttonShowExplanation.Text = "Show Explanation";
                buttonShowExplanation.UseSubmitBehavior = false;
                buttonShowExplanation.CssClass = "ExplanationButton";
                // Creates a JavaScript function for the button click so the page won't refresh when wanting to show and hide an explanation
                // - It will toggle whether it's visible and hidden and the button text
                buttonShowExplanation.Attributes.Add("onclick", "if (document.getElementById('ContentMain_panelShowExplanation_Unit3_" + xQuestion.Attributes["Id"].Value + "').style.display === 'none'){document.getElementById('ContentMain_panelShowExplanation_Unit3_" + xQuestion.Attributes["Id"].Value + "').style.display = ''; this.innerText = 'Hide Explanation'; this.value = 'Hide Explanation';}else{document.getElementById('ContentMain_panelShowExplanation_Unit3_" + xQuestion.Attributes["Id"].Value + "').style.display = 'none'; this.innerText = 'Show Explanation';this.value = 'Show Explanation';};return false;");
                panelQuestion.Controls.Add(buttonShowExplanation);

                Panel p = new Panel();
                p.ID = "panelShowExplanation_Unit3_" + xQuestion.Attributes["Id"].Value;
                p.Controls.Add(new HtmlGenericControl("hr"));
                l = new Label();
                l.Text = sExplanation;
                p.Controls.Add(l);
                p.Style["display"] = "none";

                panelQuestion.Controls.Add(p);

                l = new Label();
                l.Text = "<br /><br />";
                panelQuestion.Controls.Add(l);
                panelUnit3Questions.Controls.Add(panelQuestion);
            }

            // Calculate Users percentage and place the values into the page header
            decimal dPercent;
            dPercent = decimal.Divide(iUsersScore, 15) * 100;
            labelPageHeader.Text = labelPageHeader.Text.Replace("%QUESTIONSCORRECT%", iUsersScore.ToString());
            labelPageHeader.Text = labelPageHeader.Text.Replace("%PERCENTAGE%", Math.Round(dPercent, 2).ToString() + "%");
        }

        protected void buttonHome_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Default.aspx");
        }

        protected void buttonRetake_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Test/Page1.aspx");
        }
    }
}