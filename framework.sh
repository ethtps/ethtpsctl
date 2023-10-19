#!/bin/bash

bash_framework_name="bash-oo-framework"
bash_framework_repo="https://github.com/niieani/bash-oo-framework"

framework_installed() {
    if [ -d "./lib" ]; then
        return 0
    else
        return 1
    fi
}

git_installed() {
    if [ -x "$(command -v git)" ]; then
        return 0
    else
        return 1
    fi
}

install_framework() {
    if ! git_installed; then
        echo "Git not installed"
        echo "Please install git and try again"
        exit 1
    fi
    git clone "${bash_framework_repo}" >/dev/null 2>&1
    mv "${bash_framework_name}/lib" .
    rm -rf "${bash_framework_name}"
}

bootstrap_framework() {
    if ! framework_installed; then
        echo "Framework not installed"
        echo "Installing framework..."
        install_framework
        echo "Framework installed"
    fi
}
