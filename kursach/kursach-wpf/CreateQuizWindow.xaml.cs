using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using KursachLibrary;

namespace kursach_wpf
{
    public partial class CreateQuizWindow : Window
    {
        public bool IsQuizCreated { get; private set; } = false;

        private string questionImagePath = null;
        private string[] answerImagePaths = new string[4];

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

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(questionText) || Array.Exists(answers, string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            int quizId = DatabaseHelper.AddQuiz(title);
            DatabaseHelper.AddQuestion(new Question
            {
                QuestionText = questionText,
                ImagePath = questionImagePath,
                Answers = new List<string>(answers),
                AnswerImagePaths = new List<string>(answerImagePaths),
                CorrectAnswer = correctAnswer - 1
            }, quizId);

            IsQuizCreated = true;
            Close();
        }

        // Методы выбора изображений
        private string SelectImage()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            return dlg.ShowDialog() == true ? dlg.FileName : null;
        }

        private void SelectQuestionImage_Click(object sender, RoutedEventArgs e)
        {
            var path = SelectImage();
            if (path != null) questionImagePath = path;
        }

        private void SelectAnswerImage1_Click(object sender, RoutedEventArgs e)
        {
            var path = SelectImage();
            if (path != null) answerImagePaths[0] = path;
        }

        private void SelectAnswerImage2_Click(object sender, RoutedEventArgs e)
        {
            var path = SelectImage();
            if (path != null) answerImagePaths[1] = path;
        }

        private void SelectAnswerImage3_Click(object sender, RoutedEventArgs e)
        {
            var path = SelectImage();
            if (path != null) answerImagePaths[2] = path;
        }

        private void SelectAnswerImage4_Click(object sender, RoutedEventArgs e)
        {
            var path = SelectImage();
            if (path != null) answerImagePaths[3] = path;
        }
    }
}
