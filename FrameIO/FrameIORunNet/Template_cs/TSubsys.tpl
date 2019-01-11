
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;
using System;

namespace <%project%>
{
    public partial class <%system%>
    {

		//属性声明
		<%propertydeclare%>

		//属性初始化
		private void InitialParameter()
		{
			<%propertyinitial%>
		}

		//通道声明
        <%channeldeclare%>

		//通道初始化
		<%channelinitial%>

		//异常处理接口
        <%exceptionhandler%>

		//数据发送
		<%sendactionlist%>

		//数据接收
		<%recvactionlist%>

	}
}
