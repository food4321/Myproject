using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public class ReaderData
    {
        public string ID { get; set; }
        public object Object { get; set; }
        public string Data { get; set; }
        public Hashtable HtParam { get; set; }
    }
}
