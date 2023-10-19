#!/usr/bin/env bash
# shellcheck source=/dev/null
#source ".$(cd "${BASH_SOURCE[0]%/*}" && pwd)/lib/oo-bootstrap.sh"

import util/variable
import util/type
import util/command
import Array
import String
import util/namedParameters
import util/class
import util/exception
import util/tryCatch
import util/log

namespace framework/preprocessor
Log::AddOutput framework/preprocessor INFO

dir="$(cd "${BASH_SOURCE[0]%/*}" && pwd)"
pattern_file="${dir}/definitions"
replace_script="${dir}/replace.pl"

# [trace]
replace_in_file_until_done() {
  local destination=$1
  local temp_file
  temp_file=$(mktemp)
  local replacements_made=1

  while [[ ${replacements_made} -ne 0 ]]; do
    cp "${destination}" "${temp_file}"
    perl "${replace_script}" "${temp_file}" "${pattern_file}"
    replacements_made=(diff "${destination}" "${temp_file}")
    cp "${temp_file}" "${destination}"
  done

  rm "${temp_file}"
}

do_replacements() {
  @required string destination
  test -e "${destination}" || {
    destination="$1"
    test -e "${destination}" || {
      Log "Destination invalid. Got '${destination}'. Skipping..."
      return 1
    }
  }
  test -e "${destination}" || {
    Log "File not found: '${destination}'. Skipping..."
    return 1
  }
  Log "Preprocessing file '${destination}'..."
  replace_in_file_until_done "${destination}"
}
