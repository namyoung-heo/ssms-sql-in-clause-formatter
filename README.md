# SqlInClauseFormatter

A SQL Server Management Studio (SSMS) extension that converts line-separated values into SQL IN clause format with a single keyboard shortcut.

## Before & After

**Input** (selected text in query editor):
```
A
B
C
```

**Output** (replaced instantly):
```sql
(
'A',
'B',
'C'
)
```

## Features

- Converts newline-separated values into SQL IN clause syntax
- Wraps each value in single quotes with proper escaping (`O'Brien` → `'O''Brien'`)
- Trims whitespace and ignores blank lines
- Works directly in the SSMS query editor — select text, press shortcut, done

## Keyboard Shortcut

`Ctrl+Alt+I` (two-step chord shortcut)

## Installation

### From .vsix file

1. Download `SqlInClauseFormatter.vsix` from [Releases](https://github.com/namyoung-heo/ssms-sql-in-clause-formatter/releases)
2. Close SSMS
3. Double-click the `.vsix` file to install
4. Restart SSMS

### Build from source

1. Open `SqlInClauseFormatter.sln` in Visual Studio 2019 or 2022 (with VS Extension development workload)
2. Build in Release mode
3. Find `SqlInClauseFormatter.vsix` in `bin/Release/`
4. Close SSMS, double-click the `.vsix` file, and restart SSMS

## Requirements

- SSMS 19.x (Visual Studio 2019 Shell based)
- .NET Framework 4.7.2

## Usage

1. Open a query editor in SSMS
2. Select the text you want to convert (line-separated values)
3. Press `Ctrl+ALT+I`
4. The selected text is replaced with the IN clause format

## License

[MIT](LICENSE)
