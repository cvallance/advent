#!/bin/bash

set -euf -o pipefail

cargo build --release

days=$(find target/release -maxdepth 1 -type f -executable | sort)

for day in $days; do
    echo "*** $day ***"
    echo ''
    time $day;
    echo ''
done
