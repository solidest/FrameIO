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

        public ParseDb()
        {
            _db = new SQLiteWrapper.SQLiteBase();
            _db.OpenDatabase(@"file:C:/Kiyun/FrameIO/FrameIO/x64/Debug/frameio.db");
            
        }

        #region --Project Element--


        //加载注释
        private string LoadNotes(int id)
        {
            var tb = _db.ExecuteQuery("SELECT sb.symbol notes  FROM fio_notes nt LEFT JOIN fio_symbol sb ON nt.notesyid = sb.rowid WHERE nt.aftersyid = "
                + id.ToString());
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

        //加载枚举列表
        private ObservableCollection<Enumdef> LoadEnumList(int projectid)
        {
            var ret = new ObservableCollection<Enumdef>();
            var tb = _db.ExecuteQuery("SELECT em.rowid, em.namesyid, sy.symbol ename FROM fio_enum em LEFT JOIN fio_symbol sy ON em.namesyid=sy.rowid where em.projectid="
                + projectid.ToString());
            foreach (DataRow r in tb.Rows)
            {
                var id = Convert.ToInt32(r["namesyid"]);
                var ed = new Enumdef(r["ename"].ToString());
                ed.EnumNote = LoadNotes(id);
                ed.ItemsList = LoadEnumItemList(Convert.ToInt32(r["rowid"]));
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
                var ei = new EnumdefItem() { ItemName = r["name"].ToString(), ItemValue = v, Notes = LoadNotes(Convert.ToInt32(r["namesyid"])) };
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

        //加载项目
        public IOProject LoadProject(int projectid)
        {
            var ret = new IOProject();

            var tb = _db.ExecuteQuery("SELECT namesyid, symbol projectname FROM fio_project pj LEFT JOIN fio_symbol sb ON pj.namesyid = sb.rowid WHERE projectid="
                + projectid.ToString());

            ret.ProjectName = tb.Rows[0]["projectname"].ToString();
            ret.ProjectNotes = LoadNotes(Convert.ToInt32(tb.Rows[0]["namesyid"]));
            ret.EnumdefList = LoadEnumList(projectid);

            return ret;
        }


        #endregion


        #region --Error--
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
                        FirstSymbol = "",
                        LastSymbol = ""
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
                        FirstSymbol = r["firstsy"].ToString(),
                        LastSymbol = r["lastsy"].ToString()
                    });
                }

            }
            return ret;
        }

        #endregion

    }
}
