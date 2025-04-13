using class_library_exam.Models;

namespace class_library_exam
{
    public class GameLogic
    {
        public DifficultyLevel CurrentDifficulty { get; private set; }
        public int CurrentScore { get; private set; }
        public int Attempts { get; private set; }
        public int TimeLeft { get; private set; }
        public GameFigure[,] GameField { get; private set; }
        private Random random;

        public GameLogic()
        {
            random = new Random();
            CurrentDifficulty = DifficultyLevel.Easy;
            ResetGame();
        }

        public void ResetGame()
        {
            CurrentScore = 0;
            Attempts = 0;
            TimeLeft = 60;
            InitializeGameField();
        }

        public void SetDifficulty(DifficultyLevel difficulty)
        {
            CurrentDifficulty = difficulty;
            ResetGame();
        }

        public bool CheckFigure(int row, int col)
        {
            if (GameField[row, col]?.IsUnique == true)
            {
                CurrentScore++;
                return true;
            }

            Attempts++;
            return false;
        }

        public void DecrementTime()
        {
            if (TimeLeft > 0)
                TimeLeft--;
        }

        private void InitializeGameField()
        {
            int size = (int)CurrentDifficulty;
            GameField = new GameFigure[size, size];

            FillFieldWithRandomFigures();
            AddUniqueFigure();
        }

        private void FillFieldWithRandomFigures()
        {
            int size = GameField.GetLength(0);

            FigureType mainType = random.Next(2) == 0 ? FigureType.Circle : FigureType.Triangle;
            FigureColor mainColor = random.Next(2) == 0 ? FigureColor.Blue : FigureColor.Green;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    GameField[i, j] = new GameFigure(mainType, mainColor);
                }
            }
        }


        private void AddUniqueFigure()
        {
            int size = GameField.GetLength(0);
            List<(int, int)> candidateCells = new List<(int, int)>();

            // Сначала ищем пустые ячейки
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (GameField[i, j].Type == FigureType.Empty)
                    {
                        candidateCells.Add((i, j));
                    }
                }
            }

            // Если пустых ячеек нет — используем обычные
            if (candidateCells.Count == 0)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (GameField[i, j].Type != FigureType.Empty)
                        {
                            candidateCells.Add((i, j));
                        }
                    }
                }
            }

            if (candidateCells.Count > 0)
            {
                var (row, col) = candidateCells[random.Next(candidateCells.Count)];

                // Чтобы отличалась по цвету/форме
                FigureType uniqueType = GameField[row, col].Type == FigureType.Circle ? FigureType.Triangle : FigureType.Circle;
                FigureColor uniqueColor = GameField[row, col].Color == FigureColor.Blue ? FigureColor.Green : FigureColor.Blue;

                GameField[row, col] = new GameFigure(uniqueType, uniqueColor, true);
            }
        }



    }
}
