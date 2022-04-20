using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Name Zhengfei Ren
// Student Number 118107887


namespace Assignment_3
{
    class Program
    {
        static void Main(string[] args)
        {
            RungeKuta RK = new RungeKuta();
            // Select a model to be deal with
            RK.FcuntionSelect();
            // Initialize the parameters
            RK.Initialize();
            // Use RungeKuta to solve the model
            RK.Solve(RK.FunctionChoice);
            // Write the result
            RK.WriteCsv("../../../Results.csv");
        }
    }
    class FunctionStore
    {
        public static Vector SIR(double t, Vector pams)
        {
            // parameters for COVID case,gamma is equal to 1/14 = 0.0714
            double gamma = 0.0714;
            // parameters for COVID case:bata = R0 * gamma = 2.4 * gamma = 0.1714
            double beta = 0.1714;
            // Get S, I ,R
            double S = pams.GetData()[0];
            double I = pams.GetData()[1];
            double R = pams.GetData()[2];
            // SIR model
            double Shat = -beta * I * S;
            double Ihat = beta * I * S - gamma * I;
            double Rhat = gamma * I;
            // Output the results to a vector for each interation
            List<double> TmpArray = new List<double>();
            TmpArray.Add(Shat);
            TmpArray.Add(Ihat);
            TmpArray.Add(Rhat);
            Vector TmpVector = new Vector(TmpArray);
            return TmpVector;
        }

        public static Vector Logistic(double t, Vector pams)
        {
            // Initial value of y
            double y = pams.GetData()[0];
            double r = 1;
            double K = 1;
            List<double> TmpArray = new List<double>();
            TmpArray.Add(r * (1 - y / K) * y);
            Vector TmpVector = new Vector(TmpArray);
            return TmpVector;
        }
    }

    class Vector
    {
        // Use List generic to store model variables to adapt to a system with mulitible equations
        private List<double> data = new List<double>();

        public Vector()
        {
            // Default constructor with no parameters
        }
        public Vector(double data)
        {
            // Default constructor with double type parameter
            this.data.Add(data);
        }
        public Vector(List<double> arr)
        {
            // Default constructor with ArrayList type parameter
            for (int i = 0; i < arr.Count; i++)
            {
                data.Add(arr[i]);
            }
        }
        public List<double> GetData()
        {
            // Get whole data
            return data;
        }
        public int GetLength()
        {
            // Get the length
            return data.Count;
        }
        public static Vector operator +(Vector left, Vector right)
        {
            // Overload the addition operator used for addiction between two vectors
            int len = left.data.Count;
            List<double> tmp = new List<double>();
            for (int i = 0; i < len; i++)
            {
                tmp.Add(left.data[i] + right.data[i]);
            }
            Vector tmpvector = new Vector(tmp);
            return tmpvector;
        }
        public static Vector operator *(double left, Vector right)
        {
            // Overload the multiplication operator used between one double value and a vector
            int len = right.data.Count;
            List<double> tmp = new List<double>();
            for (int i = 0; i < len; i++)
            {
                tmp.Add(right.data[i] * left);
            }
            Vector tmpvector = new Vector(tmp);
            return tmpvector;
        }
        public void output()                       // An output method for checking a particular answer
        {
            for (int i = 0; i < data.Count; i++)
            {
                Console.WriteLine(data[i]);
            }
            Console.WriteLine();
        }
    }

    class RungeKuta
    {
        // Use delegate which references to a given model
        public delegate Vector func(double t, Vector pams);

        public double stepsize = 0.01;
        public int numsteps = 500;
        // initial value of t
        public double t0 = 0;
        // Store the variables representing continuous time slots
        public double[] tvals = new double[500];
        // Store the variables representing the outputs of a given model of all interation
        public Vector[] pvals = new Vector[500];
        // A list generic consisting of initial variables from a given model for constructing the initial vector
        public List<double> y0 = new List<double>();
        // The function selected
        public func FunctionChoice = FunctionStore.SIR;

