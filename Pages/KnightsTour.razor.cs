namespace VisualAlgorithms.Pages;


public partial class KnightsTour
{
    public int SizeN { get; set; } = 8;
    public int InitX { get; set; } = 0;
    public int InitY { get; set; } = 0;
    public int Steps { get; set; } = 0;
    public string Message { get; set; } = "";
    public int[,] Board = new int[100, 100];
    public bool IsInTour { get; set; } = false;
    public bool ToggleRefresh { get; set; } = true;

    public int CurrentKnightStep{ get; set; } = 0;
    public int SpeedSingleStep{ get; set; } = 10;
    
    public KnightsTour()
    {
        for (int x = 0; x < 100; x++)
            for (int y = 0; y < 100; y++)
                Board[x, y] = -1;
        Board[InitX, InitY]=0;
        
    }

    public async Task<bool> solveKT()
    {
        if (IsInTour == false) return false;
        for (int x = 0; x < SizeN; x++)
            for (int y = 0; y < SizeN; y++)
                Board[x, y] = -1;

        int[] xMove = { 2, 1, -1, -2, -2, -1, 1, 2 };
        int[] yMove = { 1, 2, 2, 1, -1, -2, -2, -1 };
        Board[InitX, InitY] = 0;
        await refreshBoard();

        if (!await solveKTUtil(InitX, InitY, 1, xMove, yMove))
        {
            Message = "Solution does not exist";
            IsInTour = false;
            return false;
        }
        else
        {
            await refreshBoard();
            IsInTour = false;
            await Task.Delay(10);
            this.StateHasChanged();
        }

        IsInTour = false;

        return true;
    }


    public async Task<bool> solveKTUtil(int x, int y, int movei,
                          int[] xMove,
                          int[] yMove)
    {
        int k, next_x, next_y;

        if (IsInTour == false) return true;
        if (movei >= SizeN * SizeN)
        {
            IsInTour = false;
            Message = "Solution Found!";
            return true;
        }

        if (ToggleRefresh) // Refresh board evey single step
        {
            
            await refreshBoard();
            await Task.Delay(SpeedSingleStep);
        }
        else if (Steps % 10000 == 0){ // every 10,000 steps will capture input from user, like stop/reset board, etc. 
            await Task.Delay(10);
            //await refreshBoard();
        }
        else if (Steps % 10000000 == 0){ // Refresh the board every 100,000 steps
            await Task.Delay(10);
            await refreshBoard();
        }
        Steps++;

        /* Try all next moves from
        the current coordinate x, y */
        for (k = 0; k < 8; k++)
        {
            next_x = x + xMove[k];
            next_y = y + yMove[k];
            if (isSafe(next_x, next_y))
            {
                Board[next_x, next_y] = movei;
                CurrentKnightStep= movei;
                if (await solveKTUtil(next_x, next_y, movei + 1,
                                 xMove, yMove))
                    return true;
                else
                {
                    // backtracking
                    Board[next_x, next_y] = -1;
                }
            }
        }
        return false;
    }

    public bool isSafe(int x, int y)
    {
        return (x >= 0 && x < SizeN && y >= 0 && y < SizeN
                && Board[x, y] == -1);
    }

    public async Task refreshBoard()
    {
        await Task.Delay(10);
        this.StateHasChanged();
    }

    private async void StartTour()
    {
        if (IsInTour == false)
        {
            IsInTour = true;
            Steps=0;
            CurrentKnightStep=0;
            await Task.Delay(10);

            this.StateHasChanged();


            await solveKT();
            IsInTour = false;
            await Task.Delay(10);
            this.StateHasChanged();
        }
        else
        {
            IsInTour = false;
            Message = "Tour Stopped";
        }

    }

    private async Task Reset()
    {
        for (int x = 0; x < SizeN; x++)
            for (int y = 0; y < SizeN; y++)
                Board[x, y] = -1;
        Message = "";
        Steps=0;
        CurrentKnightStep=0;
        IsInTour = false;
        Board[InitX, InitY]=0;
        await Task.Delay(10);
        this.StateHasChanged();
    }

    private void RefreshView()
    {
        if (ToggleRefresh) ToggleRefresh = false;
        else ToggleRefresh = true;
    }



}