namespace VisualAlgorithms.Pages;


public partial class Pi_Calc
{

    public int MaxIterations { get; set; }
    public int Cycles { get; set; } = 1;
    public int Iteration { get; set; }
    public int AcumIteration { get; set; }

    public double PiCalcAcum { get; set; } = 0;
    public double PiErrorAcum { get; set; } = 0;

    public int Inside { get; set; }
    public int Outside { get; set; }
    bool[,] Zone { get; set; }
    bool[,] ZoneInside { get; set; } // If cirlce or not.

    public string Message { get; set; } = "";

    const int MaxSize = 200;

    public List<CycleData> Data = new List<CycleData>();

    public bool IsRunning { get; set; } = false;

    public class CycleData
    {
        public int Inside = 0;
        public int Outside = 0;

        public double PiCalc = 0;
        public double PiCalcAcum = 0;

        public double PiError = 0;
        public double PiErrorAcum = 0;

        public CycleData(int inside, int outside, double piCalc, double piError, double piCalcAcum, double piErrorAcum)
        {
            Inside = inside;
            Outside = outside;
            PiCalc = piCalc;
            PiError = piError;
            PiCalcAcum = piCalcAcum;
            PiErrorAcum = piErrorAcum;
        }

    }
    public Pi_Calc()
    {
        MaxIterations = 10000;
        AcumIteration = 0;
        Cycles = 20;
        Zone = new bool[MaxSize, MaxSize];
        ZoneInside = new bool[MaxSize, MaxSize];
        int CenterX = MaxSize / 2;
        int CenterY = MaxSize / 2;
        double Radio = (double)MaxSize / 2.0;


        for (int i = 0; i < MaxSize; i++)
            for (int j = 0; j < MaxSize; j++)
            {
                Zone[i, j] = false;
                //Center
                double distance = Math.Sqrt((CenterX - i) * (CenterX - i) + (CenterY - j) * (CenterY - j));
                if (distance <= Radio) ZoneInside[i, j] = true;
                else ZoneInside[i, j] = false;

            }

    }

    public async Task Reset()
    {
        AcumIteration = 0;
        for (int i = 0; i < MaxSize; i++)
            for (int j = 0; j < MaxSize; j++)
            {
                Zone[i, j] = false;
            }
        Inside = 0;
        Outside = 0;
        PiCalcAcum = 0;
        PiErrorAcum = 0;

        Data.Clear();


        await Task.Delay(10);
        this.StateHasChanged();

    }

    public async Task Pi_MonteCarloRun()
    {
        if (IsRunning) return;
        IsRunning = true;
        await Task.Delay(10);
        this.StateHasChanged();
        Random rnd = new Random();
        double x, y;

        int CycleInside = 0;
        int CycleOutside = 0;
        double PiCalc = 0;
        double PiError = 0;
        Iteration = 0;

        for (int nCycles = 0; nCycles < Cycles; nCycles++)
        {

            for (Iteration = 0; Iteration < MaxIterations; Iteration++)
            {
                x = rnd.NextDouble() * 2 - 1;
                y = rnd.NextDouble() * 2 - 1;

                if (x * x + y * y < 1) Inside++;
                else Outside++;
                Zone[(int)(Math.Abs(x) * MaxSize), (int)(Math.Abs(y) * MaxSize)] = true;

            }
            AcumIteration += MaxIterations;

            Inside += CycleInside;
            Outside += CycleOutside;
            PiCalc = 4 * (double)Inside / (Double)(Inside + Outside);
            PiError = (PiCalc - Math.PI) / Math.PI * 100;

            PiCalcAcum = 4 * (double)Inside / (Double)(Inside + Outside);
            PiErrorAcum = (PiCalcAcum - Math.PI) / Math.PI * 100;
            Data.Add(new CycleData(Inside, Outside, PiCalc, PiError, PiCalcAcum, PiErrorAcum));
            await Task.Delay(10);
            this.StateHasChanged();
        }



        IsRunning = false;
        await Task.Delay(10);
        this.StateHasChanged();
    }
}