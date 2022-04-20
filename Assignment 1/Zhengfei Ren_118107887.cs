// ----Nmae : Zhengfei Ren--
// ----Student Number : 118107887--
// ----For begin the program : Directly using a public method run() to run the whole project--

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cellular_Automata
{
    class CA
    {
        private uint current = 1<<16;            // Set current defualted by 
        private uint rule = 30;                  // Set rule
        private uint steps = 30;                 // Set steps
        private int seed = 0;                    // Set a seed for the random initial value
        public void set_rule()                   // Initialize the rule
        {
            uint result;
            Console.WriteLine("Please enter the rule: any number between 0 and 255");
            String input = Console.ReadLine();
            bool a = uint.TryParse(input, out result);
            if (a == true)
                rule = result;
            else
            {
                Console.WriteLine("invalid input");
                Console.ReadLine();
            }
            rule = result;
        }
        public void set_current()                 // Initialize the current sequence
        {
            Console.WriteLine("Please enter the number of initialization: 0 for random, 1 for a single non zero entry in the middle");
            string input = Console.ReadLine();
            if (input == "0")
            {
                Random random = new Random(seed);
                Random rd = random;
                current = (uint)rd.Next();
            }
            else if (input == "1")
            { 
                // using the default current which has been difined above
            }
            else
            {
                Console.WriteLine("invalid input");
            }
        }

        public void set_steps()                     // Initialize the running steps
        {
            uint result;
            Console.WriteLine("Please enter the number of steps: any number between 0 and 200");
            String input = Console.ReadLine();
            bool a = uint.TryParse(input, out result);
            if (a == true)
                steps = result;
            else
            {
                Console.WriteLine("invalid input");
                Console.ReadLine();
            }
        }
        public void initiailize()                       //intitialize the rule,steps and the first sequence interactively
        {
            set_rule();
            set_current();
            set_steps();
        }
        public void OutputCurrentSequence(uint value)                   // Output the current step
        {
            int i = 0;
            uint mask = 1, res = 0;
            mask = mask << 31;
            for (i = 0; i < 32; i++)
            {
                res = value & mask;
                if (res == 0)
                    Console.Write('0');
                else
                    Console.Write('1');
                mask = mask >> 1;
            }
            Console.Write('\n');
        }

        public void ShowRule(uint r)                                          // Output the rule on the screen
        {
            uint mask = 1, res = 0;
            Console.WriteLine("Your entered rule:");
            res = r & mask;
            Console.WriteLine("(0,0,0)->{0}", res);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(0,0,1)->{0}", res / mask);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(0,1,0)->{0}", res / mask);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(0,1,1)->{0}", res / mask);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(1,0,0)->{0}", res / mask);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(1,0,1)->{0}", res / mask);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(1,1,0)->{0}", res / mask);
            mask = mask << 1;
            res = r & mask;
            Console.WriteLine("(1,1,1)->{0}", res / mask);
        }


        public uint GetSourceBits(uint k)                                   // Get Three bits from current sequence as source to create new sequence each time
        {
            uint res = 0;
            if (k == 0)                                                    // Get three bits for generating a bit at the first position in a new sequence
            {
                uint mask1 = 1;
                uint mask2 = 3;
                mask2 = mask2 << 30;
                res = ((current & mask1) <<2) + ((current & mask2) >> 30);      // Get three bits for generating a bit at the last position in a new sequence
            }
            else if (k == 31)
            {
                uint mask1 = 1;
                uint mask2 = 3;
                mask2 = mask2 << 30;
                res = ((current & mask1) << 2) + ((current & mask2) >> 30);      
            }
            else                                                                 // Get three consecutive bits for generating a bit at other psitions
            {
                uint mask = 7;
                int position = (int)(30 - k);
                mask = mask << position;
                res = current & mask;
                res = res >> position;
            }
            return res;
        }

        public uint GetBit(uint value)                                // Generate one bit for new sequence with the rule and current sequence each time
        {
            uint res = 0, mask = 1;
            int position = (int)value;
            mask = mask << position;
            res = rule & mask;
            res = res >> position;
            return res;
        }                                         

        public uint CreateNewSequence()                               // Generate a whole new sequence with methods GetSourceBits and Getbit 
        {
            uint res = 0, sum = 0 , tep = 0;
            for (uint k=0;k<32;k++)
            {
                // Firstly, At each position k, getting three consecutive bits from current sequence.
                tep = GetSourceBits(k);
                // Secondly, Generate relative one bit from the three bits secondly
                res = GetBit(tep);
                // Lastly, sum up the bits to generate the numeric value of whole new sequence
                sum += res * (uint)Math.Pow(2, (31 - k));           
            }
            return sum;
        }

        public void run()
        {
            initiailize();
            ShowRule(rule);
            for (int i = 0; i < steps; i++)
            {
                // Output current step
                OutputCurrentSequence(current);
                // update current value
                this.current = CreateNewSequence();
            }
            // Write some tips on screen
            for (int i = 0; i < 32; i++)
            {
                Console.Write("-");
            }
            Console.Write("\r\n");
            Console.WriteLine("Press any word and enter to quit");
            Console.ReadLine();
        }
    }
}
