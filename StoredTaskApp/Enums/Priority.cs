using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoredTaskApp.Enums
{
    // Priority holds thew following priority values -1 (Low), 0 (Normal) & 1 (High)
    // It has two methods
    //      ++ (To increment priority by 1)
    //      -- (To decrement priority by 1)

    public struct Priority
    {
        public int Value;

        public Priority(int value)
        {
            //Set value if within the range, if not, then set default value = 0 (Normal)
            if (value >= -1 && value <= 1)
            {
                Value = value; //Set passed value if within range
            }
            else
            {
                Value = 0; //Default value 0 -- Normal
            }
        }

        public static Priority operator ++(Priority a)
        {
            if (a.Value < 1)
            {
                a.Value++;
            }
            return a;
        }
        public static Priority operator --(Priority a)
        {
            if (a.Value > -1)
            {
                a.Value--;
            }
            return a;
        }

        public string Display_Priority()
        {
            string result = string.Empty;
            switch (Value)
            {
                case 0:
                    result = "Normal Priority";
                    break;
                case 1:
                    result = "High Priority";
                    break;
                case -1:
                    result = "Low Priority";
                    break;
                default:  //Shouldn't get here as it's already been handled in the ++ & -- overrided methods
                    result = "Priority value out of range";
                    break;
            }
            return result;
        }
    }
}
