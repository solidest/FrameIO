using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentOneofInto : SegmentBaseRun
    {
        ushort _by_seg_idx;
        ushort _first_case_idx;
        ushort _last_case_idx;

        internal SegmentOneofInto(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_by_segment = 16;
            const byte pos_first_caseitem = 32;
            const byte pos_last_caseitem = 48;
            _by_seg_idx = GetTokenUShort(token, pos_by_segment);
            _first_case_idx = GetTokenUShort(token, pos_first_caseitem);
            _last_case_idx = GetTokenUShort(token, pos_last_caseitem);
        }

        #region --Pack--

        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            ushort selected_idx = 0;
            var selecteditem = GetSelectedItem(value_buff, out selected_idx, ir);
            if (selecteditem == null) return 0;
            selecteditem.GetBitLen(value_buff, ref bitlen, ir.GetSetValueInfo(selected_idx), ir);
            return selecteditem.OutOneOfIdx;
        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            ushort selected_idx = 0;
            var selecteditem = GetSelectedItem(value_buff, out selected_idx, ir);
            if (selecteditem == null) return 0;
            selecteditem.Pack(value_buff, pack, ref odd, ref odd_pos, ir.GetSetValueInfo(selected_idx), ir);
            return selecteditem.OutOneOfIdx;
        }

        private SegmentOneofItem GetSelectedItem(MemoryStream value_buff, out ushort selected_idx, IPackRunExp ir)
        {
            var byvalue = FrameRuntime.Run[_by_seg_idx].GetValue(value_buff, ir.GetSetValueInfo(_by_seg_idx), ir);
            ushort default_idx = 0;
            selected_idx = 0;
            for (ushort i = _first_case_idx; i <= _last_case_idx; i++)
            {
                var item = (SegmentOneofItem)FrameRuntime.Run[i];
                if (item.IsDefault)
                    default_idx = i;
                else if (item.IntoValue == byvalue)
                {
                    selected_idx = i;
                    break;
                }
            }

            if (selected_idx == 0 && default_idx == 0) return null;
            if (selected_idx == 0) selected_idx = default_idx;
            return (SegmentOneofItem)FrameRuntime.Run[selected_idx];
        }

        #endregion

        #region --Unpack--

        private SegmentOneofItem TryGetSelectedItem(byte[] buff, out ushort selected_idx, IUnpackRunExp ir)
        {
            selected_idx = 0;
            double bydvalue=0;
            if(!FrameRuntime.Run[_by_seg_idx].TryGetValue(ref bydvalue, buff, ir.GetUnpackInfo(_by_seg_idx))) return null;
            long byvalue = (long)bydvalue;
            ushort default_idx = 0;

            for (ushort i = _first_case_idx; i <= _last_case_idx; i++)
            {
                var item = (SegmentOneofItem)FrameRuntime.Run[i];
                if (item.IsDefault)
                    default_idx = i;
                else if (item.IntoValue == byvalue)
                {
                    selected_idx = i;
                    break;
                }
            }

            if (selected_idx == 0 && default_idx == 0) return null;
            if (selected_idx == 0) selected_idx = default_idx;
            return (SegmentOneofItem)FrameRuntime.Run[selected_idx];
        }

        internal override bool TryGetBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            ushort selected_idx = 0;
            var selecteditem = TryGetSelectedItem(buff, out selected_idx, ir);
            if (selecteditem == null) return false;
            var res = selecteditem.TryGetBitLen(buff, ref bitlen, ref nextseg, ir.GetUnpackInfo(selected_idx), ir);
            if(res)
            {
                nextseg = selecteditem.OutOneOfIdx;
                return true;
            }
            return false;
        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            ushort selected_idx = 0;
            var selecteditem = TryGetSelectedItem(buff, out selected_idx, ir);
            if (selecteditem == null) throw new Exception("runtime");
            var res = selecteditem.Unpack(buff, ref pos_bit, end_bit_pos, ir.GetUnpackInfo(selected_idx), ir);
            if (res == ushort.MaxValue)
                return ushort.MaxValue;
            else
                return selecteditem.OutOneOfIdx;
        }


        #endregion

    }
}
