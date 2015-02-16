@ECHO OFF
CLS

netsh http delete sslcert ipport=0.0.0.0:12345

netsh http delete urlacl url=https://+:12345/

certutil -delstore my "86 78 8a b9 09 c5 02 22"

PAUSE