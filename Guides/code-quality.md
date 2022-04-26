# Readability and code maintenance

- from Microsoft page we get recomended [.editorconfig](https://docs.microsoft.com/es-es/dotnet/fundamentals/code-analysis/code-style-rule-options)
- install dotnet format in your machine
```
dotnet tool install -g dotnet-format
```
- in `Scripts` folder there is `init.js` which replaces our hooks to default git hooks
- make sure to execute `init.js` in order to trigger hooks, to execute this script you need to install node
```
node Scripts/init.js
```

## Pre commit

Each time you make a commit, this hook will find all .cs scripts in `Assets/Scripts` and apply dotnet-format which is declared in `.editorconfig`