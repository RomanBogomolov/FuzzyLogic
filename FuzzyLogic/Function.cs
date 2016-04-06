using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public abstract class Function
    {
        public String ID;

        public abstract double getY(double x);
    }
}
