namespace VisualAlgorithms.Pages;

using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
public partial class PathFinder: ComponentBase
{

    public int[,] map { get; set; }
  //  public int[,] blocks { get; set; }
    public int[,] visited { get; set; }
    public bool connected { get; set; } = false;

    //Origen
    int InitX, InitY;

    //Destiny
    int EndX, EndY;

    int CurrentX,  CurrentY;

    //Posible moves 
    int[] xMove;
    int[] yMove;

    int maxWidth = 200;
    int maxHeight = 100;
    int maxBlocks = 50;

    int steps =0;

    bool Running {get;set;}=false;
    string Message {get;set;} = "";
    int RefreshViewSteps {get;set;}=1000;

    class myPoint
    {
        public int i;
        public int j;
        public myPoint(int myI, int myJ)
        {
            i = myI;
            j = myJ;

        }

    }


    List<myPoint> route = new List<myPoint>();


    public PathFinder()
    {

        xMove = new int[] { 0, -1,  0 , 1};
        yMove = new int[] { -1, 0,  1, 0 };
        map = new int[maxWidth, maxHeight];
        visited = new int[maxWidth, maxHeight];
        InitX = 10;
        InitY = 10;
        CurrentX = InitX;
        CurrentY = InitY;
        EndX = 190;
        EndY = 95;
        initmap();
    }

   

    void initmap()
    {
        map = new int[maxWidth, maxHeight];
        visited = new int[maxWidth, maxHeight];
        CurrentX = InitX;
        CurrentY = InitY;
        Message="";
        int i, j;
        for (i = 0; i < maxWidth; i++)
            for (j = 0; j < maxHeight; j++)
            {
                map[i, j] = 9;
                visited[i,j]=0;
            }

    }

    public void createBlocks()
    {
        var rnd = new Random();
        int i, j;
        for (int b = 0; b < maxBlocks; b++)
        {
            i = rnd.Next(maxWidth);
            j = rnd.Next(maxHeight);
            var barsize = rnd.Next(Math.Min(maxHeight, maxWidth));
            var orientation = rnd.Next(2);
            map[i, j] = -1;
        }
    }

    public void createMaze()
    {
        bool horizontal=true; 
        bool vertical = true;

        var rnd = new Random();
        if (horizontal)
        {
             for (int k = 0; k < 10; k++)
            {
                int i, c, size;
                i = rnd.Next(maxWidth);
                c = rnd.Next(maxHeight);
                size = rnd.Next(maxHeight / 2);
                if (c + size > maxHeight) size = maxHeight - c;
                for (int j = 0; j < size; j++)
                    map[i , j+c] = -1;

            }
        }
        if (vertical)
        {
            for (int k = 0; k < 10; k++)
            {
                int j, c, size;
                j = rnd.Next(maxHeight);
                c = rnd.Next(maxWidth);
                size = rnd.Next(maxWidth / 2);
                if (c + size > maxWidth) size = maxWidth - c;
                for (int i = 0; i < size; i++)
                    map[i + c, j] = -1;
            }
        }
    }

    public bool isSafe(int x, int y)
    {
        return (x >= 0 && x < maxWidth && y >= 0 && y < maxHeight
                && (map[x, y] == 9));
    }

    public double getDistance(int i, int j, int a, int b)
    {
        double val = Math.Sqrt((a - i) * (a - i) + (b - j) * (b - j));
        if (val.ToString() == "NaN")
            Console.WriteLine("Error;");
        return val;
    }

    // Identify if the two powin are connected or not (Optional procedure)
    public void revealPath(int i, int j)
    {
        if (i < 0 || (i > maxWidth -1) ) return;
        if (j < 0 || (j > maxHeight-1)) return;

        if ((map[i, j] == 0 || map[i, j] == 5 || map[i, j] == 6)  && visited[i, j] == 0)
        {
            map[i, j] = 9;
            visited[i, j] = 1;
            revealPath( i + 1, j);
            revealPath( i - 1, j);
            revealPath( i , j - 1);
            revealPath( i , j + 1);
 
        }
        if (map[InitX, InitY] == map[EndX, EndY]) connected = true;

    }

    public async Task<bool> TrackRoute(int i, int j)
    {
        int k, next_x, next_y;
        if (i == EndX && j == EndY) return true; // Find route
        
        CurrentX = i;
        CurrentY = j;
        double[] distances = new double[4];
        List<double> A = new List<double>();

        steps++;
        if (steps % RefreshViewSteps ==0)
        {
            await Task.Delay(1);
            this.StateHasChanged();
        }

        for (k = 0; k < 4; k++)
        {
            next_x = i + xMove[k];
            next_y = j + yMove[k];
            distances[k] = getDistance(next_x, next_y, EndX, EndY);
            A.Add(getDistance(next_x, next_y, EndX, EndY));
        }

        //Method to calculate the order for Backgracking operator. Short distance first.
        // Easy algorithm to define the next step
        var sorted = A
        .Select((x, i) => new KeyValuePair<double, int>(x, i))
        .OrderBy(x => x.Key)
        .ToList();

        List<double> B = sorted.Select(x => x.Key).ToList();
        List<int> idx = sorted.Select(x => x.Value).ToList();

        for ( k = 0; k < 4; k++)
        {
            next_x = i + xMove[idx[k]];
            next_y = j + yMove[idx[k]];

            if (isSafe(next_x, next_y))
            {
                map[next_x, next_y] = 8;
                var res=await TrackRoute(next_x, next_y);
                if (res==false)  
                {
                    map[next_x, next_y] = -2; // Backtraking and mark map with Gray color
                }
                else return true;
            }
        }
        return false;
    }


    public async Task  findRoute()
    {

        if (Running) return;
        Running=true;

// Optional, revel path and see if there is a connection between Start and End
// Becase of stackoverflow, Blazor need more memory for recursion. 
// Simple methong to asign extra memory for the stack, in this case 8MB
//var thread = 
//       new Thread( 
//              _ =>   revealPath(InitX, InitY), 
//              8000000); // ~ 8MB stack
//thread.Start();
//thread.Join();

        steps=0;
        bool findRoute= await TrackRoute(InitX, InitY);
        if (findRoute) Message="Path found";
        else Message="Path not found";
        Running=false;
       
        await Task.Delay(10);
        this.StateHasChanged();
        
    }


}