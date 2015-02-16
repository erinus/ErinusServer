@ECHO OFF
CLS

SET DIRE=%~dp0

CD "%DIRE%"

certutil -p ESServer -importPFX ESServer.pfx

netsh http add urlacl url=https://+:12345/ user=Everyone

netsh http add sslcert ipport=0.0.0.0:12345 certhash=DA62DDAEF3949EC8D1D3472B71FDE5B7094E54B6 appid={30b7e476-c039-435c-b3a3-b0f8f200dece} clientcertnegotiation=enable

certutil -verifyStore my

PAUSE