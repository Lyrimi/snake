using snake;

//$"\e[{y};{x}H<Text>"
public class Box
{
    char[] boxDrawCornors = ['┏', '┓', '┗', '┛'];
    char[] boxDrawLines = ['┃', '━'];
    public Cords pos;
    public int width;
    public int height;

    public Cords center;


    public Box(Cords pos, int width, int height)
    {
        this.pos = pos;
        this.width = width;
        this.height = height;
        center = new Cords(width / 2 + pos.x, height / 2 + pos.y);
    }

    public void Draw()
    {
        string rend = "";
        rend += $"\e[{pos.y};{pos.x}H{boxDrawCornors[0]}{new string(boxDrawLines[1], width - 2)}{boxDrawCornors[1]}";

        for (int i = 1; i < height - 1; i++)
        {
            rend += $"\e[{pos.y + i};{pos.x}H{boxDrawLines[0]}{new string(' ', width - 2)}{boxDrawLines[0]}";
        }
        rend += $"\e[{pos.y + height - 1};{pos.x}H{boxDrawCornors[2]}{new string(boxDrawLines[1], width - 2)}{boxDrawCornors[3]}";
        Console.Write(rend);
    }
    public void Clear()
    {
        string rend = "";
        for (int i = 0; i < height; i++)
        {
            rend += $"\e[{pos.y + i};{pos.x}H{new string(' ', width)}";
        }
        Console.Write(rend);
    }
}

public class TextBox
{
    Box parentBox;
    public string Text;
    int boxTop;

    public TextBox(Box parent, string Text, int Top)
    {
        parentBox = parent;
        this.Text = Text;
        boxTop = Top;
    }

    public void Draw()
    {
        string rend = "";
        int padding = (parentBox.width - Text.Length - 2) / 2;
        rend += $"\e[{parentBox.pos.y + 1 + boxTop};{parentBox.pos.x + 1}H{new string(' ', padding)}{Text}{new string(' ', padding)}";
        Console.Write(rend);
    }

}
