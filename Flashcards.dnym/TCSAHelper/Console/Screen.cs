namespace TCSAHelper.Console;

public class Screen
{
    private const char _headerBar = '=';
    private const char _footerBar = '-';
    private const string _widthEllipsis = "...";
    private readonly Func<int, int, string> _header;
    private readonly Func<int, int, string> _body;
    private readonly Func<int, int, string> _footer;
    private readonly IDictionary<ConsoleKey, Action> _actions;
    private Action? _anyKeyAction;
    private Action<string>? _promptHandling;
    private bool _stayInScreen;
    private string? _defaultUserInput;
    private readonly char[] _wordSeparators = { ' ', '.', ',', ':', ';', '!', '?', '\n', '\t' };

    public Action ExitScreen => () => _stayInScreen = false;
    public const int HeaderSeparatorHeight = 1;
    public const int HeaderPadding = 1;
    public const int FooterPadding = 1;
    public const int FooterSeparatorHeight = 1;

    /// <summary>
    /// <para>Create a screen.</para>
    /// <para>A screen works by writing to console first the header, then the footer, and lastly the body, which may or may not end with a user input prompt, and then listen for keys as supplied as actions.</para>
    /// <para>As functions, the header, body, and footer can be made to depend on state unknown to this class. (Basically "if X, print this, if Y print that".)</para>
    /// </summary>
    /// <param name="header">Function taking usable width and usable height (not counting padding and separators/border) and returning header text; characters and lines outside of these limits will be cut off.</param>
    /// <param name="body">Function taking usable width and usable height and returning body text; characters and lines outside of these limits will be cut off.</param>
    /// <param name="footer">Function taking usable width and usable height (not counting padding and separators/border) and returning footer text; characters and lines outside of these limits will be cut off.</param>
    /// <param name="actions">Actions to be invoked upon key press.</param>
    /// <param name="anyKeyAction">Action to be invoked upon key press not covered by the actions dictionary. The any-key action is ignored if the prompt handler is set.</param>
    /// <param name="promptHandling">Action to invoke if the screen body is to end with a user input prompt; the user's input string is sent to this action. Note that any keys (e.g. letters) bound by the actions dictionary cannot be caught by the prompt; if needed, these may be removed while the prompt is shown.</param>
    public Screen(Func<int, int, string>? header = null, Func<int, int, string>? body = null, Func<int, int, string>? footer = null, IDictionary<ConsoleKey, Action>? actions = null, Action? anyKeyAction = null, Action<string>? promptHandling = null)
    {
        Func<int, int, string> nullFunc = (_, _) => "";
        _header = header ?? nullFunc;
        _body = body ?? nullFunc;
        _footer = footer ?? nullFunc;
        _actions = actions ?? new Dictionary<ConsoleKey, Action>();
        _anyKeyAction = anyKeyAction;
        _promptHandling = promptHandling;
    }

    public void AddAction(ConsoleKey consoleKey, Action action)
    {
        _actions.Add(consoleKey, action);
    }

    public void RemoveAction(ConsoleKey consoleKey)
    {
        _actions.Remove(consoleKey);
    }

    public void SetAnyKeyAction(Action? action)
    {
        _anyKeyAction = action;
    }

    public void SetPromptAction(Action<string>? action)
    {
        _promptHandling = action;
    }

    public void SetDefaultUserInput(string? userInput)
    {
        _defaultUserInput = userInput;
    }

    private static string CapStrings(int maxWidth, int maxLines, string text)
    {
        if (maxWidth < 0 || maxLines < 0)
        {
            throw new ArgumentException("Max width and max lines cannot be negative numbers.");
        }
        var lines = new List<string>();
        foreach (var line in text.Split('\n'))
        {
            if (lines.Count + 1 > maxLines)
            {
                break;
            }
            else if (line.Length > maxWidth)
            {
                string l;
                if (maxWidth < _widthEllipsis.Length)
                {
                    // E.g. "Hel" with "lo World" cut off.
                    l = line[..maxWidth];
                }
                else
                {
                    // E.g. "Hello..." with " World" cut off, implied by ellipsis.
                    l = line[..(maxWidth - _widthEllipsis.Length)] + _widthEllipsis;
                }
                lines.Add(l);
            }
            else
            {
                lines.Add(line);
            }
        }
        return string.Join('\n', lines);
    }

    private static int CountLines(string text) => text.Split('\n').Length;

    private static int LongestLine(string text) => text.Split('\n').Max(l => l.Length);

