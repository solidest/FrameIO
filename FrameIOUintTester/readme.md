### 数据帧 测试用例

- 数据帧解析初始化 A__FrameRunnerInitial
- 单字节收发 A__FrameOneByte
- 字节数组收发 A__FrameByteArray
- 字节未对齐 A__FrameNotAlignByte
- bool型收发 A__FrameBool
- short收发
- ushort收发
- int收发
- uint收发
- long收发
- ulong收发
- double收发
- float收发
- 整数字段最大、最小值检查
>测试用例：20.test_integer_max_min
- 浮点字段最大、最小值检查
>测试用例：21.test_real_max_min
- 整数字段大端序、小端序测试
>测试用例：22.test_integer_big
>测试用例：23.test_integer_small
- 整数字段源码、反码、补码测试
>测试用例：24.test_integer_primitive
>测试用例：25.test_integer_inversion
- 浮点字段大端序、小端序测试
>测试用例：26.test_real_big
- 浮点字段源码、反码、补码测试
- check属性设置检查
- check属性和checkrange属性设置检查
- 包头匹配match检查
- 数据帧引用字段收发
- block 内定义字段收发 A__FrameGroup
_ block 数组收发
- oneof 匹配分支收发 A__FrameOneof
- oneof 数组收发
- oneof 嵌套使用测试
- oneof other分支收发
- oneof 下空数据帧测试
- 数据帧应用数组字段收发
- block内定义字段数组收发
- bytesizeof(字段)取字段长度测试
- bytesizeof(this)取整包长度测试
- value设置为公式测试
- 设置子系统收发测试 A__FrameSubsys
- 设置子系统数组收发 A__FrameSubsysArray




