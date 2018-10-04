# team2

Building the C# files resulted in .exe and .dll files, which are unable to be run on Linux machines. We also discovered that when we tried to run dotnet run on the Linux machines, we were not able to. The errors we received were as following: 

./assignment1: /usr/lib64/libstdc++.so.6: version `GLIBCXX_3.4.18' not found (required by ./assignment1)
./assignment1: /usr/lib64/libstdc++.so.6: version `GLIBCXX_3.4.17' not found (required by ./assignment1)
./assignment1: /usr/lib64/libstdc++.so.6: version `CXXABI_1.3.5' not found (required by ./assignment1)
./assignment1: /usr/lib64/libstdc++.so.6: version `GLIBCXX_3.4.14' not found (required by ./assignment1)
./assignment1: /usr/lib64/libstdc++.so.6: version `GLIBCXX_3.4.15' not found (required by ./assignment1)
This is what we saw on the tlab machines.

![alt text](/assign1/assets/bug.png)

A solution that we're trying to get to work on the tlab machines is Mono. Mono is an open source implementation of Microsoft's .NET Framework that would allow us to run our C# code on a Linux machine. Prof. Dimoulas has noted that we can upload our current code and try to get Mono to run on the tlab machines.
