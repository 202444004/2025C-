using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week04Homework
{
    //object 클래스를 상속한 Department 클래스
    class Department // : Object 생략되어 있음
    {
        //primary key
        public string Code;
        public string Name;
        //상속
        public override string ToString()
        {
            return $"[{Code}] {Name}";
        }
    }
}
