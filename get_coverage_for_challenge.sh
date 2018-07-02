#!/usr/bin/env bash

set -x
set -e
set -u
set -o pipefail

SCRIPT_CURRENT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

CHALLENGE_ID=$1
DOTNET_TEST_REPORT_DB_FILE="${SCRIPT_CURRENT_DIR}/coverage/tests.cfg.covdb"
DOTNET_CODE_COVERAGE_INFO="${SCRIPT_CURRENT_DIR}/coverage.tdl"

mkdir -p ${SCRIPT_CURRENT_DIR}/coverage

( cd ${SCRIPT_CURRENT_DIR} && \
     mono /usr/bin/nuget restore befaster.sln)
     
( cd ${SCRIPT_CURRENT_DIR} && \
     msbuild ${SCRIPT_CURRENT_DIR}/befaster.sln /p:buildmode=debug /p:TargetFrameworkVersion=v4.5 )
     
echo "BeFaster" > ${SCRIPT_CURRENT_DIR}/tests.cfg     

( cd ${SCRIPT_CURRENT_DIR} && \
        BABOON_CFG=tests.cfg mono ${BABOON_HOME}/covtool/bin/covem.exe \
        packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe \
            --process:InProcess \
            --domain=Single \
            --framework=mono-4.0 \
        ${SCRIPT_CURRENT_DIR}/src/BeFaster.App.Tests/bin/Debug/BeFaster.App.Tests.dll || true 1>&2 )

cp ${SCRIPT_CURRENT_DIR}/tests.cfg.covdb ${SCRIPT_CURRENT_DIR}/coverage/tests.cfg.covdb

[ -e ${DOTNET_CODE_COVERAGE_INFO} ] && rm ${DOTNET_CODE_COVERAGE_INFO}

if [ -f "${DOTNET_TEST_REPORT_DB_FILE}" ]; then
    COVERED_LINES=$(sqlite3 ${DOTNET_TEST_REPORT_DB_FILE} "select count(*) from lines where fullname like '%BeFaster.App.Solutions.${CHALLENGE_ID}.%' and hits > 0")    
    TOTAL_LINES=$(sqlite3 ${DOTNET_TEST_REPORT_DB_FILE} "select count(*) from lines where fullname like '%BeFaster.App.Solutions.${CHALLENGE_ID}.%'")

    if [[ ${TOTAL_LINES} -eq 0 ]]; then
        TOTAL_COVERAGE_PERCENTAGE=0
    else
        TOTAL_COVERAGE_PERCENTAGE=$(( ${COVERED_LINES} / ${TOTAL_LINES} * 100 ))
    fi
    
    echo $((TOTAL_COVERAGE_PERCENTAGE)) > ${DOTNET_CODE_COVERAGE_INFO}
    cat ${DOTNET_CODE_COVERAGE_INFO}
    exit 0
else
    echo "No coverage report was found"
    exit -1
fi