    public void Show()
    {
        // Simple enough logic: a loop which clears screen, writes header, writes footer, writes body, and listens for and acts on single keys and/or shows a prompt for the user.
        // The reason for writing header and footer first is to know the height available to the body.
        // When listening to keys, any single key resets the loop.
        // Note that when showing a prompt, that's an inner loop, although temporary input is stored between loop iterations.
        _stayInScreen = true;
        string userInput = _defaultUserInput ?? "";
        int userInputPosition = userInput.Length;
        string? previousDefaultUserInput = _defaultUserInput;
        while (_stayInScreen)
        {
            if (previousDefaultUserInput != _defaultUserInput)
            {
                userInput = _defaultUserInput ?? "";
                userInputPosition = userInput.Length;
            }

            System.Console.Clear();
            var (winWidth, winHeight) = (System.Console.WindowWidth, System.Console.WindowHeight);
            var (usableWidth, usableHeight) = (winWidth, winHeight);

            int usableHeightForHeader = Math.Max(0, usableHeight - (HeaderSeparatorHeight + HeaderPadding));
            var header = CapStrings(usableWidth, usableHeightForHeader, _header(usableWidth, usableHeightForHeader));
            if (!string.IsNullOrEmpty(header))
            {
                // TODO: Maybe generalize and make available to users the number of "extra" lines.
                usableHeight = Math.Max(0, usableHeight - (CountLines(header) + HeaderSeparatorHeight + HeaderPadding));
            }

            int usableHeightForFooter = Math.Max(0, usableHeight - (FooterPadding + FooterSeparatorHeight));
            var footer = CapStrings(usableWidth, usableHeightForFooter, _footer(usableWidth, usableHeightForFooter));
            if (!string.IsNullOrEmpty(footer))
            {
                usableHeight = Math.Max(0, usableHeight - (FooterPadding + FooterSeparatorHeight + CountLines(footer)));
            }

            // Now body is informed of the usable space sans header and footer with separators.
            var body = CapStrings(usableWidth, usableHeight, _body(usableWidth, usableHeight));
            var barLength = Math.Max(LongestLine(header), LongestLine(footer));

            System.Console.WriteLine(header);
            if (!string.IsNullOrEmpty(header))
            {
                System.Console.WriteLine(new string(_headerBar, barLength) + "\n");
            }
            System.Console.Write(body);
            var (bodyCursorLeft, bodyCursorTop) = (System.Console.CursorLeft, System.Console.CursorTop);
            if (!string.IsNullOrEmpty(footer))
            {
                System.Console.SetCursorPosition(0, Math.Max(0, winHeight - CountLines(footer) - 1));
                System.Console.WriteLine(new string(_footerBar, barLength));
                System.Console.Write(footer);
                System.Console.SetCursorPosition(bodyCursorLeft, bodyCursorTop);
            }

            if (_promptHandling == null)
            {
                System.Console.CursorVisible = false;
                var pressedKeyInfo = System.Console.ReadKey(true);
                var pressedKey = pressedKeyInfo.Key;
                if (_actions.TryGetValue(pressedKey, out Action? action))
                {
                    action?.Invoke();
                }
                else
                {
                    _anyKeyAction?.Invoke();
                }
            }
            else
            {
                // TODO: Handle default user input longer than window width.
                System.Console.Write(userInput);
                System.Console.SetCursorPosition(Math.Min(winWidth, bodyCursorLeft + userInputPosition), bodyCursorTop);
                System.Console.CursorVisible = true;
                var takeInput = true;
                while (_stayInScreen && takeInput)
                {
                    var pressedKeyInfo = System.Console.ReadKey(true);
                    var pressedKey = pressedKeyInfo.Key;
                    if (_actions.TryGetValue(pressedKey, out Action? action))
                    {
                        action?.Invoke();
                        takeInput = false;
                    }
                    else
                    {
                        // This section handles the user's cursor movement and text editing.
                        int newCursorLeft = bodyCursorLeft;
                        int newCursorTop = bodyCursorTop;
                        if (pressedKeyInfo.Modifiers == ConsoleModifiers.Control && pressedKeyInfo.KeyChar == '\0')
                        {
                            switch (pressedKey)
                            {
                                case ConsoleKey.LeftArrow:
                                    if (userInputPosition > 0)
                                    {
                                        userInputPosition = userInput[0..(userInputPosition - 1)].LastIndexOfAny(_wordSeparators) + 1;
                                        newCursorLeft = bodyCursorLeft + userInputPosition;
                                        newCursorTop = bodyCursorTop;
                                    }
                                    break;
                                case ConsoleKey.RightArrow:
                                    if (userInputPosition < userInput.Length)
                                    {
                                        int offset = userInput[userInputPosition..].IndexOfAny(_wordSeparators);
                                        if (offset < 0)
                                        {
                                            userInputPosition = userInput.Length;
                                        }
                                        else
                                        {
                                            userInputPosition = userInputPosition + offset + 1;
                                        }
                                        newCursorLeft = bodyCursorLeft + userInputPosition;
                                        newCursorTop = bodyCursorTop;
                                    }
                                    else
                                    {
                                        userInputPosition = userInput.Length;
                                        newCursorLeft = bodyCursorLeft + userInputPosition;
                                        newCursorTop = bodyCursorTop;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (pressedKey)
                            {
                                case ConsoleKey.Enter:
                                    _promptHandling(userInput);
                                    userInput = "";
                                    userInputPosition = 0;
                                    takeInput = false;
                                    break;
                                case ConsoleKey.Home:
                                    userInputPosition = 0;
                                    newCursorLeft = bodyCursorLeft;
                                    newCursorTop = bodyCursorTop;
                                    break;
                                case ConsoleKey.End:
                                    userInputPosition = userInput.Length;
                                    newCursorLeft = bodyCursorLeft + userInputPosition;
                                    newCursorTop = bodyCursorTop;
                                    break;
                                case ConsoleKey.UpArrow:
                                case ConsoleKey.LeftArrow:
                                    userInputPosition = Math.Max(0, userInputPosition - 1);
                                    newCursorLeft = bodyCursorLeft + userInputPosition;
                                    newCursorTop = bodyCursorTop;
                                    break;
                                case ConsoleKey.DownArrow:
                                case ConsoleKey.RightArrow:
                                    userInputPosition = Math.Min(userInput.Length, userInputPosition + 1);
                                    newCursorLeft = bodyCursorLeft + userInputPosition;
                                    newCursorTop = bodyCursorTop;
                                    break;
                                case ConsoleKey.Delete:
                                    if (userInputPosition < userInput.Length)
                                    {
                                        userInput = userInput.Remove(userInputPosition, 1);
                                        var currentCursorLeft = bodyCursorLeft + userInputPosition;
                                        if (currentCursorLeft < winWidth)
                                        {
                                            System.Console.SetCursorPosition(currentCursorLeft, System.Console.CursorTop);
                                            Utils.ClearRestOfLine();
                                            var inputLimit = winWidth - currentCursorLeft;
                                            var restOfInput = userInput[userInputPosition..];
                                            restOfInput = restOfInput[..Math.Min(restOfInput.Length, inputLimit)];
                                            System.Console.Write(restOfInput);
                                        }
                                        newCursorLeft = bodyCursorLeft + userInputPosition;
                                        newCursorTop = bodyCursorTop;
                                    }
                                    break;
                                case ConsoleKey.Backspace:
                                    if (userInputPosition > 0)
                                    {
                                        userInputPosition--;
                                        userInput = userInput.Remove(userInputPosition, 1);
                                        var currentCursorLeft = bodyCursorLeft + userInputPosition;
                                        if (currentCursorLeft < winWidth)
                                        {
                                            System.Console.SetCursorPosition(currentCursorLeft, System.Console.CursorTop);
                                            Utils.ClearRestOfLine();
                                            var inputLimit = winWidth - currentCursorLeft;
                                            var restOfInput = userInput[userInputPosition..];
                                            restOfInput = restOfInput[..Math.Min(restOfInput.Length, inputLimit)];
                                            System.Console.Write(restOfInput);
                                        }
                                        newCursorLeft = bodyCursorLeft + userInputPosition;
                                        newCursorTop = bodyCursorTop;
                                    }
                                    break;
                                default:
                                    {
                                        userInput = userInput.Insert(userInputPosition, $"{pressedKeyInfo.KeyChar}");
                                        userInputPosition++;
                                        var currentCursorLeft = bodyCursorLeft + userInputPosition;
                                        if (currentCursorLeft < winWidth)
                                        {
                                            System.Console.Write(pressedKeyInfo.KeyChar);
                                            Utils.ClearRestOfLine();
                                            var inputLimit = winWidth - currentCursorLeft;
                                            var restOfInput = userInput[userInputPosition..];
                                            restOfInput = restOfInput[..Math.Min(restOfInput.Length, inputLimit)];
                                            System.Console.Write(restOfInput);
                                        }
                                        newCursorLeft = bodyCursorLeft + userInputPosition;
                                        newCursorTop = bodyCursorTop;
                                    }
                                    break;
                            }
                        }
                        if (newCursorLeft < winWidth)
                        {
                            System.Console.CursorVisible = true;
                            System.Console.SetCursorPosition(newCursorLeft, newCursorTop);
                        }
                        else
                        {
                            System.Console.CursorVisible = false;
                            System.Console.SetCursorPosition(winWidth - _widthEllipsis.Length, newCursorTop);
                            System.Console.Write(_widthEllipsis);
                        }
                    }
                }
            }
        }
    }
}
