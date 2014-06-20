using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public abstract class Reader
    {
        
        public delegate void ReadDataEventHandler(ReaderEvertArgs e);

        public ProviderEnum ProviderType { get; set; }

        public abstract void Start();

        public event ReadDataEventHandler ReadData;

        public virtual void OnReadData(ReaderEvertArgs e)
        {

        }
        public void FireReadData(ReaderEvertArgs e)
        {
            if (ReadData != null)
                ReadData(e);
        }

        public abstract void Stop();
    }
}
