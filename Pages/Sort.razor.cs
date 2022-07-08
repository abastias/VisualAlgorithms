namespace VisualAlgorithms.Pages;


public partial class Sort
{
    int[] theArray { get; set; }

    public int maxPoints { get; set; }
    public int maxHigh { get; set; }

    public string MessageTitle { get; set; } = "";
    public string Message { get; set; }

    bool cancelSort { get; set; }
    bool isAnimated { get; set; }
    bool IsSorting { get; set; } = false;

    public int StepView { get; set; } = 1;

    int Steps { get; set; } = 0;
    int Swaps { get; set; } = 0;

    int SwapsIndex1 { get; set; } = -1;
    int SwapsIndex2 { get; set; } = -1;





    double MessageExecuteTime{get;set;}

    int DelayTime {get;set;}

    int BarHeight {get;set;}=5;


    public Sort()
    {
        maxPoints = 100;
        maxHigh = 100;
        DelayTime=10;
        theArray = new int[10000];

        Message = "";
        MessageExecuteTime=0;
        cancelSort = false;
        isAnimated = true;
        SetArray();

    }

    private void SetArray()
    {
        Message = "";
        MessageTitle="Random Value";
        Random rnd = new Random();
        for (int i = 0; i < maxPoints; i++)
        {
            theArray[i] = rnd.Next(maxHigh);
        }
    }

    private async Task Reset()
    {

        SetArray();
        Steps = 0;
        Swaps = 0;
        MessageExecuteTime=0;
        await Task.Delay(10);
        this.StateHasChanged();
    }

    public string barWidthStyle(int pos)
    {
        var value = 500 * theArray[pos] / maxHigh;

        string color;
        if ((pos==SwapsIndex1) || (pos==SwapsIndex2)) color="blue";
        else color="red";


        return  $"width:{value}px;border-bottom: {BarHeight}px solid {color};height: {BarHeight+2}px;";
        
        //return "width:" + value + "px;border-bottom: 2px solid red;height: 3px;";
    }

    private async void BubbleSortClick()
    {
        if (IsSorting) return;
        IsSorting=true;
        if (theArray.Length <= 2) return;
        MessageTitle = "Bubble Sort";
        Message ="Sorting";
        Steps = 0;
        Swaps = 0;

        DateTime initialTime = DateTime.Now;
        for (int i = 0; i < maxPoints; i++)
        {
            for (int j = 0; j < maxPoints - i - 1; j++)
            {
                Steps++;
                if (theArray[j] > theArray[j + 1])
                {
                    SwapsIndex1=j;
                    SwapsIndex2=j+1;

                    Swaps++;
                    var temp = theArray[j];
                    theArray[j] = theArray[j + 1];
                    theArray[j + 1] = temp;
                }
                if (cancelSort)
                {
                    i = maxPoints;
                    j = maxPoints;
                }
            }
            if (isAnimated && (Steps % StepView) == 0)
            {
                await Task.Delay(DelayTime);
                this.StateHasChanged();
            }
        }
        SwapsIndex1=-1;
        SwapsIndex2=-1;
        DateTime endTime = DateTime.Now;
        
        MessageExecuteTime=  (endTime - initialTime).TotalSeconds;
        cancelSort = false;
        IsSorting=false;
        Message="Done!";
        this.StateHasChanged();


    }

    private async void SelectionSortClick()
    {
        if (IsSorting) return;
        IsSorting=true;
        Swaps = 0;
        MessageTitle = "Selection Sort";
        Message ="Sorting";
        DateTime initialTime = DateTime.Now;
        int greatestVal = 0;
        int GreatestIndex = 0;
        for (int i = 0; i < maxPoints; i++)
        {
            greatestVal = theArray[0];
            GreatestIndex = 0;
          
            for (int j = 1; j < maxPoints - i; j++)
            {
                Steps++;

                if (theArray[j] > greatestVal)
                {
                 
                    
                    GreatestIndex = j;
                    greatestVal = theArray[j];
                }
                if (cancelSort)
                {
                    i = maxPoints - 1;
                    j = maxPoints - 1;
                }
            }
            if (isAnimated && (Steps % StepView) == 0)
            {
                await Task.Delay(DelayTime);
                this.StateHasChanged();
            }

            // Duplicate values in the Array may produce unnecesary swaps.
            if (theArray[maxPoints - i - 1] != theArray[GreatestIndex])
            {
                SwapsIndex1=GreatestIndex;
                SwapsIndex2=maxPoints - i - 1;

                Swaps++;
                var temp = theArray[maxPoints - i - 1];
                theArray[GreatestIndex] = temp;
                theArray[maxPoints - i - 1] = greatestVal;
            }
        }

        SwapsIndex1=-1;
        SwapsIndex2=-1;
        Message="Done!";
        await Task.Delay(10);
        this.StateHasChanged();

        DateTime endTime = DateTime.Now;
        MessageExecuteTime=  (endTime - initialTime).TotalSeconds;
        cancelSort = false;
        IsSorting=false;
        this.StateHasChanged();


    }

}