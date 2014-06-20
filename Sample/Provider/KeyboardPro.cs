using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public class KeyboardPro : Reader
    {
        Hook hook = null;
        public override void Start()
        {
            if (hook == null)
            {
                hook = new Hook();

                hook.KeyUp += (o, e) =>
                {
                    FireReadData(new ReaderEvertArgs()
                                    {
                                        Data = Convert.ToString((char)e.KeyValue),
                                        ID = "ID"
                                    });
                    e.Handled = false;
                };
            }
        }

        public override void Stop()
        {
            if (hook != null)
            {
                hook.UnInstallHook();
                hook = null;
            }
        }


    }
}
