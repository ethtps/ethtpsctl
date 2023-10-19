#!/usr/bin/env bash

source "$( cd "${BASH_SOURCE[0]%/*}" && pwd )/../../lib/oo-bootstrap.sh"

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

namespace ethtpsctl/framework/types
Log::AddOutput ethtpsctl/framework/types STDERR
Log::AddOutput ethtpsctl/framework/types INFO

class:Dependency() {
  public string name
  public string absoluteOrRelativePath="./"

  Dependency.ToString(){
    @return:value "$(this absoluteOrRelativePath)$(this name)"
  }
}

Type::Initialize Dependency