        public void FcuntionSelect()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------- RungeKuta Method ---------------------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Please selecte a model,type 0 for SIR,1 for logistic, 2 for other models");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                String output = Console.ReadLine();
                if (output == "0")                // Selecte the SIR model
                {
                    FunctionChoice = FunctionStore.SIR;
                    break;
                }
                if (output == "1")                // Selecte the Logistic model
                {
                    FunctionChoice = FunctionStore.Logistic;
                    break;
                }
                else                              // Need to define the model for other models
                {
                    Console.WriteLine("Didn't find that model,please define the model in the FunctionStore method");

                    Console.WriteLine("\nPlease selecte a model,type 0 for SIR,1 for logistic, 2 for other models");
                }
            }
        }

        public void Initialize()          // Initialize the value of variables/parameters interavtively
        {
            // Transform the input to a double type data result
            double result;
            // Output from the input by users
            string tmp;
            Console.WriteLine("---------------------------  Initialization  ---------------------------");

            // When the given model is the SIR model
            if (FunctionChoice == FunctionStore.SIR)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nPlease specify the begining value of variables for the SIR model:\n");
                Console.ForegroundColor = ConsoleColor.White;
                // Three variables need to be defined in the SIR model
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                        Console.WriteLine("The begining value of S:");
                    if (i == 1)
                        Console.WriteLine("The begining value of I:");
                    if (i == 2)
                        Console.WriteLine("The begining value of R:");
                    tmp = Console.ReadLine();

                    bool par = double.TryParse(tmp, out result);
                    if (par == true)
                    {
                        // Add the initial variables from input to the variavles list y0
                        y0.Add(result);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("invalid input");
                        Console.ForegroundColor = ConsoleColor.White;
                        Environment.Exit(0);
                    }
                }
            }
            // When the given model is the Logistic model
            if (FunctionChoice == FunctionStore.Logistic)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("\nPlease specify the begining value of variables for the Logistic model:\n");
                Console.ForegroundColor = ConsoleColor.White;

                // One variable need to be specified in the Logistic model
                for (int i = 0; i < 1; i++)
                {
                    Console.WriteLine("The begining value of y: ");
                    tmp = Console.ReadLine();
                    bool par = double.TryParse(tmp, out result);
                    if (par == true)
                    {
                        // Add the initial variables from input to the variavles list y0
                        y0.Add(result);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("invalid input");
                        Console.ForegroundColor = ConsoleColor.White;
                        Environment.Exit(0);
                    }
                }
            }
        }

        public void Solve(func f)         // Interation for the Rungekuta method
        {
            // define the time variable for interation
            double ti;
            // Use the a list generic containg n initial variables to construct the initial vector for the interation
            Vector yi = new Vector(y0);
            // Plug in the initial value of t
            tvals[0] = ti = t0;
            // Plug in the initial value of variables
            pvals[0] = yi;

            // Implement the Runge-Kutta method
            for (int i = 1; i < numsteps; i++)
            {
                Vector k1 = f(ti, yi);
                Vector k2 = f(ti + stepsize / 2, yi + stepsize * 0.5 * k1);
                Vector k3 = f(ti + stepsize / 2, yi + stepsize * 0.5 * k2);
                Vector k4 = f(ti + stepsize, yi + stepsize * k3);
                yi = yi + stepsize * 1 / 6 * (k1 + k2 + k3 + k4);
                ti += stepsize;
                // Store the outputs
                tvals[i] = ti;
                pvals[i] = yi;
            }
        }

        public void WriteCsv(string filename)
        {
            // Defile file stream for writing data
            FileStream f = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(f);
            try
            {
                // Add a head line in the csv file
                string dataHeard = string.Empty;
                if (FunctionChoice == FunctionStore.SIR)
                    dataHeard = "t,S,I,R";
                if (FunctionChoice == FunctionStore.Logistic)
                    dataHeard = "t,y";
                sw.WriteLine(dataHeard);

                for (int i = 0; i < numsteps; i++)
                {
                    // Write a row to the csv file consisting of the variables in each interation
                    string datastr = string.Empty;
                    datastr = datastr + tvals[i] + ",";
                    for (int j = 0; j < pvals[i].GetLength(); j++)
                    {
                        if (j < pvals[i].GetLength() - 1)
                            // use comma to seperate the variavles in each row
                            datastr += pvals[i].GetData()[j].ToString() + ",";
                        else
                            // No comma in after the last piece of data
                            datastr += pvals[i].GetData()[j].ToString();
                    }
                    // Write a row in csv file
                    sw.WriteLine(datastr);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Error {0}", e.Message);
            }
            finally
            {
                if (f != null)
                {
                    // Close the file stream and writing stream
                    sw.Close();
                    f.Close();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nWrite csv file successfully at '.. / .. / .. / Results.csv '!");
                    Console.WriteLine("\n---------------------------  Program End  ---------------------------");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                }
            }
        }
    }
}
