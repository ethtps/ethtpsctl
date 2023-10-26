# ethtpsctl

## A simple tool to set up, control and monitor ETHTPS instances and environments

### Based on the awesome [Spectre.Console](https://spectreconsole.net/)

### Pre-requisites

- [dotnet 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation

1. Clone the repository

```bash
git clone https://github.com/ethtps/ethtpsctl
```

2. Build the script

```bash
cd ethtpsctl

dotntet restore

dotnet build --configuration Release --project=ethtpsctl.sln
```

3. Add the script to your path

```bash
export PATH=$PATH:"$(pwd)/bin/Release/net8.0"
```

### Usage

```bash
ethtpsctl [OPTIONS] COMMAND [ARGS]...
```


### Options

```bash
  --help  Show this message and exit.
```
