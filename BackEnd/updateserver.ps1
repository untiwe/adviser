$pathFolder = "E:\arhive\Web\adviser\BackEnd"
cd $pathFolder
docker build -f ./Main/dockerfile -t main:latest .;
docker save -o main.tar main;
pscp -pw ${Env:WebUserPwd} main.tar  ${Env:WebUserName}@${Env:WebUserIP}:"/web-root/";
plink -batch -ssh ${Env:WebUserName}@${Env:WebUserIP} -pw ${Env:WebUserPwd} "bash /web-root/updateserver.sh";
del main.tar;

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
