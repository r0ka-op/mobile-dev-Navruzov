using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using KursachLibrary;

namespace kursach_wpf
{
    public partial class CreateQuizWindow : Window
    {
        public bool IsQuizCreated { get; private set; } = false;

        private byte[] questionImageBytes = null;
        private byte[][] answerImageBytes = new byte[4][];

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
                MessageBox.Show("Введите номер правильного ответа от 1 до 4.");
                return;
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(questionText) || Array.Exists(answers, string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            int quizId = DatabaseHelper.AddQuiz(title);
            DatabaseHelper.AddQuestion(new Question
            {
                QuestionText = questionText,
                Image = questionImageBytes,
                Answers = new List<string>(answers),
                AnswerImages = new List<byte[]>(answerImageBytes),
                CorrectAnswer = correctAnswer - 1
            }, quizId);

            IsQuizCreated = true;
            Close();
        }

        // Универсальный метод выбора изображения
        private byte[] SelectImage()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            return dlg.ShowDialog() == true ? File.ReadAllBytes(dlg.FileName) : null;
        }

        private void SelectQuestionImage_Click(object sender, RoutedEventArgs e)
        {
            var img = SelectImage();
            if (img != null) questionImageBytes = img;
        }

        private void SelectAnswerImage1_Click(object sender, RoutedEventArgs e)
        {
            var img = SelectImage();
            if (img != null) answerImageBytes[0] = img;
        }

        private void SelectAnswerImage2_Click(object sender, RoutedEventArgs e)
        {
            var img = SelectImage();
            if (img != null) answerImageBytes[1] = img;
        }

        private void SelectAnswerImage3_Click(object sender, RoutedEventArgs e)
        {
            var img = SelectImage();
            if (img != null) answerImageBytes[2] = img;
        }

        private void SelectAnswerImage4_Click(object sender, RoutedEventArgs e)
        {
            var img = SelectImage();
            if (img != null) answerImageBytes[3] = img;
        }
    }
}
