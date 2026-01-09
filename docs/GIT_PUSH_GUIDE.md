# Push NTIS Platform WTIS Solution to Repository

## ?? Prerequisites

Before pushing, ensure you have:
- ? Git installed on your system
- ? GitHub/GitLab/Azure DevOps account
- ? Repository `ntis-platform_WTIS` created
- ? Git credentials configured

---

## ?? Step-by-Step Guide

### Step 1: Initialize Git Repository

```powershell
# Navigate to project directory
cd D:\ntis-platform-feature-master-demo

# Initialize git repository
git init

# Configure user (if not already configured globally)
git config user.name "Your Name"
git config user.email "your.email@example.com"
```

---

### Step 2: Create `.gitignore` File

Create a `.gitignore` file to exclude unnecessary files:

```gitignore
# Build results
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio cache/options
.vs/
.vscode/
*.suo
*.user
*.userosscache
*.sln.docstates

# ReSharper
_ReSharper*/
*.[Rr]e[Ss]harper
*.DotSettings.user

# NuGet Packages
*.nupkg
*.snupkg
.nuget/
packages/

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Test Results
[Tt]est[Rr]esult*/
[Bb]uild[Ll]og.*

# .NET Core
project.lock.json
project.fragment.lock.json
artifacts/

# Files built by Visual Studio
*_i.c
*_p.c
*_h.h
*.ilk
*.meta
*.obj
*.iobj
*.pch
*.pdb
*.ipdb
*.pgc
*.pgd
*.rsp
*.sbr
*.tlb
*.tli
*.tlh
*.tmp
*.tmp_proj
*_wpftmp.csproj
*.log
*.vspscc
*.vssscc
.builds
*.pidb
*.svclog
*.scc

# Visual Studio profiler
*.psess
*.vsp
*.vspx
*.sap

# ReSharper
_ReSharper*/
*.[Rr]e[Ss]harper
*.DotSettings.user

# DocFX
_site/

# Sensitive information (IMPORTANT!)
appsettings.Development.json
appsettings.Production.json
*.secrets.json
connectionstrings.json

# Environment variables
.env
.env.local

# IDE
.idea/
*.swp
*.swo
*~
```

---

### Step 3: Stage All Files

```powershell
# Add all files to staging
git add .

# Check status to see what will be committed
git status
```

---

### Step 4: Create Initial Commit

```powershell
# Commit with descriptive message
git commit -m "feat: Initial commit - WTIS Consumer Account Module

- Implemented LINQ-based repository with master table joins
- Created single universal search API endpoint
- Added hierarchical pattern search (Ward-Property-Partition)
- Optimized for performance with indexed queries
- Implemented clean code principles with regions and comments
- Added comprehensive documentation

Features:
- Consumer search by: Number, Mobile, Name, Email, ID, Pattern
- Ward-based search (all consumers in ward)
- Property-based search (all partitions in property)
- Exact partition search
- Master table data included (ConnectionType, Category, PipeSize)
- Pagination support for list results"
```

---

### Step 5: Add Remote Repository

**Option A: HTTPS (Recommended for beginners)**

```powershell
# Add remote repository (replace with your actual repository URL)
git remote add origin https://github.com/your-username/ntis-platform_WTIS.git

# Verify remote
git remote -v
```

**Option B: SSH (For advanced users with SSH keys)**

```powershell
# Add remote repository with SSH
git remote add origin git@github.com:your-username/ntis-platform_WTIS.git

# Verify remote
git remote -v
```

---

### Step 6: Push to Remote

```powershell
# Push to main branch (or master, depending on your default branch)
git push -u origin main

# If main doesn't exist, try master
# git push -u origin master

# If you want to create a new branch
# git checkout -b feature/wtis-consumer-module
# git push -u origin feature/wtis-consumer-module
```

---

## ?? Authentication Options

### Option 1: Personal Access Token (GitHub)

1. Go to GitHub ? Settings ? Developer settings ? Personal access tokens
2. Generate new token with `repo` permissions
3. Copy the token
4. When prompted for password during push, use the token

```powershell
# GitHub will prompt for credentials
Username: your-github-username
Password: <paste-your-personal-access-token>
```

### Option 2: Git Credential Manager

```powershell
# Windows Credential Manager (automatically prompts for credentials)
git config --global credential.helper manager

# Then push (credential manager will handle authentication)
git push -u origin main
```

### Option 3: SSH Key (Advanced)

