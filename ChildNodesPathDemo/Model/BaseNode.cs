using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildNodesPathDemo.Model
{
    public abstract class BaseNode
    {
        private string _OrderNr;
        public string OrderNr { get => _OrderNr; set => _OrderNr = value; }

        private string _Description;
        public string Description { get => _Description; set => _Description = value; }
    }
}
