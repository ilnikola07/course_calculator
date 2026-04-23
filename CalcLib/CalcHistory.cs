using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    public class CalcHistory
    {
        private List<string> history = new List<string>();

        public void Add(string expression, double result)
        {
            history.Add($"{expression} = {result}");
        }

        public List<string> GetAll() => history;
    }
}
