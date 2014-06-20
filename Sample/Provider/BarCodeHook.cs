using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Provider
{
    public class BarCodeHook : Hook
    {
        public delegate void BarCodeDelegate(BarCodes barCode);
        public event BarCodeDelegate BarCodeEvent;
        public struct BarCodes
        {
            public int VirtKey;      //虚拟码   
            public int ScanCode;     //扫描码   
            public string KeyName;   //键名   
            public uint AscII;       //AscII   
            public char Chr;         //字符  
            public string BarCode;   //条码信息   
            public bool IsValid;     //条码是否有效   
            public DateTime Time;    //扫描时间 
     
        }

        [DllImport("user32", EntryPoint = "GetKeyNameText")]
        private static extern int GetKeyNameText(int lParam, StringBuilder lpBuffer, int nSize);
        [DllImport("user32", EntryPoint = "GetKeyboardState")]
        private static extern int GetKeyboardState(byte[] pbKeyState);
        [DllImport("user32", EntryPoint = "ToAscii")]
        private static extern bool ToAscii(int VirtualKey, int ScanCode, byte[] lpKeyState, ref uint lpChar, int uFlags);

        BarCodes barCode = new BarCodes();

        string strBarCode = "";
        public override int HookProc(int nCode, Int32 wParam, ref KeyboardHookStruct lParam)
        {
            if (nCode == 0)
            {
                //EventMsg msg = (EventMsg)Marshal.PtrToStructure(lParam, typeof(EventMsg));
                if (wParam == 0x100)   //WM_KEYDOWN = 0x100   
                {
                    barCode.VirtKey = lParam.vkCode & 0xff; //虚拟码   
                    barCode.ScanCode = lParam.scanCode & 0xff; //扫描码  
                    StringBuilder strKeyName = new StringBuilder(255);
                    if (GetKeyNameText(barCode.ScanCode * 65536, strKeyName, 255) > 0)
                    {
                        barCode.KeyName = strKeyName.ToString().Trim(new char[] { ' ', '\0' });
                    }
                    else
                    {
                        barCode.KeyName = "";
                    }
                    byte[] kbArray = new byte[256];
                    uint uKey = 0;
                    GetKeyboardState(kbArray);
                    if (ToAscii(barCode.VirtKey, barCode.ScanCode, kbArray, ref uKey, 0))
                    {
                        barCode.AscII = uKey;
                        barCode.Chr = Convert.ToChar(uKey);
                    }
                    if (DateTime.Now.Subtract(barCode.Time).TotalMilliseconds > 50)
                    {
                        strBarCode = barCode.Chr.ToString();
                    }
                    else
                    {
                        if ((lParam.vkCode & 0xff) == 13 && strBarCode.Length > 3)   //回车   
                        {
                            barCode.BarCode = strBarCode;
                            barCode.IsValid = true;
                            if (BarCodeEvent != null) BarCodeEvent(barCode);    //触发事件   
                            return 1;
                        }
                        strBarCode += barCode.Chr.ToString();
                    }
                    barCode.Time = DateTime.Now;

                    barCode.IsValid = false;
                }
            }
            return CallNextHookEx(hook, nCode, wParam, ref lParam);
        }
    }
}
