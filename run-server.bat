@echo off

pushd "%~dp0src\Arithmetic.Evaluation.Server\"
start "Server" dotnet run
popd