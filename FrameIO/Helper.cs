﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FrameIO.Main
{
    public class Helper
    {
        static public BitmapImage GetImage(string imgName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("FrameIO.Main.img." + imgName);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = myStream;
            image.EndInit();
            myStream.Dispose();
            myStream.Close();
            return image;
        }

        static public string ValidId(string s)
        {
            //TODO
            return "";
        }
    }
}
