
$value = Read-Host "1: update main
2: update todo
"
if ( 1 -eq $value )
{
    $ServiseName = "main"
}
elseif ( 2 -eq $value )
{
    $ServiseName = "todo"
}
else
{
    echo "Couldn't read value"
}
$FolderServiseName = $ServiseName.substring(0,1).toupper()+$ServiseName.substring(1).tolower()   
$pathFolder = "E:\arhive\Web\adviser\BackEnd"
cd $pathFolder;

docker build -f ./${FolderServiseName}/dockerfile -t ${ServiseName}:latest --build-arg FolderServiseName=${FolderServiseName} .;
docker save -o "${ServiseName}.tar" $ServiseName;
pscp -pw ${Env:WebUserPwd} "${ServiseName}.tar"  ${Env:WebUserName}@${Env:WebUserIP}:"/web-root/";
plink -batch -ssh ${Env:WebUserName}@${Env:WebUserIP} -pw ${Env:WebUserPwd} "bash /web-root/updateserver.sh $ServiseName";
del "${ServiseName}.tar";

Write-Host -NoNewLine "Coplete update ${ServiseName}. Press any key to continue...";
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
