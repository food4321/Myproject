using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public class BarCodePro : Reader
    {
        BarCodeHook hook = null;
        public override void Start()
        {
            if (hook == null)
            {
                hook = new BarCodeHook();

                hook.BarCodeEvent += (e) =>
                {
                    FireReadData(new ReaderEvertArgs()
                    {
                        Data = e.BarCode,
                        ID = "ID"
                    });
                    //  e.Handled = false;
                };
                //hook.InstallHook();
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
