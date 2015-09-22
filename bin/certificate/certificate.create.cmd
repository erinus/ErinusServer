@ECHO OFF
CLS

SET DIRE=%~dp0

CD "%DIRE%"

openssl.exe req -x509 -newkey rsa:2048 -out CAServer.pem -outform PEM -days 365

openssl.exe x509 -in CAServer.pem -out CAServer.crt

openssl.exe genrsa -out ESServer.key 2048

openssl.exe req -new -key ESServer.key -out ESServer.csr

openssl.exe x509 -req -in ESServer.csr -signkey ESServer.key -out ESServer.crt -hash

openssl.exe pkcs12 -export -out ESServer.pfx -inkey ESServer.key -in ESServer.crt -certfile CAServer.crt

openssl x509 -sha1 -fingerprint -in ESServer.crt

PAUSE