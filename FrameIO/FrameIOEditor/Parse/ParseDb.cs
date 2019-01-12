using SQLiteWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class ParseDb
    {

        private SQLiteBase _db = null;

        private IList<ParseError> _err = null;
        private bool _checkSemantics = false;
        private IOProject _pj = null;

        public ParseDb()
        {
            _db = new SQLiteWrapper.SQLiteBase();
            //_db.OpenDatabase(@"file:C:/Kiyun/FrameIO/FrameIO/x64/Debug/frameio.db");
            _db.OpenDatabase(@"file::memory:?cache=shared");
            _db.ExecuteNonQuery(_createDbSqlCmd);
        }


        [DllImport("FrameIOParser.dll")]
        public extern static int parse(int projectid);

        #region --CreateDbCmd--

        private const string _createDbSqlCmd = @"
--
-- File generated with SQLiteStudio v3.2.1 on 周六 10月 6 09:28:06 2018
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: fio_enum
CREATE TABLE fio_enum (projectid INTEGER, namesyid INTEGER);

-- Table: fio_enum_item
CREATE TABLE fio_enum_item (projectid INTEGER, enumid INTEGER, namesyid INTEGER, integervid INTEGER);

-- Table: fio_error
CREATE TABLE fio_error (projectid INTEGER, errorcode INTEGER, firstsyid INTEGER, lastsyid INTEGER);

-- Table: fio_frame
CREATE TABLE fio_frame (projectid INTEGER, namesyid INTEGER, subsysid INTEGER);

-- Table: fio_frame_exp
CREATE TABLE fio_frame_exp (projectid INTEGER, propertyid INTEGER, op INTEGER, leftid INTEGER, rightid INTEGER, valuesyid INTEGER);

-- Table: fio_frame_oneof
CREATE TABLE fio_frame_oneof (projectid INTEGER, proertyid INTEGER, itemsyid INTEGER, framesyid INTEGER);

-- Table: fio_frame_segment
CREATE TABLE fio_frame_segment (projectid INTEGER, frameid INTEGER, namesyid INTEGER, segmenttype INTEGER, subsysid INTEGER);

-- Table: fio_frame_segment_property
CREATE TABLE fio_frame_segment_property (projectid INTEGER, segmentid INTEGER, proname INTEGER, vtype INTEGER, ivalue INTEGER, pvalue INTEGER);

-- Table: fio_notes
CREATE TABLE fio_notes (projectid INTEGER, notesyid INTEGER, aftersyid INTEGER);

-- Table: fio_project
CREATE TABLE fio_project (namesyid INTEGER, code TEXT);

-- Table: fio_symbol
CREATE TABLE fio_symbol (projectid INTEGER, symbol TEXT, lineno INTEGER, firstcolumn INTEGER, lastcolumn INTEGER);

-- Table: fio_sys
CREATE TABLE fio_sys (projectid INTEGER, namesyid INTEGER);

-- Table: fio_sys_action
CREATE TABLE fio_sys_action (projectid INTEGER, sysid INTEGER, namesyid INTEGER, actiontype INTEGER, frameid INTEGER, channelid INTEGER);

-- Table: fio_sys_action_map
CREATE TABLE fio_sys_action_map (projectid INTEGER, actionid INTEGER, segsyid INTEGER, sysprosyid INTEGER);

-- Table: fio_sys_channel
CREATE TABLE fio_sys_channel (projectid INTEGER, sysid INTEGER, namesyid INTEGER, channeltype INTEGER);

-- Table: fio_sys_channel_option
CREATE TABLE fio_sys_channel_option (projectid INTEGER, channelid INTEGER, nameid INTEGER, valuesyid INTEGER);

-- Table: fio_sys_property
CREATE TABLE fio_sys_property ( projectid INTEGER, sysid INTEGER, namesyid INTEGER, propertytype INTEGER, arraycount INTEGER);


COMMIT TRANSACTION;
PRAGMA foreign_keys = on;


";

        #endregion

        #region --Project Element--


        //加载注释
        private string LoadNotes(int syid)
        {
            var tb = _db.ExecuteQuery("SELECT sb.symbol notes  FROM fio_notes nt LEFT JOIN fio_symbol sb ON nt.notesyid = sb.rowid WHERE nt.aftersyid = "
                + syid.ToString());
            var ret = new StringBuilder();
            foreach (DataRow r in tb.Rows)
            {
                var l = r[0].ToString().Trim();
                if (l.StartsWith(@"//")) l = l.Substring(2);
                ret.Append(l);
                ret.Append(Environment.NewLine);
            }
            return ret.ToString().TrimEnd(Environment.NewLine.ToArray());
        }

        //加载受控对象系统
        private ObservableCollection<Subsys>  LoadSubsys(int projectid)
        {
            var ret = new ObservableCollection<Subsys>();
            var tb = _db.ExecuteQuery("SELECT ss.rowid, namesyid, symbol FROM fio_sys ss LEFT JOIN fio_symbol sy ON ss.namesyid=sy.rowid WHERE ss.projectid="
                + projectid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var syid = Convert.ToInt32(r["namesyid"]);
                var ssid = Convert.ToInt32(r["rowid"]);
                var ss = new Subsys(r["symbol"].ToString())
                {
                    Notes = LoadNotes(syid),
                    Propertys = LoadSubsysProperty(ssid),
                    Channels = LoadChannel(ssid),
                    Actions = LoadAction(ssid),
                    Syid = syid
                };
                if (_checkSemantics && ret.Where(p => p.Name == ss.Name).Count() > 0)
                {
                    AddError(syid, 3);
                }
                ret.Add(ss);
            }

            return ret;
        }


        //加载通道
        private ObservableCollection<SubsysChannel>LoadChannel(int ssid)
        {
            var ret = new ObservableCollection<SubsysChannel>();
            var tb = _db.ExecuteQuery("SELECT ch.rowid, namesyid, symbol, channeltype FROM fio_sys_channel ch LEFT JOIN fio_symbol sy ON ch.namesyid=sy.rowid WHERE ch.sysid="
                + ssid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var chid = Convert.ToInt32(r["rowid"]);
                var syid = Convert.ToInt32(r["namesyid"]);
                var chname = r["symbol"].ToString();
                var ch = new SubsysChannel()
                {
                    Name = chname,
                    ChannelType = (syschanneltype)Convert.ToInt32(r["channeltype"]),
                    Notes = LoadNotes(syid),
                    Options = LoadChannelOptions(chid),
                    Syid = syid
                };
                if (_checkSemantics && ret.Where(p => p.Name == ch.Name).Count() > 0)
                {
                    AddError(syid, 4);
                }
                ret.Add(ch);
            }

            return ret;
        }


        //加载通道选项
        private ObservableCollection<SubsysChannelOption>  LoadChannelOptions(int chid)
        {
            var ret = new ObservableCollection<SubsysChannelOption>();
            var tb = _db.ExecuteQuery("SELECT nameid, sy2.symbol name, valuesyid, sy.symbol option FROM fio_sys_channel_option op LEFT JOIN fio_symbol sy ON op.valuesyid=sy.rowid LEFT JOIN fio_symbol sy2 ON op.nameid=sy2.rowid WHERE op.channelid ="
                + chid.ToString());
            foreach(DataRow r in tb.Rows)
            {
                var op = new SubsysChannelOption()
                {
                    Notes = LoadNotes(Convert.ToInt32(r["valuesyid"])),
                    Name = r["name"].ToString(),
                    OptionValue = r["option"].ToString(),
                    Syid = Convert.ToInt32(r["nameid"])
                };
                if (ret.Where(p => p.Name == op.Name).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["valuesyid"]), 5);
                }
                ret.Add(op);
            }
            return ret;
        }

        //加载IO操作
        private ObservableCollection<SubsysAction>LoadAction(int ssid)
        {
            var ret= new ObservableCollection<SubsysAction>();
            var tb = _db.ExecuteQuery("SELECT ac.rowid, namesyid, sy1.symbol name, actiontype, sy2.symbol framename, sy3.symbol channelname FROM fio_sys_action ac " +
                "LEFT JOIN fio_symbol sy1 ON ac.namesyid = sy1.rowid LEFT JOIN fio_symbol sy2 ON ac.frameid = sy2.rowid LEFT JOIN fio_symbol sy3 ON ac.channelid = sy3.rowid WHERE ac.sysid = "
               + ssid.ToString());
            foreach(DataRow r in tb.Rows)
            {
                var syid = Convert.ToInt32(r["namesyid"]);
                var acid = Convert.ToInt32(r["rowid"]);
                ObservableCollection<SubsysActionMap> liteMaps = null;
                List<string> userBeginCode = null, userEndCode = null;
                var ac = new SubsysAction()
                {
                    ChannelName = r["channelname"].ToString(),
                    FrameName = r["framename"].ToString(),
                    IOType = (actioniotype)Convert.ToInt32(r["actiontype"]),
                    Name = r["name"].ToString(),
                    Notes = LoadNotes(Convert.ToInt32(r["namesyid"])),
                    Syid = syid
                };
                LoadActionMaps(acid, out liteMaps, out userBeginCode, out userEndCode);

                ac.LiteMaps = liteMaps;
                ac.BeginCodes = userBeginCode;
                ac.EndCodes = userEndCode;
                if (_checkSemantics && ret.Where(p => p.Name == ac.Name).Count() > 0)
                {
                    AddError(syid, 6);
                }
                ret.Add(ac);
            }
            return ret;
        }


        //加载IO操作 映射
        public ObservableCollection<SubsysActionMap> LoadActionMaps(int acid, out ObservableCollection<SubsysActionMap> liteMaps, out List<string> userBeginCode, out List<string> userEndCode)
        {
            var ret = new ObservableCollection<SubsysActionMap>();
            liteMaps = new ObservableCollection<SubsysActionMap>();
            userBeginCode = new List<string>();
            userEndCode = new List<string>();
            var tb = _db.ExecuteQuery("SELECT mp.rowid, segsyid, sy1.symbol segname, sy2.symbol proname FROM fio_sys_action_map mp LEFT JOIN fio_symbol sy1 ON mp.segsyid=sy1.rowid LEFT JOIN fio_symbol sy2 ON mp.sysprosyid=sy2.rowid WHERE mp.actionid="
                + acid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var syid = Convert.ToInt32(r["segsyid"]);
                var mid = Convert.ToInt32(r["rowid"]);
                var mp = new SubsysActionMap()
                {
                    FrameSegName = r["segname"].ToString(),
                    Notes = LoadNotes(Convert.ToInt32(r["segsyid"])),
                    SysPropertyName = r["proname"].ToString(),
                    Syid = syid
                };
                if (_checkSemantics && Convert.ToInt32(r["segsyid"])!=0 && ret.Where(p => p.FrameSegName == mp.FrameSegName).Count() > 0)
                {
                    AddError(syid, 7);
                }
                ret.Add(mp);
            }

            for(int i=0; i<ret.Count; i++)
            {
                if (!ret[i].SysPropertyName.StartsWith("@"))
                    liteMaps.Add(ret[i]);
            }

            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i].SysPropertyName.StartsWith("@"))
                    userBeginCode.Add(ret[i].SysPropertyName);
                else
                    break;
            }

            int ifind = -1;
            for(int i = ret.Count-1; i>=0; i--)
            {
                if (ret[i].SysPropertyName.StartsWith("@"))
                    ifind = i;
                else
                    break;
            }

            if(ifind>=0)
            {
                for(int i=ifind; i<ret.Count; i++)
                    userEndCode.Add(ret[i].SysPropertyName);
            }

            return ret;
        }

        //加载受控对象属性
        private ObservableCollection<SubsysProperty> LoadSubsysProperty(int ssid)
        {
            var ret = new ObservableCollection<SubsysProperty>();
            var tb = _db.ExecuteQuery("SELECT namesyid, sy.symbol name, sy2.symbol propertytype, arraycount FROM fio_sys_property pt LEFT JOIN fio_symbol sy ON pt.namesyid=sy.rowid LEFT JOIN fio_symbol sy2 ON pt.propertytype=sy2.rowid WHERE pt.sysid="
               + ssid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var syid = Convert.ToInt32(r["namesyid"]);
                var ci = Convert.ToInt32(r["arraycount"]);
                var cs = ci > 0 ? GetSymbol(ci) : "0";
          
                var sp = new SubsysProperty()
                {
                    ArrayLen = cs,
                    IsArray = ci >= 0,
                    Name = r["name"].ToString(),
                    Notes = LoadNotes(Convert.ToInt32(r["namesyid"])),
                    PropertyType = r["propertytype"].ToString(),
                    Syid = syid
                };
                if (_checkSemantics && ret.Where(p => p.Name == sp.Name).Count() > 0)
                {
                    AddError(syid, 8);
                }
                ret.Add(sp);
            }

            return ret;
        }


        ////计算表达式的值
        //private bool EvalExp(int proid, out double dv, DataRow xr, Dictionary<int, DataRow> explist)
        //{
        //    dv = -1;
        //    if (explist == null)
        //    {
        //        explist = new Dictionary<int, DataRow>();
        //        var tb = _db.ExecuteQuery(string.Format("SELECT rowid, op, leftid, rightid, valuesyid FROM fio_frame_exp ep WHERE propertyid={0} ORDER BY ep.rowid DESC",
        //               proid));
        //        foreach (DataRow r in tb.Rows)
        //        {
        //            var rop = (exptype)Convert.ToInt32(r["op"]);
        //            if (rop == exptype.EXP_BYTESIZEOF || rop == exptype.EXP_ID) return false; 
        //            explist.Add(Convert.ToInt32(r["rowid"]), r);
        //        }
        //        return EvalExp(-1,out dv, tb.Rows[0], explist);
        //    }

        //    var op = (exptype)Convert.ToInt32(xr["op"]);
        //    double i1 = -1;
        //    double i2 = -1;
        //    switch (op)
        //    {
        //        case exptype.EXP_ADD:
        //            EvalExp(-1, out i1, explist[Convert.ToInt32(xr["leftid"])], explist);
        //            EvalExp(-1, out i2, explist[Convert.ToInt32(xr["rightid"])], explist);
        //            dv = i1 + i2;
        //            return true;
        //        case exptype.EXP_SUB:
        //            EvalExp(-1, out i1, explist[Convert.ToInt32(xr["leftid"])], explist);
        //            EvalExp(-1, out i2, explist[Convert.ToInt32(xr["rightid"])], explist);
        //            dv = i1 - i2;
        //            return true;
        //        case exptype.EXP_MUL:
        //            EvalExp(-1, out i1, explist[Convert.ToInt32(xr["leftid"])], explist);
        //            EvalExp(-1, out i2, explist[Convert.ToInt32(xr["rightid"])], explist);
        //            dv = i1 * i2;
        //            return true;
        //        case exptype.EXP_DIV:
        //            EvalExp(-1, out i1, explist[Convert.ToInt32(xr["leftid"])], explist);
        //            EvalExp(-1, out i2, explist[Convert.ToInt32(xr["rightid"])], explist);
        //            dv = i1 / i2;
        //            return true;
        //        case exptype.EXP_INT:
        //            dv = Convert.ToInt32(GetSymbol(Convert.ToInt32(xr["valuesyid"])));
        //            return true;
        //        case exptype.EXP_REAL:
        //            dv = Convert.ToDouble(GetSymbol(Convert.ToInt32(xr["valuesyid"])));
        //            return true;
        //    }
        //    Debug.Assert(false);
        //    return false;
        //}


        //取exp
        private Exp GetExp(int proid, DataRow xr, Dictionary<int, DataRow> explist)
        {
            if(explist ==null)
            {
                explist = new Dictionary<int, DataRow>();
                var tb = _db.ExecuteQuery(string.Format("SELECT rowid, op, leftid, rightid, valuesyid FROM fio_frame_exp ep WHERE propertyid={0} ORDER BY ep.rowid DESC",
                       proid));
                foreach(DataRow r in tb.Rows)
                {
                    explist.Add(Convert.ToInt32(r["rowid"]), r);
                }
                return GetExp(-1, tb.Rows[0], explist);
            }

            var op = (exptype)Convert.ToInt32(xr["op"]);
            switch(op)
            {
                case exptype.EXP_ADD:
                    return new Exp() { Op= exptype.EXP_ADD, LeftExp = GetExp(-1, explist[Convert.ToInt32(xr["leftid"])], explist), RightExp = GetExp(-1, explist[Convert.ToInt32(xr["rightid"])], explist) };
                case exptype.EXP_SUB:
                    return new Exp() { Op = exptype.EXP_SUB, LeftExp = GetExp(-1, explist[Convert.ToInt32(xr["leftid"])], explist), RightExp = GetExp(-1, explist[Convert.ToInt32(xr["rightid"])], explist) };
                case exptype.EXP_MUL:
                    return new Exp() { Op = exptype.EXP_MUL, LeftExp = GetExp(-1, explist[Convert.ToInt32(xr["leftid"])], explist), RightExp = GetExp(-1, explist[Convert.ToInt32(xr["rightid"])], explist) };
                case exptype.EXP_DIV:
                    return new Exp() { Op = exptype.EXP_DIV, LeftExp = GetExp(-1, explist[Convert.ToInt32(xr["leftid"])], explist), RightExp = GetExp(-1, explist[Convert.ToInt32(xr["rightid"])], explist) };
                case exptype.EXP_BYTESIZEOF:
                    var vsyid = Convert.ToInt32(xr["valuesyid"]);
                    return new Exp() { Op = exptype.EXP_BYTESIZEOF, ConstStr = (vsyid > 0 ? GetSymbol(vsyid) : "this") };
                case exptype.EXP_ID:
                    return new Exp() { Op = exptype.EXP_ID, ConstStr = GetSymbol(Convert.ToInt32(xr["valuesyid"])) };
                case exptype.EXP_INT:
                    return new Exp() { Op = exptype.EXP_INT, ConstStr = GetSymbol(Convert.ToInt32(xr["valuesyid"])) };
                case exptype.EXP_REAL:
                    return new Exp() { Op = exptype.EXP_REAL, ConstStr = GetSymbol(Convert.ToInt32(xr["valuesyid"])) };
                default:
                    Debug.Assert(false);
                    return null;
            }
          
        }

        //取symbol
        private string GetSymbol(int syid)
        {
            if (syid == 0) return "";
            return _db.ExecuteQuery("SELECT symbol FROM fio_symbol WHERE rowid=" + syid.ToString()).Rows[0][0].ToString();
        }

        //加载数据帧
        private ObservableCollection<Frame> LoadFrame(int projectid)
        {
            var ret = new ObservableCollection<Frame>();
            var tb = _db.ExecuteQuery("SELECT fr.rowid, namesyid, sy.symbol, fr.subsysid FROM fio_frame fr LEFT JOIN fio_symbol sy ON fr.namesyid=sy.rowid WHERE fr.namesyid>0 AND fr.projectid="
               + projectid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var sid = Convert.ToInt32(r["namesyid"]);
                var fr = new Frame(r["symbol"].ToString())
                {
                     Notes = LoadNotes(sid),
                     Segments = LoadSegments(Convert.ToInt32(r["rowid"])),
                     Syid = sid
                };
                if (_checkSemantics && ret.Where(p => p.Name == fr.Name).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["namesyid"]), 9);
                }
                ret.Add(fr);
                var innersubsysname = GetSymbol(Convert.ToInt32(r["subsysid"]));
                if(innersubsysname != "")
                {
                    LoadInnerSubSys(innersubsysname, fr.Segments, Convert.ToInt32(r["subsysid"]));
                    fr.SubSysName = innersubsysname;
                }
            }
            return ret;
        }


        //加载integer字段属性
        private void LoadSegProInterger(int segid, FrameSegmentInteger seg, List<SegPro> pros)
        {
            foreach(var pro in pros)
            {
                switch(pro.proname)
                {
                    case segpropertytype.SEGP_SIGNED:
                        seg.Signed = (pro.vtype == segpropertyvaluetype.SEGPV_TRUE) ? true : false;
                        break;
                    case segpropertytype.SEGP_BITCOUNT:
                        seg.BitCount = Convert.ToInt32(pro.ivalue);
                        break;
                    case segpropertytype.SEGP_VALUE:
                        seg.Value = GetExp(pro.proid, null, null);
                        break;
                    case segpropertytype.SEGP_BYTEORDER:
                        seg.ByteOrder = (ByteOrderType)(Int32)pro.vtype;
                        break;
                    case segpropertytype.SEGP_ENCODED:
                        seg.Encoded = (EncodedType)(Int32)pro.vtype;
                        break;
                    case segpropertytype.SEGP_REPEATED:
                        seg.Repeated = GetExp(pro.proid, null, null);
                        break;
                    case segpropertytype.SEGP_MAX:
                        seg.ValidateMax = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_MIN:
                        seg.ValidateMin = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_CHECK:
                        seg.ValidateCheck = (CheckType)Convert.ToInt32(pro.vtype);
                        break;
                    case segpropertytype.SEGP_CHECKRANGE_BEGIN:
                        seg.CheckRangeBegin = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_CHECKRANGE_END:
                        seg.CheckRangeEnd = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_MATCH:
                        seg.Match = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_TOENUM:
                        seg.ToEnum = pro.ivalue;
                        if(_checkSemantics)
                        {
                            if (_pj.EnumdefList.Where(p => p.Name == pro.ivalue).Count() == 0)
                                AddError(pro.ivsyid, 14);
                        }
                        break;
                    default:
                        AddError(pro.ivsyid>0?pro.ivsyid:pro.segsyid, 12);
                        break;
                }
            }
        }

        //加载real字段属性
        private void LoadSegProReal(int segid, FrameSegmentReal seg, List<SegPro> pros)
        {
            foreach (var pro in pros)
            {
                switch (pro.proname)
                {
                    case segpropertytype.SEGP_ISDOUBLE:
                        seg.IsDouble = (pro.vtype == segpropertyvaluetype.SEGPV_TRUE) ? true : false;
                        break;
                    case segpropertytype.SEGP_VALUE:
                        seg.Value = GetExp(pro.proid, null, null);
                        break;
                    case segpropertytype.SEGP_BYTEORDER:
                        seg.ByteOrder = (ByteOrderType)(Int32)pro.vtype;
                        break;
                    case segpropertytype.SEGP_ENCODED:
                        seg.Encoded = (EncodedType)(Int32)pro.vtype;
                        break;
                    case segpropertytype.SEGP_REPEATED:
                        seg.Repeated = GetExp(pro.proid, null, null);
                        break;
                    case segpropertytype.SEGP_MAX:
                        seg.ValidateMax = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_MIN:
                        seg.ValidateMin = pro.ivalue;
                        break;
                        
                    default:
                        AddError(pro.ivsyid > 0 ? pro.ivsyid : pro.segsyid, 12);
                        break;
                }
            }
        }

        //加载text字段属性
        private void LoadSegProText(int segid, FrameSegmentText seg, List<SegPro> pros)
        {
            foreach (var pro in pros)
            {
                switch (pro.proname)
                {
                    case segpropertytype.SEGP_TAIL:
                        seg.Tail = pro.ivalue;
                        break;
                    case segpropertytype.SEGP_ALIGNEDLEN:
                        seg.AlignedLen = Convert.ToInt32(pro.ivalue);
                        break;
                    case segpropertytype.SEGP_REPEATED:
                        seg.Repeated = GetExp(pro.proid, null, null);
                        break;
                    case segpropertytype.SEGP_BYTESIZE:
                        seg.ByteSize = GetExp(pro.proid, null, null);
                        break;
                    default:
                        AddError(pro.ivsyid > 0 ? pro.ivsyid : pro.segsyid, 12);
                        break;
                }
            }
        }
        private ObservableCollection<OneOfMap> LoadOneOfList(int proid)
        {
            var ret = new ObservableCollection<OneOfMap>();
            var tb = _db.ExecuteQuery("SELECT itemsyid, sy1.symbol itname, framesyid, sy2.symbol frname FROM fio_frame_oneof nf LEFT JOIN fio_symbol sy1 ON nf.itemsyid=sy1.rowid LEFT JOIN fio_symbol sy2 ON nf.framesyid=sy2.rowid where nf.proertyid="
                + proid.ToString());
            foreach(DataRow r in tb.Rows)
            {
                var iof = new OneOfMap()
                {
                    //IsDefault = (Convert.ToInt32(r["itemsyid"]) == 0),
                    EnumItem = (Convert.ToInt32(r["itemsyid"]) == 0) ? "other" : r["itname"].ToString(),
                    FrameName = r["frname"].ToString()
                };
                if (_checkSemantics && ret.Where(p => p.EnumItem == iof.EnumItem).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["itemsyid"]), 13);
                }
                ret.Add(iof);
            }
            return ret;
        }

        //加载block字段属性
        private void LoadSegProBlock(int segid, FrameSegmentBlock seg, List<SegPro> pros)
        {
            foreach (var pro in pros)
            {
                switch (pro.proname)
                {
                    case segpropertytype.SEGP_TYPE:
                        switch(pro.vtype)
                        {
                            case segpropertyvaluetype.SEGPV_NONAMEFRAME:
                                seg.DefineSegments = LoadSegments(pro.pvalue);
                                seg.UsedType = BlockSegType.DefFrame;
                                break;
                            case segpropertyvaluetype.SEGPV_ONEOF:
                                seg.OneOfBySegment = pro.ivalue;
                                seg.OneOfCaseList = LoadOneOfList(pro.proid);
                                seg.UsedType = BlockSegType.OneOf;
                                break;
                            case segpropertyvaluetype.SEGPV_ID:
                                seg.RefFrameName = pro.ivalue;
                                seg.UsedType = BlockSegType.RefFrame;
                               break;
                            default:
                                Debug.Assert(false);
                                break;

                        }
                        break;

                    //case segpropertytype.SEGP_BYTESIZE:
                    //    seg.ByteSize = GetExp(pro.proid, null, null);
                    //    break;
                    case segpropertytype.SEGP_REPEATED:
                        seg.Repeated = GetExp(pro.proid, null, null);
                        break;
                    default:
                        AddError(pro.ivsyid > 0 ? pro.ivsyid : pro.segsyid, 12);
                        break;
                }
            }
        }

        private class SegPro
        {
            public Int32 proid { get; set; }
            public Int32 ivsyid { get; set; }
            public Int32 segsyid { get; set; }
            public string ivalue { get; set; }
            public Int32 pvalue { get; set; }
            public segpropertytype proname { get; set; }
            public segpropertyvaluetype vtype { get; set; }
        }


        private void LoadInnerSubSys(string name, ObservableCollection<FrameSegmentBase> seglist, int syid)
        {
            //if (_pj.InnerSubsysList.Where(p => p.Name == name).Count() > 0) return;
            _pj.InnerSubsysList.Add(new InnerSubsys(name, seglist) { Syid=syid});
        }

        //加载数据帧字段
        private ObservableCollection<FrameSegmentBase> LoadSegments(int frameid)
        {
            var ret = new ObservableCollection<FrameSegmentBase>();
            var tb = _db.ExecuteQuery("SELECT sg.rowid, namesyid, segmenttype, sy.symbol segname, sg.subsysid FROM fio_frame_segment sg LEFT JOIN fio_symbol sy ON sg.namesyid=sy.rowid WHERE sg.frameid="
                + frameid.ToString());
            foreach(DataRow r in tb.Rows)
            {
                segmenttype st = (segmenttype)Convert.ToInt32(r["segmenttype"]);
                var segid = Convert.ToInt32(r["rowid"]);
                var syid = Convert.ToInt32(r["namesyid"]);
                FrameSegmentBase seg = null;
                var pros = new List<SegPro>();

                var tpro = _db.ExecuteQuery("SELECT pr.rowid proid, proname, vtype, pr.ivalue, pr.pvalue, sy.symbol FROM fio_frame_segment_property pr LEFT JOIN fio_symbol sy ON pr.ivalue=sy.rowid WHERE pr.segmentid="
                    + segid.ToString());
                foreach(DataRow rp in tpro.Rows)
                {
                    var pr = new SegPro()
                    {
                        proid = Convert.ToInt32(rp["proid"]),
                        proname = (segpropertytype)Convert.ToInt32(rp["proname"]),
                        vtype = (segpropertyvaluetype)Convert.ToInt32(rp["vtype"]),
                        ivsyid = Convert.ToInt32(rp["ivalue"]),
                        ivalue = rp["symbol"].ToString(),
                        pvalue = Convert.ToInt32(rp["pvalue"]),
                        segsyid = syid
                    };
                    if (pros.Where(p => p.proname == pr.proname).Count() > 0)
                    {
                        AddError(pr.ivsyid>0?pr.ivsyid:syid, 11);
                    }
                    pros.Add(pr);
                }
                switch(st)
                {
                    case segmenttype.SEGT_INTEGER:
                        var iseg = new FrameSegmentInteger();
                        LoadSegProInterger(segid, iseg, pros);
                        seg = iseg;
                        break;
                    case segmenttype.SEGT_REAL:
                        var rseg = new FrameSegmentReal();
                        LoadSegProReal(segid, rseg, pros);
                        seg = rseg;
                        break;
                    case segmenttype.SEGT_TEXT:
                        var tseg = new FrameSegmentText();
                        LoadSegProText(segid, tseg, pros);
                        seg = tseg;
                        break;
                    case segmenttype.SEGT_BLOCK:
                        var bseg = new FrameSegmentBlock();
                        LoadSegProBlock(segid, bseg, pros);
                        seg = bseg;
                        var innersys = GetSymbol(Convert.ToInt32(r["subsysid"]));
                        if (innersys != "" && bseg.UsedType== BlockSegType.DefFrame)
                        {
                            bseg.SubSysName = innersys;
                            LoadInnerSubSys(innersys, bseg.DefineSegments, Convert.ToInt32(r["subsysid"]));
                        }
                            
                        break;
                }
                seg.Name = r["segname"].ToString();
                seg.Notes = LoadNotes(syid);
                seg.Syid = syid;
                if (_checkSemantics && ret.Where(p => p.Name == seg.Name).Count() > 0)
                {
                    AddError(syid, 10);
                }
                ret.Add(seg);

            }
            return ret;
        }



        //加载枚举列表
        private ObservableCollection<Enumdef> LoadEnumList(int projectid)
        {
            var ret = new ObservableCollection<Enumdef>();
            var tb = _db.ExecuteQuery("SELECT em.rowid, em.namesyid, sy.symbol ename FROM fio_enum em LEFT JOIN fio_symbol sy ON em.namesyid=sy.rowid where em.projectid="
                + projectid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var syid = Convert.ToInt32(r["namesyid"]);
                var ed = new Enumdef(r["ename"].ToString());
                ed.Syid = syid;
                ed.Notes = LoadNotes(syid);
                ed.ItemsList = LoadEnumItemList(Convert.ToInt32(r["rowid"]));
                if(_checkSemantics && ret.Where(p=>p.Name==ed.Name).Count()>0)
                {
                    AddError(syid, 1);
                }
                ret.Add(ed);
            }
            return ret;
        }

        //加载枚举项列表
        private ObservableCollection<EnumdefItem> LoadEnumItemList(int emid)
        {
            var ret = new ObservableCollection<EnumdefItem>();
            var tb = _db.ExecuteQuery("SELECT ei.namesyid, sy1.symbol name, sy2.symbol value FROM fio_enum_item ei LEFT JOIN fio_symbol sy1 ON ei.namesyid=sy1.rowid LEFT JOIN fio_symbol sy2 ON ei.integervid=sy2.rowid WHERE ei.enumid="
                + emid.ToString());
            foreach(DataRow r in tb.Rows)
            {
                string v = (r["value"] == null) ? "" : r["value"].ToString();
                var ei = new EnumdefItem()
                {
                    Name = r["name"].ToString(),
                    ItemValue = v,
                    Notes = LoadNotes(Convert.ToInt32(r["namesyid"]))
                };
                if (_checkSemantics && ret.Where(p => p.Name == ei.Name).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["namesyid"]), 2);
                }
                ret.Add(ei);
            }
            return ret;
        }


        #endregion
        
        #region --Project--

        //创建项目
        public int CreateProject(string code)
        {
            _db.ExecuteNonQuery("INSERT INTO fio_project (code) VALUES(:code);", code);
            return _db.GetLastRowId();
        }

        //删除项目

        public void DeleteProject(int projectid)
        {
            var ts  = _db.GetTables();
            foreach(var t in ts)
            {
                if (!t.ToString().StartsWith("fio")) continue;
                _db.ExecuteNonQuery(string.Format("DELETE FROM {0} WHERE {1} = {2}", t, t.ToString()=="fio_project"?"rowid":"projectid",projectid.ToString()));
            }

        }

        //加载项目 有错误时返回null并填充低级错误列表 checkSemantics指定是否进行语义检查
        public IOProject LoadProject(int projectid, out IList<ParseError> errorlist, bool checkSemantics=false)
        {
            _checkSemantics = checkSemantics;
            if (_err == null)
                _err = new List<ParseError>();
            else
                _err.Clear();
            _pj = new IOProject(projectid);

            var tb = _db.ExecuteQuery("SELECT namesyid, symbol projectname FROM fio_project pj LEFT JOIN fio_symbol sb ON pj.namesyid = sb.rowid WHERE projectid="
                + projectid.ToString());


            _pj.Name = tb.Rows[0]["projectname"].ToString();
            _pj.Notes = LoadNotes(Convert.ToInt32(tb.Rows[0]["namesyid"]));
            _pj.EnumdefList = LoadEnumList(projectid);
            _pj.FrameList = LoadFrame(projectid);
            _pj.SubsysList = LoadSubsys(projectid);

            errorlist = _err;
            if (_err.Count > 0) return null;
            return _pj;
        }


        #endregion
        
        #region --Error--

        public ParseError GetError(int syid, int errcode)
        {
            var r = _db.ExecuteQuery("SELECT lineno, firstcolumn, lastcolumn FROM fio_symbol WHERE rowid=" + syid.ToString()).Rows[0];
            var il = Convert.ToInt32(r["lineno"]);
            var ifc = Convert.ToInt32(r["firstcolumn"]);
            var ilc = Convert.ToInt32(r["lastcolumn"]);

           return new ParseError()
            {
                ErrorCode = errcode,
                FirstLine = il,
                LastLine = il,
                FirstCol = ifc,
                LastCol = ilc
            };
        }

        //添加错误信息到列表
        private void AddError(int syid, int errcode)
        {
            _err.Add(GetError(syid, errcode));
        }
  
        //加载错误信息
        public IList<ParseError> LoadError(int projectid)
        {
            //var test = _db.ExecuteQuery("select * from predpd_error");
            var ret = new List<ParseError>();
            var str = "SELECT err.errorcode, sy.symbol firstsy, sy.lineno firstline, sy.firstcolumn firstcol, sy2.symbol lastsy, sy2.lineno lastline, sy2.lastcolumn lastcol FROM " 
                       + " fio_error err LEFT JOIN fio_symbol sy ON err.firstsyid = sy.rowid LEFT JOIN fio_symbol sy2 ON err.lastsyid = sy2.rowid WHERE err.projectid =" 
                      + projectid.ToString();
            var tb = _db.ExecuteQuery(str);
            foreach (DataRow r in tb.Rows)
            {
                int errcode = Convert.ToInt32(r["errorcode"]);
                if(errcode ==0)
                {
                    ret.Add(new ParseError()
                    {
                        ErrorCode = errcode,
                        FirstLine = 1,
                        FirstCol = 0,
                        LastLine = 1,
                        LastCol = 1,

                    });
                }
                else
                {
                    ret.Add(new ParseError()
                    {
                        ErrorCode = errcode,
                        FirstLine = Convert.ToInt32(r["firstline"]),
                        FirstCol = Convert.ToInt32(r["firstcol"]),
                        LastLine = Convert.ToInt32(r["lastline"]),
                        LastCol = Convert.ToInt32(r["lastcol"]),
                    });
                }

            }
            return ret;
        }

        #endregion

    }
}
