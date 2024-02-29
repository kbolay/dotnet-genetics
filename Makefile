TIMESTAMP = $(shell date -u +"%Y%m%d.%H%M%S") # $(Get-Date -Format "yyyyMMddHHmmss") 
VERSION = 1.0.0

clean: 
	dotnet clean

restore:
	dotnet restore

build:
	dotnet build

test: build
	dotnet test

test-coverage:
	rm -rf tests\coverage
	rm -rf tests\report
	dotnet test --collect "XPlat Code Coverage" --results-directory "tests\coverage"
	reportgenerator -reports:"tests\coverage\*\coverage.cobertura.xml" -targetdir:"tests\report" -reporttypes:Html
	cmd /c start tests\report\index.html
	
test-timestamp:
	echo ${TIMESTAMP}

pack:
	dotnet pack --no-restore -o nuget -c Release /p:Version=${VERSION}
