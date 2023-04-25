using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace LineSumator
{
    internal ref struct LineSumContext
    {
        public const int MaxNumLenght = 29;

        public bool FoundTrimEnd;
        public bool FoundPoint;
        public bool FoundSign;
        private decimal? SumInLine;

        public bool ErrorInLine { get; private set; }
        public bool IsEmptyNum => _builder.Length == 0;
        public bool HasOneSymbol => _builder.Length == 1;
        public bool IsLastSymbolPoint => _builder.Length > 0 && _builder[^1] == '.';
        public bool IsInvalidNumber => IsEmptyNum || (HasOneSymbol && (FoundSign || FoundPoint)) || IsLastSymbolPoint;
        public bool IsInvalidNumForPoint => FoundTrimEnd || IsEmptyNum || FoundPoint || (HasOneSymbol && FoundSign);

        private decimal? _maxSum;
        private int? _maxSumIndex;
        private int _lineIndex;
        private readonly List<int> _linesWithErrors;
        private readonly StringBuilder _builder = new(MaxNumLenght);

        public LineSumContext()
        {
            _lineIndex = 1;
            _linesWithErrors = new();
        }

        public void NewLine()
        {
            ErrorInLine = false;
            SumInLine = null;
            ResetNumberState();

            _lineIndex++;
        }

        public void AddCharacterToNumber(char character)
        {
            if (_builder.Length >= MaxNumLenght)
            {
                AddLineNumberToErrorList();
                return;
            }

            _builder.Append(character);
        }

        public void AddFloatPointToNumber(char character)
        {
            FoundPoint = true;
            AddCharacterToNumber(character);
        }

        public void AddSignToNumber(char character)
        {
            FoundSign = true;
            AddCharacterToNumber(character);
        }

        public void AddToSum()
        {
            var number = _builder.ToString();

            ResetNumberState();

            if (!decimal.TryParse(number, out var result))
            {
                AddLineNumberToErrorList();
                return;
            }

            if (SumInLine == null)
            {
                SumInLine = result;

                return;
            }

            try
            {
                checked { SumInLine += result; }
            }
            catch (OverflowException)
            {
                AddLineNumberToErrorList();
            }
        }

        public void CalculateLineSum()
        {
            if (!_maxSum.HasValue || _maxSum <= SumInLine)
            {
                _maxSum = SumInLine;
                _maxSumIndex = _lineIndex;
            }

            NewLine();
        }

        public void AddLineNumberToErrorList()
        {
            ErrorInLine = true;
            _linesWithErrors.Add(_lineIndex);
        }

        private void ResetNumberState()
        {
            FoundTrimEnd = false;
            FoundPoint = false;
            FoundSign = false;
            _builder.Clear();
        }

        internal LineSumCalculateResult GetResult()
        {
            return new LineSumCalculateResult(_maxSumIndex, _linesWithErrors.AsReadOnly());
        }
    }
}