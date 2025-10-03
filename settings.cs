using snake;
public class Settings
{
    Box settingsBox;


    List<String> textOptions = new();
    List<String> descriptions = new();

    int selected = 0;

    List<TextBox> settingOptions = new();
    List<TextBox> settingOptionsDescriptions = new();

    TextBox Guide;

    public int SnakeInterval = 10;
    public int width = Console.BufferWidth;
    public int height = Console.BufferHeight;
    public Settings(Box parrent)
    {
        settingsBox = new(new Coords(parrent.pos.x + parrent.width + 1, parrent.pos.y), parrent.width, parrent.height);

        textOptions.Add($"Snake Speed: {SnakeInterval}");
        descriptions.Add("Lower = Faster");
        textOptions.Add($"GameBox Height: {height}");
        descriptions.Add("");
        textOptions.Add($"GameBox Width: {width}");
        descriptions.Add("2 Width ≈ 1 Height in size");
        Guide = new(settingsBox, " ←→ ↑↓ | Escape", settingsBox.height - 3);
        for (int i = 0; i < textOptions.Count; i++)
        {
            settingOptions.Add(new TextBox(settingsBox, textOptions[i], 1 + i * 3));
            settingOptionsDescriptions.Add(new TextBox(settingsBox, descriptions[i], 2 + i * 3));
        }

    }

    public void Start()
    {
        settingsBox.Draw();
        Guide.Draw();
        updateText();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (selected > 0)
                        {
                            selected--;
                            updateText();
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (selected < settingOptions.Count - 1)
                        {
                            selected++;
                            updateText();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (selected == 0 && SnakeInterval > 1)
                        {
                            SnakeInterval--;
                        }
                        else if (selected == 1 && height > 10)
                        {
                            height--;
                        }
                        else if (selected == 2 && width > 10)
                        {
                            width--;
                        }
                        updateText();
                        break;
                    case ConsoleKey.RightArrow:
                        if (selected == 0 && SnakeInterval < 30)
                        {
                            SnakeInterval++;
                        }
                        else if (selected == 1 && Console.BufferHeight > 10)
                        {
                            height++;
                        }
                        else if (selected == 2 && Console.BufferWidth > 10)
                        {
                            width++;
                        }
                        updateText();
                        break;
                    case ConsoleKey.Escape:
                        settingsBox.Clear();
                        return;
                    default:
                        break;
                }
            }
        }


    }


    void updateText()
    {
        textOptions[0] = $"Snake Speed: {SnakeInterval}";
        textOptions[1] = $"GameBox Height: {height}";
        textOptions[2] = $"GameBox Width: {width}";
        for (int i = 0; i < settingOptions.Count; i++)
        {
            if (i == selected)
            {
                settingOptions[i].Text = $">{textOptions[i]}<";
            }
            else
            {
                settingOptions[i].Text = textOptions[i];
            }
            settingOptions[i].Draw();
            settingOptionsDescriptions[i].Draw();
        }
    }
}