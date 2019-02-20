using ExceptionHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SoftwareDevelopmentTest.Test
{
    public partial class Page2 : System.Web.UI.Page
    {
        List<int> questionsGot = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load the XML document that contains the questions
            XmlDocument questionDocument = new XmlDocument();
            questionDocument.Load(Server.MapPath("~/Questions.xml"));
            XmlNode unitQuestions = questionDocument.SelectSingleNode("//questions/Unit[@Name='Object Orientated Programming']");

            // Clear the panel placeholder as it will create too many questions
            panelQuestionsPlaceholder.Controls.Clear();


            // Create variables needed to render the questions
            Random rand = new Random();


            // Controls for each question
            Panel panelQuestion;
            Label label;
            RadioButtonList radioOptions;
            RadioButton option;
            XmlNode question;

            int x;

            // Get 5 questions for this unit
            for (int i = 0; i < 5; i++)
            {
                if (Session["Unit2Questions"] == null)
                {
                    // Get random question
                    x = rand.Next(1, 11);
                    // if the list contains the question index - get new random index until it is unique
                    while (questionsGot.Contains(x))
                    {
                        x = rand.Next(1, 11);
                    }
                    // add it to the list
                    questionsGot.Add(x);

                    question = unitQuestions.SelectSingleNode("Question[@Id='" + x + "']");
                }
                else
                {
                    int[] iQuestionList = (int[])Session["Unit2Questions"];
                    question = unitQuestions.SelectSingleNode("Question[@Id='" + iQuestionList[i] + "']");
                }
                // instantiate the panel and add the controls to the panel
                panelQuestion = new Panel();

                string sQuestion = question.SelectSingleNode("Text").InnerText;
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
                            iImage = new Image();
                            iImage.ImageUrl = "~/Images/" + sDataValue.Split('_')[0] + "/" + sDataValue.Split('_')[1] + ".png";
                            iImage.ToolTip = question.Attributes["Id"].ToString();
                            Label l = new Label();
                            l.CssClass = "SubHeaderText";
                            l.Text = (i + 1).ToString() + ". " + sQuestion.Substring(0, sQuestion.IndexOf("|"));
                            l.Text += "<br />";
                            panelQuestion.Controls.Add(l);
                            panelQuestion.Controls.Add(iImage);
                            l = new Label();
                            l.Text = "<br />" + sQuestion.Substring(sQuestion.LastIndexOf("|") + 1) + "<br />";
                            l.CssClass = "SubHeaderText";
                            panelQuestion.Controls.Add(l);
                            break;
                    }

                }
                else
                {
                    label = new Label();
                    label.CssClass = "SubHeaderText";
                    label.Text = String.Format("{0}. {1}<br />", (i + 6), sQuestion);
                    panelQuestion.Controls.Add(label);
                }
                // create a radio button for each option
                foreach (XmlElement ele in question.SelectSingleNode("Answers").ChildNodes)
                {
                    option = new RadioButton();
                    option.Checked = false;
                    option.GroupName = "Unit1_" + question.Attributes["Id"].Value;
                    option.Text = ele.InnerText;
                    option.CheckedChanged += Option_CheckedChanged;
                    if (Session["Unit2Answers"] != null)
                    {
                        try
                        {
                            string[] asUserAnswers = (string[])Session["Unit2Answers"];
                            if (asUserAnswers.Length <= 0)
                            {
                                option.Checked = false;
                            }

                            if (ele.Name == asUserAnswers[i])
                            {
                                option.Checked = true;
                            }
                            else
                            {
                                option.Checked = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            // My Custom Exception Handler will let me know of the error so I can look into it
                            HandleException.WriteException(ex, Server.MapPath("~") + @"\ErrorLog.xml");
                        }
                    }
                    else
                    {
                        option.Checked = false;
                    }

                    option.AutoPostBack = false;
                    option.Attributes.Add("data-AnswerLetter", ele.Name);
                    panelQuestion.Controls.Add(option);
                    label = new Label();
                    label.Text = "<br />";
                    panelQuestion.Controls.Add(label);
                }
                // add spacer/line break
                label = new Label();
                label.Text = "<br /><br />";
                panelQuestion.Controls.Add(label);
                // add the created panel to the panel on the main page
                panelQuestionsPlaceholder.Controls.Add(panelQuestion);
            }
            if (Session["Unit2Questions"] == null)
            {
                Session.Add("Unit2Questions", questionsGot.ToArray());
            }
        }

        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            r.Attributes.Add("data-checked", "true");
        }

        protected void buttonNext_Click(object sender, EventArgs e)
        {
            buttonNext.Visible = false;

            List<string> listUserAnswers = new List<string>();
            foreach (Panel p in panelQuestionsPlaceholder.Controls.OfType<Panel>())
            {
                foreach (RadioButton rad in p.Controls.OfType<RadioButton>())
                {
                    if (rad.Attributes["data-checked"] == "true" || rad.Checked)
                    {
                        listUserAnswers.Add(rad.Attributes["data-AnswerLetter"]);
                    }
                }
            }
            Session.Add("Unit2Answers", listUserAnswers.ToArray());

            buttonNext.Visible = true;

            Response.Redirect("~/Test/Page3.aspx");
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            buttonBack.Visible = false;

            List<string> listUserAnswers = new List<string>();
            foreach (Panel p in panelQuestionsPlaceholder.Controls.OfType<Panel>())
            {
                foreach (RadioButton rad in p.Controls.OfType<RadioButton>())
                {
                    if (rad.Attributes["data-checked"] == "true" || rad.Checked)
                    {
                        listUserAnswers.Add(rad.Attributes["data-AnswerLetter"]);
                    }
                }
            }
            Session.Add("Unit2Answers", listUserAnswers.ToArray());

            buttonBack.Visible = true;

            Response.Redirect("~/Test/Page1.aspx");
        }
    }
}