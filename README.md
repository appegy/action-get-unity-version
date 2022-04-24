# unity-version

GitHub Action that returns unity version in specific folder.

## Inputs

### `project_path`

Path to Unity project. Used to find Unity version. Default `${{ github.workspace }}`.

### `version_env`

The name of environment variable to store the version. Keep empty if you don't want to store it.

### `changeset_env`

The name of environment variable to store the changeset. Keep empty if you don't want to store it.

## Outpust

### `version`

Unity version.

### `changeset`

Unity version changeset

## Example usage

```yaml
- name: Checkout project
  uses: actions/checkout@v2

- name: Get Unity version
  id: unity-version
  uses: appegy/action-get-unity-version@v1   

- name: Show unity version
  run: |
    echo "[Env] Version = $UNITY_VERSION"
    echo "[Env] Changeset = $UNITY_VERSION_CHANGESET"
    echo "Version = ${{ steps.unity-version.outputs.unity-version }}"
    echo "Changeset = ${{ steps.unity-version.outputs.unity-version-changeset }}"
```
