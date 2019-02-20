using ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace SoftwareDevelopmentTest.Test
{
    public partial class Page1 : System.Web.UI.Page
    {
        // List to make sure no questions are duplicated
        List<int> questionsGot = new List<int>();


        protected void Page_Load(object sender, EventArgs e)
        {
            // Load the XML document that contains the questions
            XmlDocument questionDocument = new XmlDocument();
            questionDocument.Load(Server.MapPath("~/Questions.xml"));
            XmlNode unitQuestions = questionDocument.SelectSingleNode("//Unit[@Name='Core Programming']");

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
                // If user hasn't been generated any question - generate them
                if (Session["Unit1Questions"] == null)
                {
                    // Get random number
                    x = rand.Next(1, unitQuestions.SelectNodes("Question[@Id]").Count + 1);
                    // if the list contains the question index - get new random index until it is unique
                    while (questionsGot.Contains(x))
                    {
                        x = rand.Next(1, unitQuestions.SelectNodes("Question[@Id]").Count + 1);
                    }
                    // add it to the list
                    questionsGot.Add(x);

                    // get the randoom question
                    question = unitQuestions.SelectSingleNode("//Question[@Id='" + x + "']");
                }
                else
                {
                    int[] iQuestionList = (int[])Session["Unit1Questions"];
                    question = unitQuestions.SelectSingleNode("//Question[@Id='" + iQuestionList[i] + "']");
                }
                // instantiate the panel and add the controls to the panel
                panelQuestion = new Panel();
                
                string sQuestion = question.SelectSingleNode("Text").InnerText;
                // This will add the Image to the question if it contains '|'
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
                            Label l = new Label();
                            l.CssClass = "SubHeaderText";
                            l.Text = (i + 1).ToString()+ ". " + sQuestion.Substring(0, sQuestion.IndexOf("|"));
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
                    label.Text = string.Format("{0}. {1}<br />", (i + 1), sQuestion);
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
                    if (Session["Unit1Answers"] != null)
                    {
                        try
                        {
                            string[] asUserAnswers = (string[])Session["Unit1Answers"];
                            if (asUserAnswers.Length <= 0 || asUserAnswers == null)
                            {
                                option.Checked = false;
                            }
                            // Set it to checked if user has already answered the question
                            if (ele.Name == asUserAnswers[i].ToLower())
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
                            // You can see the log in the file location if you are curious
                            HandleException.WriteException(ex, Server.MapPath("~") + @"\ErrorLog.xml");
                            option.Checked = false;
                        }
                    }
                    else
                    {
                        option.Checked = false;
                    }
                    
                    // Stops the postback to the server - increases usability
                    option.AutoPostBack = false;

                    // Adds custom attribute so i can track which option was clicked
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
            // Store the question Id's into the user's session
            if (Session["Unit1Questions"] == null)
            {
                Session.Add("Unit1Questions", questionsGot.ToArray());
            }
        }

        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            r.Attributes.Add("data-checked", "true");
        }

        protected void buttonBack_Click(object sender, EventArgs e)
        {
            // Disables the user being able to click the button for a second time as this can throw errors if button is spammed
            buttonBack.Visible = false;

            List<string> listUserAnswers = new List<string>();

            // Iterate through all the panels in the placeholder
            // Remember how a panel was created for each question
            // This is using the 'System.Linq' library
            foreach (Panel p in panelQuestionsPlaceholder.Controls.OfType<Panel>())
            {
                foreach (RadioButton rad in p.Controls.OfType<RadioButton>())
                {
                    // If it is checked - add to the list of user's answers
                    if (rad.Attributes["data-checked"] == "true" || rad.Checked)
                    {
                        listUserAnswers.Add(rad.Attributes["data-AnswerLetter"]);
                    }
                }
            }
            // Adds placeholders as they haven't answered - this stops errors later on
            while (listUserAnswers.Count < 5)
            {
                listUserAnswers.Add("a");
            }
            Session.Add("Unit1Answers", listUserAnswers.ToArray());

            buttonBack.Visible = false;

            Response.Redirect("~/Default.aspx");
        }

        protected void buttonNext_Click(object sender, EventArgs e)
        {
            // Disables the user being able to click the button for a second time as this can throw errors if button is spammed
            buttonNext.Visible = false;

            List<string> listUserAnswers = new List<string>();

            // Iterate through all the panels in the placeholder
            // Remember how a panel was created for each question
            // This is using the 'System.Linq' library
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
            // Adds placeholders as they haven't answered - this stops errors later on
            while (listUserAnswers.Count < 5)
            {
                listUserAnswers.Add("a");
            }
            Session.Add("Unit1Answers", listUserAnswers.ToArray());

            buttonNext.Visible = true;

            Response.Redirect("~/Test/Page2.aspx");
        }
    }
}