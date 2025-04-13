using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KursachLibrary;

namespace kursach_wpf
{
    public partial class MainWindow : Window
    {
        private readonly int _userId;

        public MainWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            RefreshQuizButtons();
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as FrameworkElement;
                if (button?.Tag == null || !int.TryParse(button.Tag.ToString(), out int quizId))
                {
                    MessageBox.Show("Ошибка запуска квиза. Попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var quizWindow = new QuizWindow(quizId, _userId);
                quizWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UpdateButtonStatus();
            }
        }

        private void UpdateButtonStatus()
        {
            foreach (var item in QuizItemsControl.Items)
            {
                if (item is Button button)
                {
                    var quizId = int.Parse(button.Tag.ToString());
                    var result = DatabaseHelper.GetQuizResult(_userId, quizId);

                    button.Background = result switch
                    {
                        null => Brushes.White,
                        0 => Brushes.Red,
                        1 => Brushes.Green,
                        _ => Brushes.White
                    };
                }
            }
        }


        private void CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateQuizWindow();
            createWindow.ShowDialog();

            if (createWindow.IsQuizCreated)
                RefreshQuizButtons();
        }

        private void RefreshQuizButtons()
        {
            QuizItemsControl.Items.Clear();
            var quizzes = DatabaseHelper.GetQuizzes();

            foreach (var quiz in quizzes)
            {
                var button = new Button
                {
                    Content = quiz.Title,
                    Tag = quiz.Id,
                    Margin = new Thickness(10),
                    Padding = new Thickness(10),
                    MinWidth = 150,
                    MinHeight = 80,
                    FontSize = 18,
                    Foreground = Brushes.Black,
                    Background = new SolidColorBrush(Color.FromRgb(209, 154, 102)),
                    BorderBrush = Brushes.Transparent
                };

                button.Click += QuizButton_Click;
                QuizItemsControl.Items.Add(button);
            }

            UpdateButtonStatus();
        }


    }
}
