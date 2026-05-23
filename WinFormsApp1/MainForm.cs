using TextDividerModule;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {

        private TextDivider _textDivider;

        public MainForm()
        {
            InitializeComponent();
            _textDivider = new TextDivider(new RealFileReader());
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            try
            {
                string inputText = EnterBox.Text;
                int blockLength = (int)NumericUpDown.Value;

                List<string> blocks = _textDivider.Divider(inputText, blockLength);
                ResultBox.Clear();
                for (int i = 0; i < blocks.Count; i++)
                {
                    ResultBox.Text += $"{blocks[i]} ";
                }
                ErrorLabel.Text = "Нет ошибок";
            }
            catch (ArgumentNullException ex)
            {
                ErrorLabel.Text = $"{ex.Message}\nОшибка: текст не может быть пустым";
            }
            catch (ArgumentException ex)
            {
                ErrorLabel.Text = $"{ex.Message}\nОшибка: длина блока должна быть больше 0";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = $"{ex.Message}\nНеожиданная ошибка";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void EnterBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
