namespace LineSumator
{
    public static class LineSum
    {
        public static LineSumCalculateResult Calculate(StreamReader streamReader)
        {
            if (streamReader is null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }

            if (!streamReader.BaseStream.CanRead)
            {
                throw new InvalidOperationException("The stream cannot be read.");
            }

            if (streamReader.BaseStream.Position != 0)
            {
                streamReader.BaseStream.Position = 0;
            }

            var lineSumContext = new LineSumContext();
            Span<char> buffer = stackalloc char[50];

            while (true)
            {
                int readCharacterCount = streamReader.Read(buffer);

                if (readCharacterCount == 0)
                {
                    break;
                }

                for (int i = 0; i < readCharacterCount; i++)
                {
                    char character = buffer[i];


                    if (lineSumContext.ErrorInLine)
                    {
                        if (character is '\n')
                        {
                            lineSumContext.NewLine();
                        }

                        continue;
                    }

                    switch (character)
                    {
                        case '\r': continue;
                        case '\n':
                        case ',':
                            {
                                if (lineSumContext.IsInvalidNumber)
                                {
                                    lineSumContext.AddLineNumberToErrorList();

                                    if (character is '\n')
                                    {
                                        lineSumContext.NewLine();
                                    }

                                    continue;
                                }

                                lineSumContext.AddToSum();

                                if (character is '\n')
                                {
                                    lineSumContext.CalculateLineSum();
                                }

                                break;
                            }
                        case ' ':
                        case '\t':
                            {
                                if (lineSumContext.IsEmptyNum)
                                {
                                    continue;
                                }
                                else if (!lineSumContext.FoundTrimEnd)
                                {
                                    lineSumContext.FoundTrimEnd = true;
                                }

                                continue;
                            }
                        case '.':
                            {
                                if (lineSumContext.IsInvalidNumForPoint)
                                {
                                    lineSumContext.AddLineNumberToErrorList();

                                    continue;
                                }

                                lineSumContext.FoundPoint = true;
                                lineSumContext.AddCharacterToNumber(character);

                                break;
                            }
                        case '-':
                            {
                                if (!lineSumContext.IsEmptyNum)
                                {
                                    lineSumContext.AddLineNumberToErrorList();
                                }

                                lineSumContext.FoundSign = true;
                                lineSumContext.AddCharacterToNumber(character);

                                break;
                            }
                        default:
                            {
                                if (char.IsNumber(character))
                                {
                                    if (lineSumContext.FoundTrimEnd && !lineSumContext.IsEmptyNum)
                                    {
                                        lineSumContext.AddLineNumberToErrorList();
                                    }
                                    else
                                    {
                                        lineSumContext.AddCharacterToNumber(character);
                                    }
                                }
                                else
                                {
                                    lineSumContext.AddLineNumberToErrorList();
                                }

                                break;
                            }
                    }
                }
            }

            if (!lineSumContext.ErrorInLine && !lineSumContext.IsInvalidNumber)
            {
                lineSumContext.AddToSum();

                if (!lineSumContext.ErrorInLine)
                {
                    lineSumContext.CalculateLineSum();
                }
            }
            else if (!lineSumContext.ErrorInLine && lineSumContext.IsInvalidNumber)
            {
                lineSumContext.AddLineNumberToErrorList();
            }

            return lineSumContext.GetResult();
        }
    }
}