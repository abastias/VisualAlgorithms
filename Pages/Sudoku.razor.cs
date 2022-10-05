namespace VisualAlgorithms.Pages;



public partial class Sudoku
{
    char[][] board  { get; set; }
    char[][] originalBoard  { get; set; }

    public bool IsRunning { get; set; } = false;
    public string Message { get; set; } = "";

    int RandomSetNum{get;set;}
    int RandomPackNum{get;set;}

     int IterationsRefresh{get;set;}
    int Steps {get;set;}
    public Sudoku()
    {
        IterationsRefresh=100;
        Steps=0;
        RandomPackNum=5;
        RandomSetNum=25;

        board= new char[9][];
        originalBoard= new char[9][];
        for (int i=0; i<9; i++)
        {
            board[i]=new char[9];
            originalBoard[i]=new char[9];
        }

           for (int i=0; i<9; i++)
           {
               for (int j=0; j<9; j++)
               {
                board[i][j]=' ';
                originalBoard[i][j]=' ';
               }
           }
    }

    public void AddRandomPack()
    {
         Random rnd = new Random();
        int nCount=0;
        while(nCount<RandomPackNum)
        {

         int row=  rnd.Next(0, 8);
            int col=  rnd.Next(0, 8);
            int num=0;
            if (board[row][col]==' ')
            {
                bool find=false;
                while(find==false)
                {
                     num=  rnd.Next(1, 9);
                    find=isValid(board,row,col,(num.ToString()[0]));
                }
                board[row][col]=num.ToString()[0];
                originalBoard[row][col]=board[row][col];
                nCount++;
                
            }
        }
    }

    public void LoadSudoku()
    {
        for (int i=0; i<9; i++)
           {
               for (int j=0; j<9; j++)
               {
                    board[i][j]=' ';
               }
           }
        //Random sudoku;
        Random rnd = new Random();
      //  int n=30;
        int nCount=0;
        while(nCount<RandomSetNum)
        {
            int row=  rnd.Next(0, 8);
            int col=  rnd.Next(0, 8);
            int num=0;
            if (board[row][col]==' ')
            {
                bool find=false;
                while(find==false)
                {
                     num=  rnd.Next(1, 9);
                    find=isValid(board,row,col,(num.ToString()[0]));
                }
                board[row][col]=num.ToString()[0];

                nCount++;
            }

        }

        for (int i=0; i<9; i++)
           {
               for (int j=0; j<9; j++)
               {
                    originalBoard[i][j]=board[i][j];
               }
           }
    }

    private bool isValid(char[][] board, int row, int col, char num)
    {
        for(int i = 0; i < 9; i++)
        {
            // check neither row nor colum already has num
            if(board[row][i] == num || board[i][col] == num)
                return false;
            
            // start row and col index of corresponding box
            int r = 3 * (row / 3);
            int c = 3 * (col / 3);
            
            // check the 9 cells of the corresponding box
            // i / 3 is equivalent to 0, 0, 0, 1, 1, 1, 2, 2, 2
            // i % 3 is equivalent to 0, 1, 2, 0, 1, 2, 0, 1, 2
            if(board[r + i / 3][c + i % 3] == num)
                return false;
        }

        return true;
    }
    

    private async Task<bool> backtracking(char[][] board)
    {
        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if(board[i][j] == ' ')
                {
                    for(char num = '1'; num <= '9'; num++)
                    {
                        if(isValid(board, i, j, num))
                        {
                            board[i][j] = num;
                            Steps++;
                            
                            if (Steps%IterationsRefresh==0)
                            {
                                await Task.Delay(10);
                                this.StateHasChanged();
                                if (IsRunning==false)
                                {
                                    break;
                                }
                            }
                            if(await backtracking(board))
                                return true; 
                            
                            // backtracking: reset the cell before going back the previous recursion level
                            board[i][j] = ' ';
                        }
                    }
                    
                    // tried 1-9 for current cell, but with no luck
                    return false;
                }
            }
        }
        return true;
    }

      private async Task RandomPack()
    {
        if (IsRunning==false)
        {
            AddRandomPack();
            await Task.Delay(10);
            this.StateHasChanged();
        }
    }
    private async Task RandomSet()
    {
        if (IsRunning==false)
        {
            Message ="";
            IsRunning=false;
            Steps=0;

            LoadSudoku();
            await Task.Delay(10);
            this.StateHasChanged();
        }
    }

    private async Task Stop()
    {
        if (IsRunning)
        {
            IsRunning=false;
            Message="Stopped";
            await Task.Delay(10);
            this.StateHasChanged();

        }
        else{
            IsRunning=true;
            Message="Running Back";
            await Task.Delay(10);
            this.StateHasChanged();
            bool sol=await backtracking(board);
        }
    }

     private async Task Clear()
    {
        Message ="";
        IsRunning=false;
        Steps=0;
        for (int i=0; i<9; i++)
           {
               for (int j=0; j<9; j++)
               {
                    board[i][j]=' ';
                    originalBoard[i][j]=' ';
               }
           }

        await Task.Delay(10);
        this.StateHasChanged();
    }

    private async Task Solve()
    {
        if (IsRunning==false)
        {
            bool sol=false;
            IsRunning=true;
            sol=await backtracking(board);
            IsRunning=false;
            if (sol)
            {
                Message = "Solution Found!!!";
            }
            else 
            {
                Message = "Undefined - No Solution :(";
            }

            await Task.Delay(10);
            this.StateHasChanged();
        }
    }

    private async Task Reset()
    {
        if (IsRunning==false)
        { for (int i=0; i<9; i++)
           {
               for (int j=0; j<9; j++)
               {
                    board[i][j]=originalBoard[i][j];
               }
           }
        
        }

         await Task.Delay(10);
        this.StateHasChanged();

    }

}