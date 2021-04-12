using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCows
{
    public class GameLogic
    {
        struct BullsAndCowsPair
        {
            public int Bulls { get; set; }
            public int Cows { get; set; }
        }

        public int NumbersCount { get; set; }
        public bool IsInit { get; private set; }
        private bool _isFirstTurn;
        private bool _hasWon;
        private int _turnsCount;
        private int _lastAnswer;

        private HashSet<int> _set;
        
        public GameLogic() { NumbersCount = 4; }

        public void InitGameData()
        {
            IsInit = true;
            _isFirstTurn = true;
            _hasWon = false;
            _lastAnswer = 0;
            _turnsCount = 0;
            _set = new HashSet<int>();
            int lowerValue = 100;
            int upperValue = (int)Math.Pow(10, NumbersCount) - 1;
            var repeatsDestroyer = new HashSet<int>();

            for (int i = lowerValue; i <= upperValue; i++)
            {
                var digits = NumberUtilities.DivideOnDigits(i, 4);
                repeatsDestroyer.Clear();
                for (int j = 0; j < digits.Length; j++)
                    repeatsDestroyer.Add(digits[j]);

                if (repeatsDestroyer.Count == 4)
                    _set.Add(i);
            }
        }

        public string GetAnswer()
        {
            var rand = new Random();
            if (_hasWon)
            {
                IsInit = false;
                return string.Format("Hooray, the answer was found in {0} moves!!!", _turnsCount);
            }

            int answer;
            //первая попытка
            if (_isFirstTurn)
            {
                answer = rand.GetNumberWithDifferentDigits(NumbersCount);
                _isFirstTurn = false;
            }
            else
            {
                if (_set.Count == 0)
                {
                    IsInit = false;
                    return string.Format("It seems, you made mistake somewhere in your answers. There's no any matched values.", _turnsCount);
                }
                answer = _set.ToList()[rand.Next(0, _set.Count)];
            }

            _lastAnswer = answer;
            _turnsCount++;
            return string.Format("{0:d" + NumbersCount + "}", answer);
        }

        public void SendParams(int bulls, int cows)
        {
            if (bulls == NumbersCount)
                _hasWon = true;
            else
                SiftSet(_set, bulls, cows);
        }
        
        private void SiftSet(HashSet<int> set, int bulls, int cows)
        {
            var valuesToExcept = new List<int>();
            foreach (var value in set)
            {
                var pair = GetPair(value, _lastAnswer);
                if (pair.Bulls != bulls
                 || pair.Cows != cows)
                    valuesToExcept.Add(value);
            }
            set.ExceptWith(valuesToExcept);
        }

        private BullsAndCowsPair GetPair(int value, int lastAnswer)
        {
            int[] valueDigits = NumberUtilities.DivideOnDigits(value, NumbersCount);
            int[] lastAnswerDigits = NumberUtilities.DivideOnDigits(_lastAnswer, NumbersCount);

            var pair = new BullsAndCowsPair() { Bulls = 0, Cows = 0 };

            var cowSearchSource = new List<int>(lastAnswerDigits);
            for (int i = 0; i < valueDigits.Length; i++)
            {
                if (cowSearchSource.Contains(valueDigits[i]))
                {
                    pair.Cows++;
                    cowSearchSource.Remove(valueDigits[i]);
                }
                
                if (valueDigits[i] == lastAnswerDigits[i])
                {
                    pair.Bulls++;
                    pair.Cows--;
                }
            }
            return pair;
        }
    }
}
