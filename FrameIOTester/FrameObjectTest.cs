using System;
using FrameIO.Run;
using FrameIO.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;

namespace FrameIORunTest
{
    [TestClass]
    public class FrameObjectTest
    {

        #region --TestData--

        const string strtdata = @"{
  ""segint"": -1234,
  ""segfloat"": 9.8876,
  ""segbool"": true,
  ""segintarr"": [
    100,
    200,
    88777
  ],
  ""segfoo"": {
    ""oosegint"": 99888,
    ""oosegfloat"": -2229.8876
  },
  ""segfooarr"": [
    {
      ""first"": 111,
      ""second"": 222
    },
    {
      ""first"": 111,
      ""second"": 222
    }
  ]
}";

        #endregion

        [TestMethod]
        public void XRunTestReadFrameJson()
        {
            var json = File.ReadAllText("C:\\Kiyun\\FrameIOTest\\of_cs\\Frames.cs");

            var frm = FramesRun.InitialFromJson(json);

        }

        [TestMethod]
        public void XRunTestWriteValue()
        {

            JObject tdata = JObject.Parse(strtdata);

            var fo = new FrameObject();
            fo.SetValue("segint", -1234);
            fo.SetValue("segfloat", 9.8876);
            fo.SetValue("segbool", true);


            {
                var arr = new List<int>();
                arr.Add(100);
                arr.Add(200);
                arr.Add(88777);
                fo.SetValueArray("segintarr", arr);
            }

            {
                var foo = new FrameObject();
                foo.SetValue("oosegint", 99888);
                foo.SetValue("oosegfloat", -2229.8876);
                fo.SetObject("segfoo", foo);
            }



            var ooarr = new List<FrameObject>();
            {
                var foo1 = new FrameObject();
                foo1.SetValue("first", 111);
                foo1.SetValue("second", 222);
                ooarr.Add(foo1);

                var foo2 = new FrameObject();
                foo2.SetValue("first", 111);
                foo2.SetValue("second", 222);
                ooarr.Add(foo2);
            }
            fo.SetObjectArray("segfooarr", ooarr);

            var reslt = fo.ToString();
            Assert.IsTrue(reslt == tdata.ToString());

            Assert.IsTrue(fo.GetBool("segbool"));
            Assert.IsTrue(fo.GetDouble("segfloat") == 9.8876);
            Assert.IsTrue(fo.GetInt("segint") == -1234);
            Assert.IsTrue(fo.GetIntArray("segintarr").Last() == 88777);
            Assert.IsTrue(fo.GetObject("segfoo").GetInt("oosegint") == 99888);
            Assert.IsTrue(fo.GetObjectArray("segfooarr").Last().GetInt("second") == 222);

            fo.SetValue("x1.v3.b3.s9", 1256);
            Assert.IsTrue(fo.GetInt("x1.v3.b3.s9")==1256);
            Assert.IsTrue(fo.GetObject("x1.v3.b3").GetInt("s9") == 1256);

        }
    }
}
