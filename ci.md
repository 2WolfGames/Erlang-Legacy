# CI personal license

All actions use a Unity installation, wich needs to be activated. 

To make CI success, you need to do the following:

- Request an activation file for usage on GitHub
- Use that file to acquire a license
- Set the license as a secret 

About the activation file
You may use the [Unity - Request Activation](https://github.com/marketplace/actions/unity-request-activation-file) File action with the below instructions.

The activation file uses machine identifiers so you cannot perform this step locally.

Steps:
 - Create a file called .github/workflows/activation.yml and push it to repository
 - Manually run the above workflow.
 - Download the manual activation file that now appeared as an artifact and extract the Unity_v20XX.X.XXXX.alf file from the zip.
 - Visit license.unity3d.com and upload the Unity_v20XX.X.XXXX.alf file.
 - You should now receive your license file (Unity_v20XX.x.ulf) as a download. It's ok if the numbers don't match your Unity version exactly.
 - Open Github > Go to your repository > Settings > Secrets.
 - Create the following secrets;
    - UNITY_LICENSE - (Copy the contents of your license file into here)
    - UNITY_EMAIL - (Add the email address that you use to login to Unity)
    - UNITY_PASSWORD - (Add the password that you use to login to Unity)

TO MORE INFORMATION GO TO:

    https://game.ci/docs/github/activation


# CI workflow

To make de CI you need to add to your deploy pipeline some build in actions
 - actions/checkout@v2
 - cache (for performance)
 - game-ci/unity-builder@v2 (builder for unity)
 - actions/upload-artifact@v2 (builder for uploading artifacts)
 - actions/download-artifact@v2 (builder for downloading artifacts)
 - JamesIves/github-pages-deploy-action@4.1.4 (for deploying to github pages)

example:
```
name: Erlang-Legacy CI 🎮

on: push

env:
  UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
  UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
  UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
  PROJECT_PATH: Erlang-Legacy

jobs:

  checkLicense:
    name: Check Unity license ✔️
    runs-on: ubuntu-latest
    steps:
      - name: Fail - No license 💀
        if: ${{ !startsWith(env.UNITY_LICENSE, '<') }}
        run: exit 1 

  buildWebGL:
    needs: checkLicense
    name: Build for WebGL 🖥️
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code 👁️‍🗨️
        uses: actions/checkout@v2
        with:
          lfs: true

      # Cache
      - name: Cache dependencies 🔄️
        uses: actions/cache@v2
        with:
          path: ${{ env.PROJECT_PATH }}/Library
          key: Library-${{ hashFiles('${{ env.PROJECT_PATH }}/Assets/**', '${{ env.PROJECT_PATH }}/Packages/**', '${{ env.PROJECT_PATH }}/ProjectSettings/**') }}
          restore-keys: |
            Library-

      # Build
      - name: Build project 🔧
        uses: game-ci/unity-builder@v2
        env:
          UNITY_LICENSE: ${{ env.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ env.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ env.UNITY_PASSWORD }}
        with:
          projectPath: ${{ env.PROJECT_PATH }}
          targetPlatform: WebGL

      # Output
      - name: Upload artificat ⬆️
        uses: actions/upload-artifact@v2
        with:
          name: build-WebGL
          path: build/WebGL

  deployPages:
    needs: buildWebGL
    name: Deploy to Github Pages 🚀
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code 👁️‍🗨️
        uses: actions/checkout@v2

      - name: Download artificat ⬇️
        uses: actions/download-artifact@v2
        with:
          name: build-WebGL
          path: build

      - name: Deploy 🌎
        uses: JamesIves/github-pages-deploy-action@4.1.4
        with:
          branch: gh-pages
          folder: build/WebGL
```