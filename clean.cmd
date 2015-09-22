@ECHO OFF
CLS

RD /S /Q ".\ESServer\bin"
RD /S /Q ".\ESServer\obj"
RD /S /Q ".\ESServer.Controller\bin"
RD /S /Q ".\ESServer.Controller\obj"
RD /S /Q ".\ESServer.Session\bin"
RD /S /Q ".\ESServer.Session\obj"
RD /S /Q ".\ESServer.Session.Store\bin"
RD /S /Q ".\ESServer.Session.Store\obj"
RD /S /Q ".\ESServer.Session.Store.Memory\bin"
RD /S /Q ".\ESServer.Session.Store.Memory\obj"
RD /S /Q ".\ESServer.Session.Store.MongoDB\bin"
RD /S /Q ".\ESServer.Session.Store.MongoDB\obj"
RD /S /Q ".\ESServer.Session.Store.Redis\bin"
RD /S /Q ".\ESServer.Session.Store.Redis\obj"

DEL /Q ".\bin\*.exp"
DEL /Q ".\bin\*.lib"
DEL /Q ".\bin\*.pdb"
DEL /Q ".\bin\*.vshost.exe"
DEL /Q ".\bin\*.vshost.exe.config"
DEL /Q ".\bin\*.vshost.exe.manifest"
