namespace snake;

public struct Coords
{
    public int x;
    public int y;
    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
class Program
{

    static void Close()
    {
        Console.Clear();
        Console.WriteLine("Thanks for Playing");
        Environment.Exit(0);
    }

    static void Main(string[] args)
    {
        //IMPORTANT DON'T REMOVE
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.CursorVisible = false;
        Console.Clear();
        Menu();
    }

    static void Menu()
    {
        Coords center = new(Console.BufferWidth / 2, Console.BufferHeight / 2);
        int width = 30;
        int height = 20;
        Coords boxPos = new Coords(center.x - width / 2, center.y - height / 2);
        Box menuBox = new(boxPos, width, height);
        Console.Write($"\e[{center.y};{center.x}H#");
        Thread.Sleep(1000);
        List<TextBox> menuTexts = new();
        menuTexts.Add(new TextBox(menuBox, "A snake console game", 1));
        menuTexts.Add(new TextBox(menuBox, new string('=', width - 2), 3));
        menuTexts.Add(new TextBox(menuBox, " ↑↓  |  Enter", height - 3));


        List<string> menuOptionsTitles = new();
        menuOptionsTitles.Add("Play");
        menuOptionsTitles.Add("Settings");
        menuOptionsTitles.Add("Exit");

        List<TextBox> menuOptions = new();


        for (int i = 0; i < menuOptionsTitles.Count; i++)
        {
            menuOptions.Add(new TextBox(menuBox, menuOptionsTitles[i], 5 + i * 2));
        }

        int selected = 0;



        void UpdateText()
        {
            for (int i = 0; i < menuOptions.Count; i++)
            {
                if (i == selected)
                {
                    menuOptions[i].Text = $">{menuOptionsTitles[i]}<";
                }
                else
                {
                    menuOptions[i].Text = menuOptionsTitles[i];
                }
                menuOptions[i].Draw();
            }
        }

        void DrawMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            menuBox.Draw();
            UpdateText();
            foreach (TextBox textBox in menuTexts)
            {
                textBox.Draw();
            }
        }
        DrawMenu();

        Settings settings = new(menuBox);
        Snake snake = new(settings);

        //main loop
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (selected > 0)
                    {
                        selected--;
                        UpdateText();
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selected < menuOptions.Count - 1)
                    {
                        selected++;
                        UpdateText();
                    }
                    break;
                case ConsoleKey.Enter:
                    if (selected == 1)
                    {
                        selected = -1;
                        UpdateText();
                        settings.Start();
                        selected = 1;
                        UpdateText();
                    }
                    else if (selected == 0)
                    {
                        snake.Play(); DrawMenu(); Thread.Sleep(1000);
                    }
                    else if (selected == 2)
                    {
                        Close();
                    }
                    break;
                case ConsoleKey.Escape:
                    Close();
                    break;
                default:
                    break;
            }
        }
    }
}
