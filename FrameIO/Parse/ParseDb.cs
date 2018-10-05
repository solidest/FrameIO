using SQLiteWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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

        #region --CreateDbCmd--

        private const string _createDbSqlCmd = @"
--
-- File generated with SQLiteStudio v3.2.1 on 周五 10月 5 12:03:24 2018
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
CREATE TABLE fio_frame (projectid INTEGER, namesyid INTEGER);

-- Table: fio_frame_exp
CREATE TABLE fio_frame_exp (projectid INTEGER, propertyid INTEGER, op INTEGER, leftsyid INTEGER, rightsyid INTEGER, valuesyid INTEGER);

-- Table: fio_frame_oneof
CREATE TABLE fio_frame_oneof (projectid INTEGER, proertyid INTEGER, itemsyid INTEGER, framesyid INTEGER);

-- Table: fio_frame_segment
CREATE TABLE fio_frame_segment (projectid INTEGER, frameid INTEGER, namesyid INTEGER, segmenttype INTEGER);

-- Table: fio_frame_segment_property
CREATE TABLE fio_frame_segment_property (projectid INTEGER, segmentid INTEGER, proname INTEGER, protype INTEGER, ivalue INTEGER);

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
CREATE TABLE fio_sys_property (projectid INTEGER, sysid INTEGER, namesyid INTEGER, propertytype INTEGER, isarray INTEGER);

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
            return ret.ToString();
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
                    Propertys = LoadProperty(ssid),
                    Channels = LoadChannel(ssid),
                    Actions = LoadAction(ssid)
                };
                if (_checkSemantics && ret.Where(p => p.Name == ss.Name).Count() > 0)
                {
                    AddError(syid, 3);
                }
                ret.Add(ss);
            }

            return ret;
        }


        //检查通道设置是否正确
        private void CheckChannels()
        {
            //TODO
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
                    Options = LoadChannelOptions(chid)
                };
                if (_checkSemantics && ret.Where(p => p.Name == ch.Name).Count() > 0)
                {
                    AddError(syid, 4);
                }
                if (_checkSemantics) CheckChannels();
                ret.Add(ch);
            }

            return ret;
        }


        //加载通道选项
        private ObservableCollection<SubsysChannelOption>  LoadChannelOptions(int chid)
        {
            var ret = new ObservableCollection<SubsysChannelOption>();
            var tb = _db.ExecuteQuery("SELECT nameid, valuesyid, symbol FROM fio_sys_channel_option op LEFT JOIN fio_symbol sy ON op.valuesyid=sy.rowid WHERE op.channelid="
                + chid.ToString());
            foreach(DataRow r in tb.Rows)
            {
                var op = new SubsysChannelOption()
                {
                    Notes = LoadNotes(Convert.ToInt32(r["valuesyid"])),
                    OptionType = (channeloptiontype)Convert.ToInt32(r["nameid"]),
                    OptionValue = r["symbol"].ToString()
                };
                if (ret.Where(p => p.OptionType == op.OptionType).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["valuesyid"]), 5);
                }
                ret.Add(op);
            }
            return ret;
        }

        //检查IO操作设置是否正确
        private void CheckActions()
        {
            //TODO 
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
                var acid = Convert.ToInt32(r["rowid"]);
                var ac = new SubsysAction()
                {
                    ChannelName = r["channelname"].ToString(),
                    FrameName = r["framename"].ToString(),
                    IOType = (actioniotype)Convert.ToInt32(r["actiontype"]),
                    Name = r["name"].ToString(),
                    Notes = LoadNotes(Convert.ToInt32(r["namesyid"])),
                    Maps = LoadActionMaps(acid)
                };
                if (_checkSemantics && ret.Where(p => p.Name == ac.Name).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["namesyid"]), 6);
                }
                ret.Add(ac);
            }
            if (_checkSemantics) CheckActions();
            return ret;
        }


        //加载IO操作 映射
        public ObservableCollection<SubsysActionMap> LoadActionMaps(int acid)
        {
            var ret = new ObservableCollection<SubsysActionMap>();
            var tb = _db.ExecuteQuery("SELECT mp.rowid, segsyid, sy1.symbol segname, sy2.symbol proname FROM fio_sys_action_map mp LEFT JOIN fio_symbol sy1 ON mp.segsyid=sy1.rowid LEFT JOIN fio_symbol sy2 ON mp.sysprosyid=sy2.rowid WHERE mp.actionid="
                + acid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var mid = Convert.ToInt32(r["rowid"]);
                var mp = new SubsysActionMap()
                {
                    FrameSegName = r["segname"].ToString(),
                    Notes = LoadNotes(Convert.ToInt32(r["segsyid"])),
                    SysPropertyName = r["proname"].ToString()
                };
                if (_checkSemantics && ret.Where(p => p.FrameSegName == mp.FrameSegName).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["segsyid"]), 7);
                }
                ret.Add(mp);
            }

            return ret;
        }

        //加载受控对象属性
        private ObservableCollection<SubsysProperty>LoadProperty(int ssid)
        {
            var ret = new ObservableCollection<SubsysProperty>();
            var tb = _db.ExecuteQuery("SELECT namesyid, sy.symbol name, propertytype, isarray FROM fio_sys_property pt LEFT JOIN fio_symbol sy ON pt.namesyid=sy.rowid WHERE pt.sysid="
               + ssid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var sp = new SubsysProperty()
                {
                    IsArray = Convert.ToInt32(r["isarray"]) == 0 ? false : true,
                    Name = r["name"].ToString(),
                    Notes = LoadNotes(Convert.ToInt32(r["namesyid"])),
                    PropertyType = (syspropertytype)Convert.ToInt32(r["propertytype"])
                };
                if (_checkSemantics && ret.Where(p => p.Name == sp.Name).Count() > 0)
                {
                    AddError(Convert.ToInt32(r["namesyid"]), 8);
                }
                ret.Add(sp);
            }

            return ret;
        }


        //加载数据帧
        private ObservableCollection<Frame> LoadFrame(int projectid)
        {
            var ret = new ObservableCollection<Frame>();

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

        //添加错误信息到列表
        private void AddError(int syid, int errcode)
        {
            var r = _db.ExecuteQuery("SELECT lineno, firstcolumn, lastcolumn FROM fio_symbol WHERE rowid=" + syid.ToString()).Rows[0];
            var il = Convert.ToInt32(r["lineno"]);
            var ifc = Convert.ToInt32(r["firstcolumn"]);
            var ilc = Convert.ToInt32(r["lastcolumn"]);

            _err.Add(new ParseError()
            {
                ErrorCode = errcode,
                FirstLine = il,
                LastLine = il,
                FirstCol = ifc,
                LastCol = ilc
            });
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
