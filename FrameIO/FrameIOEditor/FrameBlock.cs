﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{

    //数据帧结构
    [Serializable]
    public class FrameBlockInfo
    {
        public SegTreeInfo RootSegmentInfo { get; set; }
        public SegBlockInfoGroup RootSegBlockGroupInfo { get; set; }
        public Frame TheFrame { get; set; }

    }

    //字段结构群组
    [Serializable]
    public class SegBlockInfoGroup
    {
        public List<SegBlockInfo> SegBlockList { get; set; } = new List<SegBlockInfo>();


        public bool IsOneOfGroup { get; set; } = false;
        public string OneOfSegFullName { get; set; }
        public Dictionary<ulong, SegBlockInfoGroup> OneOfGroupList { get; set; }
        public SegBlockInfoGroup Next { get; set; }
        public SegBlockInfoGroup Parent { get; set; }

    }

    //字段类型枚举
    [Serializable]
    public enum SegBlockType
    {
        Integer,
        Real,
        Text
    }

    //字段结构
    [Serializable]
    public class SegBlockInfo
    {


        public FrameSegmentBase Segment { get; private set; }
        public SegTreeInfo RefSegTree { get; private set; }
        public List<string> CheckBeginSegs { get; set; }
        public List<string> CheckEndSegs { get; set; }
        public SegBlockInfo(int idx, FrameSegmentBase seg, SegTreeInfo segt)
        {
            Idx = idx;
            Segment = seg;
            RefSegTree = segt;
        }
        public int Idx { get; private set; }
        public SegBlockType SegType { get; set; }
        public int BitSizeNumber { get; set; }
        public int RepeatedNumber { get; set; }
        public Exp BitSize { get; set; }
        public Exp Repeated { get; set; }
        public bool IsFixed { get; set; } = false;
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int Syid { get; set; }
        public SegBlockInfoGroup Parent { get; set; }


    }



    //字段信息
    [Serializable]
    public class SegTreeInfo
    {
        //字段标识
        public string Name { get; set; } = "";

        //父字段
        public SegTreeInfo Parent { get; set; }

        //子字段
        public IList<SegTreeInfo> Children { get; private set; } = new List<SegTreeInfo>();

        //对应的字段
        public FrameSegmentBase Segment { get; set; }

        //字段所属的frame
        public Frame SegmentOwnerFrame { get; set; }

        //是否引用了字段所属frame
        public bool IsRefFrame { get; set; }

        //代码位置
        public int Syid { get; set; }
        public bool IsOneOf { get; set; } = false;
    }
}