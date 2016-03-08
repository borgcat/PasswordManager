%teamcity.tool.NuGet.CommandLine.DEFAULT%\tools\nuget.exe install KoreBuild -ExcludeVersion -o packages -nocache -pre -Source https://www.myget.org/F/aspnetvnext/api/v2
& packages\KoreBuild\build\kvm upgrade -runtime CLR -x86
& packages\KoreBuild\build\kvm install default -runtime CoreCLR -x86
& packages\KoreBuild\build\kvm use default -runtime CLR -x86
& %env.TEAMCITY_CAPTURE_ENV%