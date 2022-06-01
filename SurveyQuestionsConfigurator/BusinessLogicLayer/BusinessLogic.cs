using SurveyQuestionsConfigurator.CommonLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SurveyQuestionsConfigurator
{
    public class BusinessLogic
    {
        public static bool CheckSmileyQuestionInputFields(RichTextBox questionTextRichTextBox)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonHelpers.Logger(ex);
                return false;
            }
        } // end func.

        public static bool CheckSliderQuestionInputFields(RichTextBox questionTextRichTextBox, TextBox genericTextBox1, TextBox genericTextBox2, NumericUpDown genericNumericUpDown1, NumericUpDown genericNumericUpDown2)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                    if (!String.IsNullOrWhiteSpace(genericTextBox1.Text))
                        if (!String.IsNullOrWhiteSpace(genericTextBox2.Text))
                            if (genericNumericUpDown1.Value < genericNumericUpDown2.Value)
                                return true;
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonHelpers.Logger(ex);
                return false;
            }
        } // end func.

        public static bool CheckStarQuestionInputFields(RichTextBox questionTextRichTextBox)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(questionTextRichTextBox.Text)) //if Question text is not null or empty 
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened, please try again\n");
                CommonHelpers.Logger(ex);
                return false;
            }
        } // end func.
    }
}
