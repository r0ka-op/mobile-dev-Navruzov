using System;
using System.Windows;
using KursachLibrary;

namespace kursach_wpf
{
    public partial class CreateQuizWindow : Window
    {
        public bool IsQuizCreated { get; private set; } = false;

        public CreateQuizWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string title = QuizTitleTextBox.Text.Trim();
            string questionText = QuestionTextBox.Text.Trim();
            string[] answers = {
                Answer1TextBox.Text.Trim(),
                Answer2TextBox.Text.Trim(),
                Answer3TextBox.Text.Trim(),
                Answer4TextBox.Text.Trim()
            };
            if (!int.TryParse(CorrectAnswerTextBox.Text.Trim(), out int correctAnswer) || correctAnswer < 1 || correctAnswer > 4)
            {
                MessageBox.Show("Введите правильный номер ответа от 1 до 4.");
                return;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(questionText) || Array.Exists(answers, a => string.IsNullOrEmpty(a)))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            int quizId = DatabaseHelper.AddQuiz(title); // вернёт ID викторины
            DatabaseHelper.AddQuestion(new Question
            {
                Id = 0,
                QuestionText = questionText,
                ImagePath = null,
                Answers = new System.Collections.Generic.List<string>(answers),
                AnswerImagePaths = new System.Collections.Generic.List<string> { null, null, null, null },
                CorrectAnswer = correctAnswer - 1,
            }, quizId);

            IsQuizCreated = true;
            Close();
        }
    }
}
