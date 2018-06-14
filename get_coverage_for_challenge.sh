#!/usr/bin/env bash

set -x
set -e
set -u
set -o pipefail

SCRIPT_CURRENT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

CHALLENGE_ID=$1
DOTNET_TEST_REPORT_CSV_FILE="${SCRIPT_CURRENT_DIR}/coverage/results.csv"
DOTNET_CODE_COVERAGE_INFO="${SCRIPT_CURRENT_DIR}/coverage.tdl"

( cd ${SCRIPT_CURRENT_DIR} && \
    xbuild befaster.sln || true 1>&2 )

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