```powershell
# Generate SSH key (if you don't have one)
ssh-keygen -t ed25519 -C "your.email@example.com"

# Add SSH key to ssh-agent
ssh-add ~/.ssh/id_ed25519

# Copy public key and add to GitHub/GitLab
cat ~/.ssh/id_ed25519.pub
# Add the key to: GitHub ? Settings ? SSH and GPG keys
```

---

## ?? Common Git Commands

```powershell
# Check repository status
git status

# View commit history
git log --oneline

# View remote repositories
git remote -v

# Create new branch
git checkout -b feature/new-feature

# Switch branches
git checkout main

# Pull latest changes
git pull origin main

# Push changes
git push origin main

# View differences
git diff

# Undo changes in working directory
git checkout -- <file>

# Undo last commit (keep changes)
git reset --soft HEAD~1

# View all branches
git branch -a
```

---

## ?? Branch Strategy (Recommended)

### Create Feature Branch

```powershell
# Create and switch to feature branch
git checkout -b feature/wtis-consumer-account

# Make changes and commit
git add .
git commit -m "feat: Add consumer account search functionality"

# Push feature branch
git push -u origin feature/wtis-consumer-account
```

### Merge to Main (After Review)

```powershell
# Switch to main branch
git checkout main

# Pull latest changes
git pull origin main

# Merge feature branch
git merge feature/wtis-consumer-account

# Push to remote
git push origin main
```

---

## ?? What Will Be Pushed

### Project Structure
```
ntis-platform_WTIS/
??? src/
?   ??? Core/
?   ?   ??? NtisPlatform.Core/
?   ?       ??? Entities/
?   ?           ??? WTIS/
?   ?               ??? ConsumerAccountEntity.cs
?   ?               ??? ConsumerAccountWithMasterData.cs
?   ?               ??? MasterEntities.cs
?   ??? Application/
?   ?   ??? NtisPlatform.Application/
?   ?       ??? DTOs/WTIS/
?   ?       ??? Interfaces/WTIS/
?   ?       ??? Mappings/WTIS/
?   ?       ??? Services/WTIS/
?   ??? Infrastructure/
?   ?   ??? NtisPlatform.Infrastructure/
?   ?       ??? Data/
?   ?       ?   ??? ApplicationDbContext.cs
?   ?       ??? Repositories/WTIS/
?   ??? Presentation/
?       ??? NtisPlatform.Api/
?           ??? Controllers/WTIS/
??? docs/
?   ??? WTIS_OPTIMIZATION_SUMMARY.md
?   ??? WTIS_LINQ_IMPLEMENTATION.md
?   ??? HIERARCHICAL_SEARCH_API.md
?   ??? ONE_PARAMETER_API.md
??? .gitignore
??? README.md
??? NtisPlatform.sln
```

---

## ? Verification Checklist

Before pushing, verify:

- [ ] Code compiles successfully (`dotnet build`)
- [ ] All tests pass (`dotnet test`)
- [ ] No sensitive information in code (passwords, connection strings, etc.)
- [ ] `.gitignore` file is configured
- [ ] Commit messages are descriptive
- [ ] Documentation is included
- [ ] Code follows project conventions

---

## ?? Troubleshooting

### Issue: "fatal: not a git repository"
```powershell
# Solution: Initialize git
git init
```

### Issue: "failed to push some refs"
```powershell
# Solution: Pull first, then push
git pull origin main --rebase
git push origin main
```

### Issue: "Authentication failed"
```powershell
# Solution: Use Personal Access Token or SSH
# Or configure credential manager
git config --global credential.helper manager
```

### Issue: "remote origin already exists"
```powershell
# Solution: Remove and re-add
git remote remove origin
git remote add origin <your-repo-url>
```

### Issue: Large file rejection
```powershell
# Solution: Add to .gitignore and remove from git
git rm --cached <large-file>
echo "<large-file>" >> .gitignore
git commit -m "Remove large file"
```

---

## ?? Additional Resources

- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [Git Cheat Sheet](https://education.github.com/git-cheat-sheet-education.pdf)
- [Conventional Commits](https://www.conventionalcommits.org/)

---

## ?? Quick Start Commands

```powershell
# Complete workflow in one go
cd D:\ntis-platform-feature-master-demo
git init
git add .
git commit -m "feat: Initial commit - WTIS Consumer Account Module"
git remote add origin https://github.com/your-username/ntis-platform_WTIS.git
git push -u origin main
```

---

## ?? Need Help?

If you encounter issues:
1. Check the error message carefully
2. Search for the error on Stack Overflow
3. Check Git documentation
4. Verify your repository URL and credentials

---

**Good luck with your push!** ??
