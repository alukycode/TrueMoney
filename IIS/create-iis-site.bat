REM Define variables
set hostspath=%windir%\System32\drivers\etc\hosts
set appcmdpath=%windir%\System32\inetsrv
pushd "%CD%\..\"
set sitedir="%CD%\TrueMoney\TrueMoney.Web"
set project=trueMoney

REM Delete site and pool
%appcmdpath%\appcmd.exe delete site /site.name:"tofi-%project%"
%appcmdpath%\appcmd.exe delete apppool /apppool.name:"tofi-%project%"

REM Create AppPool
%appcmdpath%\appcmd.exe add apppool /name:"tofi-%project%"
%appcmdpath%\appcmd.exe set apppool /apppool.name:"tofi-%project%" /processModel.identityType:NetworkService
%appcmdpath%\appcmd.exe set apppool /apppool.name:"tofi-%project%" /enable32BitAppOnWin64:False
%appcmdpath%\appcmd.exe set apppool /apppool.name:"tofi-%project%" /managedPipelineMode:Integrated
%appcmdpath%\appcmd.exe set apppool /apppool.name:"tofi-%project%" /managedRuntimeVersion:v4.0
%appcmdpath%\appcmd.exe set apppool /apppool.name:"tofi-%project%" /autoStart:True
%appcmdpath%\appcmd.exe start apppool /apppool.name:"tofi-%project%"

REM Create site
%appcmdpath%\appcmd.exe add site /name:"tofi-%project%" /physicalPath:"%sitedir%"
%appcmdpath%\appcmd.exe set app /app.name:"tofi-%project%/" /applicationPool:"tofi-%project%"

REM Define bindings (leave first as)
%appcmdpath%\appcmd set site /site.name:"tofi-%project%" /+bindings.[protocol='http',bindingInformation='*:80:trueMoney.local']

REM start site
%appcmdpath%\appcmd.exe start site /site.name:"tofi-%project%"

@echo off
REM Add hosts to host file
echo. >> %hostspath%
echo 127.0.0.1		trueMoney.local >> %hostspath%
