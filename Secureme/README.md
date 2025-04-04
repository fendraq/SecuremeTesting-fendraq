# ~ seCuReMe ~

# For teammates that wants to clone your code use Git Bash:
1. Choose a location for your file repos (cd/.../desktop)
2. git clone -> SSH-Key (located in GitHub <> Code choose: SSH)



# For main user, Git Bash
1. Choose a location for your file repos (cd/.../desktop)
2. cat ~/.ssh/id_ed25519.pub (Which is located in your .SSH file)
3. git clone -> SSH-Key



# Preview:
Utilizing Trello.com to divide user stories in to smaller task:

## Working with tasks:
1. Choose a task from Trello.com
2. Create a feature-branch -> name as task (git branch (branchName))
3. git checkout (branchName)
4. git push -u origin (branchName)

## *** IMPORTANT *** Pull from Dev-Branch for latest version as soon as you open the program.
1. Start working on the task
2. When task is finished:
3. Move Task to review on Trello.com
4. Code review with at least one teammate

## Update your branch with the latest code from dev.
1. git checkout dev
2. git pull
3. git checkout BRANCHNAME
4. git merge dev (Any problems that happens could be that there is missing an upstream).

# Push new code
Check if anyone else is pushing and alert everyone:

1.  git checkout dev
2.  git pull
3.  git checkout BRANCHNAME
4.  git add .
5.  git commit -m "Write a description on what has been done"
6.  git status
7.  git push
8.  git checkout dev
9.  git merge BRANCHNAME -- Ev resolve conflicts -->git add, git commit
10. git status // Before pushing anything
11. git push

## After review:
1. delete branch from dev using ->: git branch -d branch-name
2. move task to done in Trello.com
3. git fetch
4. git rebase branch(dev)  - (to merge your old branch with new pushed dev branches)
5. :q - VIM Git error without commit
6. :wq - VIM git error