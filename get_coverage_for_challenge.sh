#!/usr/bin/env bash

set -x
set -e
set -u
set -o pipefail

SCRIPT_CURRENT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

CHALLENGE_ID=$1
DOTNET_TEST_REPORT_CSV_FILE="${SCRIPT_CURRENT_DIR}/coverage/results.csv"
DOTNET_CODE_COVERAGE_INFO="${SCRIPT_CURRENT_DIR}/coverage.tdl"

mkdir -p ${SCRIPT_CURRENT_DIR}/coverage

( cd ${SCRIPT_CURRENT_DIR} && \
     mono /usr/bin/nuget restore befaster.sln)
     
( cd ${SCRIPT_CURRENT_DIR} && \
     msbuild ${SCRIPT_CURRENT_DIR}/befaster.sln /p:TargetFrameworkVersion=v4.5 )

( cd ${SCRIPT_CURRENT_DIR} && \
    mono --debug \
    --profile=coverage \
    packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe \
    --framework=mono-4.0 \
    ${SCRIPT_CURRENT_DIR}/src/BeFaster.App.Tests/bin/Debug/BeFaster.App.Tests.dll || true 1>&2 )
# <--- PLACEHOLDER FOR COVERAGE REPORT GENERATION AND PARSING --->
[ -e ${DOTNET_CODE_COVERAGE_INFO} ] && rm ${DOTNET_CODE_COVERAGE_INFO}

if [ -f "${DOTNET_TEST_REPORT_CSV_FILE}" ]; then
    TOTAL_COVERAGE_PERCENTAGE=$(( 0 ))
    NUMBER_OF_FILES=$(( 0 ))
    AVERAGE_COVERAGE_PERCENTAGE=$(( 0 ))

    COVERAGE_OUTPUT=$(grep "\/${CHALLENGE_ID}\/" ${DOTNET_TEST_REPORT_CSV_FILE} | tr ',' ' ' || true)
    if [[ ! -z "${COVERAGE_OUTPUT}" ]]; then
        while read coveragePerFile;
        do
            coverageForThisFile=$(echo ${coveragePerFile} | awk '{print $2}')
            TOTAL_COVERAGE_PERCENTAGE=$(( ${TOTAL_COVERAGE_PERCENTAGE} + ${coverageForThisFile} ))
            NUMBER_OF_FILES=$(( ${NUMBER_OF_FILES} + 1 ))
        done <<< ${COVERAGE_OUTPUT}
        AVERAGE_COVERAGE_PERCENTAGE=$(( ${TOTAL_COVERAGE_PERCENTAGE} / ${NUMBER_OF_FILES} ))
    fi

    echo $((AVERAGE_COVERAGE_PERCENTAGE)) > ${DOTNET_CODE_COVERAGE_INFO}
    cat ${DOTNET_CODE_COVERAGE_INFO}
    exit 0
else
    echo "No coverage report was found"
    exit -1
fi
