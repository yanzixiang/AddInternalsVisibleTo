向 dll 文件添加 InternalsVisibleToAttribute

# 原因和目的
在分析 siemens 的 TIA 的结构和 Kuka 的 SmartHMI 时发现，他们的代码都使用internal 关键字来限制其他 dll 文件对其内部成员的使用，想要对 TIA 和 SmartHMI 进行改动的话，就算想要改动的地方很少，也需要重新解包和编译一整个 dll 文件，多则几千个文件，少则几十个文件，这个过程很浪费时间。直接添加一个 InternalsVisibleToAttribute 标签可以解决这个问题。

# 用法
使用时将 AddInternalsVisibleTo.exe, Mono.Cecil.dll, Mono.Cecil.Mdb.dll, Mono.Cecil.Pdb.dll, Mono.Cecil.Rocks.dll 都复制到想要修改的 dll 文件所在的文件夹下，然后运行

+ AddInternalsVisibleTo.exe A.dll B
其中 A.dll 是想要修改的 dll 文件, B 是想要访问 A.dll 中 internal 成员的另外一个 dll。

+ AddInternalsVisibleTo.exe A.dll B PUBKEY
其中 A.dll 是想要修改的 dll 文件, B 是想要访问 A.dll 中 internal 成员的另外一个 dll, PUBKEY为公钥。

# 合法吗？
当前不合法，只用于测试使用哦。所有的法律责任请自负。

<a href="http://www.wtfpl.net/"><img
       src="http://www.wtfpl.net/wp-content/uploads/2012/12/wtfpl-badge-4.png"
       width="80" height="15" alt="WTFPL" /></a>
