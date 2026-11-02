Project was created with Visual Studio 2008 SP1 -> .NET Framwork 3.5 SP1

For using the Windows.System.Interactivity library the following components have to be installed with Visual Studio:
- /Preinstall/BlendSDK_en.msi
- /Preinstall/VS90SP1-KB958017-x86.exe

From SVN Revision 399, GUI_XRS\GUI_XRS.suo is checked in. The reason for this is to build the project always for x86.
That is necessary because OpenCV.dll and System.Data.SQLite.dll are just x86. If the target "Any CPU" is choosen in
Visual Studio, the project cannnot run on x64 systems because of the x86 libraries. If the configuration for the full 
project is x86 it can run on x64 systems.

!!!---------!!!
The GUI_XRS\GUI_XRS.suo file can cause an error after building when it is tried to start: "The file app.xaml cannot be found."
To solve this, choose in Visual Studio -> Configuration Manager -> Active solution platform. Switch to "Any CPU" and
then again to "x86"
!!!---------!!!