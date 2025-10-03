using System.Xml.Serialization;
using snake;

public class Snake(Settings settings)
{

    //\e[38;5;{id} where id is a color id from 0 to 255 refrencing a color table

    Settings settings = settings;

    Random random = new();

    static void AddSegment(ref List<Cords> snake)
    {
        snake.Add(snake[snake.Count - 1]);
    }


    static Cords AddArrayToCords(Cords cords, int[] array2)
    {
        return new Cords(cords.x + array2[0], cords.y + array2[1]);
    }


    static bool Moveplayer(ref List<Cords> snake, int[] dir, Cords[] bounds, ref Cords apple)
    {
        int[] zero = [0, 0];
        if (Enumerable.SequenceEqual(dir, zero))
        {
            return true;
        }
        Cords head = AddArrayToCords(snake[0], dir);

        //Colision checks
        if (head.x <= bounds[0].x | head.x >= bounds[1].x + 1 | head.y <= bounds[0].y | head.y >= bounds[1].y + 2)
        {
            return false;
        }
        if (snake.Contains(head))
        {
            return false;
        }

        for (int i = snake.Count - 1; i > 0; i--)
        {
            snake[i] = snake[i - 1];
        }

        snake[0] = head;

        return true;

    }


    static void DrawPlayer(ref List<Cords> snake, Cords removedTail)
    {
        string playerRend = "";
        playerRend += $"\e[{removedTail.y};{removedTail.x}H ";
        foreach (Cords snakeSeg in snake)
        {
            playerRend += $"\e[{snakeSeg.y};{snakeSeg.x}H\e[38;5;28m#";
        }
        Console.Write(playerRend);
    }

    static Cords NewApple(List<Cords> snake, Cords[] bounds, Random random)
    {
        Cords apple = new(random.Next(bounds[0].x + 1, bounds[1].x + 1), random.Next(bounds[0].y + 1, bounds[1].y + 2));
        while (snake.Contains(apple))
        {
            apple = new(random.Next(bounds[0].x + 1, bounds[1].x + 1), random.Next(bounds[0].y + 1, bounds[1].y + 2));
        }
        Console.Write($"\e[{apple.y};{apple.x}H\e[38;5;196m@");
        return apple;
    }

    static void GameEnd(bool win, int score)
    {
        string EndText = "You Died";

        if (win)
        {
            EndText = "You Won :D";
        }

        int width = 50;
        int height = 15;
        Cords center = new(Console.BufferWidth / 2 + 1, Console.BufferHeight / 2);
        Console.ForegroundColor = ConsoleColor.White;
        Box deathScreen = new(new Cords(center.x - width / 2, center.y - height / 2), width, height);

        deathScreen.Draw();
        new TextBox(deathScreen, EndText, 0).Draw();
        new TextBox(deathScreen, $"Final Score: {score}", height / 2 - 1).Draw();
        new TextBox(deathScreen, $"Press enter to contuine", height - 3).Draw();
        //new(deathScreen, "", height - 4);
        while (true)
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                break;
            }
        }
    }



    int[] dir = [0, 0];
    int[] bufferDir = [0, 0];
    //A list containg all the directions the snake can move
    int[][] dirList = [[0, -1], [-1, 0], [0, 1], [1, 0]];

    bool alive = true;
    bool moved = false;
    bool devMode = false;

    const int frameRate = 120;
    const int snakeDefaultLength = 3;
    const int characterLimit = 20;


    Box? gameBox;
    Cords apple = new();

    Cords center = new();


    public void Play()
    {
        int score = 0;

        bufferDir = [0, 0];
        dir = [0, 0];

        bool win = false;
        devMode = false;
        alive = true;




        int width = settings.width;
        int height = settings.height;
        int snakeMoveInterval = settings.SnakeInterval;
        int currentSnakeInterval = 0;

        int boardlength = (width - 2) * (height - 2);

        List<Cords> snake = new();
        Cords removedTail = new(2, 2);

        center = new(Console.BufferWidth / 2, Console.BufferHeight / 2);

        Cords[] bounds = [new Cords(center.x - width / 2 + 1, center.y - height / 2 + 1), new Cords(center.x - width / 2 + width - 1, center.y - height / 2 + height - 2)];
        gameBox = new(bounds[0], width, height);
        Console.Clear();
        gameBox.Draw();

        snake.Add(gameBox.center);
        for (int i = 0; i < snakeDefaultLength - 1; i++)
        {
            AddSegment(ref snake);
        }

        int sleepInterval = (int)(1000f / frameRate);

        DrawPlayer(ref snake, removedTail);

        apple = NewApple(snake, bounds, random);


        //Main loop
        while (alive)
        {
            currentSnakeInterval++;

            if (Console.KeyAvailable)
            {
                GetDirection();
            }

            if (currentSnakeInterval >= snakeMoveInterval)
            {
                dir = bufferDir;
                removedTail = snake[snake.Count - 1];
                alive = Moveplayer(ref snake, dir, bounds, ref apple);
                DrawPlayer(ref snake, removedTail);
                currentSnakeInterval = 0;


            }


            //Apple Check
            if (snake[0].x == apple.x && snake[0].y == apple.y)
            {
                //win checking
                if (snake.Count >= boardlength)
                {
                    alive = false;
                    win = true;
                }

                score += 100;
                snake.Add(snake[snake.Count - 1]);
                apple = NewApple(snake, bounds, random);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(bounds[0].x, bounds[0].y - 1);
            Console.WriteLine($"Score: {score}");

            //DebugTools
            if (devMode)
            {
                Console.SetCursorPosition(6, 6);
                Console.WriteLine($"{snake.Count} | {boardlength}");
            }


            Thread.Sleep(sleepInterval);
        }
        GameEnd(win, score);
    }

    void GetDirection()
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.W:
                if (dir != dirList[2])
                {
                    bufferDir = dirList[0];
                }
                break;
            case ConsoleKey.A:
                if (dir != dirList[3])
                {
                    bufferDir = dirList[1];
                }
                break;
            case ConsoleKey.S:
                if (dir != dirList[0])
                {
                    bufferDir = dirList[2];
                }
                break;
            case ConsoleKey.D:
                if (dir != dirList[1])
                {
                    bufferDir = dirList[3];
                }
                break;
            case ConsoleKey.UpArrow:
                if (dir != dirList[2])
                {
                    bufferDir = dirList[0];
                }
                break;
            case ConsoleKey.LeftArrow:
                if (dir != dirList[3])
                {
                    bufferDir = dirList[1];
                }
                break;
            case ConsoleKey.DownArrow:
                if (dir != dirList[0])
                {
                    bufferDir = dirList[2];
                }
                break;
            case ConsoleKey.RightArrow:
                if (dir != dirList[1])
                {
                    bufferDir = dirList[3];
                }
                break;
            case ConsoleKey.F1:

                devMode = true;
                break;

            default:
                break;
        }
    }
}

