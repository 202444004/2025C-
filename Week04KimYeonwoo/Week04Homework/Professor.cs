using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week04Homework
{
    class Professor
    {
        //foreign key
        public string DepartmentCode;
        public string Number;
        public string Name;

        public override string ToString()
        {
            return $"[{Number}]{Name}";
        }
    }
}
