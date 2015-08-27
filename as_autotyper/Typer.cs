using AlliSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace as_autotyper
{
    [Serializable()]
    public class Typer : NotifyPropertyChangedBase
    {
        private string text;
        private int fkey = 0;
        private bool pressenter = true;
        private bool isactive = true;
        private bool sendkeys = false;

        [NonSerialized]
        public int hotKeyId = 0;

        public Typer(string text, int fkey, bool sendkeys, bool pressenter, bool isactive)
        {
            this.text = text;
            this.fkey = fkey;
            this.pressenter = pressenter;
            this.isactive = isactive;
            this.sendkeys = sendkeys;
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                NotifyChange("Text");
            }
        }

        public int FKey
        {
            get
            {
                return fkey;
            }
            set
            {
                fkey = value;
                NotifyChange("FKey");
            }
        }

        public bool SendKeys
        {
            get
            {
                return sendkeys;
            }
            set
            {
                sendkeys = value;
                NotifyChange("SendKeys");
            }
        }

        public bool PressEnter
        {
            get
            {
                return pressenter;
            }
            set
            {
                pressenter = value;
                NotifyChange("PressEnter");
            }
        }

        public bool IsActive
        {
            get
            {
                return isactive;
            }
            set
            {
                isactive = value;
                NotifyChange("IsActive");
            }
        }
    }
}
