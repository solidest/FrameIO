Tags：FrameIO

FrameIO更新记录
====

---

[TOC]

---

###版本：v3.5.1

####更新内容
1. 生成代码中枚举不再作为int使用
2. ui操作模式下增加注释功能
3. 修正第一次使用时使用datagrid出现的问题
4. 生成的代码缩进全部使用4个空格
5. 包长校验增加对部分字段校验的功能

    >//默认校验从0字节至校验字段之前的内容
    integer SEG_CHECK signed=false check=sum32_false;
    
    >//校验从字段SEGA至SEGB之间的内容 注意：SEGA和SEGB均为基础类型
    integer SEG_CHECK signed=false check=sum32_false checkrange=(SEGA,SEGB);
6. 设置自动计算包长功能

    >//整包长度
    integer SEG_LEN value=bytesizeof(this);
    
    >//不含包头
    integer SEG_LEN value=bytesizeof(this)-bytesizeof(SEG_HEADER)；

####遗留问题
 1. datagrid插入新行时自动复用上一行数据
    由于目前选用的datagrid控件只能绑定数据对象，不支持对单元格数据的操作，所以这个功能无法实现


###版本3.5.2

####更新内容
1. 增加子系统功能 
      分系统属性类型可以引用另外一个分系统
      action中设置数据帧字段与属性关联时可以引用子系统内属性
      语法检查、图形编辑界面、c#代码生成均增加相关内容

2. 分系统属性增加枚举类型
3. 分系统属性 set设置为私有
4. 通道设置全部增加超时时间，有默认值，读取的时候超时抛异常
5. 修复校验算法bug
6. 修复图形界面下保存会失效bug
7. 运行时，runtime错误提示进行细化

####遗留问题
1. 当前版本不支持配置子协议数组
2. 当前版本未实现根据数据帧帧头进行匹配从通道中读取数据的功能
以上遗留问题因涉及运行时库（runtime.dll）的底层架构，需要对现有的程序进行大幅改动。故留待C++版本开发过程中对底层架构进行调整后，再实现相应功能，并与最终版一并交付。





