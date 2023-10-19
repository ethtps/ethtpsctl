#!/usr/bin/env bash
source "$(cd "${BASH_SOURCE[0]%/*}" && pwd)/../../lib/oo-bootstrap.sh"

import util/variable
import util/log
import util/exception
import util/tryCatch
import util/namedParameters
import util/class
import util/test
import util/type
import util/command
import Array
import String

namespace ethtpsctl/framework/utils
Log::AddOutput ethtpsctl/framework/utils STDERR
Log::AddOutput ethtpsctl/framework/utils INFO

command_available() {
  @required command
  if [ -x "$(command -v "${var:command}")" ]; then
    return 0
  fi
  return 1
}

dependency_installed() {
  @required command
  describe "${var:command}"
  it 'should be installed'
  try
  result=$(command_available "${var:command}")
  if ! ${result}; then
    e="${result} missing" throw
  fi
  expectPass
  summary
}

check_system_requirements() {
  @required array requirements
  describe "Dependency check"
  it 'should have all dependencies installed'
  try
  for command in ${var:requirements}; do
    ins=$(dependency_installed "${command}")
    Log "${ins}"
    if ${ins}; then
      ${var:missing_dependencies} push "${var:command}"
    fi
  done
  if [ "${var:missing_dependencies:length}" ]; then
    Log "Missing dependencies: ${var:missing_dependencies:List}"
  fi
  expectPass
  summary
}
