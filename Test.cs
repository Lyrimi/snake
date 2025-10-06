using snake;

public class Test()
{
    public void Main()
    {
        Coords position = new(3, 3);
        int width = 16;
        int height = 7;

        Box box = new(position, width, height);

        box.Draw();

    }
}