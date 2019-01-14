private void HandleFrameIOError(Exception ex)
{
	if (ex.GetType() == typeof(FrameIOException))
	{
		switch (((FrameIOException)ex).ErrType)
		{
			case FrameIOErrorType.ChannelErr:
			case FrameIOErrorType.SendErr:
			case FrameIOErrorType.RecvErr:
			case FrameIOErrorType.CheckDtaErr:
				Debug.WriteLine("位置：{0}    错误：{1}", ((FrameIOException)ex).Position, ((FrameIOException)ex).ErrInfo);
				break;
		}
	}
	else
		Debug.WriteLine(ex.ToString());
}