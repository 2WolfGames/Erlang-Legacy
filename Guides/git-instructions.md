# Git instructions

## Commands

### Create new branch

    git checkout -b
    git switch -c

### Change branch

    git checkout <branch>
    git switch <branch>

### Track files

    git add <file> | <regex>

### Commit changes

    git commit -m <title>
    git commit -m <title> -m <body>

### Update remote state

    git fetch

### Update remote state and prune deleted branches

    git fetch --prune

### Merge remote state with local state

    git merge

### Update and merge remote state with local state

    git pull

### Set remote upstream

    git push -u origin <branch>

### Delete remote usptream

    git push -d origin <branch>

### Test remote changes

    => move to testing branch
    git pull
    => test changes
    => if you like what you see, go to Github and accept PR
    => if you don't, make needed changes and push them and accept PR or ask for revision
