namespace FlashCards.Ramseis
{
    internal class Menu
    {
        public string TopLeft { get; set; } = "╔";
        public string TopRight { get; set; } = "╛";
        public string Top { get; set; } = "═";
        public string Left { get; set; } = "║";
        public string Bottom { get; set; } = "═";
        public string BottomLeft { get; set; } = "╚";
        public string BottomRight { get; set; } = "╕";
        public string Right { get; set; } = "│";
        public string RightInnerTop { get; set; } = "┌┐";
        public string RightInnerBottom { get; set; } = "└┘";
        public string LeftHDivider { get; set; } = "╟";
        public string RightHDivider { get; set; } = "┤";
        public string HDivider { get; set; } = "─";
        public List<string> Options { get; set; } = new List<string> { "Default Option 1!", "Default Option 2!"};
        public List<string> Titles { get; set; } = new List<string>{"Default title!", "Subtitle!"};
        public int InputRow { get; set; }
        public int MinWidth { get; set; } = 50;

        public void Draw()
        {
            Console.Clear();
            int maxStringLength = 0;
            foreach (string option in Options)
            {
                if (maxStringLength < option.Length + 4) { maxStringLength = option.Length + 4; }
            }
            foreach (string title in Titles)
            {
                if (maxStringLength < title.Length) { maxStringLength = title.Length; }
            }
            if (maxStringLength < MinWidth - 6) { maxStringLength = MinWidth - 6; }

            int row = 0;
            
            // Top bar
            Console.SetCursorPosition(0, row);
            Console.Write(TopLeft);
            for (int i = 0;  i < maxStringLength + 4; i++)
            {
                Console.Write(Top);
            }
            Console.Write(TopRight);
            row++;

            // Title rows and divider (if any)
            if (Titles.Count > 0)
            {
                for (int i = 0;i < Titles.Count;i++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write(Left + " ");
                    Console.Write(Titles[i]);
                    Console.SetCursorPosition(maxStringLength + 4, row);
                    if (i == 0)
                    {
                        Console.Write(RightInnerTop);
                    }
                    else
                    {
                        Console.Write(Right + Right);
                    }
                    row++;
                }
                Console.SetCursorPosition(0, row);
                Console.Write(LeftHDivider);
                for (int i = 0; i < maxStringLength + 3; i++)
                {
                    Console.Write(HDivider);
                }
                Console.Write(RightHDivider + Right);
                row++;
            }

            // Option rows and divider (if any)
            if (Options.Count > 0)
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write(Left + " ");
                    Console.Write(Options[i]);
                    Console.SetCursorPosition(maxStringLength + 4, row);
                    Console.Write(Right + Right);
                    row++;
                }
                Console.SetCursorPosition(0, row);
                Console.Write(LeftHDivider);
                for (int i = 0; i < maxStringLength + 3; i++)
                {
                    Console.Write(HDivider);
                }
                Console.Write(RightHDivider + Right);
                row++;
            }

            // Input bar
            Console.SetCursorPosition(0, row);
            Console.Write(Left);
            Console.SetCursorPosition(maxStringLength + 4, row);
            Console.Write(RightInnerBottom);
            row++;

            // Bottom bar
            Console.SetCursorPosition(0, row);
            Console.Write(BottomLeft);
            for (int i = 0; i < maxStringLength + 4; i++)
            {
                Console.Write(Bottom);
            }
            Console.Write(BottomRight);
            this.InputRow = row - 1;
            Console.SetCursorPosition(2, this.InputRow);
        }

        public void Draw(string message)
        {
            Draw();
            Console.SetCursorPosition(2, this.InputRow + 2);
            Console.Write(message);
            Console.SetCursorPosition(2, this.InputRow);
        }
    }
}
