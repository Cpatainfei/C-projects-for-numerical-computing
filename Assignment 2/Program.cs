using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.IO;
using System.Collections;

namespace Assignment_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Perceptron p = new Perceptron();
            // Read and store data
            p.ReadData("data.csv");
            // Training
            p.TrainData();
            // Output the summary of weights
            p.Output();
            // Avoid program exiting quickly
            Console.ReadLine();
        }
    }
    class Vector
    {
        // Data stucture ArrayList used to store data in Vector object
        private ArrayList data = new ArrayList();

        public Vector()                          // Default constructor
        {
        }
        public Vector(double data)               // Construct the Vector using data in double type
        {
            this.data.Add(data);
        }

        public Vector(double[] arr)              // Construct the Vector using an Array
        {
            for (int i = 0; i < arr.Length; i++)
            {
                data.Add(arr[i]);
            }
        }

        public Vector(ArrayList arr)             // Construct the Vector using an ArrayList
        {
            for (int i = 0; i < arr.Count; i++)
            {
                data.Add((double)arr[i]);
            }
        }

        public ArrayList GetData()               // Get the private data  
        {
            return data;
        }

        public static Vector operator +(Vector left, Vector right)      // Overload the addition operator 
        {

            int len = left.data.Count;
            double[] TempData = new double[len];
            for (int i = 0; i < len; i++)
            {
                TempData[i] = (double)left.data[i] + (double)right.data[i];
            }

            Vector tmp = new Vector(TempData);
            return tmp;
        }

        public static Vector operator -(Vector left, Vector right)        // Overload the suctraction operator
        {
            int len = left.data.Count;
            double[] TempData = new double[len];
            for (int i = 0; i < len; i++)
            {
                TempData[i] = (double)left.data[i] - (double)right.data[i];
            }
            Vector tmp = new Vector(TempData);
            return tmp;
        }

        public static double operator *(Vector left, Vector right)         // Overload the multiplication operator between two Vectors
        {
            int len = left.data.Count;
            double res = 0;
            for (int i = 0; i < len; i++)
            {
                res += (double)left.data[i] * (double)right.data[i];
            }
            return res;
        }

        public static Vector operator *(double left, Vector right)        // Overload the multiplication operator between double and Vector
        {
            int len = right.data.Count;
            for (int i = 0; i < len; i++)
            {
                right.data[i] = (double)right.data[i] * left;
            }
            return right;
        }

    }

    class Perceptron
    {
        // Data structure used for store the inputs, each input is a vector
        private ArrayList InputData = new ArrayList();
        // Data structure used for store the calculated outputs
        private ArrayList OutputData = new ArrayList();
        // Set learning rate
        private double LearnRate = 0.1;
        // Data structure uesd for store the Weights
        private ArrayList Weights = new ArrayList();
        // Epochs used for interation
        private int steps = 0;

        public void ReadData(string filename)             // Read Data from the data file and put them into relatie data structure
        {
            StreamReader sr = null;
            // String read from each row of the file
            string tmp = null;
            // String Seperator
            char[] charSeperators = new char[] { ',' };
            // An Array contains strings after seperating
            string[] StringOutput = null;
            // An datastucture used for storing the inputs of each row temporarily and initializing the input Vector latter
            ArrayList InputVector = new ArrayList();
            // double Changed from string
            double DoubleOutput = 0;
            // Check the header line
            bool IsFirst = true;
            // Check if the string can be changed to double 
            bool IsDouble = true;

            try
            {
                sr = new StreamReader(filename);
                do
                {
                    tmp = sr.ReadLine();
                    if (IsFirst == true)
                    {
                        // Check the header line
                        IsFirst = false;
                        continue;
                    }
                    // Stop reading at the end of the file
                    if (tmp == null)
                        break;
                    StringOutput = tmp.Split(charSeperators);
                    // Insert 1 into the Input Vector
                    InputVector.Add(1.0);
                    // put data read from file into relatie data structure
                    for (int i = 0; i < StringOutput.Length; i++)
                    {
                        // Skip the ID at the first column of file
                        if (i == 0)
                            continue;
                        // Change String type to Double type
                        IsDouble = double.TryParse(StringOutput[i], out DoubleOutput);
                        if (IsDouble == true)
                        {
                            // Adding data at the last column of file to OutputData arraylist
                            if (i == StringOutput.Length - 1)
                            {
                                OutputData.Add(DoubleOutput);
                            }
                            // Adding inputs to the InputData arraylist, which are at the other columns expcet the first and last column.
                            else
                            {
                                InputVector.Add(DoubleOutput);
                            }
                        }
                    }
                    // Create an instance of Vector object and initialize it with each set of inputs
                    Vector TempVector = new Vector(InputVector);
                    // Put each Vector into the Inputdata arraylist
                    InputData.Add(TempVector);
                    // Clear up the InputVector for initialize load the next input vector at next the row of file
                    InputVector.Clear();
                } while (true);
                for (int i = 0; i < StringOutput.Length - 1; i++)
                {
                    Weights.Add(0.0);
                }
            }
            // Catch Error
            catch (Exception e)
            {
                Console.Write("Error {0}", e.Message);
            }
            // Close the file
            finally
            {
                if (sr != null)
                    sr.Close();
            }

        }


        public void TrainData()
        {
            Console.WriteLine("Perceptron");
            Console.WriteLine("****************************");
            Console.WriteLine("Begin train");
            Console.WriteLine("****************************");
            // Set a random value for error to make it positive befor training
            int error = 1;
            // Change Weights to a Vector type in order to conduct dot product
            Vector Weight = new Vector(Weights);
            // Begin training process
            while (error > 0)
            {
                // Set error to be zero
                error = 0;
                for (int i = 0; i < InputData.Count; i++)
                {
                    // Difference between predictive output and the true output
                    double Difference = 0;
                    // Predicted Output
                    double yhat = 0;
                    // Calculate the forecast output  
                    yhat = (Vector)InputData[i] * Weight;
                    // Classify the output to be 0 or 1
                    double output = ClassifyPoint((double)yhat);
                    // Subtraction between predicted output and true output
                    Difference = (double)OutputData[i] - output;
                    if (Difference != 0)
                    {
                        // Updata Weight Vector
                        Weight = Weight + LearnRate * Difference * (Vector)InputData[i];
                        // Update Error
                        error += 1;
                    }
                }
                // Count the epoches used for interation
                steps += 1;
            }
            // Update the Weight to be the private data in Perceptron class
            Weights = Weight.GetData();
        }

        public int ClassifyPoint(double x)
        {
            if (x >= 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void Output()
        {
            Console.WriteLine("Total steps for training: {0}", steps);
            Console.WriteLine("Weights:");
            Console.WriteLine("{0,-12}{1,-6}", "Noise", Weights[0]);
            Console.WriteLine("{0,-12}{1,-6}", "RPM", Weights[1]);
            Console.WriteLine("{0,-12}{1,-6}", "VIBRATION", Weights[2]);
            Console.WriteLine("****************************");
        }
    }
}
