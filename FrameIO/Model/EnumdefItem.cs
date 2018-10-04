﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class EnumdefItem : INotifyPropertyChanged
    {
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public string Notes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
