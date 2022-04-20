using System;
using System.IO;

// Name : Zhengfei Ren
// ID : 118107887


namespace Exam2020
{
    class Program
    {
        static void Main(string[] args)
        {
            // predator prey simulation
            //(1) single value
            PredPrey p = new PredPrey();
            p.Nsettle = 1000;
            Vector v0 = new Vector(new double[] { 0.83, 0.55 });
            p.Delta = 1.38;  //this uses the current value of Delta.
            p.run1sim(v0, "c:\\users\\km\\outfile.csv");

            //(2) produce bifurcation plot data use default values
            p.runsimDrange(v0, 1.26, 1.4, 1000, "c:\\users\\km\\outfile1.csv");

            //(3) produce second bifurcation plot
            p.R = 3;
            p.B = 3.5;
            p.D = 2;
            v0 = new Vector(new double[]{ 0.57, 0.37 });
            p.runsimDrange(v0, 0.5, 0.95, 1000, "c:\\users\\km\\outfile2.csv");
            
        }
    }
    class Vector
    {
        private double[] values;

        public double[] Values
        {
            get { return values; }
            set { values = value; }
        }
        public Vector()        
        {
            values = new double[2];
        }

        public Vector(int size)         
        {
            if (size <= 0)
            {
                size = 2;         // Default to 2 if input is erroneous
            }
            values = new double[size];
        }

        public Vector(double[] values)         
        {
            this.values = new double[values.Length];     // Size the array of delegates according to the size of functions
            for (int i = 0; i < values.Length; i++)
            {
                this.values[i] = values[i];
            }
        }

        public void setSize(int size)
        {
            if(size < this.values.Length)          // Resizes the array if necessary
            {
                double[] tmp = new double[size];
                for (int i = 0; i < size; i++)
                {
                    tmp[i] = values[i];
                }
                values = tmp;
            }
            else
            {
                // Don't need to resize the arry if the size is greater than the original length
            }
        }

        public static Vector operator +(Vector lhs, Vector rhs)     // Overload + for addiction between two Vectors
        {
            if (lhs.Values.Length != rhs.Values.Length)
            {
                throw new Exception("The sizes of two vectirs used do not match");      // Exception situation
            }
            int size = lhs.Values.Length;
            Vector TmpVector = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                TmpVector[i] = lhs[i] + rhs[i];
            }
            return TmpVector;
        }

