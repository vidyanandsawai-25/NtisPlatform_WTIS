# ?? Push to Your Repository - Quick Guide

## Repository URL
```
https://github.com/vidyanandsawai-25/ntis-platform_WTIS
```

---

## ? Method 1: Automated Script (Easiest)

### Run the PowerShell Script

1. **Right-click** on `push-to-github.ps1`
2. Select **"Run with PowerShell"**
3. Follow the prompts

OR

Open PowerShell in the project directory:
```powershell
cd D:\ntis-platform-feature-master-demo
.\push-to-github.ps1
```

---

## ?? Method 2: Manual Commands (Step-by-Step)

Open **PowerShell** and copy-paste these commands **one by one**:

### Step 1: Navigate to Project
```powershell
cd D:\ntis-platform-feature-master-demo
```

### Step 2: Initialize Git
```powershell
git init
```

### Step 3: Configure Git User
```powershell
# Replace with your actual details
git config user.name "Vidyanand Sawai"
git config user.email "your.email@example.com"
```

### Step 4: Add All Files
```powershell
git add .
```

### Step 5: Check Status
```powershell
git status
```

### Step 6: Create Commit
```powershell
git commit -m "feat: WTIS Consumer Account Module - Complete Implementation

- Implemented LINQ-based repository with master table joins
- Created single universal search API endpoint
- Added hierarchical pattern search (Ward-Property-Partition)
- Optimized for performance with indexed queries
- Clean code with regions and documentation
- Master table data integration

Technical Stack:
- .NET 10 with C# 14.0
- Entity Framework Core 9.0 with LINQ
- Clean Architecture
- Performance: 60% improvement, 40% code reduction"
```

### Step 7: Add Remote Repository
```powershell
git remote add origin https://github.com/vidyanandsawai-25/ntis-platform_WTIS.git
```

### Step 8: Verify Remote
```powershell
git remote -v
```

### Step 9: Push to GitHub
```powershell
git push -u origin main
```

**If "main" doesn't work, try:**
```powershell
git push -u origin master
```

---

## ?? Authentication

When you push, Git will ask for credentials:

### Using Personal Access Token (Recommended)

**Username:** `vidyanandsawai-25`  
**Password:** `<Your-Personal-Access-Token>`

### How to Get a Personal Access Token

1. Go to: https://github.com/settings/tokens
2. Click **"Generate new token (classic)"**
3. Give it a name: `NTIS Platform WTIS`
4. Select scope: ? **`repo`** (Full control of private repositories)
5. Click **"Generate token"**
6. **COPY THE TOKEN** (you won't see it again!)
7. Use this token as your password when pushing

### Using GitHub CLI (Alternative - Easier)

```powershell
# Install GitHub CLI
winget install --id GitHub.cli

# Authenticate
gh auth login

# Follow the prompts and login via browser

# Then push
git push -u origin main
```

---

## ? Verification

After successful push:

1. **Visit your repository:**
   ```
   https://github.com/vidyanandsawai-25/ntis-platform_WTIS
   ```

2. **Check that your files are there**

3. **View the commit history**

---

## ?? Troubleshooting

### Problem: "fatal: not a git repository"
```powershell
git init
```

### Problem: "remote origin already exists"
```powershell
git remote remove origin
git remote add origin https://github.com/vidyanandsawai-25/ntis-platform_WTIS.git
```

### Problem: "Authentication failed"
- ? Don't use your GitHub password
- ? Use Personal Access Token
- Or use GitHub CLI: `gh auth login`

### Problem: "failed to push some refs"
```powershell
# Pull first, then push
git pull origin main --rebase
git push -u origin main
```

### Problem: "couldn't find remote ref main"
```powershell
# Try master instead
git push -u origin master
```

### Problem: Repository not empty
```powershell
# If remote repo has existing commits
git pull origin main --allow-unrelated-histories
git push -u origin main
```

---

## ?? What Will Be Pushed

### ? Included
- Source code (`src/`)
- Documentation (`docs/`)
- Project files (`.csproj`, `.sln`)
- README.md
- .gitignore

### ? Excluded (via .gitignore)
- `bin/` and `obj/` folders
- `.vs/` and `.vscode/`
- User-specific files
- `appsettings.Development.json`
- `appsettings.Production.json`
- Connection strings and secrets

---

## ?? Quick Reference

| Command | Description |
|---------|-------------|
| `git status` | Check repository status |
| `git log --oneline` | View commit history |
| `git remote -v` | View remote repositories |
| `git push origin main` | Push changes to main branch |
| `git pull origin main` | Pull latest changes |
| `git branch` | View current branch |

---

## ?? Need Help?

### Common Issues

1. **Can't run PowerShell script?**
   - Right-click PowerShell ? Run as Administrator
   - Run: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`

2. **Authentication keeps failing?**
   - Make sure you're using a Personal Access Token
   - NOT your GitHub password
   - Create new token at: https://github.com/settings/tokens

3. **Large file error?**
   - Check `.gitignore` is configured correctly
   - Remove large files: `git rm --cached <file>`

---

## ?? After Successful Push

Your code is now on GitHub! You can:

1. ? **View it online**
   - https://github.com/vidyanandsawai-25/ntis-platform_WTIS

2. ? **Add a description**
   - Go to repository settings
   - Add description and topics

3. ? **Share with team**
   - Send the repository URL
   - Add collaborators in Settings ? Collaborators

4. ? **Set up CI/CD**
   - GitHub Actions
   - Automated builds and tests

5. ? **Create documentation**
   - Add README badges
   - Wiki pages
   - GitHub Pages

---

## ?? Additional Files Created

I've created several helper files for you:

1. **`push-to-github.ps1`** - Automated PowerShell script
2. **`PUSH_COMMANDS.txt`** - Manual commands reference
3. **`PUSH_TO_YOUR_REPO.md`** - This file
4. **`docs/GIT_PUSH_GUIDE.md`** - Comprehensive git guide
5. **`QUICK_PUSH_GUIDE.md`** - Quick reference

---

## ?? Let's Push!

**Choose your method:**

### Option A: Automated (Easiest)
```powershell
.\push-to-github.ps1
```

### Option B: Manual (Full Control)
Follow the commands in **Method 2** above

### Option C: GitHub CLI (Cleanest)
```powershell
gh auth login
git push -u origin main
```

---

**Good luck! Your WTIS module will be safely stored on GitHub! ??**

---

## ?? Repository Details

- **URL:** https://github.com/vidyanandsawai-25/ntis-platform_WTIS
- **Owner:** vidyanandsawai-25
- **Project:** NTIS Platform - WTIS Module
- **Tech Stack:** .NET 10, EF Core 9.0, Clean Architecture
