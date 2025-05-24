#!/bin/bash

FAILURES=0
TEST_PROJECTS=$(find . -name "*.csproj" | grep -E ".*\.Tests\.csproj$")

has_param() {
    local term="$1"
    shift
    for arg; do
        if [[ $arg == "$term" ]]; then
            return 0
        fi
    done
    return 1
}

for PROJECT in $TEST_PROJECTS
do
    echo "Running tests for $PROJECT"
    if has_param "--coverage" "$@"
    then
        echo "Running with coverage"
        dotnet test ./$PROJECT --collect:"XPlat Code Coverage" --no-build --verbosity quiet --logger html --results-directory ./TestResult
    else 
        echo "Running without coverage"
        dotnet test ./$PROJECT --no-build --verbosity quiet
    fi
    
    if test "$?" != "0" 
      then 
        ((FAILURES+=1))
        echo "Tests failed for $PROJECT"
    fi
done

echo "Failures: $FAILURES"
exit $FAILURES