#!/usr/bin/env bash

day=$1
dotnet new console -o Day${day}
rm ./Day${day}/Program.cs
sed "s/DayX/Day${day}/" ./templates/Program.cs > ./Day${day}/Program.cs
sed "s/DayX/Day${day}/" ./templates/Solver.cs > ./Day${day}/Solver.cs
dotnet sln add Day${day}

dotnet new xunit -o Day${day}Tests
rm ./Day${day}Tests/UnitTest1.cs
sed "s/DayX/Day${day}/" ./templates/Tests.cs > ./Day${day}Tests/Tests.cs
dotnet add ./Day${day}Tests/Day${day}Tests.csproj reference ./Day${day}
dotnet sln add Day${day}Tests
