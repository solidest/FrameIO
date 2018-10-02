using SQLiteWrapper;
using System;
using System.Collections.Generic;
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

        //创建项目
        public int CreateProject(string code)
        {
            _db.ExecuteNonQuery("INSERT INTO fio_project (code) VALUES(:code);", code);
            return _db.GetLastRowId();
        }

        //删除项目

        public void DeleteProject(int projectid)
        {
            _db.ExecuteNonQuery("DELETE FROM fio_project WHERE rowid = " + projectid.ToString());
            _db.ExecuteNonQuery("DELETE FROM fio_symbol WHERE projectid = " + projectid.ToString());
            _db.ExecuteNonQuery("DELETE FROM fio_error WHERE projectid = " + projectid.ToString());
            //TODO delete other table
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
                ret.Add(new ParseError()
                {
                    ErrorCode = Convert.ToInt32(r["errorcode"]),
                    FirstLine = Convert.ToInt32(r["firstline"]),
                    FirstCol = Convert.ToInt32(r["firstcol"]),
                    LastLine = Convert.ToInt32(r["lastline"]),
                    LastCol = Convert.ToInt32(r["lastcol"]),
                    FirstSymbol = r["firstsy"].ToString(),
                    LastSymbol = r["lastsy"].ToString()
                });
            }
            return ret;
        }

    }
}
