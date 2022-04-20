using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Name Zhengfei Ren
// Student Number 118107887

namespace Assignment_4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Matrix m = new Matrix(4);
                Vector b = new Vector(4);
                m[0, 0] = 9; m[0, 1] = -2; m[0, 2] = 3; m[0, 3] = 2;
                m[1, 0] = 2; m[1, 1] = 8; m[1, 2] = -2; m[1, 3] = 3;
                m[2, 0] = -3; m[2, 1] = 2;  m[2, 2] = 11; m[2, 3] = -4;
                m[3, 0] = -2; m[3, 1] = 3; m[3, 2] = 2; m[3, 3] = 10;
                b[0] = 54.5; b[1] = -14; b[2] = 12.5; b[3] = -21;
                Console.WriteLine("The matrix m is {0}", m);
                Console.WriteLine("The vector b is {0}", b);
                LinSolve l = new LinSolve();
                Vector ans = l.Solve(m, b);
                Console.WriteLine("The solution to m x = b is {0}", ans);

            }
            catch(Exception e)
            {
                Console.WriteLine("Error eccountered: {0}",e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
    class Matrix
    {
        // Store the matrix data in a suitably sized 2D array.
        public double[,] m_data;

        public Matrix()           //  A Default constuctor which sets all values to 0
        {
            m_data = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    m_data[i, j] = 0;
                }
            }
        }

        public Matrix(int size)         //  A default constuctor with specified size with 0 values
        {
            // Check if  the size is valid
            if (size <= 1)
            {
                throw new Exception("The size set for the matrix is invalid");
            }
            m_data = new double[size, size];
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    m_data[i, j] = 0;
                }
            }
            
        }

        public static Matrix operator +(Matrix lhs, Matrix rhs)        // Overload + for addiction between two matirces
        {
            // Check if  the matrices are of the same size
            if (lhs.m_data.GetLength(0) != rhs.m_data.GetLength(0))
            {
                throw new Exception("The size of two matrix used for addiction do not match");
            }
            // The size of the matrix
            int size = lhs.m_data.GetLength(0);
            Matrix tmp_matrix = new Matrix(size);
            double tmp;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tmp_matrix[i, j] = lhs[i,j]+rhs[i,j];
                }
            }
            return tmp_matrix;
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)        // Overload * for multiplication between two matirces
        {
            // Check if  the matrices are of the same size
            if (lhs.m_data.GetLength(0) != rhs.m_data.GetLength(0))
            {
                throw new Exception("The size of two matrix used for multiplication do not match");
            }
            // The size of the matrix
            int size = lhs.m_data.GetLength(0);
            Matrix tmp_matrix = new Matrix(size);
            double tmp;
            // Implement the matrix multiplication
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    tmp = 0;
                    for (int k = 0; k < size; k++)
                    {
                        tmp += lhs[i, k] * rhs[k, j];
                    }
                    tmp_matrix[i, j] = tmp;
                }
            }
            return tmp_matrix;
        }

        public static Vector operator *(Matrix lhs, Vector rhs)      // Overload * for multiplication between a matirce and vector
        {
            // Check if  the matrices are of the same size
            if (lhs.m_data.GetLength(0) != rhs.v_data.GetLength(0))
            {
                throw new Exception("The size of two matrix used for multiplication do not match");
            }
            // The size of the matrix which is also the size of vector
            int size = lhs.m_data.GetLength(0);
            Vector tmp_vector = new Vector(size);
            double tmp;
            // Implement the matrix multiplication
            for (int i = 0; i < size; i++)
            {
                tmp = 0;
                for (int j = 0; j < size; j++)
                {
                    tmp += lhs[i, j] * rhs[j];
                }
                tmp_vector[i] = tmp;
            }
            return tmp_vector;
        }

        public double this[int row, int col]     // An indexer function to allow users to access or set elements of the matrix
        {
            get 
            {
                // Check if  the index is out of the range
                if (row > m_data.GetLength(0) || col > m_data.GetLength(0))
                {
                    throw new Exception("Index exceeds matrix size");
                }
                return m_data[row, col]; 
            }
            set { m_data[row, col] = value; }
        }

        public override string ToString()        // Returns a string representation of the matrix for use in Console.WriteLine
        {
            // string for output
            string tmp = "";
            tmp += '\n';
            int size = m_data.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                tmp += '{'; 
                for (int j = 0; j < size-1; j++)
                {
                    // Sepetate the data with a comma and a space
                    tmp += m_data[i, j] + ", ";
                }
                // The last piece of data of a row in matrix
                tmp += m_data[i, size-1];
                tmp += '}';
                tmp += '\n';
            }
            return tmp;
        }

    }

    class Vector
    {
        public double[] v_data;

        public Vector()        //  Default constuctor which sets all values to 0
        {
            v_data = new double[3];
         
            for (int i = 0; i < 3; i++)
            {
                v_data[i] = 0;
            }
        }

        public Vector(int size)         //  A default constuctor with specified size
        {
            v_data = new double[size];
            if (size <= 1)
            {
                throw new Exception("The size set for the vector is invalid");
            }
            for (int i = 0; i < size; i++)
            {
                v_data[i] = 0;
            }
        }

        static public Vector operator +(Vector a, Vector b)     // Overload + for addiction between two Vectors
        {
            if (a.v_data.GetLength(0) != b.v_data.GetLength(0))
            {
                throw new Exception("The size of two matrix used for addiction do not match");
            }
            int size = a.v_data.GetLength(0);
            Vector tmp_vector = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                tmp_vector[i] = a[i]+b[i];
            }
            return tmp_vector;
        }

        static public Vector operator -(Vector a, Vector b)   // Overload - for subtraction between two Vectors
        {
            if (a.v_data.GetLength(0) != b.v_data.GetLength(0) )
            {
                throw new Exception("The size of two matrix used for subtraction do not match");
            }
            int size = a.v_data.GetLength(0);
            Vector tmp_vector = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                tmp_vector[i] = a[i] + b[i];
            }
            return tmp_vector;
        }

        public double this[int index]     // An indexer function to allow users to access or set elements of the Vectors
        {
            get
            {
                if (index > v_data.GetLength(0))
                {
                    throw new Exception("Index exceeds vector size");
                }
                return v_data[index];
            }
            set { v_data[index] = value; }
        }

        public double Norm()        // The abosolute value of a vector
        {
            double tmp = 0;
            for (int i = 0; i < v_data.GetLength(0); i++)
            {
                tmp += v_data[i]* v_data[i];
            }
            tmp = Math.Sqrt(tmp);
            return tmp;
        }

        public override string ToString()     //Returns a string representation of the vector for use in Console.WriteLine.
        {
            // string for output
            string tmp = "";
            // Formatted string with 3 decimal places
            string tmpformat;
            tmp += '{';
            int size = v_data.GetLength(0);
            for (int i = 0; i < size-1; i++)
            {
                // keep 3 decimal places
                tmpformat = string.Format("{0:F3}", v_data[i]);
                // Seperate the data in vector
                tmp += tmpformat + ", ";
            }
            // Last piece of data in vector
            tmpformat = string.Format("{0:F3}", v_data[size - 1]);
            tmp += tmpformat;
            tmp += '}';
            return tmp;
        }

    }

    delegate double Function(Vector v);

    class FunctionVector
    {
        private Function[] functionVector;

        public FunctionVector()
        {
            functionVector = new Function[2];
            for (int i = 0;i < 2;i++)
            {
                functionVector[i] = new Function((Vector v) =>{return 0;});
            }

        }
        public FunctionVector(int size)
        {
            functionVector = new Function[size];
            if (size <= 0)
                size = 2;
            for (int i = 0; i < size; i++)
            {
                functionVector[i] = new Function((Vector v) => { return 0; });
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
            int size = values.v_data.Length;
            Vector result = new Vector(size);
            for (int i = 0; i < size; i++)
            {
                result[i] = functionVector[i](values);
            }
            return result;
        }

        public Function this[int index]
        {
            get
            {
                return functionVector[index];
            }
            set 
            { 
                functionVector[index] = value; 
            }
        }
    }


    class LinSolve
    {
        // Maximum loops
        private int maxiters = 100;
        // Tolerance
        private double tolerance = 1e-7;

        public Vector Solve(Matrix A, Vector b)        // Gauss Jacobi Method
        {
            // Size of matrice
            int size = A.m_data.GetLength(0);
            // Matrix D
            Matrix D = new Matrix(size);
            for (int i = 0; i < size; i++)
            {
                D[i, i] = A[i, i];
            }
            // The inverse of D
            Matrix DInverse = new Matrix(size);
            for(int i = 0; i < size; i++)
            {
                DInverse[i, i] = 1 / A[i, i];
            }
            // Matrix L
            Matrix L = new Matrix(size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i > j)
                    {
                        L[i, j] = -A[i, j];
                    }
                }
            }
            // Matrix U
            Matrix U = new Matrix(size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i < j)
                    {
                        U[i, j] = -A[i, j];
                    }
                }
            }
            // Get Matrix T and Vector c with matrices above
            Matrix T = DInverse * (L + U);
            Vector c = DInverse * b;
            // Vector xk and xk1 for the interation
            Vector xk = new Vector(size);
            Vector xk1 = new Vector(size);  
            // Interation with stopping condition
            for(int i = 0; i < maxiters; i++)
            {
                xk1 = T * xk + c;
                if ((xk1 - xk).Norm()/xk.Norm()<tolerance)
                {
                    break;
                }
                xk = xk1;
            }
            return xk1;
        }
    }
}
