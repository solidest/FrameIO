
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace Project_com
{
    public partial class Subsystem
    {

        //属性声明
        public Parameter<Subsystem_GongZuoKongZhi?> YiJianTiaoPing { get; private set;}
        public Parameter<Subsystem_GongZuoKongZhi?> ZhiDanJiGongZuoKongZhi { get; private set;}
        public Parameter<ushort?> GuangXueCangWenDuSheDing { get; private set;}
        public Parameter<ushort?> GuangXueCangShiDuSheDing { get; private set;}
        public Parameter<Subsystem_ReceiveCommands?> ReceivedCommand { get; private set;}
        public Parameter<Subsystem_GongZuoKongZhi?> YiJianTiaoPingFanKui { get; private set;}
        public Parameter<Subsystem_GongZuoKongZhi?> ZhiDanJiGongZuoKongZhiFanKui { get; private set;}
        public Parameter<Subsystem_DingGaiKaiQiZhuangTai?> DingGaiKaiQiZhuangTai { get; private set;}
        public Parameter<Subsystem_ShengJiangPingTaiZhuangTai?> ShengJiangPingTaiZhuangTai { get; private set;}
        public Parameter<Subsystem_TiaoPingZhiTuiZhuangTai?> TiaoPingZhiTuiZhuangTai { get; private set;}
        public Parameter<Subsystem_QiDongZhuangTai?> HuanKongKaiQiZhuangTai { get; private set;}
        public Parameter<Subsystem_QiDongZhuangTai?> ZhiDanJiGongZuoZhuangTai { get; private set;}
        public Parameter<Subsystem_KongTiaoGongZuoZhuangTai?> GuangXueCangKongTiaoGongZuoZhuangTai { get; private set;}
        public Parameter<Subsystem_QiDongZhuangTai?> JiaReQiGongZuoZhuangTai { get; private set;}
        public Parameter<Subsystem_QiDongZhuangTai?> ChuShiQiGongZuoZhuangTai { get; private set;}
        public Parameter<int?> Raw_TiaoPingChuanGanQiX { get; private set;}
        public Parameter<int?> Raw_TiaoPingChuanGanQiY { get; private set;}
        public Parameter<int?> Raw_GuangXueCangNeiWenDu { get; private set;}
        public Parameter<int?> Raw_GuangXueCangNeiShiDu { get; private set;}
        public Parameter<int?> Raw_CangWaiWenDu { get; private set;}
        public Parameter<int?> Raw_CangWaiShiDu { get; private set;}
        public Parameter<CheShouZhuangTai?> CheShouZhuangTai { get; private set;}
        public Parameter<Subsystem_GuZhangDaiMa?> GuZhangDaiMa { get; private set;}
        public Parameter<byte?> BaoJingZhuangTai { get; private set;}
        public Parameter<ZiJianZhuangTai?> ZiJianZhuangTai { get; private set;}
        public Parameter<ushort?> GuangXueCangWenDuSheDingFanKui { get; private set;}
        public Parameter<ushort?> GuangXueCangShiDuSheDingFanKui { get; private set;}

        //属性初始化
        public void InitialParameter()
        {
            YiJianTiaoPing = new Parameter<Subsystem_GongZuoKongZhi?>();
            ZhiDanJiGongZuoKongZhi = new Parameter<Subsystem_GongZuoKongZhi?>();
            GuangXueCangWenDuSheDing = new Parameter<ushort?>();
            GuangXueCangShiDuSheDing = new Parameter<ushort?>();
            ReceivedCommand = new Parameter<Subsystem_ReceiveCommands?>();
            YiJianTiaoPingFanKui = new Parameter<Subsystem_GongZuoKongZhi?>();
            ZhiDanJiGongZuoKongZhiFanKui = new Parameter<Subsystem_GongZuoKongZhi?>();
            DingGaiKaiQiZhuangTai = new Parameter<Subsystem_DingGaiKaiQiZhuangTai?>();
            ShengJiangPingTaiZhuangTai = new Parameter<Subsystem_ShengJiangPingTaiZhuangTai?>();
            TiaoPingZhiTuiZhuangTai = new Parameter<Subsystem_TiaoPingZhiTuiZhuangTai?>();
            HuanKongKaiQiZhuangTai = new Parameter<Subsystem_QiDongZhuangTai?>();
            ZhiDanJiGongZuoZhuangTai = new Parameter<Subsystem_QiDongZhuangTai?>();
            GuangXueCangKongTiaoGongZuoZhuangTai = new Parameter<Subsystem_KongTiaoGongZuoZhuangTai?>();
            JiaReQiGongZuoZhuangTai = new Parameter<Subsystem_QiDongZhuangTai?>();
            ChuShiQiGongZuoZhuangTai = new Parameter<Subsystem_QiDongZhuangTai?>();
            Raw_TiaoPingChuanGanQiX = new Parameter<int?>();
            Raw_TiaoPingChuanGanQiY = new Parameter<int?>();
            Raw_GuangXueCangNeiWenDu = new Parameter<int?>();
            Raw_GuangXueCangNeiShiDu = new Parameter<int?>();
            Raw_CangWaiWenDu = new Parameter<int?>();
            Raw_CangWaiShiDu = new Parameter<int?>();
            CheShouZhuangTai = new Parameter<CheShouZhuangTai?>();
            GuZhangDaiMa = new Parameter<Subsystem_GuZhangDaiMa?>();
            BaoJingZhuangTai = new Parameter<byte?>();
            ZiJianZhuangTai = new Parameter<ZiJianZhuangTai?>();
            GuangXueCangWenDuSheDingFanKui = new Parameter<ushort?>();
            GuangXueCangShiDuSheDingFanKui = new Parameter<ushort?>();
        }

        //通道声明
        public FioChannel CH_COM3;
        public FioChannel CH_COM4;
        
        //通道初始化
        public void InitialChannelCH_COM3(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("portname")) ops.SetOption("portname", "COM3");
            if (!ops.Contains("baudrate")) ops.SetOption("baudrate", 9600);
            if (!ops.Contains("parity")) ops.SetOption("parity", 0);
            if (!ops.Contains("databits")) ops.SetOption("databits", 8);
            if (!ops.Contains("stopbits")) ops.SetOption("stopbits", 1);
            ops.SetOption("$channeltype", 1);
            CH_COM3 = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCH_COM4(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("baudrate")) ops.SetOption("baudrate", 9600);
            if (!ops.Contains("portname")) ops.SetOption("portname", "COM4");
            if (!ops.Contains("parity")) ops.SetOption("parity", 0);
            if (!ops.Contains("databits")) ops.SetOption("databits", 8);
            if (!ops.Contains("stopbits")) ops.SetOption("stopbits", 1);
            ops.SetOption("$channeltype", 1);
            CH_COM4 = FioNetRunner.GetChannel(ops);
        }

        //异常处理接口
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

        //数据发送
        

        //数据接收
        public void ReceiveFrame()
        {
            var __v__ = FioNetRunner.RecvFrame("Subsystem_ReceiveFrame", CH_COM4);
            ReceivedCommand.Value = (Subsystem_ReceiveCommands)__v__.GetValue("ReceivedCommand");
            switch((Subsystem_ReceiveCommands)__v__.GetValue("ReceivedCommand"))
            {
                case Subsystem_ReceiveCommands.YiJianTiaoPingFanKui:
                {
                    YiJianTiaoPingFanKui.Value = (Subsystem_GongZuoKongZhi)__v__.GetValue("Data.YiJianTiaoPingFanKui.YiJianTiaoPingFanKui");
                    break;
                }
                case Subsystem_ReceiveCommands.ZhiDanJiGongZuoKongZhiFanKui:
                {
                    ZhiDanJiGongZuoKongZhiFanKui.Value = (Subsystem_GongZuoKongZhi)__v__.GetValue("Data.ZhiDanJiGongZuoKongZhiFanKui.ZhiDanJiGongZuoKongZhiFanKui");
                    break;
                }
                case Subsystem_ReceiveCommands.ZhuangTaiShangBao:
                {
                    DingGaiKaiQiZhuangTai.Value = (Subsystem_DingGaiKaiQiZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.DingGaiKaiQiZhuangTai");
                    ShengJiangPingTaiZhuangTai.Value = (Subsystem_ShengJiangPingTaiZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.ShengJiangPingTaiZhuangTai");
                    TiaoPingZhiTuiZhuangTai.Value = (Subsystem_TiaoPingZhiTuiZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.TiaoPingZhiTuiZhuangTai");
                    HuanKongKaiQiZhuangTai.Value = (Subsystem_QiDongZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.HuanKongKaiQiZhuangTai");
                    ZhiDanJiGongZuoZhuangTai.Value = (Subsystem_QiDongZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.ZhiDanJiGongZuoZhuangTai");
                    GuangXueCangKongTiaoGongZuoZhuangTai.Value = (Subsystem_KongTiaoGongZuoZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.GuangXueCangKongTiaoGongZuoZhuangTai");
                    JiaReQiGongZuoZhuangTai.Value = (Subsystem_QiDongZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.JiaReQiGongZuoZhuangTai");
                    ChuShiQiGongZuoZhuangTai.Value = (Subsystem_QiDongZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.ChuShiQiGongZuoZhuangTai");
                    __v__.GetValue("Data.ZhuangTaiShangBao.Raw_TiaoPingChuanGanQiX", Raw_TiaoPingChuanGanQiX);
                    __v__.GetValue("Data.ZhuangTaiShangBao.Raw_TiaoPingChuanGanQiY", Raw_TiaoPingChuanGanQiY);
                    __v__.GetValue("Data.ZhuangTaiShangBao.Raw_GuangXueCangNeiWenDu", Raw_GuangXueCangNeiWenDu);
                    __v__.GetValue("Data.ZhuangTaiShangBao.Raw_GuangXueCangNeiShiDu", Raw_GuangXueCangNeiShiDu);
                    __v__.GetValue("Data.ZhuangTaiShangBao.Raw_CangWaiWenDu", Raw_CangWaiWenDu);
                    __v__.GetValue("Data.ZhuangTaiShangBao.Raw_CangWaiShiDu", Raw_CangWaiShiDu);
                    CheShouZhuangTai.Value = (CheShouZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.CheShouZhuangTai");
                    GuZhangDaiMa.Value = (Subsystem_GuZhangDaiMa)__v__.GetValue("Data.ZhuangTaiShangBao.GuZhangDaiMa");
                    __v__.GetValue("Data.ZhuangTaiShangBao.BaoJingZhuangTai", BaoJingZhuangTai);
                    ZiJianZhuangTai.Value = (ZiJianZhuangTai)__v__.GetValue("Data.ZhuangTaiShangBao.ZiJianZhuangTai");
                    break;
                }
                case Subsystem_ReceiveCommands.GuangXueCangWenDuSheDingFanKui:
                {
                    __v__.GetValue("Data.GuangXueCangWenDuSheDingFanKui.GuangXueCangWenDuSheDingFanKui", GuangXueCangWenDuSheDingFanKui);
                    break;
                }
                case Subsystem_ReceiveCommands.GuangXueCangShiDuSheDingFanKui:
                {
                    __v__.GetValue("Data.GuangXueCangShiDuSheDingFanKui.GuangXueCangShiDuSheDingFanKui", GuangXueCangShiDuSheDingFanKui);
                    break;
                }
            }
        }

    }
}
