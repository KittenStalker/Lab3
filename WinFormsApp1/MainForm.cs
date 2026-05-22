

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
            try
            {
                string inputText = EnterBox.Text;
                int blockLength = (int)numericUpDown2.Value;

                List<string> blocks = _textDivider.Divider(inputText, blockLength);
                ResultBox.Clear();
                for (int i = 0; i < blocks.Count; i++)
                {
                    ResultBox.Text += $"{blocks[i]} ";
                }


            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nОшибка: текст не может быть пустым", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nОшибка: длина блока должна быть больше 0", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