        public static Vector operator -(Vector lhs, Vector rhs)   // Overload - for subtraction between two Vectors
        {
            if (lhs.Values.Length != rhs.Values.Length)
            {
                throw new Exception("The sizes of two vectors do not match");        // Exception situation
            }
            int size = lhs.Values.Length;
            Vector TmpVector = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                TmpVector[i] = lhs[i] - rhs[i];
            }
            return TmpVector;
        }

        public static Vector operator *(double lhs, Vector rhs)   // Overload the multiplication operator used between one double value and a vector
        {
            int size = rhs.Values.Length;
            Vector TmpVector = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                TmpVector[i] = rhs.Values[i] * lhs;
            }
            return TmpVector;
        }

        public static Vector operator *(Vector lhs, double rhs)   // Overload the multiplication operator used between a vector and a double value
        {
            int size = lhs.Values.Length;
            Vector TmpVector = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                TmpVector[i] = lhs.Values[i] * rhs;
            }
            return TmpVector;
        }

        public double this[int index]     // An indexer function to allow users to access or set elements of the Vectors
        {
            get
            {
                if (index > values.Length)
                {
                    throw new Exception("Index exceeds vector size");      // Exception situation
                }
                return values[index];
            }
            set 
            {
                if (index > values.Length)
                {
                    throw new Exception("Index exceeds vector size");      // Exception situation
                }
                values[index] = value; 
            }
        }

        public override string ToString()     //Returns a string representation of the vector for the output
        {
            // string for output
            string tmp = "";
            tmp += '{';
            int size = values.Length;
            for (int i = 0; i < size - 1; i++)
            {
                tmp += values[i].ToString() + ",";
            }
            // Last piece of data in vector
            tmp += values[size - 1].ToString();
            tmp += '}';
            return tmp;
        }
    }

    //  A delegate which can reference methods that take a vector as an argument and returns a double
    delegate double Function(Vector v);

    class FunctionVector
    {
        // The array of delegates of type Function
        private Function[] functionVector;

        public FunctionVector()        
        {
            functionVector = new Function[2];
            for (int i = 0; i < 2; i++)
            {
                functionVector[i] = v => 0;
            }
        }
        public FunctionVector(int size)
        {
            if (size <= 0)           // Default to 2 if input is erroneous
            {
                size = 2;
            }
            functionVector = new Function[size];
            for (int i = 0; i < size; i++)
            {
                functionVector[i] = v => 0;
            }
        }

        public FunctionVector(Function[] functions)
        {
            int size = functions.Length;
            functionVector = new Function[size];
            for (int i = 0; i < size; i++)
            {
                functionVector[i] = functions[i];
            }
        }

        public Vector Evaluate(Vector values)
        {
            int size = values.Values.Length;
            // A vector of doubles containing the result of the evaluation.
            Vector tmp = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                // Evaluate the methods in function vector at the vector values
                tmp[i] = functionVector[i](values);
            }
            return tmp;
        }

        public Function this[int index]
        {
            get
            {
                if (index > functionVector.Length)
                {
                    throw new Exception("Index exceeds vector size");      // Exception situation
                }
                return functionVector[index];
            }
            set
            {
                if (index > functionVector.Length)
                {
                    throw new Exception("Index exceeds vector size");      // Exception situation
                }
                functionVector[index] = value;
            }
        }
    }

    class PredPrey
    {
        // Vector of delegates of type Function
        private FunctionVector fv;
        // Parameters
        private double delta = 0.5, r = 2, b = 0.6, d = 0.5;
        private int nsettle = 200;
        private int nreps = 200;

        // Propertis for private data
        public double Delta
        {
            get { return delta; }
            set { delta = value; }
        }
        public double R
        {
            get { return r; }
            set { r = value; }
        }
        public double B
        {
            get { return b; }
            set { b = value; }
        }
        public double D
        {
            get { return d; }
            set { d = value; }
        }
        public int Nsettle
        {
            get { return nsettle; }
            set 
            {
                // The minimum value is 10
                nsettle = value < 10 ? 10 : value; 
            }
        }
        public int Nreps
        {
            get { return nreps; }
            set
            {
                // The minimum value is 10
                nreps = value < 10 ? 10 : value;
            }
        }
      
        public PredPrey()
        {
            // Construct the prey and predator system through the lambda expressions
            fv = new FunctionVector(new Function[2] { x => r * x[0] * (1 - x[0]) - b * x[0] * x[1], x => (-d + b * x[0]) * x[1] });
        }

        public void runsimDrange(Vector v0, double deltafrom, double deltato, int numsteps, string filename)
        {
            // The span between deltas
            double deltaspan = (deltato - deltafrom) / (numsteps-1);
            // Simulation for values of delta ranging from deltafrom to deltato in numsteps
            for (double i = deltafrom; i < deltato + deltaspan; i += deltaspan)
            {
                delta = i; 
                run1sim(v0, filename);
            }
        }

        public void run1sim(Vector v0, string filename)
        {
            // Running nsettle times for settling down 
            for (int i = 0; i < nsettle; i++)
            {
                v0 = v0 + this.delta * fv.Evaluate(v0);
            }

            // Prepare the writing stream for writing data
            FileStream f = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(f);

            try
            {
                for (int i = 0; i < nreps; i++)
                {
                    // Iterates the equations for nreps times
                    v0 = v0 + delta * fv.Evaluate(v0);
                    // String to be written
                    string tmp = "";
                    tmp += delta.ToString() + ',' + v0[0].ToString() + ',' + v0[1].ToString();
                    sw.WriteLine(tmp);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {0}", e.Message);
            }
            finally
            {
                if (f != null)
                {
                    sw.Close();
                    f.Close();
                }   
            }
        }
    }

}
