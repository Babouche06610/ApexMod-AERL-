# AERL smoke tests

`AERL.SmokeTests` is a dependency-free console smoke-test project. It validates the local preset store, session history, plugin manifest catalog and the complete Mock Stats API stream/commands.

Run it with:

```powershell
dotnet run --project .\tests\AERL.SmokeTests\AERL.SmokeTests.csproj -c Release
```

The successful final line is `AERL_SMOKE_TESTS_OK`.
