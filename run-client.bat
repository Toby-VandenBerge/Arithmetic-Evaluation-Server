@echo off

pushd "%~dp0src\Arithmetic.Evaluation.Client\"
start "Client" dotnet run
popd