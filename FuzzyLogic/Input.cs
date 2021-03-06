﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic
{
    public abstract class Input
    {

        protected Rule[] rl = null;
        protected Function[] func = null;
        public int PARAMS_CNT = -1;
        public int FUNC_CNT = -1;

        public abstract Rule[] makeRules(double[][] arr);

        public abstract void initFunc(Function[] func);

        public Function[] getFunctions()
        {
            return func;
        }
    }
}
