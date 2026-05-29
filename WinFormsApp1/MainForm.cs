using TextDividerModule;

namespace WinFormsApp1
{
    /// <summary>
    /// Главная форма приложения.
    /// </summary>
    /// <remarks>
    /// Форма содержит поле ввода текста, выбора длины блока,
    /// кнопку запуска и поле для отображения результата.
    /// </remarks>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Экземпляр класса TextDivider для выполнения логики разделения текста.
        /// </summary>
        private TextDivider _textDivider;

        /// <summary>
        /// Инициализирует новый экземпляр формы MainForm.
        /// </summary>
        /// <remarks>
        /// Выполняет инициализацию компонентов формы и создает экземпляр
        /// TextDivider с реальным читателем файлов.
        /// </remarks>
        public MainForm()
        {
            InitializeComponent();
            _textDivider = new TextDivider(new RealFileReader());
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Divide".
        /// </summary>
        /// <remarks>
        /// Выполняет следующие действия:
        /// 1. Очищает метку ошибки
        /// 2. Получает текст из поля EnterBox
        /// 3. Получает длину блока из NumericUpDown
        /// 4. Вызывает метод Divider для разделения текста
        /// 5. Отображает результат в ResultBox
        /// 6. Обрабатывает возможные исключения и отображает их в ErrorLabel
        /// </remarks>
        private void ResultButton_Click(object sender, EventArgs e)
        {
            // Очистка строки
            ErrorLabel.Text = "";
            try
            {
                // Получение входных данных
                string inputText = EnterBox.Text;
                int blockLength = (int)NumericUpDown.Value;

                // Разделение текста на блоки
                List<string> blocks = _textDivider.Divider(inputText, blockLength);
                
                // Отображение результата
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
    }
}
