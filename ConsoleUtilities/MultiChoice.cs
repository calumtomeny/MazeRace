using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleUtilities
{
    public class MultiChoice
    {
        private const int OPTION_SPACING = 1;
        private const ConsoleColor SELECTED_COLOUR = ConsoleColor.Red;

        private int consoleWidth;
        private int consoleHeight;

        private int consoleVerticalCenter;
        private int consoleHorizontalCenter;

        private int startingVerticalPosition;
        private int startingHorizontalPosition;

        private int longestOptionLength;

        private int selectedIndex = 0;

        private List<Option> Choices;
        public MultiChoice(IEnumerable<Option> Choices)
        {
            this.Choices = Choices.ToList();
            consoleWidth = Console.WindowWidth;
            consoleHeight = Console.WindowHeight;

            consoleHorizontalCenter = consoleWidth / 2;
            consoleVerticalCenter = consoleHeight / 2;

            startingVerticalPosition = Choices.Count() + OPTION_SPACING / 2;
            longestOptionLength = Choices.Select(x => x.OptionText).ToList().Aggregate("", (max,cur) => max.Length > cur.Length ? max : cur).Length;
            startingHorizontalPosition = longestOptionLength / 2;
        }

        private void Render()
        {
            Console.Clear();
            int initialVerticalRenderPosition = consoleVerticalCenter - startingVerticalPosition;
            int initialHorizontalRenderPosition = consoleHorizontalCenter - startingHorizontalPosition;

            for(int i = 0; i < Choices.Count() * (OPTION_SPACING + 1); i = i + OPTION_SPACING + 1)
            {
                int choiceIndex = i == 0 ? 0 : i/(OPTION_SPACING + 1);
                Console.SetCursorPosition(initialHorizontalRenderPosition, initialVerticalRenderPosition + i);

                if (choiceIndex == selectedIndex)
                {
                    Console.ForegroundColor = SELECTED_COLOUR;
                    Console.SetCursorPosition(Console.CursorLeft - 3, initialVerticalRenderPosition + i);
                    Console.Write("-> ");
                    Console.SetCursorPosition(initialHorizontalRenderPosition, initialVerticalRenderPosition + i);
                }

                Console.WriteLine(Choices[choiceIndex].OptionText);

                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public string Show()
        {
            Render();
            HandleInput();
            Console.Clear();
            return Choices[selectedIndex].OptionValue;
        }

        private void HandleInput()
        {
            bool enterKeyPressed = false;

            while (!enterKeyPressed)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            OptionUp();
                            break;
                        case ConsoleKey.DownArrow:
                            OptionDown();
                            break;
                        case ConsoleKey.Enter:
                            enterKeyPressed = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void OptionDown()
        {
            if (selectedIndex < Choices.Count - 1)
            {
                selectedIndex++;
                Render();
            }
        }

        private void OptionUp()
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
                Render();
            }
        }
    }
}
